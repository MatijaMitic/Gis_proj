﻿namespace Gis_rekreacija
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
            this.pictureBox_point = new System.Windows.Forms.PictureBox();
            this.label_color_point = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbPointWidth = new System.Windows.Forms.TextBox();
            this.groupBoxPolygon = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.textBox_poly_width = new System.Windows.Forms.TextBox();
            this.pictureBox_polygon = new System.Windows.Forms.PictureBox();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBoxLine = new System.Windows.Forms.GroupBox();
            this.pictureBox_line = new System.Windows.Forms.PictureBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.tbLineWidth = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.button_point_save = new System.Windows.Forms.Button();
            this.button_poly_save = new System.Windows.Forms.Button();
            this.button_line_save = new System.Windows.Forms.Button();
            this.groupBoxPoint.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_point)).BeginInit();
            this.groupBoxPolygon.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_polygon)).BeginInit();
            this.groupBoxLine.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_line)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(301, 368);
            this.button1.Margin = new System.Windows.Forms.Padding(2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(134, 32);
            this.button1.TabIndex = 0;
            this.button1.Text = "Apply";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // style_listBox1
            // 
            this.style_listBox1.FormattingEnabled = true;
            this.style_listBox1.Location = new System.Drawing.Point(96, 71);
            this.style_listBox1.Name = "style_listBox1";
            this.style_listBox1.Size = new System.Drawing.Size(120, 329);
            this.style_listBox1.TabIndex = 1;
            this.style_listBox1.SelectedIndexChanged += new System.EventHandler(this.style_listBox1_SelectedIndexChanged);
            // 
            // point_color_button
            // 
            this.point_color_button.Location = new System.Drawing.Point(6, 71);
            this.point_color_button.Name = "point_color_button";
            this.point_color_button.Size = new System.Drawing.Size(75, 23);
            this.point_color_button.TabIndex = 2;
            this.point_color_button.Text = "New color";
            this.point_color_button.UseVisualStyleBackColor = true;
            this.point_color_button.Click += new System.EventHandler(this.point_color_button_Click);
            // 
            // buttonLineColor
            // 
            this.buttonLineColor.Location = new System.Drawing.Point(9, 73);
            this.buttonLineColor.Name = "buttonLineColor";
            this.buttonLineColor.Size = new System.Drawing.Size(75, 23);
            this.buttonLineColor.TabIndex = 3;
            this.buttonLineColor.Text = "New color";
            this.buttonLineColor.UseVisualStyleBackColor = true;
            this.buttonLineColor.Click += new System.EventHandler(this.buttonLineColor_Click);
            // 
            // buttonPolyg
            // 
            this.buttonPolyg.Location = new System.Drawing.Point(6, 71);
            this.buttonPolyg.Name = "buttonPolyg";
            this.buttonPolyg.Size = new System.Drawing.Size(75, 23);
            this.buttonPolyg.TabIndex = 4;
            this.buttonPolyg.Text = "New color";
            this.buttonPolyg.UseVisualStyleBackColor = true;
            this.buttonPolyg.Click += new System.EventHandler(this.buttonPolyg_Click);
            // 
            // groupBoxPoint
            // 
            this.groupBoxPoint.Controls.Add(this.button_point_save);
            this.groupBoxPoint.Controls.Add(this.pictureBox_point);
            this.groupBoxPoint.Controls.Add(this.label_color_point);
            this.groupBoxPoint.Controls.Add(this.label2);
            this.groupBoxPoint.Controls.Add(this.tbPointWidth);
            this.groupBoxPoint.Controls.Add(this.point_color_button);
            this.groupBoxPoint.Enabled = false;
            this.groupBoxPoint.Location = new System.Drawing.Point(235, 38);
            this.groupBoxPoint.Name = "groupBoxPoint";
            this.groupBoxPoint.Size = new System.Drawing.Size(200, 100);
            this.groupBoxPoint.TabIndex = 5;
            this.groupBoxPoint.TabStop = false;
            this.groupBoxPoint.Text = "Point";
            // 
            // pictureBox_point
            // 
            this.pictureBox_point.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.pictureBox_point.Location = new System.Drawing.Point(66, 55);
            this.pictureBox_point.Name = "pictureBox_point";
            this.pictureBox_point.Size = new System.Drawing.Size(22, 13);
            this.pictureBox_point.TabIndex = 12;
            this.pictureBox_point.TabStop = false;
            // 
            // label_color_point
            // 
            this.label_color_point.AutoSize = true;
            this.label_color_point.ForeColor = System.Drawing.Color.Black;
            this.label_color_point.Location = new System.Drawing.Point(6, 55);
            this.label_color_point.Name = "label_color_point";
            this.label_color_point.Size = new System.Drawing.Size(60, 13);
            this.label_color_point.TabIndex = 5;
            this.label_color_point.Text = "Point color:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Point width:";
            // 
            // tbPointWidth
            // 
            this.tbPointWidth.Location = new System.Drawing.Point(9, 32);
            this.tbPointWidth.Name = "tbPointWidth";
            this.tbPointWidth.Size = new System.Drawing.Size(100, 20);
            this.tbPointWidth.TabIndex = 3;
            // 
            // groupBoxPolygon
            // 
            this.groupBoxPolygon.Controls.Add(this.button_poly_save);
            this.groupBoxPolygon.Controls.Add(this.label8);
            this.groupBoxPolygon.Controls.Add(this.textBox_poly_width);
            this.groupBoxPolygon.Controls.Add(this.pictureBox_polygon);
            this.groupBoxPolygon.Controls.Add(this.label5);
            this.groupBoxPolygon.Controls.Add(this.buttonPolyg);
            this.groupBoxPolygon.Enabled = false;
            this.groupBoxPolygon.Location = new System.Drawing.Point(235, 144);
            this.groupBoxPolygon.Name = "groupBoxPolygon";
            this.groupBoxPolygon.Size = new System.Drawing.Size(200, 100);
            this.groupBoxPolygon.TabIndex = 6;
            this.groupBoxPolygon.TabStop = false;
            this.groupBoxPolygon.Text = "Polygon";
            this.groupBoxPolygon.Enter += new System.EventHandler(this.groupBoxPolygon_Enter);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 16);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(71, 13);
            this.label8.TabIndex = 15;
            this.label8.Text = "Outline width:";
            // 
            // textBox_poly_width
            // 
            this.textBox_poly_width.Location = new System.Drawing.Point(6, 32);
            this.textBox_poly_width.Name = "textBox_poly_width";
            this.textBox_poly_width.Size = new System.Drawing.Size(100, 20);
            this.textBox_poly_width.TabIndex = 14;
            // 
            // pictureBox_polygon
            // 
            this.pictureBox_polygon.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.pictureBox_polygon.Location = new System.Drawing.Point(66, 55);
            this.pictureBox_polygon.Name = "pictureBox_polygon";
            this.pictureBox_polygon.Size = new System.Drawing.Size(22, 13);
            this.pictureBox_polygon.TabIndex = 13;
            this.pictureBox_polygon.TabStop = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 55);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(48, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "Fill color:";
            // 
            // groupBoxLine
            // 
            this.groupBoxLine.Controls.Add(this.button_line_save);
            this.groupBoxLine.Controls.Add(this.pictureBox_line);
            this.groupBoxLine.Controls.Add(this.label7);
            this.groupBoxLine.Controls.Add(this.label6);
            this.groupBoxLine.Controls.Add(this.tbLineWidth);
            this.groupBoxLine.Controls.Add(this.buttonLineColor);
            this.groupBoxLine.Enabled = false;
            this.groupBoxLine.Location = new System.Drawing.Point(235, 260);
            this.groupBoxLine.Name = "groupBoxLine";
            this.groupBoxLine.Size = new System.Drawing.Size(200, 100);
            this.groupBoxLine.TabIndex = 7;
            this.groupBoxLine.TabStop = false;
            this.groupBoxLine.Text = "Line";
            // 
            // pictureBox_line
            // 
            this.pictureBox_line.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.pictureBox_line.Location = new System.Drawing.Point(66, 57);
            this.pictureBox_line.Name = "pictureBox_line";
            this.pictureBox_line.Size = new System.Drawing.Size(22, 13);
            this.pictureBox_line.TabIndex = 14;
            this.pictureBox_line.TabStop = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.Color.Black;
            this.label7.Location = new System.Drawing.Point(6, 57);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(56, 13);
            this.label7.TabIndex = 6;
            this.label7.Text = "Line color:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 16);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(58, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "Line width:";
            // 
            // tbLineWidth
            // 
            this.tbLineWidth.Location = new System.Drawing.Point(9, 32);
            this.tbLineWidth.Name = "tbLineWidth";
            this.tbLineWidth.Size = new System.Drawing.Size(100, 20);
            this.tbLineWidth.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Layer: SomeLayer";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(96, 38);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 21);
            this.comboBox1.TabIndex = 9;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 41);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(81, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Select attribute:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 71);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(69, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Select value:";
            // 
            // button_point_save
            // 
            this.button_point_save.Location = new System.Drawing.Point(119, 71);
            this.button_point_save.Name = "button_point_save";
            this.button_point_save.Size = new System.Drawing.Size(75, 23);
            this.button_point_save.TabIndex = 13;
            this.button_point_save.Text = "Save";
            this.button_point_save.UseVisualStyleBackColor = true;
            this.button_point_save.Click += new System.EventHandler(this.button_point_save_Click);
            // 
            // button_poly_save
            // 
            this.button_poly_save.Location = new System.Drawing.Point(119, 71);
            this.button_poly_save.Name = "button_poly_save";
            this.button_poly_save.Size = new System.Drawing.Size(75, 23);
            this.button_poly_save.TabIndex = 16;
            this.button_poly_save.Text = "Save";
            this.button_poly_save.UseVisualStyleBackColor = true;
            this.button_poly_save.Click += new System.EventHandler(this.button_poly_save_Click);
            // 
            // button_line_save
            // 
            this.button_line_save.Location = new System.Drawing.Point(119, 71);
            this.button_line_save.Name = "button_line_save";
            this.button_line_save.Size = new System.Drawing.Size(75, 23);
            this.button_line_save.TabIndex = 15;
            this.button_line_save.Text = "Save";
            this.button_line_save.UseVisualStyleBackColor = true;
            this.button_line_save.Click += new System.EventHandler(this.button_line_save_Click);
            // 
            // LayerStyle
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(455, 421);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label1);
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
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_point)).EndInit();
            this.groupBoxPolygon.ResumeLayout(false);
            this.groupBoxPolygon.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_polygon)).EndInit();
            this.groupBoxLine.ResumeLayout(false);
            this.groupBoxLine.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_line)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

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
        private System.Windows.Forms.TextBox tbLineWidth;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label_color_point;
        private System.Windows.Forms.PictureBox pictureBox_point;
        private System.Windows.Forms.PictureBox pictureBox_polygon;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.PictureBox pictureBox_line;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBox_poly_width;
        private System.Windows.Forms.Button button_point_save;
        private System.Windows.Forms.Button button_poly_save;
        private System.Windows.Forms.Button button_line_save;
    }
}