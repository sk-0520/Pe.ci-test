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
			this.SuspendLayout();
			// 
			// CommandForm
			// 
			this.ClientSize = new System.Drawing.Size(284, 262);
			this.Name = "CommandForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CommandForm_FormClosing);
			this.ResumeLayout(false);

		}

		#endregion
	}
}
