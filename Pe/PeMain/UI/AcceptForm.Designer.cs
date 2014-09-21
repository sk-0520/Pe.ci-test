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
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.webDocument = new System.Windows.Forms.WebBrowser();
			this.commandAccept = new System.Windows.Forms.Button();
			this.commandCancel = new System.Windows.Forms.Button();
			this.tableLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.Controls.Add(this.webDocument, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.commandAccept, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.commandCancel, 1, 1);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 2;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.Size = new System.Drawing.Size(464, 322);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// webDocument
			// 
			this.tableLayoutPanel1.SetColumnSpan(this.webDocument, 2);
			this.webDocument.Dock = System.Windows.Forms.DockStyle.Fill;
			this.webDocument.IsWebBrowserContextMenuEnabled = false;
			this.webDocument.Location = new System.Drawing.Point(3, 4);
			this.webDocument.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.webDocument.MinimumSize = new System.Drawing.Size(23, 25);
			this.webDocument.Name = "webDocument";
			this.webDocument.Size = new System.Drawing.Size(458, 281);
			this.webDocument.TabIndex = 0;
			// 
			// commandAccept
			// 
			this.commandAccept.AutoSize = true;
			this.commandAccept.Location = new System.Drawing.Point(3, 293);
			this.commandAccept.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.commandAccept.Name = "commandAccept";
			this.commandAccept.Size = new System.Drawing.Size(150, 25);
			this.commandAccept.TabIndex = 1;
			this.commandAccept.Text = "{OK}";
			this.commandAccept.UseVisualStyleBackColor = true;
			// 
			// commandCancel
			// 
			this.commandCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.commandCancel.AutoSize = true;
			this.commandCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.commandCancel.Location = new System.Drawing.Point(317, 293);
			this.commandCancel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.commandCancel.Name = "commandCancel";
			this.commandCancel.Size = new System.Drawing.Size(144, 25);
			this.commandCancel.TabIndex = 2;
			this.commandCancel.Text = "{CANCEL}";
			this.commandCancel.UseVisualStyleBackColor = true;
			// 
			// AcceptForm
			// 
			this.AcceptButton = this.commandAccept;
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.commandCancel;
			this.ClientSize = new System.Drawing.Size(464, 322);
			this.Controls.Add(this.tableLayoutPanel1);
			this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.MinimumSize = new System.Drawing.Size(480, 360);
			this.Name = "AcceptForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = ":window/accept";
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.ResumeLayout(false);
		}
		private System.Windows.Forms.Button commandCancel;
		private System.Windows.Forms.Button commandAccept;
		private System.Windows.Forms.WebBrowser webDocument;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
	}
}
