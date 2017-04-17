using DotSpatial.Projections;
using GeoAPI;
using GeoAPI.CoordinateSystems.Transformations;
using GeoAPI.Geometries;
using NetTopologySuite.Geometries;
using Npgsql;
using ProjNet.CoordinateSystems;
using SharpMap.Converters.WellKnownText;
using SharpMap.Data;
using SharpMap.Data.Providers;
using SharpMap.Layers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gis_rekreacija
{
    //dummy comment
    enum Modes { Selection = 1, Pan = 2, DrawPolygon = 3, SelectFirstFeature = 4, SelectSecondFeatures = 5, DrawRectangle=6, ZoomWindow=7 };

    public partial class Form1 : Form
    {
        string connStr = "Server=" + DbConfig.host + ";Port=" + DbConfig.port + ";User Id=" + DbConfig.username + ";Password=" + DbConfig.password + ";Database=" + DbConfig.database + ";Pooling=False";
        string tablename = "poligoni_srbija";
        string tablename_point = "tacke_srbija";
        string tablename_line = "putevi_srbija";
        string geomname = "geom";
        string idname = "gid";

        Dictionary<string, SharpMap.Layers.ILayer> all_layers;

        List<VectorLayer> selectedLayers;

        Dictionary<string,Dictionary<string, SharpMap.Styles.IStyle>> style_dict;

        int mode;

        public Form1()
        {
            //
            InitializeComponent();

            DataLayer.DataLayer.CreateDatabaseConnection();

            this.checkedListBox1.AllowDrop = true;

            all_layers = new Dictionary<string, SharpMap.Layers.ILayer>();
            style_dict = new Dictionary<string, Dictionary<string, SharpMap.Styles.IStyle>>();

            SharpMap.Layers.VectorLayer polygons = new SharpMap.Layers.VectorLayer("blabla");
            polygons.DataSource = new SharpMap.Data.Providers.PostGIS(connStr, tablename, geomname, idname);
            all_layers.Add(polygons.ToString(), polygons);

            SharpMap.Layers.VectorLayer points = new SharpMap.Layers.VectorLayer("Points");
            points.DataSource = new SharpMap.Data.Providers.PostGIS(connStr, tablename_point, geomname, idname);
            all_layers.Add(points.ToString(), points);

            SharpMap.Layers.VectorLayer roads = new SharpMap.Layers.VectorLayer("Roads");
            roads.DataSource = new SharpMap.Data.Providers.PostGIS(connStr, tablename_line, geomname, idname);
            all_layers.Add(roads.ToString(), roads);

            
            mapBox1.Map.BackColor = Color.White;
            mapBox1.Map.Layers.Add(points);
            mapBox1.Map.Layers.Add(polygons);
            mapBox1.Map.Layers.Add(roads);

            this.checkedListBox1.Items.Add(points.ToString(), true);
            this.checkedListBox1.Items.Add(polygons.ToString(), true);
            this.checkedListBox1.Items.Add(roads.ToString(), true);

            mapBox1.Map.BackgroundLayer.Add(new SharpMap.Layers.TileAsyncLayer(
             new BruTile.Web.OsmTileSource(), "OSM"));

            mapBox1.Map.ZoomToBox(new GeoAPI.Geometries.Envelope(1510112.16978685, 3324281.86207363, 5865971.47258954, 5155265.8199111));
            mapBox1.Refresh();
            mapBox1.ActiveTool = SharpMap.Forms.MapBox.Tools.Pan;
            mode = (int)Modes.Pan;
            //mapBox1.ActiveTool = SharpMap.Forms.MapBox.Tools.DrawPolygon;
            this.mapDigitizeGeometriesToolStrip1.Enabled = true;

            checkBox1.Checked = false;
            tbDistanceMeters.Enabled = false;
            tbObjectNumber.Enabled = false;

            FillSpatialQueryInitialData();
        }
        #region PublicF
        public void AddStyles(VectorLayer vl, Dictionary<string, SharpMap.Styles.IStyle> styles) {
            if (style_dict.ContainsKey(vl.LayerName)) {
                style_dict[vl.LayerName] = styles;
            }
            else
            {
                style_dict.Add(vl.LayerName, styles);
            }
        }
        public Dictionary<string, SharpMap.Styles.IStyle> GetStyles(string layer_name) {
            if (style_dict.ContainsKey(layer_name))
                return style_dict[layer_name];
            else
                return null;
        }
        public void AddLabelLayer(LabelLayer labelLayer) {
            all_layers.Add(labelLayer.ToString(), labelLayer);
            mapBox1.Map.Layers.Add(labelLayer);
            this.checkedListBox1.Items.Add(labelLayer.ToString(), true);
        }
        public void RemoveLabelLayer(string labelLayer) {
            if (all_layers.ContainsKey(labelLayer)) {
                var layer = all_layers[labelLayer];
                all_layers.Remove(labelLayer);
                mapBox1.Map.Layers.Remove(layer);
                this.checkedListBox1.Items.Remove(labelLayer);
            }
        }
        public void AddLayer(string layer_name,string table_name) {
            SharpMap.Layers.VectorLayer lay = new SharpMap.Layers.VectorLayer(layer_name);
            lay.DataSource = new SharpMap.Data.Providers.PostGIS(connStr, table_name, geomname, idname);
            all_layers.Add(lay.ToString(), lay);
            mapBox1.Map.Layers.Add(lay);
            this.checkedListBox1.Items.Add(lay.ToString(), true);
            mapBox1.Refresh();
        }
        public void RemoveLayer(string layer_name) {
            this.checkedListBox1.Items.Remove(layer_name);
            VectorLayer lay = (VectorLayer)all_layers[layer_name];
            mapBox1.Map.Layers.Remove(lay);
            all_layers.Remove(layer_name);
            //brisi selection layere i provera za label layere
            if (this.checkedListBox1.Items.Contains(layer_name + " Labels")) {
                this.checkedListBox1.Items.Remove(layer_name + " Labels");
                LabelLayer lay_lab = (LabelLayer)all_layers[layer_name + " Labels"];
                mapBox1.Map.Layers.Remove(lay_lab);
                all_layers.Remove(layer_name + " Labels");
            }
            mapBox1.Refresh();
        }
        public VectorLayer GetLayer(string layer_name) {
            return (VectorLayer)all_layers[layer_name];
        }
        #endregion
        private void mapBox1_MouseMove(GeoAPI.Geometries.Coordinate worldPos, MouseEventArgs imagePos)
        {
            //Sets up a array to contain the x and y coordinates
            double[] xy = new double[2];
            xy[0] = worldPos.X;
            xy[1] = worldPos.Y;
            //An array for the z coordinate
            double[] z = new double[1];
            z[0] = 1;
            this.label2.Text = xy[0] + " " + xy[1];
            //Defines the starting coordiante system
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.World.Mercatorworld;
            //Defines the ending coordiante system
            ProjectionInfo pEnd = KnownCoordinateSystems.Geographic.World.WGS1984;
            //Calls the reproject function that will transform the input location to the output locaiton
            Reproject.ReprojectPoints(xy, z, pStart, pEnd, 0, 1);

            this.label1.Text = xy[0] + " " + xy[1];
        }

        private int oriIndex;
        private int newIndex;

        private void checkedListBox1_DragDrop(object sender, DragEventArgs e)
        {
            // Ensure that the list item index is contained in the data.    
            if (e.Data.GetDataPresent(typeof(System.String)))
            {
                if (e.Data.GetDataPresent(DataFormats.StringFormat))
                {
                    string str = (string)e.Data.GetData(DataFormats.StringFormat);

                    //New part
                    newIndex = ((CheckedListBox)sender).IndexFromPoint(((CheckedListBox)sender).PointToClient(new System.Drawing.Point(e.X, e.Y)));
                    var wasItemChecked = ((CheckedListBox)sender).GetItemChecked(oriIndex);

                    if (newIndex != oriIndex)
                    {
                        if (newIndex > -1)
                        {
                            if (newIndex > oriIndex)
                            {
                                ((CheckedListBox)sender).Items.Insert(newIndex + 1, str);
                                ((CheckedListBox)sender).SetItemChecked(newIndex + 1, wasItemChecked);
                                ((CheckedListBox)sender).Items.RemoveAt(oriIndex);
                            }
                            else
                            {
                                ((CheckedListBox)sender).Items.Insert(newIndex, str);
                                ((CheckedListBox)sender).SetItemChecked(newIndex, wasItemChecked);
                                ((CheckedListBox)sender).Items.RemoveAt(oriIndex + 1);
                            }
                        }

                        else
                        {
                            ((CheckedListBox)sender).Items.Add(str);
                            var count = ((CheckedListBox)sender).Items.Count;
                            ((CheckedListBox)sender).SetItemChecked(count - 1, wasItemChecked);
                            ((CheckedListBox)sender).Items.RemoveAt(oriIndex);
                        }
                    }

                    //
                    mapBox1.Map.Layers.Clear();

                    var listCount = this.checkedListBox1.Items.Count;

                    for (int i = 0; i < listCount; i++)
                    {
                        string layerName = this.checkedListBox1.Items[i].ToString();
                        bool isChecked = this.checkedListBox1.GetItemChecked(i);

                        var layerMap = all_layers[layerName];
                        layerMap.Enabled = isChecked;
                        mapBox1.Map.Layers.Add(layerMap);
                    }

                    mapBox1.Refresh();

                }

            }

            Debug.WriteLine("DragDrop");


        }

        private void checkedListBox1_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
            Debug.WriteLine("DragOver");
        }

        private void checkedListBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (((CheckedListBox)sender).Items.Count == 0)
                return;

            oriIndex = ((CheckedListBox)sender).IndexFromPoint(e.X, e.Y);

            string s = "";

            if (oriIndex >= 0)
            {
                s = ((CheckedListBox)sender).Items[oriIndex].ToString();
            }

            DragDropEffects dde1 = DoDragDrop(s, DragDropEffects.Move);

            Debug.WriteLine("MouseDown");
        }

        private void checkedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            string layerName = this.checkedListBox1.Items[e.Index].ToString();

            bool isChecked = e.NewValue == CheckState.Checked ? true : false;

            mapBox1.Map.Layers.GetLayerByName(layerName).Enabled = isChecked;

            mapBox1.Refresh();

            Debug.WriteLine("ItemCheck");
        }

        private void checkedListBox1_DoubleClick(object sender, EventArgs e)
        {
            var checkBox = (CheckedListBox)sender;

            if (checkBox.SelectedIndex >= 0)
            {
                var layerName = checkedListBox1.GetItemText(checkedListBox1.SelectedItem);

                var layer = all_layers[layerName];

                LayerStyle styleForm = new LayerStyle((VectorLayer)layer);
                styleForm.mainForm = this;
                styleForm.Show();

            }
        }

        private void btnEditLayer_Click(object sender, EventArgs e) // stilovi
        {
            if (checkedListBox1.SelectedIndex >= 0)
            {
                var layerName = checkedListBox1.GetItemText(checkedListBox1.SelectedItem);
                if (mapBox1.Map.Layers.GetLayerByName(layerName).Enabled) {
                    var layer = all_layers[layerName];
                    if (layer.GetType() == typeof(VectorLayer))
                    {
                        LayerStyle styleForm = new LayerStyle((VectorLayer)layer);
                        styleForm.mainForm = this;
                        styleForm.Show();

                        mapBox1.Refresh();
                    }
                }
            }
        }

        private void clearSelectionLayers() {
            //remove selected layers
            if (selectedLayers != null)
            {
                foreach (VectorLayer layer in selectedLayers)
                {
                    all_layers.Remove(layer.LayerName);
                    mapBox1.Map.Layers.Remove(layer);
                    checkedListBox1.Items.Remove(layer.LayerName);
                }
            }
            selectedLayers = new List<VectorLayer>();
        }
        private void insertSelectionLayers()
        {
            //insert new layers
            foreach (VectorLayer lay in selectedLayers)
            {
                if (!this.all_layers.ContainsKey(lay.ToString()))
                {
                    mapBox1.Map.Layers.Add(lay);
                    this.checkedListBox1.Items.Add(lay.ToString(), true);
                    this.all_layers.Add(lay.ToString(), lay);
                }
                else
                {
                    //TODO DONE - drugi put nece da se reselektuje
                    var layerToRemove = mapBox1.Map.Layers.FirstOrDefault(l => l.LayerName == lay.ToString());
                    var indexInsertAt = mapBox1.Map.Layers.IndexOf(layerToRemove);
                    mapBox1.Map.Layers.Remove(layerToRemove);
                    mapBox1.Map.Layers.Insert(indexInsertAt, lay);
                }
            }
            //mapBox1.Map.ZoomToBox(selectedLayers[0].Envelope);
            mapBox1.Refresh();
        }


        private void mapBox1_MouseUp(GeoAPI.Geometries.Coordinate worldPos, MouseEventArgs imagePos)
        {
            if (mode != (int)Modes.Selection)
                return;

            LayerCollection layers = new LayerCollection();
            Brush layerStyle;

            if (queryModeFirstObject || queryModeSecondObject)
            {
                string layerName;
                if (queryModeFirstObject)
                {
                    clearSelectionLayers();
                    layerName = cbFirstLayer.SelectedItem.ToString();
                    layerStyle = new System.Drawing.SolidBrush(System.Drawing.Color.Blue);
                }
                else
                {
                    layerName = cbSecondLayer.SelectedItem.ToString();
                    layerStyle = new System.Drawing.SolidBrush(System.Drawing.Color.Green);
                }

                var layer = all_layers[layerName];

                DataLayer.DataLayer.OpenConnection();

                if (layer.Enabled && layer.GetType() == typeof(VectorLayer))
                {
                    var vectorLayer = layer as VectorLayer;
                    var tableName = vectorLayer.DataSource.GetFeature(1).Table.TableName;

                    GenericSpatialLayerQuery(vectorLayer, layerStyle, DataLayer.DataLayer.CreateQueryDWithin, tableName, worldPos.X, worldPos.Y, 10);
                }

                insertSelectionLayers();

                DataLayer.DataLayer.CloseConnection();

            }
            else
            {
                //TODO
                clearSelectionLayers();
                DataLayer.DataLayer.OpenConnection();
                layerStyle = new System.Drawing.SolidBrush(System.Drawing.Color.Yellow);

                foreach (var layer in mapBox1.Map.Layers)
                {
                    if (layer.Enabled && layer.GetType() == typeof(VectorLayer))
                    {
                        var vectorLayer = layer as VectorLayer;
                        var tableName = vectorLayer.DataSource.GetFeature(1).Table.TableName;

                        GenericSpatialLayerQuery(vectorLayer, layerStyle, DataLayer.DataLayer.CreateQueryDWithin, tableName, worldPos.X, worldPos.Y, 10);
                    }
                }
                insertSelectionLayers();

                DataLayer.DataLayer.CloseConnection();
            }

        }


        #region OldMousUpMapBox

        /*
         *      private void mapBox1_MouseUp(GeoAPI.Geometries.Coordinate worldPos, MouseEventArgs imagePos)
        {
            if (mode != (int)Modes.Selection)
                return;
            
            FeatureDataTable dt = new FeatureDataTable();
            DataSet ds = new DataSet();

            NpgsqlConnection conn = new NpgsqlConnection("server=" + DbConfig.host + ";port=" + DbConfig.port + ";user=" + DbConfig.username + ";pwd=" + DbConfig.password + ";database=" + DbConfig.database + "");
            conn.Open();
            ds.Reset();

            if (queryModeFirstObject || queryModeSecondObject)
            {
                string layerName;
                Color color;
                if (queryModeFirstObject)
                {
                    clearSelectionLayers();
                    layerName = cbFirstLayer.SelectedItem.ToString();
                    color = System.Drawing.Color.Blue;
                }
                else
                {
                    layerName = cbSecondLayer.SelectedItem.ToString();
                    color = System.Drawing.Color.Green;
                }
                var layer = all_layers[layerName];

                if (!layer.Enabled)
                    return;
                FeatureDataTable fdt = new FeatureDataTable();
                if (layer.GetType() == typeof(VectorLayer))
                {
                    var vectorLayer = layer as VectorLayer;
                    var tableName = vectorLayer.DataSource.GetFeature(1).Table.TableName;
                    var geometry = vectorLayer.DataSource.GetFeature(1).Geometry;
                    //string sql = "SELECT code, fclass, name, geom AS _smtmp_ FROM " + tableName + " WHERE ST_Intersects(ST_SetSRID(ST_MakePoint(" + worldPos.X + ", " + worldPos.Y + "), 3857),geom)";
                    string sql = "SELECT code, fclass, name, geom AS _smtmp_ FROM " + tableName + " WHERE ST_DWithin(ST_SetSRID(ST_MakePoint(" + worldPos.X + ", " + worldPos.Y + "), 3857),geom,10)";
                    NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, conn);
                    ds.Reset();
                    da.Fill(ds);

                    foreach (DataColumn col in ds.Tables[0].Columns)
                    {
                        fdt.Columns.Add(col.Namespace, col.DataType);
                    }
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        FeatureDataRow fDR = fdt.NewRow();
                        fDR.ItemArray = row.ItemArray;
                        IGeometryFactory geometryFactory = GeometryServiceProvider.Instance.CreateGeometryFactory(3857);
                        NetTopologySuite.IO.WKBReader wkbReader = new NetTopologySuite.IO.WKBReader();
                        byte[] b2 = NetTopologySuite.IO.WKBReader.HexToBytes(row[3].ToString());
                        //byte[] b1 = System.Text.Encoding.UTF8.GetBytes(row[3].ToString());
                        byte[] b1 = System.Text.Encoding.Unicode.GetBytes(row[3].ToString());
                        fDR.Geometry = SharpMap.Converters.WellKnownBinary.GeometryFromWKB.Parse(b2, geometryFactory);
                        //fDR.Geometry = SharpMap.Converters.WellKnownText.GeometryFromWKT.Parse(row[3].ToString());
                        //AddRow(fdr);
                        fdt.AddRow(fDR);
                    }
                    SharpMap.Layers.VectorLayer laySelected = new SharpMap.Layers.VectorLayer(layer.LayerName + "Selection");
                    laySelected.DataSource = new GeometryProvider(fdt);
                    laySelected.Style.Fill = new System.Drawing.SolidBrush(color);
                    laySelected.Style.PointColor = new System.Drawing.SolidBrush(color);
                    laySelected.Style.Line = new System.Drawing.Pen(color);
                    if (fdt.Count > 0)
                    {
                        selectedLayers.Add(laySelected);
                    }
                }

                foreach (VectorLayer laySelected in selectedLayers)
                {
                    if (!this.all_layers.ContainsKey(laySelected.ToString()))
                    {
                        mapBox1.Map.Layers.Add(laySelected);
                        this.checkedListBox1.Items.Add(laySelected.ToString(), true);
                        this.all_layers.Add(laySelected.ToString(), laySelected);
                    }
                }
                mapBox1.Refresh();
            }
            else
            {
                clearSelectionLayers();
                foreach (var layer in mapBox1.Map.Layers)
                {
                    if (!layer.Enabled)
                        continue;
                    FeatureDataTable fdt = new FeatureDataTable();
                    if (layer.GetType() == typeof(VectorLayer))
                    {
                        var vectorLayer = layer as VectorLayer;
                        var tableName = vectorLayer.DataSource.GetFeature(1).Table.TableName;
                        var geometry = vectorLayer.DataSource.GetFeature(1).Geometry;
                        //string sql = "SELECT code, fclass, name, geom AS _smtmp_ FROM " + tableName + " WHERE ST_Intersects(ST_SetSRID(ST_MakePoint(" + worldPos.X + ", " + worldPos.Y + "), 3857),geom)";
                        string sql = "SELECT code, fclass, name, geom AS _smtmp_ FROM " + tableName + " WHERE ST_DWithin(ST_SetSRID(ST_MakePoint(" + worldPos.X + ", " + worldPos.Y + "), 3857),geom,10)";
                        NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, conn);
                        ds.Reset();
                        da.Fill(ds);

                        foreach (DataColumn col in ds.Tables[0].Columns)
                        {
                            fdt.Columns.Add(col.Namespace, col.DataType);
                        }
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            FeatureDataRow fDR = fdt.NewRow();
                            fDR.ItemArray = row.ItemArray;
                            IGeometryFactory geometryFactory = GeometryServiceProvider.Instance.CreateGeometryFactory(3857);
                            NetTopologySuite.IO.WKBReader wkbReader = new NetTopologySuite.IO.WKBReader();
                            byte[] b2 = NetTopologySuite.IO.WKBReader.HexToBytes(row[3].ToString());
                            //byte[] b1 = System.Text.Encoding.UTF8.GetBytes(row[3].ToString());
                            byte[] b1 = System.Text.Encoding.Unicode.GetBytes(row[3].ToString());
                            fDR.Geometry = SharpMap.Converters.WellKnownBinary.GeometryFromWKB.Parse(b2, geometryFactory);
                            //fDR.Geometry = SharpMap.Converters.WellKnownText.GeometryFromWKT.Parse(row[3].ToString());
                            //AddRow(fdr);
                            fdt.AddRow(fDR);
                        }
                        SharpMap.Layers.VectorLayer laySelected = new SharpMap.Layers.VectorLayer(layer.LayerName + "Selection");
                        laySelected.DataSource = new GeometryProvider(fdt);
                        laySelected.Style.Fill = new System.Drawing.SolidBrush(System.Drawing.Color.Yellow);
                        laySelected.Style.PointColor = new System.Drawing.SolidBrush(System.Drawing.Color.Yellow);
                        laySelected.Style.Line = new System.Drawing.Pen(System.Drawing.Color.Yellow);

                        if (fdt.Count > 0)
                        {
                            selectedLayers.Add(laySelected);
                        }

                    }
                }
                foreach (VectorLayer laySelected in selectedLayers)
                {
                    mapBox1.Map.Layers.Add(laySelected);
                    this.checkedListBox1.Items.Add(laySelected.ToString(), true);
                    this.all_layers.Add(laySelected.ToString(), laySelected);
                }
                mapBox1.Refresh(); 
            }
          
        }
         */
        #endregion


        private void mapDigitizeGeometriesToolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void btnFeatureQuery_Click(object sender, EventArgs e)
        {
            if (checkedListBox1.SelectedIndex >= 0)
            {
                var layerName = checkedListBox1.GetItemText(checkedListBox1.SelectedItem);

                var layer = all_layers[layerName];
                if (layer.GetType() == typeof(VectorLayer))
                {
                    LayerFeatureQuery styleForm = new LayerFeatureQuery((VectorLayer)layer);

                    styleForm.ShowDialog();

                    mapBox1.Map.Layers.Add(styleForm.selectionLayer);

                    mapBox1.Refresh();
                }
            }
        }



        #region SpatialQuery

        public void SelectFirstFeatures()
        {

        }

        public void SelectSecondFeatures()
        {

        }

        public void ExecuteSpatialComplex()
        {

        }

        private IGeometry firstObjectGeometry;
        private IGeometry secondObjectGeometry;

        bool queryModeFirstObject = false;
        bool queryModeSecondObject = false;
        List<int> geometriesGidModeFirst = new List<int>();
        List<int> geometriesGidModeSecond = new List<int>();


        private void cbToggleFirstObject_CheckedChanged(object sender, EventArgs e)
        {
            if (cbToggleFirstObject.Checked == true)
            {
                lbFirstObjectInfo.Enabled = true;
                btnSelectFirstObject.Enabled = true;
            }
            else
            {
                lbFirstObjectInfo.Enabled = false;
                btnSelectFirstObject.Enabled = false;
            }
        }

        private void cbToggleSecondObject_CheckedChanged(object sender, EventArgs e)
        {
            if (cbToggleSecondObject.Checked == true)
            {
                lbSecondObjectInfo.Enabled = true;
                btnSelectSecondObject.Enabled = true;
            }
            else
            {
                lbSecondObjectInfo.Enabled = false;
                btnSelectSecondObject.Enabled = false;
            }
        }

        private void FillSpatialQueryInitialData()
        {
            foreach (var layer in all_layers)
            {
                cbFirstLayer.Items.Add(layer.Key);
                cbSecondLayer.Items.Add(layer.Key);
            }

            cbOperation.Items.Add("Contains");
            cbOperation.Items.Add("Crosses");
            cbOperation.Items.Add("Intersects");
            cbOperation.Items.Add("Overlaps");
            cbOperation.Items.Add("Within");

            lbFirstObjectInfo.Enabled = false;
            btnSelectFirstObject.Enabled = false;
            lbSecondObjectInfo.Enabled = false;
            btnSelectSecondObject.Enabled = false;
        }

        private void btnSelectFirstObject_Click(object sender, EventArgs e)
        {
            
            geometriesGidModeFirst = new List<int>(); //IMPORTANT
            queryModeFirstObject = true;
            queryModeSecondObject = false;
        }

        private void btnSelectSecondObject_Click(object sender, EventArgs e)
        {          
            geometriesGidModeSecond = new List<int>(); //IMPORTANT
            queryModeFirstObject = false;
            queryModeSecondObject = true;
        }



        #endregion




        private void mapBox1_GeomDefined(IGeometry geometry)
        {
            string geomString = geometry.AsText();
            if (mode != (int)Modes.DrawPolygon)
                return;

            Brush layerStyle;
            LayerCollection layers = new LayerCollection();

            string layerName;

            if (queryModeFirstObject)
            {
                //polygon selection for first mode
                clearSelectionLayers();
                layerName = cbFirstLayer.SelectedItem.ToString();
                var layer = all_layers[layerName];
                layers.Add(layer);
                layerStyle = new System.Drawing.SolidBrush(System.Drawing.Color.Blue);
            }
            else if (queryModeSecondObject)
            {
                //polygon selection for second mode
                layerName = cbSecondLayer.SelectedItem.ToString();
                var layer = all_layers[layerName];
                layers.Add(layer);
                layerStyle = new System.Drawing.SolidBrush(System.Drawing.Color.Green);
            }
            else
            {
                //normal selection
                clearSelectionLayers();
                layerStyle = new System.Drawing.SolidBrush(System.Drawing.Color.Yellow);
                layers = mapBox1.Map.Layers;
            }


            DataLayer.DataLayer.OpenConnection();

            foreach (var layer in layers)
            {
                if (layer.Enabled && layer.GetType() == typeof(VectorLayer))
                {
                    var vectorLayer = layer as VectorLayer;
                    var tableName = vectorLayer.DataSource.GetFeature(1).Table.TableName;

                    GenericSpatialLayerQuery(vectorLayer, layerStyle, DataLayer.DataLayer.CreateQueryIntersects, tableName, geomString);
                }
            }

            insertSelectionLayers();

            DataLayer.DataLayer.CloseConnection();
        }


        public delegate string QueryDelegate(params object[] args);

        public object InvokeMethod(QueryDelegate method, params object[] args)
        {
            var result = method.Invoke(args);

            return result;
        }

        private void GenericSpatialLayerQuery(VectorLayer layer, Brush layerStyle, QueryDelegate method, params object[] args)
        {
            FeatureDataTable fdt;
            DataSet ds = new DataSet();
            string sql = InvokeMethod(method, args).ToString();

            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, DataLayer.DataLayer.DbConnection);
            ds.Reset();
            da.Fill(ds);

            fdt = CreateFeatureDataTable(ds);

            VectorLayer laySelected = CreateNewSelectionLayer(layer, fdt, layerStyle);

            if (fdt.Count > 0)
                selectedLayers.Add(laySelected);
        }

        private VectorLayer CreateNewSelectionLayer(ILayer layer, FeatureDataTable fdt, Brush style)
        {
            // SharpMap.Layers.VectorLayer laySelected = new SharpMap.Layers.VectorLayer(layer.LayerName + "Selection");
            string selectionLayerName = GenerateSelectionLayerName(layer.LayerName);
            SharpMap.Layers.VectorLayer laySelected = new SharpMap.Layers.VectorLayer(selectionLayerName);
            laySelected.DataSource = new GeometryProvider(fdt);
            laySelected.Style.Fill = style;
            laySelected.Style.PointColor = style;
            laySelected.Style.Line = new System.Drawing.Pen(style);

            return laySelected;
        }

        private string GenerateSelectionLayerName(string layerName)
        {
            string selectionLayerName = "";

            if (queryModeFirstObject)
            {
                selectionLayerName = layerName + "1Selection";
            }
            else if (queryModeSecondObject)
            {
                selectionLayerName = layerName + "2Selection";
            }
            else
            {
                selectionLayerName = layerName + "Selection";
            }

            
            return selectionLayerName;
        }

        private FeatureDataTable CreateFeatureDataTable(DataSet ds)
        {
            FeatureDataTable fdt = new FeatureDataTable();

            foreach (DataColumn col in ds.Tables[0].Columns)
            {
                fdt.Columns.Add(col.Namespace, col.DataType);
            }
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                FeatureDataRow fDR = fdt.NewRow();
                fDR.ItemArray = row.ItemArray;
                IGeometryFactory geometryFactory = GeometryServiceProvider.Instance.CreateGeometryFactory(3857);
                NetTopologySuite.IO.WKBReader wkbReader = new NetTopologySuite.IO.WKBReader();
                byte[] b2 = NetTopologySuite.IO.WKBReader.HexToBytes(row[3].ToString());
                byte[] b1 = System.Text.Encoding.Unicode.GetBytes(row[3].ToString());
                fDR.Geometry = SharpMap.Converters.WellKnownBinary.GeometryFromWKB.Parse(b2, geometryFactory);
                fdt.AddRow(fDR);

                if (queryModeFirstObject)
                {
                    geometriesGidModeFirst.Add(Int32.Parse(row[0].ToString()));
                }
                else if (queryModeSecondObject)
                {
                    geometriesGidModeSecond.Add(Int32.Parse(row[0].ToString()));
                }
            }
            return fdt;
        }


        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (mode == (int)Modes.DrawRectangle)
            {
                //return all layers
                mapBox1.Map.Layers.Clear();
                foreach (ILayer i in all_layers.Values)
                {
                    mapBox1.Map.Layers.Add(i);
                }

                mapBox1.Refresh();
            }
            mode = (int)Modes.Selection;
            mapBox1.ActiveTool = SharpMap.Forms.MapBox.Tools.None;
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (mode == (int)Modes.DrawRectangle)
            {
                //return all layers
                mapBox1.Map.Layers.Clear();
                foreach (ILayer i in all_layers.Values)
                {
                    mapBox1.Map.Layers.Add(i);
                }

                mapBox1.Refresh();
            }
            mode = (int)Modes.Pan;
            mapBox1.ActiveTool = SharpMap.Forms.MapBox.Tools.Pan;
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            if (mode == (int)Modes.DrawRectangle) {
                //return all layers
                mapBox1.Map.Layers.Clear();
                foreach (ILayer i in all_layers.Values) {
                    mapBox1.Map.Layers.Add(i);
                }

                mapBox1.Refresh();
            }
            mode = (int)Modes.DrawPolygon;
            mapBox1.ActiveTool = SharpMap.Forms.MapBox.Tools.DrawPolygon;
        }

        private void button1_Click(object sender, EventArgs e) //labele
        {
            if (checkedListBox1.SelectedIndex >= 0)
            {
                var layerName = checkedListBox1.GetItemText(checkedListBox1.SelectedItem);
                //ako je selection -> skip
                if (mapBox1.Map.Layers.GetLayerByName(layerName).Enabled) // dal je vidljiv
                {
                    var layer = all_layers[layerName];
                    if (layer.GetType() == typeof(VectorLayer))
                    {
                        int selectedIndex = 0;

                        LabelForm lf = new LabelForm((VectorLayer)layer, selectedIndex, this);
                        lf.ShowDialog();

                        mapBox1.Refresh();
                    }
                }
            }
        }

        private string CreateSqlForGidList(List<int> listOfGids)
        {
            string gidSQLList = "(";

            foreach (int gid in listOfGids)
            {
                gidSQLList += gid + ",";
            }
            gidSQLList = gidSQLList.Remove(gidSQLList.Length - 1, 1);
            gidSQLList += ")";

            return gidSQLList;
        }


        private void button_add_new_lay_Click(object sender, EventArgs e)
        {
            //show dialog
            AddLayer add_lay_form = new AddLayer();
            add_lay_form.main_form = this;
            add_lay_form.ShowDialog();
        }

        private void button_remove_lay_Click(object sender, EventArgs e)
        {
            if (this.checkedListBox1.SelectedItem.ToString().Contains("Selection") || this.checkedListBox1.SelectedItem.ToString().Contains("Labels"))
                return;
            RemoveLayer(this.checkedListBox1.SelectedItem.ToString());
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            if (checkedListBox1.SelectedIndex < 0)
                return;
            var temp_layer_name = this.checkedListBox1.SelectedItem.ToString();
            var temp_layer = this.all_layers[temp_layer_name];
            if (mapBox1.Map.Layers[mapBox1.Map.Layers.IndexOf(temp_layer)].Enabled)
            {
                mode = (int)Modes.DrawRectangle;
                mapBox1.ActiveTool = SharpMap.Forms.MapBox.Tools.QueryBox;
                //ostavi upaljen samo jedan selektovani layer
                mapBox1.Map.Layers.Clear();
                mapBox1.Map.Layers.Add(temp_layer);
                mapBox1.Refresh();
                //ostale sve izbaci iz mapboxa
            }
        }
        private void mapBox1_MapQuerid(FeatureDataTable fdt)
        {
            clearSelectionLayers();
            Color color = Color.Yellow;
            SharpMap.Layers.VectorLayer laySelected = new SharpMap.Layers.VectorLayer(fdt.TableName + "Selection");
            laySelected.DataSource = new GeometryProvider(fdt);
            laySelected.Style.Fill = new System.Drawing.SolidBrush(color);
            laySelected.Style.PointColor = new System.Drawing.SolidBrush(color);
            laySelected.Style.Line = new System.Drawing.Pen(color);
            this.mapBox1.Map.Layers.Add(laySelected);
            this.all_layers.Add(laySelected.LayerName, laySelected);
            this.selectedLayers.Add(laySelected);
            this.checkedListBox1.Items.Add(laySelected.LayerName,true);
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            if (mode == (int)Modes.DrawRectangle)
            {
                //return all layers
                mapBox1.Map.Layers.Clear();
                foreach (ILayer i in all_layers.Values)
                {
                    mapBox1.Map.Layers.Add(i);
                }

                mapBox1.Refresh();
            }
            mode = (int)Modes.ZoomWindow;
            mapBox1.ActiveTool = SharpMap.Forms.MapBox.Tools.ZoomWindow;
        }

        private void btnExecuteQuery_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            NpgsqlConnection conn = new NpgsqlConnection("server=" + DbConfig.host + ";port=" + DbConfig.port + ";user=" + DbConfig.username + ";pwd=" + DbConfig.password + ";database=" + DbConfig.database + "");
            conn.Open();

            var firstLayer = all_layers[cbFirstLayer.SelectedItem.ToString()] as VectorLayer;
            var secondLayer = all_layers[cbSecondLayer.SelectedItem.ToString()] as VectorLayer;

            var firstLayerName = firstLayer.DataSource.GetFeature(1).Table.TableName;
            var secondLayerName = secondLayer.DataSource.GetFeature(1).Table.TableName;


            string queryColumns = firstLayerName + ".geom" + "," + secondLayerName + ".geom";
            string queryFrom = firstLayerName + "," + secondLayerName;
            string select_queryColumns = firstLayerName + ".geom" + "," + secondLayerName + ".geom";

            if (firstLayerName == secondLayerName)
            {
                select_queryColumns = firstLayerName + ".geom";
                queryFrom = firstLayerName;
            }

                
            string sql = "";
            bool oneToOne = false;
            string firstGidSQLList = CreateSqlForGidList(geometriesGidModeFirst);
            string secondGidSQLList = CreateSqlForGidList(geometriesGidModeSecond);

            if (!distanceMode)
            {             
                string operation = cbOperation.SelectedItem.ToString();

                    sql = "SELECT " + select_queryColumns +
                      " FROM " + queryFrom +
                      " WHERE ST_" + operation + "(" + firstLayerName + ".geom" + "," + secondLayerName + ".geom)" +
                      " AND " + firstLayerName + ".gid IN " + firstGidSQLList +
                      " AND " + secondLayerName + ".gid IN " + secondGidSQLList;
            }
            else
            {
                if (geometriesGidModeFirst.Count == 1 && geometriesGidModeSecond.Count == 1)
                {
                    oneToOne = true;
                    sql = "SELECT ST_Distance(" + queryColumns + ")" +
                            " FROM " + queryFrom +
                            " WHERE " + firstLayerName + ".gid IN " + firstGidSQLList +
                            " AND " + secondLayerName + ".gid IN " + secondGidSQLList;
                }
                else
                {
                    select_queryColumns = secondLayerName + ".geom";
                    var gidsOfSelectedObject = CreateSqlForGidList(geometriesGidModeFirst);
                    int meters = Int32.Parse(tbDistanceMeters.Text.ToString());
                    int numberOfObjects;
                    bool parsedNumberOfObjects = Int32.TryParse(tbObjectNumber.Text.ToString(), out numberOfObjects);

                    sql = "SELECT " + select_queryColumns +
                          " FROM " + queryFrom +
                          " WHERE " + firstLayerName + ".gid IN " + gidsOfSelectedObject +
                          " AND ST_DWithin(" + queryColumns + ", " + meters + ")";

                    if (parsedNumberOfObjects)
                    {
                        sql += " ORDER BY ST_Distance(" + queryColumns + ") LIMIT " + numberOfObjects;
                    }      
                }

               
            }


            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, conn);
            da.Fill(ds);

            if (oneToOne && distanceMode)
            {
                var row = ds.Tables[0].Rows[0].ItemArray[0];
                tbDistanceMeters.Text = row.ToString();
            }
            else
            {
                dt = ds.Tables[0];

                FeatureDataTable fdt = new FeatureDataTable();

                foreach (DataColumn col in ds.Tables[0].Columns)
                {
                    fdt.Columns.Add(col.Namespace, col.DataType);
                }

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    foreach (var rowItem in row.ItemArray)
                    {
                        FeatureDataRow fDR = fdt.NewRow();
                        fDR.ItemArray = row.ItemArray;
                        IGeometryFactory geometryFactory = GeometryServiceProvider.Instance.CreateGeometryFactory(3857);
                        NetTopologySuite.IO.WKBReader wkbReader = new NetTopologySuite.IO.WKBReader();
                        byte[] b2 = NetTopologySuite.IO.WKBReader.HexToBytes(rowItem.ToString());
                        fDR.Geometry = SharpMap.Converters.WellKnownBinary.GeometryFromWKB.Parse(b2, geometryFactory);
                        fdt.AddRow(fDR);
                    }
                }

                if (fdt.Rows.Count > 0)
                {
                    clearSelectionLayers();
                    VectorLayer laySelected = new SharpMap.Layers.VectorLayer("Selection");
                    laySelected.DataSource = new GeometryProvider(fdt);
                    laySelected.Style.Fill = new System.Drawing.SolidBrush(System.Drawing.Color.Yellow);
                    laySelected.Style.Line = new Pen(System.Drawing.Color.Yellow);
                    // laySelected.Style.PointColor = new Brush(System.Drawing.Color.Yellow);
                    selectedLayers.Add(laySelected);
                    //this.selectionLayer = laySelected;
                }

                this.insertSelectionLayers();
            }
        
            conn.Close();

        }

        private void btnDisableSelection_Click(object sender, EventArgs e)
        {
            queryModeFirstObject = false;
            queryModeSecondObject = false;
            geometriesGidModeFirst = new List<int>();
            geometriesGidModeSecond = new List<int>();
        }

        bool distanceMode = false;
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox distanceCheckbox = (CheckBox)sender;

            if (distanceCheckbox.Checked)
            {
                cbOperation.Enabled = false;
                //cbToggleSecondObject.Enabled = false;
                //btnSelectSecondObject.Enabled = false;
                //lbSecondObjectInfo.Enabled = false;
                tbDistanceMeters.Enabled = true;
                tbObjectNumber.Enabled = true;
                distanceMode = true;
            }
            else
            {
                cbOperation.Enabled = true;
                //cbToggleSecondObject.Enabled = true;
                //btnSelectSecondObject.Enabled = true;
                //lbSecondObjectInfo.Enabled = true;
                tbDistanceMeters.Enabled = false;
                tbObjectNumber.Enabled = false;
                distanceMode = false;
            }
        }

       
        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            //Route
            DataLayer.DataLayer.OpenConnection();
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();

            var firstLayer = all_layers[cbFirstLayer.SelectedItem.ToString()] as VectorLayer;
            var secondLayer = all_layers[cbSecondLayer.SelectedItem.ToString()] as VectorLayer;

            var firstLayerName = firstLayer.DataSource.GetFeature(1).Table.TableName;
            var secondLayerName = secondLayer.DataSource.GetFeature(1).Table.TableName;
            var roadLayerName = "putevi_srbija";

            var queryColumns = firstLayerName + ".geom" + "," + roadLayerName + ".geom";
            var queryFrom = firstLayerName + "," + roadLayerName;

            var select_queryColumns = roadLayerName + ".source, " + roadLayerName + ".target, " + roadLayerName + ".gid";

            int gidOfSelectedObject = geometriesGidModeFirst.FirstOrDefault();

            string sql = "SELECT " + select_queryColumns +
                      " FROM " + queryFrom +
                      " WHERE " + firstLayerName + ".gid=" + gidOfSelectedObject +
                      " AND ST_DWithin(" + queryColumns + ", " + 2000 + ") ORDER BY ST_Distance(" + queryColumns + ") LIMIT 1";

            string sourceId="";
            string targetId="";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, DataLayer.DataLayer.DbConnection);
            da.Fill(ds);

            dt = ds.Tables[0];

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                 sourceId = row.ItemArray[0].ToString();
            }

            queryColumns = secondLayerName + ".geom" + "," + roadLayerName + ".geom";
            queryFrom = secondLayerName + "," + roadLayerName;

            gidOfSelectedObject = geometriesGidModeSecond.FirstOrDefault();

            sql = "SELECT " + select_queryColumns +
                  " FROM " + queryFrom +
                  " WHERE " + secondLayerName + ".gid=" + gidOfSelectedObject +
                  " AND ST_DWithin(" + queryColumns + ", " + 2000 + ") ORDER BY ST_Distance(" + queryColumns + ") LIMIT 1";

            ds.Reset();

            da = new NpgsqlDataAdapter(sql, DataLayer.DataLayer.DbConnection);
            da.Fill(ds);

            dt = ds.Tables[0];

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                targetId = row.ItemArray[1].ToString();
            }

            //imamo oba
            string rout_sql = "select node from pgr_dijkstra('SELECT gid AS id, source, target, st_length(geom) as cost FROM putevi_srbija', "+sourceId+", "+targetId+", false)";
            ds.Reset();
            da = new NpgsqlDataAdapter(rout_sql, DataLayer.DataLayer.DbConnection);
            da.Fill(ds);

            List<int> rout_ids = new List<int>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                rout_ids.Add(Int32.Parse(row.ItemArray[0].ToString()));
            }
            string in_part = CreateSqlForGidList(rout_ids);

            string get_features = "select the_geom from putevi_srbija_vertices_pgr where id in "+ in_part;
            ds.Reset();
            da = new NpgsqlDataAdapter(get_features, DataLayer.DataLayer.DbConnection);

            try
            {
                da.Fill(ds);

                FeatureDataTable fdt = new FeatureDataTable();

                foreach (DataColumn col in ds.Tables[0].Columns)
                {
                    fdt.Columns.Add(col.Namespace, col.DataType);
                }

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    FeatureDataRow fDR = fdt.NewRow();
                    fDR.ItemArray = row.ItemArray;
                    IGeometryFactory geometryFactory = GeometryServiceProvider.Instance.CreateGeometryFactory(3857);
                    NetTopologySuite.IO.WKBReader wkbReader = new NetTopologySuite.IO.WKBReader();
                    byte[] b2 = NetTopologySuite.IO.WKBReader.HexToBytes(row.ItemArray[0].ToString());
                    fDR.Geometry = SharpMap.Converters.WellKnownBinary.GeometryFromWKB.Parse(b2, geometryFactory);
                    fdt.AddRow(fDR);
                }

                if (fdt.Rows.Count > 0)
                {
                    clearSelectionLayers();
                    VectorLayer laySelected = new SharpMap.Layers.VectorLayer("Selection");
                    laySelected.DataSource = new GeometryProvider(fdt);
                    laySelected.Style.Fill = new System.Drawing.SolidBrush(System.Drawing.Color.Red);
                    laySelected.Style.Line = new Pen(System.Drawing.Color.Red);
                    selectedLayers.Add(laySelected);
                }

                insertSelectionLayers();

            }
            catch (Exception ex)
            {

            }

           


            DataLayer.DataLayer.CloseConnection();
        }
    }
}
