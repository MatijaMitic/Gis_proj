using GeoAPI;
using GeoAPI.Geometries;
using Npgsql;
using SharpMap.Data;
using SharpMap.Data.Providers;
using SharpMap.Layers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gis_rekreacija
{
    public partial class LayerFeatureQuery : Form
    {
        private VectorLayer layer;

        public VectorLayer selectionLayer { get; set; }
        public LayerFeatureQuery(VectorLayer layer)
        {
            this.layer = layer;
            InitializeComponent();

            this.button1.Click += OperatorClick;
            this.button2.Click += OperatorClick;
            this.button3.Click += OperatorClick;
            this.button4.Click += OperatorClick;
            this.button5.Click += OperatorClick;
            this.button6.Click += OperatorClick;
           // this.button7.Click += OperatorClick;
            this.button8.Click += OperatorClick;
            this.button9.Click += OperatorClick;

            GetDistinctValues(layer);
        }

        private void btnSample_Click(object sender, EventArgs e)
        {
            lbValues.Items.Clear();
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            NpgsqlConnection conn = new NpgsqlConnection("server=" + DbConfig.host + ";port=" + DbConfig.port + ";user=" + DbConfig.username + ";pwd=" + DbConfig.password + ";database=" + DbConfig.database + "");
            conn.Open();

            string tableName = layer.DataSource.GetFeature(1).Table.TableName;

            string attribute = lbAttributes.SelectedItem.ToString();

            string sql = " SELECT distinct " + attribute + " FROM " + tableName;
            NpgsqlDataAdapter da1 = new NpgsqlDataAdapter(sql, conn);
            ds.Reset();
            da1.Fill(ds);
            dt = ds.Tables[0];
            foreach (DataRow row in dt.Rows)
            {
                lbValues.Items.Add(row.ItemArray[0]);
            }
            conn.Close();
        }

        private void OperatorClick(object sender, EventArgs e)
        {
            Button operatorButton = sender as Button;

            string expression = this.richTextBox1.Text;
            expression += " " + operatorButton.Text + " ";
            this.richTextBox1.Text = expression;
        }

        private void GetDistinctValues(VectorLayer layer)
        {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            NpgsqlConnection conn = new NpgsqlConnection("server=" + DbConfig.host + ";port=" + DbConfig.port + ";user=" + DbConfig.username + ";pwd=" + DbConfig.password + ";database=" + DbConfig.database + "");
            conn.Open();

            string tableName = layer.DataSource.GetFeature(1).Table.TableName; 
            string sqlColumns = "SELECT * FROM information_schema.columns WHERE table_schema = 'public' AND table_name = '" + tableName + "'";

            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sqlColumns, conn);
            ds.Reset();
            da.Fill(ds);
            dt = ds.Tables[0];
            foreach (DataRow row in dt.Rows)
            {
                lbAttributes.Items.Add(row.ItemArray[3]);
            }

            conn.Close();
        }

        private void btnExecute_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            NpgsqlConnection conn = new NpgsqlConnection("server=" + DbConfig.host + ";port=" + DbConfig.port + ";user=" + DbConfig.username + ";pwd=" + DbConfig.password + ";database=" + DbConfig.database + "");
            conn.Open();

            string tableName = layer.DataSource.GetFeature(1).Table.TableName;
            string expression = richTextBox1.Text;
            string sqlColumns = "SELECT geom, * FROM " + tableName + " WHERE " + expression;

            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sqlColumns, conn);
            ds.Reset();

            try
            {
                da.Fill(ds);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Malformed query, please try again.");
                this.richTextBox1.Text = "";
                return;
            }
            
            dt = ds.Tables[0];

            dataGridView1.DataSource = ds.Tables[0];

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
                byte[] b2 = NetTopologySuite.IO.WKBReader.HexToBytes(row[0].ToString());
                fDR.Geometry = SharpMap.Converters.WellKnownBinary.GeometryFromWKB.Parse(b2, geometryFactory);
                fdt.AddRow(fDR);
            }

            if (fdt.Rows.Count > 0)
            {
                VectorLayer laySelected = new SharpMap.Layers.VectorLayer("Selection");
                laySelected.DataSource = new GeometryProvider(fdt);
                laySelected.Style.Fill = new System.Drawing.SolidBrush(System.Drawing.Color.Yellow);
                this.selectionLayer = laySelected;
            }

            conn.Close();

        }

        private void btnShowOnMap_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void lbAttributes_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int selectedIndex = lbAttributes.SelectedIndex;

            if (selectedIndex > -1)
            {
                string selectedItemText = lbAttributes.SelectedItem.ToString();
                string expression = this.richTextBox1.Text;
                expression += selectedItemText;
                this.richTextBox1.Text = expression;  
            }
        }

        private void lbValues_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int selectedIndex = lbValues.SelectedIndex;

            if (selectedIndex > -1)
            {
                string selectedItemText = lbValues.SelectedItem.ToString();
                string expression = this.richTextBox1.Text;

                long number;
                bool succeeded = Int64.TryParse(selectedItemText, out number);
                if (succeeded)
                {
                    expression += selectedItemText;
                }
                else
                {
                    expression += "'" + selectedItemText + "'";
                }
                
                this.richTextBox1.Text = expression;
            }
        }

        private void btnClearQuery_Click(object sender, EventArgs e)
        {
            this.richTextBox1.Text = "";
        }
    }
}
