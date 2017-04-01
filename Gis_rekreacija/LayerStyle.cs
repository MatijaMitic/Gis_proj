using Npgsql;
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
    public partial class LayerStyle : Form
    {
      

        VectorLayer layer;
        string mode;
        Dictionary<string, SharpMap.Styles.IStyle> styles;
        public LayerStyle()
        {
            InitializeComponent();
        }

        public LayerStyle(VectorLayer layer)
        {
            InitializeComponent();
            this.layer = layer;
            var type = layer.DataSource.GetType();
            var feature = layer.DataSource.GetFeature(1);
            var row = feature.Geometry.GeometryType;
            styles = new Dictionary<string, SharpMap.Styles.IStyle>();
            GetDistinctValues(this.layer);
            if (row == "Point")
            {
                groupBoxPoint.Visible = true;
            }
            else if (row == "MultiPolygon") {

            }
        }

        private void GetDistinctValues(VectorLayer layer) {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            NpgsqlConnection conn = new NpgsqlConnection("server=" + DbConfig.host + ";port=" + DbConfig.port + ";user=" + DbConfig.username + ";pwd=" + DbConfig.password + ";database=" + DbConfig.database + "");
                conn.Open();
                string sql = " SELECT distinct fclass FROM "+layer.DataSource.GetFeature(1).Table.TableName +" limit 10";
                NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, conn);
                ds.Reset();
                da.Fill(ds);
                dt = ds.Tables[0];
            foreach(DataRow row in dt.Rows)
                style_listBox1.Items.Add(row.ItemArray[0]);
        }

    private void button1_Click(object sender, EventArgs e)
    {
            var oldStyle = this.layer.Style;
     
            layer.Theme=new SharpMap.Rendering.Thematics.UniqueValuesTheme<string>("fclass", styles, oldStyle);
    }

    private void CreatePointStyle()
    {
        //size color
    }

    private void CreateLineStyle()
    {
        //color width
    }

    private void CreatePolyStyle()
    {
        //fill outline
    }

        private void point_color_button_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
            Color color = colorDialog1.Color;
            SharpMap.Styles.VectorStyle pointStyle = new SharpMap.Styles.VectorStyle();
            pointStyle.PointColor = new System.Drawing.SolidBrush(color);
            pointStyle.PointSize = float.Parse(tbPointWidth.Text);

            AddToStyle(pointStyle);
        }

        private void groupBoxPolygon_Enter(object sender, EventArgs e)
        {

        }

        private void buttonPolyg_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
            Color color = colorDialog1.Color;
            SharpMap.Styles.VectorStyle pointStyle = new SharpMap.Styles.VectorStyle();
            pointStyle.Fill = new System.Drawing.SolidBrush(color);
            pointStyle.Outline = new Pen(new System.Drawing.SolidBrush(color), float.Parse(tbPolygonWidth.Text));
            AddToStyle(pointStyle);
        }

        private void buttonLineColor_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
            Color color = colorDialog1.Color;
            SharpMap.Styles.VectorStyle pointStyle = new SharpMap.Styles.VectorStyle();
            pointStyle.Line = new Pen(new System.Drawing.SolidBrush(color), float.Parse(tbLineWidth.Text));
            AddToStyle(pointStyle);
        }

        private void AddToStyle(SharpMap.Styles.VectorStyle pointStyle)
        {
            if (styles.ContainsKey(style_listBox1.SelectedItem.ToString()))
            {
                styles[style_listBox1.SelectedItem.ToString()] = pointStyle;
            }
            else
            {
                styles.Add(style_listBox1.SelectedItem.ToString(), pointStyle);
            }
        }
    }
}
