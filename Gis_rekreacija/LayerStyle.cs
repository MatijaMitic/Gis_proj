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
        string attribute_column;
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
            this.label1.Text = "Selected layer: "+layer.LayerName;

            DataRowCollection dtr = DataLayer.DataLayer.GetLayerColumns(layer);
            //fill combo box
            foreach(DataRow dr in dtr) {
                this.comboBox1.Items.Add(dr.ItemArray[3]);
            }

            var type = layer.DataSource.GetType();
            var feature = layer.DataSource.GetFeature(1);
            var row = feature.Geometry.GeometryType;
            styles = new Dictionary<string, SharpMap.Styles.IStyle>();
            if (row == "Point")
            {
                mode = "point";
            }
            else if (row == "MultiPolygon")
            {
                mode = "polygone";
            }
            else {//treba da se doda provera
                mode = "line";
            }
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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBox1.SelectedIndex != -1) {
                string attr_col = this.comboBox1.SelectedItem.ToString();
                attribute_column = attr_col;
                DataRowCollection dtRow = DataLayer.DataLayer.GetDistinctValues(this.layer, attr_col);
                this.style_listBox1.Items.Clear();
                foreach (DataRow dr in dtRow) {
                    this.style_listBox1.Items.Add(dr.ItemArray[0]);
                }
            }
        }

        private void style_listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBox1.SelectedIndex != -1) {
                //enable types...
                if (mode == "point")
                {
                    groupBoxPoint.Enabled = true;
                    colorPictureBox(pictureBox_point, layer.Style.PointColor);
                }
                else if (mode == "polygon")
                {
                    groupBoxPolygon.Enabled = true;
                }
                else {
                    groupBoxLine.Enabled = true;
                }
            }
        }
        private void colorPictureBox(PictureBox box, Brush b) {
            Bitmap bmp = new Bitmap(box.Width, box.Height);
            box.Image = bmp;
            Graphics graphics = Graphics.FromImage(box.Image);
            graphics.FillRectangle(b, new System.Drawing.Rectangle(0, 0, box.Width, box.Height));
        }
    }
}
