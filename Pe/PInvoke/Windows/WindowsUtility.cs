/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/03/12
 * 時刻: 5:26
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Drawing;

namespace PInvoke.Windows
{
	/// <summary>
	/// Description of Utility.
	/// </summary>
	public static class WindowsUtility
	{
		public static Point ScreenPointFromLParam(IntPtr lParam)
		{
			return new Point(
				(int)(lParam.ToInt64() & 0xFFFF),
				(int)((lParam.ToInt64() & 0xFFFF0000) >> 16)
			);
		}
	}
}
