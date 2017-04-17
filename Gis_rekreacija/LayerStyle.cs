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
        public Form1 mainForm;
        Dictionary<string, SharpMap.Styles.IStyle> styles;
        public LayerStyle()
        {
            InitializeComponent();
        }
        Brush temp_brush;

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
                mode = "polygon";
            }
            else {//treba da se doda provera
                mode = "line";
            }
        }

    private void button1_Click(object sender, EventArgs e)
    {
            var oldStyle = this.layer.Style;
     
            layer.Theme=new SharpMap.Rendering.Thematics.UniqueValuesTheme<string>(attribute_column, styles, oldStyle);
            mainForm.AddStyles(this.layer, this.styles);
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
            Brush b = new SolidBrush(color);
            temp_brush = b;
            colorPictureBox(pictureBox_point, b);
        }

        private void groupBoxPolygon_Enter(object sender, EventArgs e)
        {

        }

        private void buttonPolyg_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
            Color color = colorDialog1.Color;
            Brush b = new SolidBrush(color);
            temp_brush = b;
            colorPictureBox(pictureBox_polygon, b);
        }

        private void buttonLineColor_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
            Color color = colorDialog1.Color;
            Brush b = new SolidBrush(color);
            colorPictureBox(pictureBox_line, b);
            temp_brush = b;
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

        private SharpMap.Styles.VectorStyle GetStyle(string value) {
            if (styles.ContainsKey(style_listBox1.SelectedItem.ToString()))
            {
                return (SharpMap.Styles.VectorStyle)styles[value];
            }
            else {
                return null;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBox1.SelectedIndex != -1) {
                string attr_col = this.comboBox1.SelectedItem.ToString();
                attribute_column = attr_col;
                DataRowCollection dtRow = DataLayer.DataLayer.GetDistinctValues(this.layer, attr_col);
                this.style_listBox1.Items.Clear();
                styles = new Dictionary<string, SharpMap.Styles.IStyle>();
                foreach (DataRow dr in dtRow) {
                    this.style_listBox1.Items.Add(dr.ItemArray[0]);
                }
                //chech styles
                this.styles = mainForm.GetStyles(this.layer.LayerName);
                if (styles == null) {
                    styles = new Dictionary<string, SharpMap.Styles.IStyle>();
                }
            }
        }

        private void style_listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBox1.SelectedIndex != -1) {
                this.label1.Text = "Selected layer: " + layer.LayerName+" for attribute "+ this.comboBox1.SelectedItem.ToString();
                //enable types...
                SharpMap.Styles.VectorStyle style = GetStyle(style_listBox1.SelectedItem.ToString());
                if (mode == "point")
                {
                    groupBoxPoint.Enabled = true;
                    if (style == null)
                    {
                        colorPictureBox(pictureBox_point, layer.Style.PointColor);
                        tbPointWidth.Text = layer.Style.PointSize.ToString();
                        temp_brush = layer.Style.PointColor;
                    }
                    else {
                        colorPictureBox(pictureBox_point, style.PointColor);
                        tbPointWidth.Text = style.PointSize.ToString();
                        temp_brush = style.PointColor;
                    }
                }
                else if (mode == "polygon")
                {
                    groupBoxPolygon.Enabled = true;
                    if (style == null)
                    {
                        colorPictureBox(pictureBox_polygon, layer.Style.Fill);
                        textBox_poly_width.Text = layer.Style.Outline.Width.ToString();
                        temp_brush = layer.Style.Fill;
                    }
                    else {
                        colorPictureBox(pictureBox_polygon, style.Fill);
                        textBox_poly_width.Text = style.Outline.Width.ToString();
                        temp_brush = style.Fill;
                    }
                }
                else {
                    groupBoxLine.Enabled = true;
                    if (style == null)
                    {
                        colorPictureBox(pictureBox_line, new SolidBrush(layer.Style.Line.Color));
                        tbLineWidth.Text = layer.Style.Line.Width.ToString();
                        temp_brush = new SolidBrush(layer.Style.Line.Color);
                    }
                    else {
                        colorPictureBox(pictureBox_line, new SolidBrush(style.Line.Color));
                        tbLineWidth.Text = style.Line.Width.ToString();
                        temp_brush = new SolidBrush(style.Line.Color);
                    }
                }
            }
        }
        private void colorPictureBox(PictureBox box, Brush b) {
            Bitmap bmp = new Bitmap(box.Width, box.Height);
            box.Image = bmp;
            Graphics graphics = Graphics.FromImage(box.Image);
            graphics.FillRectangle(b, new System.Drawing.Rectangle(0, 0, box.Width, box.Height));
        }

        private void button_point_save_Click(object sender, EventArgs e)
        {
            SharpMap.Styles.VectorStyle pointStyle = new SharpMap.Styles.VectorStyle();
            pointStyle.PointColor = temp_brush;
            pointStyle.PointSize = float.Parse(tbPointWidth.Text);

            AddToStyle(pointStyle);
        }

        private void button_poly_save_Click(object sender, EventArgs e)
        {
            SharpMap.Styles.VectorStyle polyStyle = new SharpMap.Styles.VectorStyle();
            polyStyle.Fill = temp_brush;
            polyStyle.Outline = new Pen(temp_brush, float.Parse(textBox_poly_width.Text));
            AddToStyle(polyStyle);
        }

        private void button_line_save_Click(object sender, EventArgs e)
        {
            SharpMap.Styles.VectorStyle lineStyle = new SharpMap.Styles.VectorStyle();
            lineStyle.Line = new Pen(temp_brush, float.Parse(tbLineWidth.Text));
            AddToStyle(lineStyle);
        }
    }
}
