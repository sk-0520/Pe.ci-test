namespace ContentTypeTextNet.Pe.PeMain.UI
{
	partial class CommandForm
	{
		/// <summary>
		/// 必要なデザイナー変数です。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 使用中のリソースをすべてクリーンアップします。
		/// </summary>
		/// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
		protected override void Dispose(bool disposing)
		{
			if(disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows フォーム デザイナーで生成されたコード

		/// <summary>
		/// デザイナー サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディターで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.imageIcon = new System.Windows.Forms.PictureBox();
			this.panelMain = new System.Windows.Forms.TableLayoutPanel();
			this.commandExecute = new System.Windows.Forms.Button();
			this.inputCommand = new ContentTypeTextNet.Pe.PeMain.UI.Ex.CommandComboBox();
			this.timerHidden = new System.Windows.Forms.Timer(this.components);
			((System.ComponentModel.ISupportInitialize)(this.imageIcon)).BeginInit();
			this.panelMain.SuspendLayout();
			this.SuspendLayout();
			// 
			// imageIcon
			// 
			this.imageIcon.Location = new System.Drawing.Point(3, 3);
			this.imageIcon.Name = "imageIcon";
			this.imageIcon.Size = new System.Drawing.Size(32, 32);
			this.imageIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
			this.imageIcon.TabIndex = 0;
			this.imageIcon.TabStop = false;
			// 
			// panelMain
			// 
			this.panelMain.AutoSize = true;
			this.panelMain.ColumnCount = 3;
			this.panelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.panelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.panelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.panelMain.Controls.Add(this.imageIcon, 0, 0);
			this.panelMain.Controls.Add(this.commandExecute, 2, 0);
			this.panelMain.Controls.Add(this.inputCommand, 1, 0);
			this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelMain.Location = new System.Drawing.Point(0, 0);
			this.panelMain.Name = "panelMain";
			this.panelMain.RowCount = 1;
			this.panelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.panelMain.Size = new System.Drawing.Size(303, 39);
			this.panelMain.TabIndex = 2;
			// 
			// commandExecute
			// 
			this.commandExecute.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.commandExecute.Location = new System.Drawing.Point(236, 8);
			this.commandExecute.Name = "commandExecute";
			this.commandExecute.Size = new System.Drawing.Size(64, 23);
			this.commandExecute.TabIndex = 2;
			this.commandExecute.Text = ":command/execute";
			this.commandExecute.UseVisualStyleBackColor = true;
			this.commandExecute.Click += new System.EventHandler(this.commandExecute_Click);
			// 
			// inputCommand
			// 
			this.inputCommand.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.inputCommand.FormattingEnabled = true;
			this.inputCommand.Location = new System.Drawing.Point(41, 8);
			this.inputCommand.Name = "inputCommand";
			this.inputCommand.Size = new System.Drawing.Size(189, 23);
			this.inputCommand.TabIndex = 3;
			this.inputCommand.SelectedIndexChanged += new System.EventHandler(this.inputCommand_SelectedIndexChanged);
			this.inputCommand.DropDownClosed += new System.EventHandler(this.inputCommand_DropDownClosed);
			this.inputCommand.KeyDown += new System.Windows.Forms.KeyEventHandler(this.inputCommand_KeyDown);
			this.inputCommand.KeyUp += new System.Windows.Forms.KeyEventHandler(this.inputCommand_KeyUp);
			// 
			// timerHidden
			// 
			this.timerHidden.Tick += new System.EventHandler(this.timerHidden_Tick);
			// 
			// CommandForm
			// 
			this.AutoSize = true;
			this.ClientSize = new System.Drawing.Size(303, 39);
			this.Controls.Add(this.panelMain);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "CommandForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = ":window/command";
			this.Activated += new System.EventHandler(this.CommandForm_Activated);
			this.Deactivate += new System.EventHandler(this.CommandForm_Deactivate);
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CommandForm_FormClosing);
			this.SizeChanged += new System.EventHandler(this.CommandForm_SizeChanged);
			this.VisibleChanged += new System.EventHandler(this.CommandForm_VisibleChanged);
			((System.ComponentModel.ISupportInitialize)(this.imageIcon)).EndInit();
			this.panelMain.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.PictureBox imageIcon;
		private System.Windows.Forms.TableLayoutPanel panelMain;
		private System.Windows.Forms.Button commandExecute;
		private ContentTypeTextNet.Pe.PeMain.UI.Ex.CommandComboBox inputCommand;
		private System.Windows.Forms.Timer timerHidden;

	}
}
