using System.Windows.Forms;

namespace Gis_rekreacija
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.mapBox1 = new SharpMap.Forms.MapBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
            this.btnEditLayer = new System.Windows.Forms.Button();
            this.btnFeatureQuery = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnExecuteQuery = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lbSecondObjectInfo = new System.Windows.Forms.ListBox();
            this.lbFirstObjectInfo = new System.Windows.Forms.ListBox();
            this.btnSelectSecondObject = new System.Windows.Forms.Button();
            this.btnSelectFirstObject = new System.Windows.Forms.Button();
            this.cbToggleSecondObject = new System.Windows.Forms.CheckBox();
            this.cbToggleFirstObject = new System.Windows.Forms.CheckBox();
            this.cbSecondLayer = new System.Windows.Forms.ComboBox();
            this.cbOperation = new System.Windows.Forms.ComboBox();
            this.cbFirstLayer = new System.Windows.Forms.ComboBox();
            this.mapDigitizeGeometriesToolStrip1 = new SharpMap.Forms.ToolBar.MapDigitizeGeometriesToolStrip(this.components);
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.mapDigitizeGeometriesToolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // mapBox1
            // 
            this.mapBox1.ActiveTool = SharpMap.Forms.MapBox.Tools.None;
            this.mapBox1.Cursor = System.Windows.Forms.Cursors.Default;
            this.mapBox1.FineZoomFactor = 10D;
            this.mapBox1.Location = new System.Drawing.Point(146, 36);
            this.mapBox1.MapQueryMode = SharpMap.Forms.MapBox.MapQueryType.LayerByIndex;
            this.mapBox1.Name = "mapBox1";
            this.mapBox1.QueryGrowFactor = 5F;
            this.mapBox1.QueryLayerIndex = 0;
            this.mapBox1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))));
            this.mapBox1.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))));
            this.mapBox1.ShowProgressUpdate = false;
            this.mapBox1.Size = new System.Drawing.Size(1026, 381);
            this.mapBox1.TabIndex = 2;
            this.mapBox1.Text = "mapBox1";
            this.mapBox1.WheelZoomMagnitude = -2D;
            this.mapBox1.MouseMove += new SharpMap.Forms.MapBox.MouseEventHandler(this.mapBox1_MouseMove);
            this.mapBox1.MouseUp += new SharpMap.Forms.MapBox.MouseEventHandler(this.mapBox1_MouseUp);
            this.mapBox1.GeometryDefined += new SharpMap.Forms.MapBox.GeometryDefinedHandler(this.mapBox1_GeomDefined);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(602, 440);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "label1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(602, 459);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "label2";
            // 
            // checkedListBox1
            // 
            this.checkedListBox1.FormattingEnabled = true;
            this.checkedListBox1.Location = new System.Drawing.Point(10, 105);
            this.checkedListBox1.Name = "checkedListBox1";
            this.checkedListBox1.Size = new System.Drawing.Size(120, 319);
            this.checkedListBox1.TabIndex = 6;
            this.checkedListBox1.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.checkedListBox1_ItemCheck);
            this.checkedListBox1.DragDrop += new System.Windows.Forms.DragEventHandler(this.checkedListBox1_DragDrop);
            this.checkedListBox1.DragOver += new System.Windows.Forms.DragEventHandler(this.checkedListBox1_DragOver);
            this.checkedListBox1.DoubleClick += new System.EventHandler(this.checkedListBox1_DoubleClick);
            this.checkedListBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.checkedListBox1_MouseDown);
            // 
            // btnEditLayer
            // 
            this.btnEditLayer.Location = new System.Drawing.Point(10, 28);
            this.btnEditLayer.Name = "btnEditLayer";
            this.btnEditLayer.Size = new System.Drawing.Size(119, 29);
            this.btnEditLayer.TabIndex = 7;
            this.btnEditLayer.Text = "Edit layer";
            this.btnEditLayer.UseVisualStyleBackColor = true;
            this.btnEditLayer.Click += new System.EventHandler(this.btnEditLayer_Click);
            // 
            // btnFeatureQuery
            // 
            this.btnFeatureQuery.Location = new System.Drawing.Point(10, 62);
            this.btnFeatureQuery.Margin = new System.Windows.Forms.Padding(2);
            this.btnFeatureQuery.Name = "btnFeatureQuery";
            this.btnFeatureQuery.Size = new System.Drawing.Size(119, 33);
            this.btnFeatureQuery.TabIndex = 8;
            this.btnFeatureQuery.Text = "Feature query";
            this.btnFeatureQuery.UseVisualStyleBackColor = true;
            this.btnFeatureQuery.Click += new System.EventHandler(this.btnFeatureQuery_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnExecuteQuery);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.lbSecondObjectInfo);
            this.groupBox1.Controls.Add(this.lbFirstObjectInfo);
            this.groupBox1.Controls.Add(this.btnSelectSecondObject);
            this.groupBox1.Controls.Add(this.btnSelectFirstObject);
            this.groupBox1.Controls.Add(this.cbToggleSecondObject);
            this.groupBox1.Controls.Add(this.cbToggleFirstObject);
            this.groupBox1.Controls.Add(this.cbSecondLayer);
            this.groupBox1.Controls.Add(this.cbOperation);
            this.groupBox1.Controls.Add(this.cbFirstLayer);
            this.groupBox1.Location = new System.Drawing.Point(146, 422);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox1.Size = new System.Drawing.Size(382, 230);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Spatial Queries";
            // 
            // btnExecuteQuery
            // 
            this.btnExecuteQuery.Location = new System.Drawing.Point(160, 177);
            this.btnExecuteQuery.Margin = new System.Windows.Forms.Padding(2);
            this.btnExecuteQuery.Name = "btnExecuteQuery";
            this.btnExecuteQuery.Size = new System.Drawing.Size(79, 31);
            this.btnExecuteQuery.TabIndex = 12;
            this.btnExecuteQuery.Text = "Execute Query";
            this.btnExecuteQuery.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(262, 18);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(90, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "2nd Layer/Object";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(146, 18);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Operation";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(35, 18);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(86, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "1st Layer/Object";
            // 
            // lbSecondObjectInfo
            // 
            this.lbSecondObjectInfo.FormattingEnabled = true;
            this.lbSecondObjectInfo.Location = new System.Drawing.Point(266, 132);
            this.lbSecondObjectInfo.Margin = new System.Windows.Forms.Padding(2);
            this.lbSecondObjectInfo.Name = "lbSecondObjectInfo";
            this.lbSecondObjectInfo.Size = new System.Drawing.Size(91, 95);
            this.lbSecondObjectInfo.TabIndex = 8;
            // 
            // lbFirstObjectInfo
            // 
            this.lbFirstObjectInfo.FormattingEnabled = true;
            this.lbFirstObjectInfo.Location = new System.Drawing.Point(35, 132);
            this.lbFirstObjectInfo.Margin = new System.Windows.Forms.Padding(2);
            this.lbFirstObjectInfo.Name = "lbFirstObjectInfo";
            this.lbFirstObjectInfo.Size = new System.Drawing.Size(91, 95);
            this.lbFirstObjectInfo.TabIndex = 7;
            // 
            // btnSelectSecondObject
            // 
            this.btnSelectSecondObject.Location = new System.Drawing.Point(246, 83);
            this.btnSelectSecondObject.Margin = new System.Windows.Forms.Padding(2);
            this.btnSelectSecondObject.Name = "btnSelectSecondObject";
            this.btnSelectSecondObject.Size = new System.Drawing.Size(140, 45);
            this.btnSelectSecondObject.TabIndex = 6;
            this.btnSelectSecondObject.Text = "Click to enable Map select mode for second object";
            this.btnSelectSecondObject.UseVisualStyleBackColor = true;
            this.btnSelectSecondObject.Click += new System.EventHandler(this.btnSelectSecondObject_Click);
            // 
            // btnSelectFirstObject
            // 
            this.btnSelectFirstObject.Location = new System.Drawing.Point(4, 84);
            this.btnSelectFirstObject.Margin = new System.Windows.Forms.Padding(2);
            this.btnSelectFirstObject.Name = "btnSelectFirstObject";
            this.btnSelectFirstObject.Size = new System.Drawing.Size(146, 43);
            this.btnSelectFirstObject.TabIndex = 5;
            this.btnSelectFirstObject.Text = "Click to enable Map select mode for first object";
            this.btnSelectFirstObject.UseVisualStyleBackColor = true;
            this.btnSelectFirstObject.Click += new System.EventHandler(this.btnSelectFirstObject_Click);
            // 
            // cbToggleSecondObject
            // 
            this.cbToggleSecondObject.AutoSize = true;
            this.cbToggleSecondObject.Location = new System.Drawing.Point(265, 61);
            this.cbToggleSecondObject.Margin = new System.Windows.Forms.Padding(2);
            this.cbToggleSecondObject.Name = "cbToggleSecondObject";
            this.cbToggleSecondObject.Size = new System.Drawing.Size(126, 17);
            this.cbToggleSecondObject.TabIndex = 4;
            this.cbToggleSecondObject.Text = "Select second object";
            this.cbToggleSecondObject.UseVisualStyleBackColor = true;
            this.cbToggleSecondObject.CheckedChanged += new System.EventHandler(this.cbToggleSecondObject_CheckedChanged);
            // 
            // cbToggleFirstObject
            // 
            this.cbToggleFirstObject.AutoSize = true;
            this.cbToggleFirstObject.Location = new System.Drawing.Point(35, 62);
            this.cbToggleFirstObject.Margin = new System.Windows.Forms.Padding(2);
            this.cbToggleFirstObject.Name = "cbToggleFirstObject";
            this.cbToggleFirstObject.Size = new System.Drawing.Size(107, 17);
            this.cbToggleFirstObject.TabIndex = 3;
            this.cbToggleFirstObject.Text = "Select first object";
            this.cbToggleFirstObject.UseVisualStyleBackColor = true;
            this.cbToggleFirstObject.CheckedChanged += new System.EventHandler(this.cbToggleFirstObject_CheckedChanged);
            // 
            // cbSecondLayer
            // 
            this.cbSecondLayer.FormattingEnabled = true;
            this.cbSecondLayer.Location = new System.Drawing.Point(265, 37);
            this.cbSecondLayer.Margin = new System.Windows.Forms.Padding(2);
            this.cbSecondLayer.Name = "cbSecondLayer";
            this.cbSecondLayer.Size = new System.Drawing.Size(92, 21);
            this.cbSecondLayer.TabIndex = 2;
            // 
            // cbOperation
            // 
            this.cbOperation.FormattingEnabled = true;
            this.cbOperation.Location = new System.Drawing.Point(148, 37);
            this.cbOperation.Margin = new System.Windows.Forms.Padding(2);
            this.cbOperation.Name = "cbOperation";
            this.cbOperation.Size = new System.Drawing.Size(99, 21);
            this.cbOperation.TabIndex = 1;
            // 
            // cbFirstLayer
            // 
            this.cbFirstLayer.FormattingEnabled = true;
            this.cbFirstLayer.Location = new System.Drawing.Point(35, 37);
            this.cbFirstLayer.Margin = new System.Windows.Forms.Padding(2);
            this.cbFirstLayer.Name = "cbFirstLayer";
            this.cbFirstLayer.Size = new System.Drawing.Size(92, 21);
            this.cbFirstLayer.TabIndex = 0;
            // 
            // mapDigitizeGeometriesToolStrip1
            // 
            this.mapDigitizeGeometriesToolStrip1.Enabled = false;
            this.mapDigitizeGeometriesToolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripButton2,
            this.toolStripButton3});
            this.mapDigitizeGeometriesToolStrip1.Location = new System.Drawing.Point(0, 0);
            this.mapDigitizeGeometriesToolStrip1.MapControl = null;
            this.mapDigitizeGeometriesToolStrip1.Name = "mapDigitizeGeometriesToolStrip1";
            this.mapDigitizeGeometriesToolStrip1.Size = new System.Drawing.Size(1181, 25);
            this.mapDigitizeGeometriesToolStrip1.TabIndex = 10;
            this.mapDigitizeGeometriesToolStrip1.Text = "mapDigitizeGeometriesToolStrip1";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton1.Text = "toolStripButton1";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton2.Text = "toolStripButton2";
            this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // toolStripButton3
            // 
            this.toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton3.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton3.Image")));
            this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton3.Text = "toolStripButton3";
            this.toolStripButton3.Click += new System.EventHandler(this.toolStripButton3_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(10, 435);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(119, 45);
            this.button1.TabIndex = 11;
            this.button1.Text = "Label Settings";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1181, 663);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.mapDigitizeGeometriesToolStrip1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnFeatureQuery);
            this.Controls.Add(this.btnEditLayer);
            this.Controls.Add(this.checkedListBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.mapBox1);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Text = "Form1";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.mapDigitizeGeometriesToolStrip1.ResumeLayout(false);
            this.mapDigitizeGeometriesToolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private SharpMap.Forms.MapBox mapBox1;
        private Label label1;
        private Label label2;
        private CheckedListBox checkedListBox1;
        private Button btnEditLayer;
        private Button btnFeatureQuery;
        private GroupBox groupBox1;
        private Button btnExecuteQuery;
        private Label label5;
        private Label label4;
        private Label label3;
        private ListBox lbSecondObjectInfo;
        private ListBox lbFirstObjectInfo;
        private Button btnSelectSecondObject;
        private Button btnSelectFirstObject;
        private CheckBox cbToggleSecondObject;
        private CheckBox cbToggleFirstObject;
        private ComboBox cbSecondLayer;
        private ComboBox cbOperation;
        private ComboBox cbFirstLayer;
        private SharpMap.Forms.ToolBar.MapDigitizeGeometriesToolStrip mapDigitizeGeometriesToolStrip1;
        private ToolStripButton toolStripButton1;
        private ToolStripButton toolStripButton2;
        private ToolStripButton toolStripButton3;
        private Button button1;
    }
}

