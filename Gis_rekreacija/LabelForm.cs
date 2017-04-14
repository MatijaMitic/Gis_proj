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
    public partial class LabelForm : Form
    {
        private VectorLayer v_layer;
        private int selectedIndex;
        public Form1 mainForm;
        public LabelForm()
        {
            InitializeComponent();
        }
        public LabelForm(VectorLayer layer, int selInd, Form1 m)
        {
            InitializeComponent();
            this.selectedIndex = selInd;
            this.v_layer = layer;
            DataRowCollection dtRows;
            mainForm = m;
            dtRows = DataLayer.DataLayer.GetLayerColumns(layer);
            listBox1.Items.Add("None");//0
            foreach (DataRow row in dtRows)
            {
                listBox1.Items.Add(row.ItemArray[3]);
            }
            listBox1.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //if some column is selected, create labelLayer
            if (listBox1.SelectedIndex != 0)
            {
                //labele
                SharpMap.Layers.LabelLayer labelLayer = new SharpMap.Layers.LabelLayer(v_layer.LayerName + " Labels");
                labelLayer.DataSource = this.v_layer.DataSource;
                labelLayer.LabelColumn = listBox1.SelectedItem.ToString();
                labelLayer.Style.CollisionDetection = true;
                labelLayer.Style.CollisionBuffer = new SizeF(10, 10);
                mainForm.RemoveLabelLayer(labelLayer.LayerName);
                mainForm.AddLabelLayer(labelLayer);
            }
            else {
                mainForm.RemoveLabelLayer(v_layer.LayerName + " Labels");
            }
            this.Close();
        }
    }
}
