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
using PI.Windows;

namespace PeUtility
{
	/// <summary>
	/// Description of PointingUtility.
	/// </summary>
	public static class PointingUtility
	{
		public static void Over(Control target)
		{
			var targetSize = target.Size;
			var targetPos = target.PointToScreen(target.Location);
			var pointingPos = new Point(targetPos.X + targetSize.Width / 2, targetPos.Y + targetSize.Width / 2);
			
			Cursor.Position = pointingPos;
		}
		
		private static bool IsAutoMove()
		{
			var result = new IntPtr();
			var funcReturn = API.SystemParametersInfo(SPI.SPI_GETSNAPTODEFBUTTON, 0, result,SPIF.None);
			return funcReturn && result.ToInt32() != 0;
		}
		
		public static void OverAuto(Control target)
		{
			if(IsAutoMove()) {
				Over(target);
			}
		}
		
		public static void OverForm(Form form)
		{
			OverAuto((Control)form.AcceptButton);
		}
		
		public static void AppendEventFormLoad(Form form)
		{
			form.Load += new EventHandler(form_Load);
		}

		static void form_Load(object sender, EventArgs e)
		{
			OverForm((Form)sender);
		}
		
		
	}
}
