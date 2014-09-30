/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/09/28
 * 時刻: 21:19
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
namespace PeMain.UI
{
	partial class UpdateForm
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
			this.webUpdate = new System.Windows.Forms.WebBrowser();
			this.commandOk = new System.Windows.Forms.Button();
			this.commandCancel = new System.Windows.Forms.Button();
			this.labelVersion = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// webUpdate
			// 
			this.webUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
									| System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.webUpdate.IsWebBrowserContextMenuEnabled = false;
			this.webUpdate.Location = new System.Drawing.Point(12, 13);
			this.webUpdate.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.webUpdate.MinimumSize = new System.Drawing.Size(23, 25);
			this.webUpdate.Name = "webUpdate";
			this.webUpdate.Size = new System.Drawing.Size(500, 261);
			this.webUpdate.TabIndex = 0;
			// 
			// commandOk
			// 
			this.commandOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.commandOk.Location = new System.Drawing.Point(320, 300);
			this.commandOk.Name = "commandOk";
			this.commandOk.Size = new System.Drawing.Size(92, 30);
			this.commandOk.TabIndex = 2;
			this.commandOk.Text = "{OK}";
			this.commandOk.UseVisualStyleBackColor = true;
			this.commandOk.Click += new System.EventHandler(this.CommandOk_Click);
			// 
			// commandCancel
			// 
			this.commandCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.commandCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.commandCancel.Location = new System.Drawing.Point(420, 300);
			this.commandCancel.Name = "commandCancel";
			this.commandCancel.Size = new System.Drawing.Size(92, 30);
			this.commandCancel.TabIndex = 3;
			this.commandCancel.Text = "{CANCEL}";
			this.commandCancel.UseVisualStyleBackColor = true;
			// 
			// labelVersion
			// 
			this.labelVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.labelVersion.AutoSize = true;
			this.labelVersion.Location = new System.Drawing.Point(12, 278);
			this.labelVersion.Name = "labelVersion";
			this.labelVersion.Size = new System.Drawing.Size(133, 15);
			this.labelVersion.TabIndex = 1;
			this.labelVersion.Text = ":update/label/version";
			// 
			// UpdateForm
			// 
			this.AcceptButton = this.commandOk;
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.commandCancel;
			this.ClientSize = new System.Drawing.Size(524, 342);
			this.Controls.Add(this.labelVersion);
			this.Controls.Add(this.commandCancel);
			this.Controls.Add(this.commandOk);
			this.Controls.Add(this.webUpdate);
			this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.Icon = global::PeMain.Properties.Images.Pe;
			this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.MinimumSize = new System.Drawing.Size(540, 380);
			this.Name = "UpdateForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = ":window/update";
			this.ResumeLayout(false);
			this.PerformLayout();
		}
		private System.Windows.Forms.Label labelVersion;
		private System.Windows.Forms.Button commandCancel;
		private System.Windows.Forms.Button commandOk;
		private System.Windows.Forms.WebBrowser webUpdate;
	}
}
