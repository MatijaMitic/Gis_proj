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
    public partial class AddLayer : Form
    {
        public Form1 main_form;
        public AddLayer()
        {
            InitializeComponent();
            DataRowCollection dtr = DataLayer.DataLayer.GetTablesFromDB("srbija");
            foreach (DataRow dr in dtr) {
                this.listBox1.Items.Add(dr.ItemArray[0]);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.listBox1.SelectedIndex > -1 && this.textBox1.Text.ToString()!="") {
                main_form.AddLayer(this.textBox1.Text.ToString(), this.listBox1.SelectedItem.ToString());
            }
            this.DialogResult = DialogResult.Yes;
            this.Close();
        }
    }
}
