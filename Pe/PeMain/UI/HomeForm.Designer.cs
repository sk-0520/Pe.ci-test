/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 10/16/2014
 * 時刻: 20:29
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
namespace PeMain.UI
{
	partial class HomeForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the form.
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
			this.commandClose = new System.Windows.Forms.Button();
			this.tabHome = new System.Windows.Forms.TabControl();
			this.tabHome_pageMain = new System.Windows.Forms.TabPage();
			this.panelHome = new System.Windows.Forms.TableLayoutPanel();
			this.tabHome.SuspendLayout();
			this.panelHome.SuspendLayout();
			this.SuspendLayout();
			// 
			// commandClose
			// 
			this.commandClose.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.commandClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.commandClose.Location = new System.Drawing.Point(442, 239);
			this.commandClose.Name = "commandClose";
			this.commandClose.Size = new System.Drawing.Size(99, 31);
			this.commandClose.TabIndex = 0;
			this.commandClose.Text = ":common/command/close";
			this.commandClose.UseVisualStyleBackColor = true;
			// 
			// tabHome
			// 
			this.tabHome.Controls.Add(this.tabHome_pageMain);
			this.tabHome.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabHome.HotTrack = true;
			this.tabHome.Location = new System.Drawing.Point(3, 3);
			this.tabHome.Multiline = true;
			this.tabHome.Name = "tabHome";
			this.tabHome.SelectedIndex = 0;
			this.tabHome.Size = new System.Drawing.Size(538, 230);
			this.tabHome.TabIndex = 1;
			// 
			// tabHome_pageMain
			// 
			this.tabHome_pageMain.Location = new System.Drawing.Point(4, 24);
			this.tabHome_pageMain.Name = "tabHome_pageMain";
			this.tabHome_pageMain.Padding = new System.Windows.Forms.Padding(3);
			this.tabHome_pageMain.Size = new System.Drawing.Size(530, 202);
			this.tabHome_pageMain.TabIndex = 0;
			this.tabHome_pageMain.Text = ":home/page/main";
			this.tabHome_pageMain.UseVisualStyleBackColor = true;
			// 
			// panelHome
			// 
			this.panelHome.ColumnCount = 1;
			this.panelHome.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.panelHome.Controls.Add(this.commandClose, 0, 1);
			this.panelHome.Controls.Add(this.tabHome, 0, 0);
			this.panelHome.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelHome.Location = new System.Drawing.Point(0, 0);
			this.panelHome.Name = "panelHome";
			this.panelHome.RowCount = 2;
			this.panelHome.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.panelHome.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.panelHome.Size = new System.Drawing.Size(544, 273);
			this.panelHome.TabIndex = 2;
			// 
			// HomeForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(544, 273);
			this.Controls.Add(this.panelHome);
			this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.Icon = global::PeMain.Properties.Images.App;
			this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.Name = "HomeForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = ":window/home";
			this.tabHome.ResumeLayout(false);
			this.panelHome.ResumeLayout(false);
			this.ResumeLayout(false);
		}
		private System.Windows.Forms.TableLayoutPanel panelHome;
		private System.Windows.Forms.TabPage tabHome_pageMain;
		private System.Windows.Forms.TabControl tabHome;
		private System.Windows.Forms.Button commandClose;
	}
}
