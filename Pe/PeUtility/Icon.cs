/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/12/15
 * 時刻: 20:14
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

using PI.Windows;

namespace PeUtility
{
	public enum IconSize
	{
		/// <summary>
		/// 16px
		/// </summary>
		Small = 16,
		/// <summary>
		/// 32px
		/// </summary>
		Normal = 32,
		/// <summary>
		/// 48px
		/// </summary>
		Big = 48,
		/// <summary>
		/// 256px
		/// </summary>
		Large = 256,
	}
	
	public static class IconLoader
	{
		public static int ToHeight(this IconSize iconSize)
		{
			return (int)iconSize;
		}
		public static Size ToSize(this IconSize iconSize)
		{
			var size = iconSize.ToHeight();
			return new Size(size, size);
		}
		public static Icon Load(string iconPath, IconSize iconSize, int iconIndex)
		{
			Icon result = null;
			if (iconSize == IconSize.Small || iconSize == IconSize.Normal) {
				// 16, 32 px
				var fileInfo = new SHFILEINFO();
				fileInfo.iIcon = iconIndex;

				var iconFlag = iconSize == IconSize.Small ? SHGFI.SHGFI_SMALLICON : SHGFI.SHGFI_LARGEICON;

				var hImgSmall = API.SHGetFileInfo(iconPath, 0, ref fileInfo, (uint)Marshal.SizeOf(fileInfo), SHGFI.SHGFI_ICON | iconFlag);
				if (hImgSmall != IntPtr.Zero) {
					result = (Icon)System.Drawing.Icon.FromHandle(fileInfo.hIcon).Clone();
					API.DestroyIcon(fileInfo.hIcon);
				}
			} else {
				var shellImageList = iconSize == IconSize.Big ? SHIL.SHIL_EXTRALARGE : SHIL.SHIL_JUMBO;
				var fileInfo = new SHFILEINFO();
				var hImgSmall = API.SHGetFileInfo(iconPath, 0, ref fileInfo, (uint)Marshal.SizeOf(fileInfo), SHGFI.SHGFI_SYSICONINDEX);

				IImageList imageList = null;
				var getImageListResult = API.SHGetImageList((int)shellImageList, ref API.IID_IImageList, ref imageList);

				if (getImageListResult == ComResult.S_OK) {
					IntPtr hIcon = IntPtr.Zero;
					var hResult = imageList.GetIcon(fileInfo.iIcon, (int)ImageListDrawItemConstants.ILD_TRANSPARENT, ref hIcon);
					result = (Icon)System.Drawing.Icon.FromHandle(hIcon).Clone();
					API.DestroyIcon(hIcon);
				}

			}

			return result;
		}
	}
	
	public class OpenIconDialog: CommonDialog
	{
		public string IconPath { set; get; }
		public int IconIndex { set; get; }
		
		//表示
		protected override bool RunDialog(IntPtr hwndOwner)
		{
			const int MAX_PATH = 260;
			
			int iconIndex = 0;
			var sb = new StringBuilder(IconPath,MAX_PATH);
			bool result = API.SHChangeIconDialog(hwndOwner, sb , MAX_PATH, ref iconIndex);
			if (result) {
				IconIndex = iconIndex;
				IconPath = sb.ToString();
			}
			return result;
		}
		//ダイアログを初期化する。
		public override void Reset()
		{
			IconPath = string.Empty;
			IconIndex = 0;
		}  
	}
}
