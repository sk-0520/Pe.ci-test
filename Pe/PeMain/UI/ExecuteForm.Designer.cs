/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/01/14
 * 時刻: 23:11
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
namespace PeMain.UI
{
	partial class ExecuteForm
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
			this.viewCommand = new System.Windows.Forms.TextBox();
			this.inputOption = new System.Windows.Forms.ComboBox();
			this.inputWorkDirPath = new System.Windows.Forms.ComboBox();
			this.tabExecute = new System.Windows.Forms.TabControl();
			this.pageBasic = new System.Windows.Forms.TabPage();
			this.selectStdStream = new System.Windows.Forms.CheckBox();
			this.commandWorkDirPath = new System.Windows.Forms.Button();
			this.commandOption_dir = new System.Windows.Forms.Button();
			this.commandOption_file = new System.Windows.Forms.Button();
			this.labelWorkDirPath = new System.Windows.Forms.Label();
			this.labelOption = new System.Windows.Forms.Label();
			this.pageEnv = new System.Windows.Forms.TabPage();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.selectUserDefault = new System.Windows.Forms.CheckBox();
			this.groupUpdate = new System.Windows.Forms.GroupBox();
			this.envUpdate = new PeMain.UI.EnvUpdateControl();
			this.groupRemove = new System.Windows.Forms.GroupBox();
			this.envRemove = new PeMain.UI.EnvRemoveControl();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.commandCancel = new System.Windows.Forms.Button();
			this.commandSubmit = new System.Windows.Forms.Button();
			this.tabExecute.SuspendLayout();
			this.pageBasic.SuspendLayout();
			this.pageEnv.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.groupUpdate.SuspendLayout();
			this.groupRemove.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.flowLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// viewCommand
			// 
			this.viewCommand.Location = new System.Drawing.Point(6, 8);
			this.viewCommand.Name = "viewCommand";
			this.viewCommand.ReadOnly = true;
			this.viewCommand.Size = new System.Drawing.Size(395, 23);
			this.viewCommand.TabIndex = 0;
			// 
			// inputOption
			// 
			this.inputOption.FormattingEnabled = true;
			this.inputOption.Location = new System.Drawing.Point(117, 42);
			this.inputOption.Name = "inputOption";
			this.inputOption.Size = new System.Drawing.Size(260, 23);
			this.inputOption.TabIndex = 1;
			// 
			// inputWorkDirPath
			// 
			this.inputWorkDirPath.FormattingEnabled = true;
			this.inputWorkDirPath.Location = new System.Drawing.Point(117, 71);
			this.inputWorkDirPath.Name = "inputWorkDirPath";
			this.inputWorkDirPath.Size = new System.Drawing.Size(260, 23);
			this.inputWorkDirPath.TabIndex = 2;
			// 
			// tabExecute
			// 
			this.tabExecute.Alignment = System.Windows.Forms.TabAlignment.Left;
			this.tabExecute.Controls.Add(this.pageBasic);
			this.tabExecute.Controls.Add(this.pageEnv);
			this.tabExecute.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabExecute.Location = new System.Drawing.Point(0, 0);
			this.tabExecute.Multiline = true;
			this.tabExecute.Name = "tabExecute";
			this.tabExecute.SelectedIndex = 0;
			this.tabExecute.Size = new System.Drawing.Size(561, 183);
			this.tabExecute.TabIndex = 3;
			// 
			// pageBasic
			// 
			this.pageBasic.Controls.Add(this.selectStdStream);
			this.pageBasic.Controls.Add(this.commandWorkDirPath);
			this.pageBasic.Controls.Add(this.commandOption_dir);
			this.pageBasic.Controls.Add(this.commandOption_file);
			this.pageBasic.Controls.Add(this.labelWorkDirPath);
			this.pageBasic.Controls.Add(this.labelOption);
			this.pageBasic.Controls.Add(this.viewCommand);
			this.pageBasic.Controls.Add(this.inputOption);
			this.pageBasic.Controls.Add(this.inputWorkDirPath);
			this.pageBasic.Location = new System.Drawing.Point(25, 4);
			this.pageBasic.Name = "pageBasic";
			this.pageBasic.Size = new System.Drawing.Size(532, 175);
			this.pageBasic.TabIndex = 0;
			this.pageBasic.Text = "{BASIC}";
			this.pageBasic.UseVisualStyleBackColor = true;
			// 
			// selectStdStream
			// 
			this.selectStdStream.Location = new System.Drawing.Point(11, 110);
			this.selectStdStream.Name = "selectStdStream";
			this.selectStdStream.Size = new System.Drawing.Size(128, 24);
			this.selectStdStream.TabIndex = 8;
			this.selectStdStream.Text = "{STD_STREAM}";
			this.selectStdStream.UseVisualStyleBackColor = true;
			// 
			// commandWorkDirPath
			// 
			this.commandWorkDirPath.Image = global::PeMain.Properties.Images.Dir;
			this.commandWorkDirPath.Location = new System.Drawing.Point(383, 69);
			this.commandWorkDirPath.Name = "commandWorkDirPath";
			this.commandWorkDirPath.Size = new System.Drawing.Size(30, 26);
			this.commandWorkDirPath.TabIndex = 7;
			this.commandWorkDirPath.UseVisualStyleBackColor = true;
			this.commandWorkDirPath.Click += new System.EventHandler(this.CommandWorkDirPath_Click);
			// 
			// commandOption_dir
			// 
			this.commandOption_dir.Image = global::PeMain.Properties.Images.Dir;
			this.commandOption_dir.Location = new System.Drawing.Point(419, 41);
			this.commandOption_dir.Name = "commandOption_dir";
			this.commandOption_dir.Size = new System.Drawing.Size(30, 26);
			this.commandOption_dir.TabIndex = 6;
			this.commandOption_dir.UseVisualStyleBackColor = true;
			this.commandOption_dir.Click += new System.EventHandler(this.CommandOption_dir_Click);
			// 
			// commandOption_file
			// 
			this.commandOption_file.Image = global::PeMain.Properties.Images.File;
			this.commandOption_file.Location = new System.Drawing.Point(383, 41);
			this.commandOption_file.Name = "commandOption_file";
			this.commandOption_file.Size = new System.Drawing.Size(30, 26);
			this.commandOption_file.TabIndex = 5;
			this.commandOption_file.UseVisualStyleBackColor = true;
			this.commandOption_file.Click += new System.EventHandler(this.CommandOption_file_Click);
			// 
			// labelWorkDirPath
			// 
			this.labelWorkDirPath.Location = new System.Drawing.Point(11, 71);
			this.labelWorkDirPath.Name = "labelWorkDirPath";
			this.labelWorkDirPath.Size = new System.Drawing.Size(100, 23);
			this.labelWorkDirPath.TabIndex = 4;
			this.labelWorkDirPath.Text = "{WORKDIR}";
			// 
			// labelOption
			// 
			this.labelOption.Location = new System.Drawing.Point(11, 42);
			this.labelOption.Name = "labelOption";
			this.labelOption.Size = new System.Drawing.Size(100, 23);
			this.labelOption.TabIndex = 3;
			this.labelOption.Text = "{OPTION}";
			// 
			// pageEnv
			// 
			this.pageEnv.Controls.Add(this.tableLayoutPanel1);
			this.pageEnv.Location = new System.Drawing.Point(22, 4);
			this.pageEnv.Name = "pageEnv";
			this.pageEnv.Size = new System.Drawing.Size(535, 175);
			this.pageEnv.TabIndex = 1;
			this.pageEnv.Text = "{ENV}";
			this.pageEnv.UseVisualStyleBackColor = true;
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 65F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35F));
			this.tableLayoutPanel1.Controls.Add(this.selectUserDefault, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.groupUpdate, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.groupRemove, 1, 1);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 2;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(535, 175);
			this.tableLayoutPanel1.TabIndex = 1;
			// 
			// selectUserDefault
			// 
			this.tableLayoutPanel1.SetColumnSpan(this.selectUserDefault, 2);
			this.selectUserDefault.Dock = System.Windows.Forms.DockStyle.Fill;
			this.selectUserDefault.Location = new System.Drawing.Point(3, 3);
			this.selectUserDefault.Name = "selectUserDefault";
			this.selectUserDefault.Size = new System.Drawing.Size(529, 24);
			this.selectUserDefault.TabIndex = 0;
			this.selectUserDefault.Text = "{USER_DEFAULT}";
			this.selectUserDefault.UseVisualStyleBackColor = true;
			this.selectUserDefault.CheckedChanged += new System.EventHandler(this.SelectUserDefault_CheckedChanged);
			// 
			// groupUpdate
			// 
			this.groupUpdate.Controls.Add(this.envUpdate);
			this.groupUpdate.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupUpdate.Location = new System.Drawing.Point(3, 33);
			this.groupUpdate.Name = "groupUpdate";
			this.groupUpdate.Size = new System.Drawing.Size(341, 139);
			this.groupUpdate.TabIndex = 1;
			this.groupUpdate.TabStop = false;
			this.groupUpdate.Text = "{INSERT/UPDATE}";
			// 
			// envUpdate
			// 
			this.envUpdate.Dock = System.Windows.Forms.DockStyle.Fill;
			this.envUpdate.Location = new System.Drawing.Point(3, 19);
			this.envUpdate.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.envUpdate.Name = "envUpdate";
			this.envUpdate.Size = new System.Drawing.Size(335, 117);
			this.envUpdate.TabIndex = 0;
			// 
			// groupRemove
			// 
			this.groupRemove.Controls.Add(this.envRemove);
			this.groupRemove.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupRemove.Location = new System.Drawing.Point(350, 33);
			this.groupRemove.Name = "groupRemove";
			this.groupRemove.Size = new System.Drawing.Size(182, 139);
			this.groupRemove.TabIndex = 2;
			this.groupRemove.TabStop = false;
			this.groupRemove.Text = "{REMOVE}";
			// 
			// envRemove
			// 
			this.envRemove.Dock = System.Windows.Forms.DockStyle.Fill;
			this.envRemove.Location = new System.Drawing.Point(3, 19);
			this.envRemove.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.envRemove.Name = "envRemove";
			this.envRemove.Size = new System.Drawing.Size(176, 117);
			this.envRemove.TabIndex = 0;
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
			this.splitContainer1.IsSplitterFixed = true;
			this.splitContainer1.Location = new System.Drawing.Point(0, 0);
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.tabExecute);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.flowLayoutPanel1);
			this.splitContainer1.Size = new System.Drawing.Size(561, 224);
			this.splitContainer1.SplitterDistance = 183;
			this.splitContainer1.TabIndex = 4;
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.Controls.Add(this.commandCancel);
			this.flowLayoutPanel1.Controls.Add(this.commandSubmit);
			this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
			this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size(561, 37);
			this.flowLayoutPanel1.TabIndex = 0;
			// 
			// commandCancel
			// 
			this.commandCancel.AutoSize = true;
			this.commandCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.commandCancel.Location = new System.Drawing.Point(480, 3);
			this.commandCancel.Name = "commandCancel";
			this.commandCancel.Size = new System.Drawing.Size(78, 29);
			this.commandCancel.TabIndex = 4;
			this.commandCancel.Text = "{CANCEL}";
			this.commandCancel.UseVisualStyleBackColor = true;
			// 
			// commandSubmit
			// 
			this.commandSubmit.AutoSize = true;
			this.commandSubmit.Location = new System.Drawing.Point(399, 3);
			this.commandSubmit.Name = "commandSubmit";
			this.commandSubmit.Size = new System.Drawing.Size(75, 29);
			this.commandSubmit.TabIndex = 0;
			this.commandSubmit.Text = "{OK}";
			this.commandSubmit.UseVisualStyleBackColor = true;
			this.commandSubmit.Click += new System.EventHandler(this.CommandSubmit_Click);
			// 
			// ExecuteForm
			// 
			this.AcceptButton = this.commandSubmit;
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.commandCancel;
			this.ClientSize = new System.Drawing.Size(561, 224);
			this.Controls.Add(this.splitContainer1);
			this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ExecuteForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "ExecuteForm";
			this.tabExecute.ResumeLayout(false);
			this.pageBasic.ResumeLayout(false);
			this.pageBasic.PerformLayout();
			this.pageEnv.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.groupUpdate.ResumeLayout(false);
			this.groupRemove.ResumeLayout(false);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.flowLayoutPanel1.ResumeLayout(false);
			this.flowLayoutPanel1.PerformLayout();
			this.ResumeLayout(false);
		}
		private PeMain.UI.EnvRemoveControl envRemove;
		private PeMain.UI.EnvUpdateControl envUpdate;
		private System.Windows.Forms.GroupBox groupRemove;
		private System.Windows.Forms.GroupBox groupUpdate;
		private System.Windows.Forms.CheckBox selectUserDefault;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Button commandSubmit;
		private System.Windows.Forms.Button commandCancel;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.TabPage pageEnv;
		private System.Windows.Forms.Label labelOption;
		private System.Windows.Forms.Label labelWorkDirPath;
		private System.Windows.Forms.Button commandOption_file;
		private System.Windows.Forms.Button commandOption_dir;
		private System.Windows.Forms.Button commandWorkDirPath;
		private System.Windows.Forms.CheckBox selectStdStream;
		private System.Windows.Forms.TabPage pageBasic;
		private System.Windows.Forms.TabControl tabExecute;
		private System.Windows.Forms.ComboBox inputWorkDirPath;
		private System.Windows.Forms.ComboBox inputOption;
		private System.Windows.Forms.TextBox viewCommand;
	}
}
