/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/12/15
 * 時刻: 21:39
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
namespace PeMain.UI
{
	partial class SettingForm
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
			this.components = new System.ComponentModel.Container();
			this.tabSetting = new System.Windows.Forms.TabControl();
			this.pageMain = new System.Windows.Forms.TabPage();
			this.pageLauncher = new System.Windows.Forms.TabPage();
			this.pageCommand = new System.Windows.Forms.TabPage();
			this.checkCommandTopmost = new System.Windows.Forms.CheckBox();
			this.titleCommandHideTime = new System.Windows.Forms.Label();
			this.commandCommandFont = new System.Windows.Forms.Button();
			this.titleCommandFont = new System.Windows.Forms.Label();
			this.commandCancel = new System.Windows.Forms.Button();
			this.commandSubmit = new System.Windows.Forms.Button();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
			this.pageToolbar = new System.Windows.Forms.TabPage();
			this.pageDisplay = new System.Windows.Forms.TabPage();
			this.inputCommandHideTime = new System.Windows.Forms.NumericUpDown();
			this.tabSetting.SuspendLayout();
			this.pageCommand.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.flowLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.inputCommandHideTime)).BeginInit();
			this.SuspendLayout();
			// 
			// tabSetting
			// 
			this.tabSetting.Controls.Add(this.pageMain);
			this.tabSetting.Controls.Add(this.pageLauncher);
			this.tabSetting.Controls.Add(this.pageCommand);
			this.tabSetting.Controls.Add(this.pageToolbar);
			this.tabSetting.Controls.Add(this.pageDisplay);
			this.tabSetting.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabSetting.Location = new System.Drawing.Point(3, 4);
			this.tabSetting.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.tabSetting.Name = "tabSetting";
			this.tabSetting.SelectedIndex = 0;
			this.tabSetting.Size = new System.Drawing.Size(468, 233);
			this.tabSetting.TabIndex = 0;
			// 
			// pageMain
			// 
			this.pageMain.Location = new System.Drawing.Point(4, 24);
			this.pageMain.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.pageMain.Name = "pageMain";
			this.pageMain.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.pageMain.Size = new System.Drawing.Size(460, 205);
			this.pageMain.TabIndex = 0;
			this.pageMain.Text = "{Pe}";
			this.pageMain.UseVisualStyleBackColor = true;
			// 
			// pageLauncher
			// 
			this.pageLauncher.Location = new System.Drawing.Point(4, 24);
			this.pageLauncher.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.pageLauncher.Name = "pageLauncher";
			this.pageLauncher.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.pageLauncher.Size = new System.Drawing.Size(460, 205);
			this.pageLauncher.TabIndex = 1;
			this.pageLauncher.Text = "{LAUNCHER}";
			this.pageLauncher.UseVisualStyleBackColor = true;
			// 
			// pageCommand
			// 
			this.pageCommand.Controls.Add(this.inputCommandHideTime);
			this.pageCommand.Controls.Add(this.checkCommandTopmost);
			this.pageCommand.Controls.Add(this.titleCommandHideTime);
			this.pageCommand.Controls.Add(this.commandCommandFont);
			this.pageCommand.Controls.Add(this.titleCommandFont);
			this.pageCommand.Location = new System.Drawing.Point(4, 24);
			this.pageCommand.Name = "pageCommand";
			this.pageCommand.Padding = new System.Windows.Forms.Padding(3);
			this.pageCommand.Size = new System.Drawing.Size(460, 205);
			this.pageCommand.TabIndex = 2;
			this.pageCommand.Text = "{COMMAND}";
			this.pageCommand.UseVisualStyleBackColor = true;
			// 
			// checkCommandTopmost
			// 
			this.checkCommandTopmost.Location = new System.Drawing.Point(35, 125);
			this.checkCommandTopmost.Name = "checkCommandTopmost";
			this.checkCommandTopmost.Size = new System.Drawing.Size(104, 24);
			this.checkCommandTopmost.TabIndex = 4;
			this.checkCommandTopmost.Text = "{TOPMOST}";
			this.checkCommandTopmost.UseVisualStyleBackColor = true;
			// 
			// titleCommandHideTime
			// 
			this.titleCommandHideTime.Location = new System.Drawing.Point(35, 78);
			this.titleCommandHideTime.Name = "titleCommandHideTime";
			this.titleCommandHideTime.Size = new System.Drawing.Size(100, 23);
			this.titleCommandHideTime.TabIndex = 2;
			this.titleCommandHideTime.Text = "{HIDE TIME}";
			// 
			// commandCommandFont
			// 
			this.commandCommandFont.Location = new System.Drawing.Point(156, 23);
			this.commandCommandFont.Name = "commandCommandFont";
			this.commandCommandFont.Size = new System.Drawing.Size(171, 23);
			this.commandCommandFont.TabIndex = 1;
			this.commandCommandFont.Text = "{FAMILY} {PT} ...";
			this.commandCommandFont.UseVisualStyleBackColor = true;
			// 
			// titleCommandFont
			// 
			this.titleCommandFont.Location = new System.Drawing.Point(35, 28);
			this.titleCommandFont.Name = "titleCommandFont";
			this.titleCommandFont.Size = new System.Drawing.Size(100, 23);
			this.titleCommandFont.TabIndex = 0;
			this.titleCommandFont.Text = "{FONT}";
			// 
			// commandCancel
			// 
			this.commandCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.commandCancel.Location = new System.Drawing.Point(96, 4);
			this.commandCancel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.commandCancel.Name = "commandCancel";
			this.commandCancel.Size = new System.Drawing.Size(87, 29);
			this.commandCancel.TabIndex = 1;
			this.commandCancel.Text = "{CANCEL}";
			this.commandCancel.UseVisualStyleBackColor = true;
			// 
			// commandSubmit
			// 
			this.commandSubmit.Location = new System.Drawing.Point(3, 4);
			this.commandSubmit.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.commandSubmit.Name = "commandSubmit";
			this.commandSubmit.Size = new System.Drawing.Size(87, 29);
			this.commandSubmit.TabIndex = 0;
			this.commandSubmit.Text = "{OK}";
			this.commandSubmit.UseVisualStyleBackColor = true;
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 1;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.tabSetting, 0, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 2;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.Size = new System.Drawing.Size(474, 284);
			this.tableLayoutPanel1.TabIndex = 2;
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.AutoSize = true;
			this.flowLayoutPanel1.Controls.Add(this.commandSubmit);
			this.flowLayoutPanel1.Controls.Add(this.commandCancel);
			this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Right;
			this.flowLayoutPanel1.Location = new System.Drawing.Point(285, 244);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size(186, 37);
			this.flowLayoutPanel1.TabIndex = 3;
			// 
			// errorProvider
			// 
			this.errorProvider.ContainerControl = this;
			// 
			// pageToolbar
			// 
			this.pageToolbar.Location = new System.Drawing.Point(4, 24);
			this.pageToolbar.Name = "pageToolbar";
			this.pageToolbar.Size = new System.Drawing.Size(460, 205);
			this.pageToolbar.TabIndex = 3;
			this.pageToolbar.Text = "{TOOLBAR}";
			this.pageToolbar.UseVisualStyleBackColor = true;
			// 
			// pageDisplay
			// 
			this.pageDisplay.Location = new System.Drawing.Point(4, 24);
			this.pageDisplay.Name = "pageDisplay";
			this.pageDisplay.Size = new System.Drawing.Size(460, 205);
			this.pageDisplay.TabIndex = 5;
			this.pageDisplay.Text = "{DISPLAY}";
			this.pageDisplay.UseVisualStyleBackColor = true;
			// 
			// inputCommandHideTime
			// 
			this.inputCommandHideTime.Location = new System.Drawing.Point(156, 76);
			this.inputCommandHideTime.Name = "inputCommandHideTime";
			this.inputCommandHideTime.Size = new System.Drawing.Size(120, 23);
			this.inputCommandHideTime.TabIndex = 5;
			// 
			// SettingForm
			// 
			this.AcceptButton = this.commandSubmit;
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.commandCancel;
			this.ClientSize = new System.Drawing.Size(474, 284);
			this.Controls.Add(this.tableLayoutPanel1);
			this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.Name = "SettingForm";
			this.Text = "{SETTING}";
			this.tabSetting.ResumeLayout(false);
			this.pageCommand.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.flowLayoutPanel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.inputCommandHideTime)).EndInit();
			this.ResumeLayout(false);
		}
		private System.Windows.Forms.TabPage pageDisplay;
		private System.Windows.Forms.TabPage pageToolbar;
		private System.Windows.Forms.NumericUpDown inputCommandHideTime;
		private System.Windows.Forms.Label titleCommandHideTime;
		private System.Windows.Forms.CheckBox checkCommandTopmost;
		private System.Windows.Forms.Button commandCommandFont;
		private System.Windows.Forms.Label titleCommandFont;
		private System.Windows.Forms.ErrorProvider errorProvider;
		private System.Windows.Forms.TabPage pageCommand;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Button commandCancel;
		private System.Windows.Forms.Button commandSubmit;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
		private System.Windows.Forms.TabPage pageLauncher;
		private System.Windows.Forms.TabPage pageMain;
		private System.Windows.Forms.TabControl tabSetting;
		
	}
}
