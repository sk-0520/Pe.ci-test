namespace ContentTypeTextNet.Pe.PeMain.UI
{
	partial class ClipboardForm
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
			if(disposing && (components != null)) {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ClipboardForm));
			this.panelMain = new System.Windows.Forms.ToolStripContainer();
			this.statusClipboard = new System.Windows.Forms.StatusStrip();
			this.toolClipboard = new System.Windows.Forms.ToolStrip();
			this.toolClipboard_itemSave = new System.Windows.Forms.ToolStripButton();
			this.toolClipboard_itemClear = new System.Windows.Forms.ToolStripButton();
			this.panelMain.BottomToolStripPanel.SuspendLayout();
			this.panelMain.TopToolStripPanel.SuspendLayout();
			this.panelMain.SuspendLayout();
			this.toolClipboard.SuspendLayout();
			this.SuspendLayout();
			// 
			// panelMain
			// 
			// 
			// panelMain.BottomToolStripPanel
			// 
			this.panelMain.BottomToolStripPanel.Controls.Add(this.statusClipboard);
			// 
			// panelMain.ContentPanel
			// 
			this.panelMain.ContentPanel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.panelMain.ContentPanel.Size = new System.Drawing.Size(310, 231);
			this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelMain.LeftToolStripPanelVisible = false;
			this.panelMain.Location = new System.Drawing.Point(0, 0);
			this.panelMain.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.panelMain.Name = "panelMain";
			this.panelMain.RightToolStripPanelVisible = false;
			this.panelMain.Size = new System.Drawing.Size(310, 278);
			this.panelMain.TabIndex = 0;
			this.panelMain.Text = "toolStripContainer1";
			// 
			// panelMain.TopToolStripPanel
			// 
			this.panelMain.TopToolStripPanel.Controls.Add(this.toolClipboard);
			// 
			// statusClipboard
			// 
			this.statusClipboard.Dock = System.Windows.Forms.DockStyle.None;
			this.statusClipboard.Location = new System.Drawing.Point(0, 0);
			this.statusClipboard.Name = "statusClipboard";
			this.statusClipboard.Size = new System.Drawing.Size(310, 22);
			this.statusClipboard.TabIndex = 0;
			// 
			// toolClipboard
			// 
			this.toolClipboard.Dock = System.Windows.Forms.DockStyle.None;
			this.toolClipboard.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolClipboard.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolClipboard_itemSave,
            this.toolClipboard_itemClear});
			this.toolClipboard.Location = new System.Drawing.Point(3, 0);
			this.toolClipboard.Name = "toolClipboard";
			this.toolClipboard.Size = new System.Drawing.Size(282, 25);
			this.toolClipboard.TabIndex = 0;
			// 
			// toolClipboard_itemSave
			// 
			this.toolClipboard_itemSave.Image = ((System.Drawing.Image)(resources.GetObject("toolClipboard_itemSave.Image")));
			this.toolClipboard_itemSave.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolClipboard_itemSave.Name = "toolClipboard_itemSave";
			this.toolClipboard_itemSave.Size = new System.Drawing.Size(124, 22);
			this.toolClipboard_itemSave.Text = "toolStripButton1";
			// 
			// toolClipboard_itemClear
			// 
			this.toolClipboard_itemClear.Image = ((System.Drawing.Image)(resources.GetObject("toolClipboard_itemClear.Image")));
			this.toolClipboard_itemClear.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolClipboard_itemClear.Name = "toolClipboard_itemClear";
			this.toolClipboard_itemClear.Size = new System.Drawing.Size(124, 22);
			this.toolClipboard_itemClear.Text = "toolStripButton1";
			// 
			// ClipboardForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(310, 278);
			this.Controls.Add(this.panelMain);
			this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.Name = "ClipboardForm";
			this.Text = ":window/clipboard";
			this.panelMain.BottomToolStripPanel.ResumeLayout(false);
			this.panelMain.BottomToolStripPanel.PerformLayout();
			this.panelMain.TopToolStripPanel.ResumeLayout(false);
			this.panelMain.TopToolStripPanel.PerformLayout();
			this.panelMain.ResumeLayout(false);
			this.panelMain.PerformLayout();
			this.toolClipboard.ResumeLayout(false);
			this.toolClipboard.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ToolStripContainer panelMain;
		private System.Windows.Forms.StatusStrip statusClipboard;
		private System.Windows.Forms.ToolStrip toolClipboard;
		private System.Windows.Forms.ToolStripButton toolClipboard_itemSave;
		private System.Windows.Forms.ToolStripButton toolClipboard_itemClear;
	}
}