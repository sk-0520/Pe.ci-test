namespace ContentTypeTextNet.Pe.Library.Utility
{
	using System;
	using System.Drawing;
	using System.Windows.Forms;
	using ContentTypeTextNet.Pe.Library.PlatformInvoke.Windows;

	/// <summary>
	/// カーソルに関する共通処理。
	/// </summary>
	public static class PointingUtility
	{
		/// <summary>
		/// 指定コントロールの上にカーソルを移動させる。
		/// </summary>
		/// <param name="target"></param>
		public static void Over(Control target)
		{
			//Debug.WriteLine(((Form)sender).PointToScreen(((Button)((Form)sender).AcceptButton).Location));
			var targetSize = target.Size;
			var targetPos = target.Parent.PointToScreen(target.Location);

			var pointingPos = new Point(targetPos.X + targetSize.Width / 2, targetPos.Y + targetSize.Height / 2);
			//Debug.WriteLine(pointingPos);
			Cursor.Position = pointingPos;
		}
		
		private static bool IsAutoMove()
		{
			int result = 0;
			var funcReturn = NativeMethods.SystemParametersInfo(SPI.SPI_GETSNAPTODEFBUTTON, 0, ref result, SPIF.None);
			return funcReturn && result != 0;
		}

		/// <summary>
		/// システムが自動移動設定になっていれば指定コントロールの上にカーソルを移動させる。
		/// </summary>
		/// <param name="target"></param>
		public static void OverAuto(Control target)
		{
			if(IsAutoMove()) {
				Over(target);
			}
		}
		
		/// <summary>
		/// 指定ウィンドウの各情報からカーソルを自動設定。
		/// </summary>
		/// <param name="form"></param>
		public static void OverForm(Form form)
		{
			var accept = form.AcceptButton as Control;
			var cancel = form.CancelButton as Control;
			if(accept != null) {
				OverAuto(accept);
			} else if(cancel != null) {
				OverAuto(cancel);
			} else {
				OverAuto(form);
			}
		}
		
		static void EventDefaultButton(object sender, EventArgs e)
		{
			OverForm((Form)sender);
		}
		
		/// <summary>
		/// OverFormを表示時に実行するイベント設定。
		/// </summary>
		/// <param name="form"></param>
		public static void AttachmentDefaultButton(Form form)
		{
			form.Shown += EventDefaultButton;
		}
	}
}
