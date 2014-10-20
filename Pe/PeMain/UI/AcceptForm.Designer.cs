/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 09/21/2014
 * 時刻: 10:08
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
namespace PeMain.UI
{
	partial class AcceptForm
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
			this.panelMain = new System.Windows.Forms.TableLayoutPanel();
			this.webDocument = new System.Windows.Forms.WebBrowser();
			this.commandAccept = new System.Windows.Forms.Button();
			this.commandCancel = new System.Windows.Forms.Button();
			this.panelUpdate = new System.Windows.Forms.FlowLayoutPanel();
			this.selectUpdateCheck = new System.Windows.Forms.CheckBox();
			this.selectUpdateCheckRC = new System.Windows.Forms.CheckBox();
			this.panelMain.SuspendLayout();
			this.panelUpdate.SuspendLayout();
			this.SuspendLayout();
			// 
			// panelMain
			// 
			this.panelMain.ColumnCount = 2;
			this.panelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.panelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.panelMain.Controls.Add(this.webDocument, 0, 0);
			this.panelMain.Controls.Add(this.commandAccept, 0, 2);
			this.panelMain.Controls.Add(this.commandCancel, 1, 2);
			this.panelMain.Controls.Add(this.panelUpdate, 0, 1);
			this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelMain.Location = new System.Drawing.Point(0, 0);
			this.panelMain.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.panelMain.Name = "panelMain";
			this.panelMain.RowCount = 3;
			this.panelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.panelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
			this.panelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.panelMain.Size = new System.Drawing.Size(624, 442);
			this.panelMain.TabIndex = 0;
			// 
			// webDocument
			// 
			this.panelMain.SetColumnSpan(this.webDocument, 2);
			this.webDocument.Dock = System.Windows.Forms.DockStyle.Fill;
			this.webDocument.Location = new System.Drawing.Point(3, 4);
			this.webDocument.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.webDocument.MinimumSize = new System.Drawing.Size(23, 25);
			this.webDocument.Name = "webDocument";
			this.webDocument.Size = new System.Drawing.Size(618, 361);
			this.webDocument.TabIndex = 0;
			// 
			// commandAccept
			// 
			this.commandAccept.AutoSize = true;
			this.commandAccept.Location = new System.Drawing.Point(3, 413);
			this.commandAccept.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.commandAccept.Name = "commandAccept";
			this.commandAccept.Size = new System.Drawing.Size(150, 25);
			this.commandAccept.TabIndex = 1;
			this.commandAccept.Text = "{OK}";
			this.commandAccept.UseVisualStyleBackColor = true;
			this.commandAccept.Click += new System.EventHandler(this.CommandAccept_Click);
			// 
			// commandCancel
			// 
			this.commandCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.commandCancel.AutoSize = true;
			this.commandCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.commandCancel.Location = new System.Drawing.Point(477, 413);
			this.commandCancel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.commandCancel.Name = "commandCancel";
			this.commandCancel.Size = new System.Drawing.Size(144, 25);
			this.commandCancel.TabIndex = 2;
			this.commandCancel.Text = "{CANCEL}";
			this.commandCancel.UseVisualStyleBackColor = true;
			// 
			// panelUpdate
			// 
			this.panelUpdate.AutoSize = true;
			this.panelMain.SetColumnSpan(this.panelUpdate, 2);
			this.panelUpdate.Controls.Add(this.selectUpdateCheck);
			this.panelUpdate.Controls.Add(this.selectUpdateCheckRC);
			this.panelUpdate.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelUpdate.Location = new System.Drawing.Point(3, 372);
			this.panelUpdate.Name = "panelUpdate";
			this.panelUpdate.Size = new System.Drawing.Size(618, 34);
			this.panelUpdate.TabIndex = 1;
			// 
			// selectUpdateCheck
			// 
			this.selectUpdateCheck.AutoSize = true;
			this.selectUpdateCheck.Location = new System.Drawing.Point(3, 3);
			this.selectUpdateCheck.Name = "selectUpdateCheck";
			this.selectUpdateCheck.Size = new System.Drawing.Size(192, 19);
			this.selectUpdateCheck.TabIndex = 0;
			this.selectUpdateCheck.Text = ":accept/check/update-check";
			this.selectUpdateCheck.UseVisualStyleBackColor = true;
			// 
			// selectUpdateCheckRC
			// 
			this.selectUpdateCheckRC.AutoSize = true;
			this.selectUpdateCheckRC.Location = new System.Drawing.Point(201, 3);
			this.selectUpdateCheckRC.Name = "selectUpdateCheckRC";
			this.selectUpdateCheckRC.Size = new System.Drawing.Size(212, 19);
			this.selectUpdateCheckRC.TabIndex = 1;
			this.selectUpdateCheckRC.Text = ":accept/check/update-check.RC";
			this.selectUpdateCheckRC.UseVisualStyleBackColor = true;
			// 
			// AcceptForm
			// 
			this.AcceptButton = this.commandAccept;
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.commandCancel;
			this.ClientSize = new System.Drawing.Size(624, 442);
			this.Controls.Add(this.panelMain);
			this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.Icon = global::PeMain.Properties.Images.App;
			this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.MinimumSize = new System.Drawing.Size(640, 480);
			this.Name = "AcceptForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = ":window/accept";
			this.Shown += new System.EventHandler(this.AcceptForm_Shown);
			this.panelMain.ResumeLayout(false);
			this.panelMain.PerformLayout();
			this.panelUpdate.ResumeLayout(false);
			this.panelUpdate.PerformLayout();
			this.ResumeLayout(false);
		}
		private System.Windows.Forms.CheckBox selectUpdateCheckRC;
		private System.Windows.Forms.CheckBox selectUpdateCheck;
		private System.Windows.Forms.FlowLayoutPanel panelUpdate;
		private System.Windows.Forms.Button commandCancel;
		private System.Windows.Forms.Button commandAccept;
		private System.Windows.Forms.WebBrowser webDocument;
		private System.Windows.Forms.TableLayoutPanel panelMain;
	}
}
