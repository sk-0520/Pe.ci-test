/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/10/16
 * 時刻: 23:48
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using PeUtility;
using PInvoke.Windows;

namespace PeMain.Logic
{
	public static class AppUtility
	{
		/// <summary>
		/// 自身のショートカットを作成。
		/// </summary>
		/// <param name="savePath"></param>
		public static void MakeAppShortcut(string savePath)
		{
			var shortcut = new ShortcutFile(savePath, true);
			shortcut.TargetPath = Literal.ApplicationExecutablePath;
			shortcut.IconPath = Literal.ApplicationExecutablePath;
			shortcut.IconIndex = 0;
			shortcut.WorkingDirectory = Literal.ApplicationRootDirPath;
			shortcut.Save();
		}
		
		public static Image GetAppIcon(IconScale iconScale)
		{
			var iconSize = iconScale.ToSize();
			using(var icon = new Icon(global::PeMain.Properties.Images.App, iconSize)) {
				return icon.ToBitmap();
			}
		}
		
		/// <summary>
		/// 拡張状態か。
		/// </summary>
		/// <returns></returns>
		public static bool IsExtension()
		{
			return Control.ModifierKeys == Keys.Shift;
		}
		
		public static Image CreateBoxColorImage(Color borderColor, Color backColor, Size size)
		{
			var image = new Bitmap(size.Width, size.Height);
			
			using(var g = Graphics.FromImage(image)) {
				using(var brush = new SolidBrush(backColor)) {
					using(var pen = new Pen(borderColor)) {
						g.FillRectangle(brush, new Rectangle(new Point(1, 1), new Size(size.Width - 2, size.Height - 2)));
						g.DrawRectangle(pen, new Rectangle(Point.Empty, new Size(size.Width - 1, size.Height - 1)));
					}
				}
			}
			
			return image;
		}
		
		public static Image CreateNoteBoxImage(Color color, Size size)
		{
			return CreateBoxColorImage(Color.FromArgb(160, DrawUtility.CalcAutoColor(color)), color, size);
		}

	}
}
