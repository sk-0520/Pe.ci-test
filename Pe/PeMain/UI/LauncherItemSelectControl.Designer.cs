/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/12/22
 * 時刻: 1:04
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
namespace PeMain.UI
{
	partial class LauncherItemSelectControl
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the control.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			this.listLauncherItems = new PeMain.UI.LauncherItemListBox();
			this.toolLauncherItems = new System.Windows.Forms.ToolStrip();
			this.toolLauncherItems_create = new System.Windows.Forms.ToolStripButton();
			this.toolLauncherItems_remove = new System.Windows.Forms.ToolStripButton();
			this.toolLauncherItems_editSeparator = new System.Windows.Forms.ToolStripSeparator();
			this.toolLauncherItems_filter = new System.Windows.Forms.ToolStripButton();
			this.toolLauncherItems_type = new System.Windows.Forms.ToolStripDropDownButton();
			this.toolLauncherItems_type_full = new System.Windows.Forms.ToolStripMenuItem();
			this.toolLauncherItems_type_name = new System.Windows.Forms.ToolStripMenuItem();
			this.toolLauncherItems_type_display = new System.Windows.Forms.ToolStripMenuItem();
			this.toolLauncherItems_type_tag = new System.Windows.Forms.ToolStripMenuItem();
			this.toolLauncherItems_input = new System.Windows.Forms.ToolStripTextBox();
			this.toolLauncherItems.SuspendLayout();
			this.SuspendLayout();
			// 
			// listLauncherItems
			// 
			this.listLauncherItems.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listLauncherItems.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.listLauncherItems.FormattingEnabled = true;
			this.listLauncherItems.IntegralHeight = false;
			this.listLauncherItems.ItemHeight = 12;
			this.listLauncherItems.Location = new System.Drawing.Point(0, 25);
			this.listLauncherItems.Name = "listLauncherItems";
			this.listLauncherItems.Size = new System.Drawing.Size(147, 125);
			this.listLauncherItems.TabIndex = 4;
			this.listLauncherItems.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.ListLauncherItems_DrawItem);
			this.listLauncherItems.SelectedIndexChanged += new System.EventHandler(this.ListLauncherItemsSelectedIndexChanged);
			// 
			// toolLauncherItems
			// 
			this.toolLauncherItems.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolLauncherItems.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
									this.toolLauncherItems_create,
									this.toolLauncherItems_remove,
									this.toolLauncherItems_editSeparator,
									this.toolLauncherItems_filter,
									this.toolLauncherItems_type,
									this.toolLauncherItems_input});
			this.toolLauncherItems.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
			this.toolLauncherItems.Location = new System.Drawing.Point(0, 0);
			this.toolLauncherItems.Name = "toolLauncherItems";
			this.toolLauncherItems.Size = new System.Drawing.Size(147, 25);
			this.toolLauncherItems.TabIndex = 3;
			this.toolLauncherItems.Text = "toolStrip1";
			// 
			// toolLauncherItems_create
			// 
			this.toolLauncherItems_create.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolLauncherItems_create.Image = global::PeMain.Properties.Images.Add;
			this.toolLauncherItems_create.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolLauncherItems_create.Name = "toolLauncherItems_create";
			this.toolLauncherItems_create.Size = new System.Drawing.Size(23, 20);
			this.toolLauncherItems_create.Text = "{LAUNCHER_ITEM_CREATE}";
			this.toolLauncherItems_create.Click += new System.EventHandler(this.ToolLauncherItems_createClick);
			// 
			// toolLauncherItems_remove
			// 
			this.toolLauncherItems_remove.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolLauncherItems_remove.Image = global::PeMain.Properties.Images.Remove;
			this.toolLauncherItems_remove.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolLauncherItems_remove.Name = "toolLauncherItems_remove";
			this.toolLauncherItems_remove.Size = new System.Drawing.Size(23, 20);
			this.toolLauncherItems_remove.Text = "{LAUNCHER_ITEM_REMOVE}";
			this.toolLauncherItems_remove.Click += new System.EventHandler(this.ToolLauncherItems_removeClick);
			// 
			// toolLauncherItems_editSeparator
			// 
			this.toolLauncherItems_editSeparator.Name = "toolLauncherItems_editSeparator";
			this.toolLauncherItems_editSeparator.Size = new System.Drawing.Size(6, 23);
			// 
			// toolLauncherItems_filter
			// 
			this.toolLauncherItems_filter.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolLauncherItems_filter.Image = global::PeMain.Properties.Images.Filter;
			this.toolLauncherItems_filter.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolLauncherItems_filter.Name = "toolLauncherItems_filter";
			this.toolLauncherItems_filter.Size = new System.Drawing.Size(23, 20);
			this.toolLauncherItems_filter.Text = "{LAUNCHER_FILTERING}";
			// 
			// toolLauncherItems_type
			// 
			this.toolLauncherItems_type.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolLauncherItems_type.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
									this.toolLauncherItems_type_full,
									this.toolLauncherItems_type_name,
									this.toolLauncherItems_type_display,
									this.toolLauncherItems_type_tag});
			this.toolLauncherItems_type.Image = global::PeMain.Properties.Images.NotImpl;
			this.toolLauncherItems_type.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolLauncherItems_type.Name = "toolLauncherItems_type";
			this.toolLauncherItems_type.Size = new System.Drawing.Size(29, 20);
			this.toolLauncherItems_type.Text = "{?}";
			// 
			// toolLauncherItems_type_full
			// 
			this.toolLauncherItems_type_full.Image = global::PeMain.Properties.Images.NotImpl;
			this.toolLauncherItems_type_full.Name = "toolLauncherItems_type_full";
			this.toolLauncherItems_type_full.Size = new System.Drawing.Size(262, 22);
			this.toolLauncherItems_type_full.Text = "{LAUNCHER_FILTER_FULL}";
			this.toolLauncherItems_type_full.Click += new System.EventHandler(this.ToolLauncherItems_type_Click);
			// 
			// toolLauncherItems_type_name
			// 
			this.toolLauncherItems_type_name.Image = global::PeMain.Properties.Images.NotImpl;
			this.toolLauncherItems_type_name.Name = "toolLauncherItems_type_name";
			this.toolLauncherItems_type_name.Size = new System.Drawing.Size(262, 22);
			this.toolLauncherItems_type_name.Text = "{LAUNCHER_FILTER_NAME}";
			this.toolLauncherItems_type_name.Click += new System.EventHandler(this.ToolLauncherItems_type_Click);
			// 
			// toolLauncherItems_type_display
			// 
			this.toolLauncherItems_type_display.Image = global::PeMain.Properties.Images.NotImpl;
			this.toolLauncherItems_type_display.Name = "toolLauncherItems_type_display";
			this.toolLauncherItems_type_display.Size = new System.Drawing.Size(262, 22);
			this.toolLauncherItems_type_display.Text = "{LAUNCHER_FILTER_DISPLAY}";
			this.toolLauncherItems_type_display.Click += new System.EventHandler(this.ToolLauncherItems_type_Click);
			// 
			// toolLauncherItems_type_tag
			// 
			this.toolLauncherItems_type_tag.Image = global::PeMain.Properties.Images.NotImpl;
			this.toolLauncherItems_type_tag.Name = "toolLauncherItems_type_tag";
			this.toolLauncherItems_type_tag.Size = new System.Drawing.Size(262, 22);
			this.toolLauncherItems_type_tag.Text = "{LAUNCHER_FILTER_TAG}";
			this.toolLauncherItems_type_tag.Click += new System.EventHandler(this.ToolLauncherItems_type_Click);
			// 
			// toolLauncherItems_input
			// 
			this.toolLauncherItems_input.Name = "toolLauncherItems_input";
			this.toolLauncherItems_input.Size = new System.Drawing.Size(20, 25);
			// 
			// LauncherItemSelectControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.listLauncherItems);
			this.Controls.Add(this.toolLauncherItems);
			this.Name = "LauncherItemSelectControl";
			this.Size = new System.Drawing.Size(147, 150);
			this.Load += new System.EventHandler(this.LauncherItemSelectControlLoad);
			this.Resize += new System.EventHandler(this.LauncherItemSelectControlResize);
			this.toolLauncherItems.ResumeLayout(false);
			this.toolLauncherItems.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();
		}
		private System.Windows.Forms.ToolStripTextBox toolLauncherItems_input;
		private System.Windows.Forms.ToolStripMenuItem toolLauncherItems_type_tag;
		private System.Windows.Forms.ToolStripMenuItem toolLauncherItems_type_display;
		private System.Windows.Forms.ToolStripMenuItem toolLauncherItems_type_name;
		private System.Windows.Forms.ToolStripMenuItem toolLauncherItems_type_full;
		private System.Windows.Forms.ToolStripDropDownButton toolLauncherItems_type;
		private System.Windows.Forms.ToolStripButton toolLauncherItems_filter;
		private System.Windows.Forms.ToolStripSeparator toolLauncherItems_editSeparator;
		private System.Windows.Forms.ToolStripButton toolLauncherItems_remove;
		private System.Windows.Forms.ToolStripButton toolLauncherItems_create;
		private System.Windows.Forms.ToolStrip toolLauncherItems;
		private PeMain.UI.LauncherItemListBox listLauncherItems;
	}
}
