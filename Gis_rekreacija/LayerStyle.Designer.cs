namespace Gis_rekreacija
{
    partial class LayerStyle
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.style_listBox1 = new System.Windows.Forms.ListBox();
            this.point_color_button = new System.Windows.Forms.Button();
            this.buttonLineColor = new System.Windows.Forms.Button();
            this.buttonPolyg = new System.Windows.Forms.Button();
            this.groupBoxPoint = new System.Windows.Forms.GroupBox();
            this.groupBoxPolygon = new System.Windows.Forms.GroupBox();
            this.groupBoxLine = new System.Windows.Forms.GroupBox();
            this.tbPointWidth = new System.Windows.Forms.TextBox();
            this.tbPolygonWidth = new System.Windows.Forms.TextBox();
            this.tbLineWidth = new System.Windows.Forms.TextBox();
            this.groupBoxPoint.SuspendLayout();
            this.groupBoxPolygon.SuspendLayout();
            this.groupBoxLine.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(318, 356);
            this.button1.Margin = new System.Windows.Forms.Padding(2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(134, 32);
            this.button1.TabIndex = 0;
            this.button1.Text = "Save";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // style_listBox1
            // 
            this.style_listBox1.FormattingEnabled = true;
            this.style_listBox1.Location = new System.Drawing.Point(13, 13);
            this.style_listBox1.Name = "style_listBox1";
            this.style_listBox1.Size = new System.Drawing.Size(120, 329);
            this.style_listBox1.TabIndex = 1;
            // 
            // point_color_button
            // 
            this.point_color_button.Location = new System.Drawing.Point(119, 71);
            this.point_color_button.Name = "point_color_button";
            this.point_color_button.Size = new System.Drawing.Size(75, 23);
            this.point_color_button.TabIndex = 2;
            this.point_color_button.Text = "Color";
            this.point_color_button.UseVisualStyleBackColor = true;
            this.point_color_button.Click += new System.EventHandler(this.point_color_button_Click);
            // 
            // buttonLineColor
            // 
            this.buttonLineColor.Location = new System.Drawing.Point(119, 71);
            this.buttonLineColor.Name = "buttonLineColor";
            this.buttonLineColor.Size = new System.Drawing.Size(75, 23);
            this.buttonLineColor.TabIndex = 3;
            this.buttonLineColor.Text = "Color";
            this.buttonLineColor.UseVisualStyleBackColor = true;
            this.buttonLineColor.Click += new System.EventHandler(this.buttonLineColor_Click);
            // 
            // buttonPolyg
            // 
            this.buttonPolyg.Location = new System.Drawing.Point(119, 62);
            this.buttonPolyg.Name = "buttonPolyg";
            this.buttonPolyg.Size = new System.Drawing.Size(75, 23);
            this.buttonPolyg.TabIndex = 4;
            this.buttonPolyg.Text = "Color";
            this.buttonPolyg.UseVisualStyleBackColor = true;
            this.buttonPolyg.Click += new System.EventHandler(this.buttonPolyg_Click);
            // 
            // groupBoxPoint
            // 
            this.groupBoxPoint.Controls.Add(this.tbPointWidth);
            this.groupBoxPoint.Controls.Add(this.point_color_button);
            this.groupBoxPoint.Location = new System.Drawing.Point(168, 13);
            this.groupBoxPoint.Name = "groupBoxPoint";
            this.groupBoxPoint.Size = new System.Drawing.Size(200, 100);
            this.groupBoxPoint.TabIndex = 5;
            this.groupBoxPoint.TabStop = false;
            this.groupBoxPoint.Text = "Point";
            this.groupBoxPoint.Visible = false;
            // 
            // groupBoxPolygon
            // 
            this.groupBoxPolygon.Controls.Add(this.tbPolygonWidth);
            this.groupBoxPolygon.Controls.Add(this.buttonPolyg);
            this.groupBoxPolygon.Location = new System.Drawing.Point(168, 119);
            this.groupBoxPolygon.Name = "groupBoxPolygon";
            this.groupBoxPolygon.Size = new System.Drawing.Size(200, 100);
            this.groupBoxPolygon.TabIndex = 6;
            this.groupBoxPolygon.TabStop = false;
            this.groupBoxPolygon.Text = "Polygon";
            this.groupBoxPolygon.Enter += new System.EventHandler(this.groupBoxPolygon_Enter);
            // 
            // groupBoxLine
            // 
            this.groupBoxLine.Controls.Add(this.tbLineWidth);
            this.groupBoxLine.Controls.Add(this.buttonLineColor);
            this.groupBoxLine.Location = new System.Drawing.Point(168, 235);
            this.groupBoxLine.Name = "groupBoxLine";
            this.groupBoxLine.Size = new System.Drawing.Size(200, 100);
            this.groupBoxLine.TabIndex = 7;
            this.groupBoxLine.TabStop = false;
            this.groupBoxLine.Text = "Line";
            // 
            // tbPointWidth
            // 
            this.tbPointWidth.Location = new System.Drawing.Point(6, 19);
            this.tbPointWidth.Name = "tbPointWidth";
            this.tbPointWidth.Size = new System.Drawing.Size(100, 20);
            this.tbPointWidth.TabIndex = 3;
            // 
            // tbPolygonWidth
            // 
            this.tbPolygonWidth.Location = new System.Drawing.Point(7, 20);
            this.tbPolygonWidth.Name = "tbPolygonWidth";
            this.tbPolygonWidth.Size = new System.Drawing.Size(100, 20);
            this.tbPolygonWidth.TabIndex = 5;
            // 
            // tbLineWidth
            // 
            this.tbLineWidth.Location = new System.Drawing.Point(7, 30);
            this.tbLineWidth.Name = "tbLineWidth";
            this.tbLineWidth.Size = new System.Drawing.Size(100, 20);
            this.tbLineWidth.TabIndex = 4;
            // 
            // LayerStyle
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(463, 399);
            this.Controls.Add(this.groupBoxLine);
            this.Controls.Add(this.groupBoxPolygon);
            this.Controls.Add(this.groupBoxPoint);
            this.Controls.Add(this.style_listBox1);
            this.Controls.Add(this.button1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "LayerStyle";
            this.Text = "LayerStyle";
            this.groupBoxPoint.ResumeLayout(false);
            this.groupBoxPoint.PerformLayout();
            this.groupBoxPolygon.ResumeLayout(false);
            this.groupBoxPolygon.PerformLayout();
            this.groupBoxLine.ResumeLayout(false);
            this.groupBoxLine.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.ListBox style_listBox1;
        private System.Windows.Forms.Button point_color_button;
        private System.Windows.Forms.Button buttonLineColor;
        private System.Windows.Forms.Button buttonPolyg;
        private System.Windows.Forms.GroupBox groupBoxPoint;
        private System.Windows.Forms.GroupBox groupBoxPolygon;
        private System.Windows.Forms.GroupBox groupBoxLine;
        private System.Windows.Forms.TextBox tbPointWidth;
        private System.Windows.Forms.TextBox tbPolygonWidth;
        private System.Windows.Forms.TextBox tbLineWidth;
    }
}