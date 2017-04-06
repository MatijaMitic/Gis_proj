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
    enum Modes { Selection = 1, Pan = 2, DrawPolygon = 3, SelectFirstFeature = 4, SelectSecondFeatures = 5, Thu, Fri };

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

        int mode;

        public Form1()
        {
            //
            InitializeComponent();

            this.checkedListBox1.AllowDrop = true;

            all_layers = new Dictionary<string, SharpMap.Layers.ILayer>();

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

            FillSpatialQueryInitialData();
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

        private void mapBox1_MouseUp(GeoAPI.Geometries.Coordinate worldPos, MouseEventArgs imagePos)
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


        #region OldMousUpMapBox

        //private void mapBox1_MouseUp(GeoAPI.Geometries.Coordinate worldPos, MouseEventArgs imagePos)
        //{
        //    //Ovo celo zakomentarisano zapravo radi samo u jednom slucaju, kad kliknes na poligon koji je preklopljen (ima kod Caira)
        //    //Iscrta se i zuto cak, ostalo ako kliknes nista nece

        //    //FeatureDataSet ds = new FeatureDataSet();

        //    ////example uses a map image, but this could be a layer generated with code
        //    //foreach (var layer in mapBox1.Map.Layers)
        //    //{
        //    //    var queryLayer = layer as SharpMap.Layers.ICanQueryLayer;
        //    //    if (queryLayer != null)
        //    //    {
        //    //        var newEnvelope = new Envelope(worldPos);
        //    //        queryLayer.ExecuteIntersectionQuery(newEnvelope, ds);
        //    //    } 
        //    //}

        //    //if (ds.Tables.Count > 0 && ds.Tables.Any(t => t.Rows.Count > 0))
        //    //{
        //    //    SharpMap.Layers.VectorLayer laySelected = new SharpMap.Layers.VectorLayer("Selection");
        //    //    laySelected.DataSource = new GeometryProvider(ds.Tables[1]);

        //    //    laySelected.Style.Fill = new System.Drawing.SolidBrush(System.Drawing.Color.Yellow);
        //    //    mapBox1.Map.Layers.Add(laySelected);
        //    //    mapBox1.Refresh();
        //    //}

        //    //Ovde uspem da pribavim geometrije koje se seku s tackom (ne radi za tacka sloj, mislim da ipak koordinata se treba prosiri malo da bi uhvatila tacke)
        //    //Ali ne uspem da ih prikazem, jer sve mi je DataSet i DataRow, a kao prikaz radi sa FeatureDataSet/Row/Table
        //    //Pa sam pokusavala da to konvertujem u Feature (a kad pokusam direkt u to da smestim opet se nista ne vidi ko za slucaj gore)

        //    DataTable dt = new DataTable();
        //    DataSet ds = new DataSet();
        //    NpgsqlConnection conn = new NpgsqlConnection("server=" + DbConfig.host + ";port=" + DbConfig.port + ";user=" + DbConfig.username + ";pwd=" + DbConfig.password + ";database=" + DbConfig.database + "");
        //    conn.Open();
        //    ds.Reset();

        //    List<DataRow> dataRows = new List<DataRow>();

        //    FeatureDataTable fdt = new FeatureDataTable();

        //    foreach (var layer in mapBox1.Map.Layers)
        //    {
        //        if (layer.GetType() == typeof(VectorLayer))
        //        {
        //            var vectorLayer = layer as VectorLayer;
        //            var tableName = vectorLayer.DataSource.GetFeature(1).Table.TableName;
        //            var geometry = vectorLayer.DataSource.GetFeature(1).Geometry;
        //            string sql = "SELECT code, fclass, name, geom AS _smtmp_ FROM " + tableName + " WHERE ST_Intersects(ST_SetSRID(ST_MakePoint(" + worldPos.X + ", " + worldPos.Y + "), 3857),geom)";
        //            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, conn);

        //            da.Fill(ds);

        //            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        //            {
        //                var rows = ds.Tables[0].Rows;
        //                var rowCount = rows.Count;

        //                for (int i = 0; i < rowCount; i++)
        //                {
        //                    dataRows.Add(rows[i]);

        //                    //Blago retardirani pokusaj pretvaranja
        //                    var featureDataRow = rows[i] as FeatureDataRow;
        //                    NetTopologySuite.IO.WKTReader reader = new NetTopologySuite.IO.WKTReader();
        //                    var wkt = rows[i][3].ToString(); 
        //                    Geometry geom = (Geometry)reader.Read(wkt);

        //                    featureDataRow.Geometry = geom;
        //                    fdt.AddRow(featureDataRow);
        //                }
        //            }
        //        }
        //    }

        //    if (dataRows.Count > 0)
        //    {
        //        SharpMap.Layers.VectorLayer laySelected = new SharpMap.Layers.VectorLayer("Selection");
        //        laySelected.DataSource = new GeometryProvider(fdt);
        //        laySelected.Style.Fill = new System.Drawing.SolidBrush(System.Drawing.Color.Yellow);
        //        mapBox1.Map.Layers.Add(laySelected);
        //        this.checkedListBox1.Items.Add(laySelected.ToString(), true);
        //        mapBox1.Refresh();

        //    }




        //}
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
            queryModeFirstObject = true;
            queryModeSecondObject = false;
        }

        private void btnSelectSecondObject_Click(object sender, EventArgs e)
        {
            queryModeFirstObject = false;
            queryModeSecondObject = true;
        }


        #endregion

        private void mapBox1_GeomDefined(IGeometry geometry)
        {
            string geomString = geometry.AsText();
            if (mode != (int)Modes.DrawPolygon)
                return;
            //remove selected layers
            if (selectedLayers != null)
                foreach (VectorLayer layer in selectedLayers)
                {
                    all_layers.Remove(layer.LayerName);
                    mapBox1.Map.Layers.Remove(layer);
                    checkedListBox1.Items.Remove(layer.LayerName);
                }

            selectedLayers = new List<VectorLayer>();

            FeatureDataTable dt = new FeatureDataTable();
            DataSet ds = new DataSet();

            NpgsqlConnection conn = new NpgsqlConnection("server=" + DbConfig.host + ";port=" + DbConfig.port + ";user=" + DbConfig.username + ";pwd=" + DbConfig.password + ";database=" + DbConfig.database + "");
            conn.Open();
            ds.Reset();



            foreach (var layer in mapBox1.Map.Layers)
            {
                if (!layer.Enabled)
                    continue;
                FeatureDataTable fdt = new FeatureDataTable();
                if (layer.GetType() == typeof(VectorLayer))
                {
                    var vectorLayer = layer as VectorLayer;
                    var tableName = vectorLayer.DataSource.GetFeature(1).Table.TableName;
                    string sql = "SELECT code, fclass, name, geom AS _smtmp_ FROM " + tableName + " WHERE ST_Intersects(ST_SetSRID(ST_GeomFromText('"+ geomString + "'), 3857),geom)";
                    //string sql = "SELECT code, fclass, name, geom AS _smtmp_ FROM " + tableName + " WHERE ST_DWithin(ST_SetSRID(ST_MakePoint(" + worldPos.X + ", " + worldPos.Y + "), 3857),geom,10)";
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
                        selectedLayers.Add(laySelected);
                }
            }
            //insert new layers
            foreach (VectorLayer lay in selectedLayers)
            {
                mapBox1.Map.Layers.Add(lay);
                this.checkedListBox1.Items.Add(lay.ToString(), true);
                this.all_layers.Add(lay.ToString(), lay);
                mapBox1.Refresh();
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            mode = (int)Modes.Selection;
            mapBox1.ActiveTool = SharpMap.Forms.MapBox.Tools.None;
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            mode = (int)Modes.Pan;
            mapBox1.ActiveTool = SharpMap.Forms.MapBox.Tools.Pan;
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            mode = (int)Modes.DrawPolygon;
            mapBox1.ActiveTool = SharpMap.Forms.MapBox.Tools.DrawPolygon;
        }

        private void button1_Click(object sender, EventArgs e) //labele
        {
            if (checkedListBox1.SelectedIndex >= 0)
            {
                var layerName = checkedListBox1.GetItemText(checkedListBox1.SelectedItem);
                if (mapBox1.Map.Layers.GetLayerByName(layerName).Enabled) // dal je vidljiv
                {
                    var layer = all_layers[layerName];
                    if (layer.GetType() == typeof(VectorLayer))
                    {
                        int selectedIndex = 0;
                        LabelForm lf = new LabelForm((VectorLayer)layer, selectedIndex);
                        lf.mainForm = this;
                        lf.ShowDialog();

                        mapBox1.Refresh();
                    }
                }
            }
        }
    }
}
