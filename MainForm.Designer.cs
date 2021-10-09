
namespace CalenTrack {
	partial class MainForm {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.sidePanel = new System.Windows.Forms.Panel();
			this.maxAppRows = new System.Windows.Forms.NumericUpDown();
			this.label1 = new System.Windows.Forms.Label();
			this.timeView = new System.Windows.Forms.ListView();
			this.toolTip = new System.Windows.Forms.ToolTip(this.components);
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
			this.mouseLabel = new System.Windows.Forms.ToolStripLabel();
			this.mainPanel = new System.Windows.Forms.Panel();
			this.tickTimer = new System.Windows.Forms.Timer(this.components);
			this.firstUpdate = new System.Windows.Forms.Timer(this.components);
			this.canvas = new System.Windows.Forms.PictureBox();
			this.btNew = new System.Windows.Forms.ToolStripButton();
			this.btEditConfig = new System.Windows.Forms.ToolStripButton();
			this.btReloadConfig = new System.Windows.Forms.ToolStripButton();
			this.btRefresh = new System.Windows.Forms.ToolStripButton();
			this.btAddMarker = new System.Windows.Forms.ToolStripButton();
			this.btEndBreak = new System.Windows.Forms.ToolStripButton();
			this.sidePanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.maxAppRows)).BeginInit();
			this.toolStrip1.SuspendLayout();
			this.mainPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.canvas)).BeginInit();
			this.SuspendLayout();
			// 
			// sidePanel
			// 
			this.sidePanel.Controls.Add(this.maxAppRows);
			this.sidePanel.Controls.Add(this.label1);
			this.sidePanel.Controls.Add(this.timeView);
			this.sidePanel.Dock = System.Windows.Forms.DockStyle.Right;
			this.sidePanel.Location = new System.Drawing.Point(560, 0);
			this.sidePanel.Name = "sidePanel";
			this.sidePanel.Size = new System.Drawing.Size(240, 450);
			this.sidePanel.TabIndex = 0;
			// 
			// maxAppRows
			// 
			this.maxAppRows.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.maxAppRows.Location = new System.Drawing.Point(178, 7);
			this.maxAppRows.Name = "maxAppRows";
			this.maxAppRows.Size = new System.Drawing.Size(50, 20);
			this.maxAppRows.TabIndex = 2;
			this.maxAppRows.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(6, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(33, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "AT: 0";
			// 
			// timeView
			// 
			this.timeView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.timeView.HideSelection = false;
			this.timeView.Location = new System.Drawing.Point(9, 32);
			this.timeView.Name = "timeView";
			this.timeView.Size = new System.Drawing.Size(219, 406);
			this.timeView.TabIndex = 0;
			this.timeView.UseCompatibleStateImageBehavior = false;
			this.timeView.View = System.Windows.Forms.View.Details;
			this.timeView.SelectedIndexChanged += new System.EventHandler(this.timeView_SelectedIndexChanged);
			// 
			// splitter1
			// 
			this.splitter1.BackColor = System.Drawing.SystemColors.ControlLight;
			this.splitter1.Dock = System.Windows.Forms.DockStyle.Right;
			this.splitter1.Location = new System.Drawing.Point(557, 0);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(3, 450);
			this.splitter1.TabIndex = 2;
			this.splitter1.TabStop = false;
			// 
			// toolStrip1
			// 
			this.toolStrip1.BackColor = System.Drawing.SystemColors.Control;
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btNew,
            this.toolStripSeparator1,
            this.btEditConfig,
            this.btReloadConfig,
            this.toolStripSeparator2,
            this.btRefresh,
            this.btEndBreak,
            this.toolStripSeparator3,
            this.btAddMarker,
            this.toolStripSeparator4,
            this.mouseLabel});
			this.toolStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
			this.toolStrip1.Location = new System.Drawing.Point(0, 0);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(557, 25);
			this.toolStrip1.TabIndex = 3;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
			// 
			// toolStripSeparator4
			// 
			this.toolStripSeparator4.Name = "toolStripSeparator4";
			this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
			// 
			// mouseLabel
			// 
			this.mouseLabel.Name = "mouseLabel";
			this.mouseLabel.Size = new System.Drawing.Size(14, 22);
			this.mouseLabel.Text = "(:";
			// 
			// mainPanel
			// 
			this.mainPanel.AutoScroll = true;
			this.mainPanel.Controls.Add(this.canvas);
			this.mainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mainPanel.Location = new System.Drawing.Point(0, 25);
			this.mainPanel.Name = "mainPanel";
			this.mainPanel.Size = new System.Drawing.Size(557, 425);
			this.mainPanel.TabIndex = 4;
			// 
			// tickTimer
			// 
			this.tickTimer.Tick += new System.EventHandler(this.tickTimer_Tick);
			// 
			// firstUpdate
			// 
			this.firstUpdate.Enabled = true;
			this.firstUpdate.Interval = 50;
			this.firstUpdate.Tick += new System.EventHandler(this.firstUpdate_Tick);
			// 
			// canvas
			// 
			this.canvas.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.canvas.Location = new System.Drawing.Point(9, 7);
			this.canvas.Name = "canvas";
			this.canvas.Size = new System.Drawing.Size(542, 126);
			this.canvas.TabIndex = 3;
			this.canvas.TabStop = false;
			this.canvas.MouseDown += new System.Windows.Forms.MouseEventHandler(this.canvas_MouseDown);
			this.canvas.MouseMove += new System.Windows.Forms.MouseEventHandler(this.canvas_MouseMove);
			this.canvas.MouseUp += new System.Windows.Forms.MouseEventHandler(this.canvas_MouseUp);
			// 
			// btNew
			// 
			this.btNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btNew.Image = global::CalenTrack.Properties.Resources.page;
			this.btNew.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btNew.Name = "btNew";
			this.btNew.Size = new System.Drawing.Size(23, 22);
			this.btNew.Text = "New session";
			this.btNew.Click += new System.EventHandler(this.btNew_Click);
			// 
			// btEditConfig
			// 
			this.btEditConfig.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btEditConfig.Image = global::CalenTrack.Properties.Resources.cog_edit;
			this.btEditConfig.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btEditConfig.Name = "btEditConfig";
			this.btEditConfig.Size = new System.Drawing.Size(23, 22);
			this.btEditConfig.Text = "Edit configuration";
			this.btEditConfig.Click += new System.EventHandler(this.btEditConfig_Click);
			// 
			// btReloadConfig
			// 
			this.btReloadConfig.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btReloadConfig.Image = global::CalenTrack.Properties.Resources.cog_go;
			this.btReloadConfig.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btReloadConfig.Name = "btReloadConfig";
			this.btReloadConfig.Size = new System.Drawing.Size(23, 22);
			this.btReloadConfig.Text = "Reload configuration";
			this.btReloadConfig.Click += new System.EventHandler(this.btReloadConfig_Click);
			// 
			// btRefresh
			// 
			this.btRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btRefresh.Image = global::CalenTrack.Properties.Resources.arrow_refresh;
			this.btRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btRefresh.Name = "btRefresh";
			this.btRefresh.Size = new System.Drawing.Size(23, 22);
			this.btRefresh.Text = "Refresh";
			this.btRefresh.Click += new System.EventHandler(this.btRefresh_Click);
			// 
			// btAddMarker
			// 
			this.btAddMarker.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btAddMarker.Image = global::CalenTrack.Properties.Resources.star;
			this.btAddMarker.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btAddMarker.Name = "btAddMarker";
			this.btAddMarker.Size = new System.Drawing.Size(23, 22);
			this.btAddMarker.Text = "Add marker";
			this.btAddMarker.Click += new System.EventHandler(this.btAddMarker_Click);
			// 
			// btEndBreak
			// 
			this.btEndBreak.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btEndBreak.Image = global::CalenTrack.Properties.Resources.accept;
			this.btEndBreak.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btEndBreak.Name = "btEndBreak";
			this.btEndBreak.Size = new System.Drawing.Size(23, 22);
			this.btEndBreak.Text = "toolStripButton1";
			this.btEndBreak.Click += new System.EventHandler(this.btEndBreak_Click);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(800, 450);
			this.Controls.Add(this.mainPanel);
			this.Controls.Add(this.toolStrip1);
			this.Controls.Add(this.splitter1);
			this.Controls.Add(this.sidePanel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "MainForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "CalenTrack";
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.sidePanel.ResumeLayout(false);
			this.sidePanel.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.maxAppRows)).EndInit();
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.mainPanel.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.canvas)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Panel sidePanel;
		private System.Windows.Forms.ToolTip toolTip;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.Panel mainPanel;
		private System.Windows.Forms.Timer tickTimer;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ToolStripButton btNew;
		internal System.Windows.Forms.PictureBox canvas;
		internal System.Windows.Forms.NumericUpDown maxAppRows;
		internal System.Windows.Forms.ListView timeView;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripButton btEditConfig;
		private System.Windows.Forms.ToolStripButton btReloadConfig;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripButton btRefresh;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
		private System.Windows.Forms.ToolStripButton btAddMarker;
		private System.Windows.Forms.Timer firstUpdate;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
		private System.Windows.Forms.ToolStripLabel mouseLabel;
		private System.Windows.Forms.ToolStripButton btEndBreak;
	}
}

