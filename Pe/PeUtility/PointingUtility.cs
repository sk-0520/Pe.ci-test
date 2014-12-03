/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/03/02
 * 時刻: 20:56
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Drawing;
using System.Windows.Forms;

using ContentTypeTextNet.Pe.Library.PInvoke.Windows;

namespace ContentTypeTextNet.Pe.Library.Utility
{
	/// <summary>
	/// Description of PointingUtility.
	/// </summary>
	public static class PointingUtility
	{
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
		
		public static void OverAuto(Control target)
		{
			if(IsAutoMove()) {
				Over(target);
			}
		}
		
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
		
		public static void AttachmentDefaultButton(Form form)
		{
			form.Shown += EventDefaultButton;
		}
	}
}
