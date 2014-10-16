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
			this.panelHome = new System.Windows.Forms.TableLayoutPanel();
			this.panelCommand = new System.Windows.Forms.TableLayoutPanel();
			this.commandLauncher = new System.Windows.Forms.Button();
			this.commandNotify = new System.Windows.Forms.Button();
			this.commandStartup = new System.Windows.Forms.Button();
			this.labelLauncher = new System.Windows.Forms.Label();
			this.labelNotify = new System.Windows.Forms.Label();
			this.labelStartup = new System.Windows.Forms.Label();
			this.panelHome.SuspendLayout();
			this.panelCommand.SuspendLayout();
			this.SuspendLayout();
			// 
			// commandClose
			// 
			this.commandClose.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.commandClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.commandClose.Location = new System.Drawing.Point(542, 268);
			this.commandClose.Name = "commandClose";
			this.commandClose.Size = new System.Drawing.Size(99, 31);
			this.commandClose.TabIndex = 0;
			this.commandClose.Text = ":common/command/close";
			this.commandClose.UseVisualStyleBackColor = true;
			// 
			// panelHome
			// 
			this.panelHome.ColumnCount = 1;
			this.panelHome.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.panelHome.Controls.Add(this.commandClose, 0, 1);
			this.panelHome.Controls.Add(this.panelCommand, 0, 0);
			this.panelHome.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelHome.Location = new System.Drawing.Point(0, 0);
			this.panelHome.Name = "panelHome";
			this.panelHome.RowCount = 2;
			this.panelHome.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.panelHome.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.panelHome.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.panelHome.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.panelHome.Size = new System.Drawing.Size(644, 302);
			this.panelHome.TabIndex = 2;
			// 
			// panelCommand
			// 
			this.panelCommand.ColumnCount = 3;
			this.panelCommand.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
			this.panelCommand.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
			this.panelCommand.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
			this.panelCommand.Controls.Add(this.commandLauncher, 0, 0);
			this.panelCommand.Controls.Add(this.commandNotify, 1, 0);
			this.panelCommand.Controls.Add(this.commandStartup, 2, 0);
			this.panelCommand.Controls.Add(this.labelLauncher, 0, 1);
			this.panelCommand.Controls.Add(this.labelNotify, 1, 1);
			this.panelCommand.Controls.Add(this.labelStartup, 2, 1);
			this.panelCommand.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelCommand.Location = new System.Drawing.Point(3, 3);
			this.panelCommand.Name = "panelCommand";
			this.panelCommand.RowCount = 2;
			this.panelCommand.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 200F));
			this.panelCommand.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.panelCommand.Size = new System.Drawing.Size(638, 259);
			this.panelCommand.TabIndex = 1;
			// 
			// commandLauncher
			// 
			this.commandLauncher.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.commandLauncher.Image = global::PeMain.Properties.Images.Finder;
			this.commandLauncher.Location = new System.Drawing.Point(11, 5);
			this.commandLauncher.Name = "commandLauncher";
			this.commandLauncher.Size = new System.Drawing.Size(190, 190);
			this.commandLauncher.TabIndex = 0;
			this.commandLauncher.Text = ":home/command/launcher";
			this.commandLauncher.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
			this.commandLauncher.UseVisualStyleBackColor = true;
			// 
			// commandNotify
			// 
			this.commandNotify.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.commandNotify.Image = global::PeMain.Properties.Images.Flag;
			this.commandNotify.Location = new System.Drawing.Point(223, 5);
			this.commandNotify.Name = "commandNotify";
			this.commandNotify.Size = new System.Drawing.Size(190, 190);
			this.commandNotify.TabIndex = 0;
			this.commandNotify.Text = ":home/command/notify";
			this.commandNotify.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
			this.commandNotify.UseVisualStyleBackColor = true;
			this.commandNotify.Click += new System.EventHandler(this.CommandNotify_Click);
			// 
			// commandStartup
			// 
			this.commandStartup.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.commandStartup.Image = global::PeMain.Properties.Images.Windows;
			this.commandStartup.Location = new System.Drawing.Point(436, 5);
			this.commandStartup.Name = "commandStartup";
			this.commandStartup.Size = new System.Drawing.Size(190, 190);
			this.commandStartup.TabIndex = 0;
			this.commandStartup.Text = ":home/command/startup";
			this.commandStartup.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
			this.commandStartup.UseVisualStyleBackColor = true;
			this.commandStartup.Click += new System.EventHandler(this.CommandStartup_Click);
			// 
			// labelLauncher
			// 
			this.labelLauncher.Dock = System.Windows.Forms.DockStyle.Fill;
			this.labelLauncher.Location = new System.Drawing.Point(3, 200);
			this.labelLauncher.Name = "labelLauncher";
			this.labelLauncher.Padding = new System.Windows.Forms.Padding(5);
			this.labelLauncher.Size = new System.Drawing.Size(206, 59);
			this.labelLauncher.TabIndex = 1;
			this.labelLauncher.Text = ":home/label/launcher";
			// 
			// labelNotify
			// 
			this.labelNotify.Dock = System.Windows.Forms.DockStyle.Fill;
			this.labelNotify.Location = new System.Drawing.Point(215, 200);
			this.labelNotify.Name = "labelNotify";
			this.labelNotify.Padding = new System.Windows.Forms.Padding(5);
			this.labelNotify.Size = new System.Drawing.Size(206, 59);
			this.labelNotify.TabIndex = 1;
			this.labelNotify.Text = ":home/label/notify";
			// 
			// labelStartup
			// 
			this.labelStartup.Dock = System.Windows.Forms.DockStyle.Fill;
			this.labelStartup.Location = new System.Drawing.Point(427, 200);
			this.labelStartup.Name = "labelStartup";
			this.labelStartup.Padding = new System.Windows.Forms.Padding(5);
			this.labelStartup.Size = new System.Drawing.Size(208, 59);
			this.labelStartup.TabIndex = 1;
			this.labelStartup.Text = ":home/label/startup";
			// 
			// HomeForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(644, 302);
			this.ControlBox = false;
			this.Controls.Add(this.panelHome);
			this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = global::PeMain.Properties.Images.App;
			this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.Name = "HomeForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = ":window/home";
			this.panelHome.ResumeLayout(false);
			this.panelCommand.ResumeLayout(false);
			this.ResumeLayout(false);
		}
		private System.Windows.Forms.Label labelStartup;
		private System.Windows.Forms.Label labelNotify;
		private System.Windows.Forms.Label labelLauncher;
		private System.Windows.Forms.Button commandStartup;
		private System.Windows.Forms.Button commandNotify;
		private System.Windows.Forms.Button commandLauncher;
		private System.Windows.Forms.TableLayoutPanel panelCommand;
		private System.Windows.Forms.TableLayoutPanel panelHome;
		private System.Windows.Forms.Button commandClose;
	}
}
