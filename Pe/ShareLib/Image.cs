/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/08/30
 * 時刻: 23:04
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Drawing;
using System.Runtime.InteropServices;


namespace ShareLib
{
	public enum IconSize
	{
		/// <summary>
		/// 16px
		/// </summary>
		Small,
		/// <summary>
		/// 32px
		/// </summary>
		Normal,
		/// <summary>
		/// 48px
		/// </summary>
		Big,
		/// <summary>
		/// 256px
		/// </summary>
		Large,
	}
	
	public class IconFile
	{
		public string IconPath { get; set; }
		public int Index { get; set; }
		public IconSize IconSize { get; set; }
		public Icon Icon { get; private set; }
		
		public TestAndInfo Load()
		{
			if (Common.IsIn(IconSize, IconSize.Small, IconSize.Normal)) {
				// 16, 32 px
				var fileInfo = new SHFILEINFO();
				fileInfo.iIcon = Index;

				var iconFlag = IconSize == IconSize.Small ? SHGFI.SHGFI_SMALLICON : SHGFI.SHGFI_LARGEICON;

				var hImgSmall = WindowsAPI.SHGetFileInfo(IconPath, 0, ref fileInfo, (uint)Marshal.SizeOf(fileInfo), SHGFI.SHGFI_ICON | iconFlag);
				if (hImgSmall != IntPtr.Zero) {
					Icon = (Icon)System.Drawing.Icon.FromHandle(fileInfo.hIcon).Clone();
					WindowsAPI.DestroyIcon(fileInfo.hIcon);
				}
			} else {
				var shellImageList = IconSize == IconSize.Big ? SHIL.SHIL_EXTRALARGE : SHIL.SHIL_JUMBO;
				var fileInfo = new SHFILEINFO();
				var hImgSmall = WindowsAPI.SHGetFileInfo(IconPath, 0, ref fileInfo, (uint)Marshal.SizeOf(fileInfo), SHGFI.SHGFI_SYSICONINDEX);

				IImageList imageList = null;
				var getImageListResult = WindowsAPI.SHGetImageList((int)shellImageList, ref WindowsAPI.IID_IImageList, ref imageList);

				if (getImageListResult == ComResult.S_OK) {
					IntPtr hIcon = IntPtr.Zero;
					var hResult = imageList.GetIcon(fileInfo.iIcon, (int)ImageListDrawItemConstants.ILD_TRANSPARENT, ref hIcon);
					Icon = (Icon)System.Drawing.Icon.FromHandle(hIcon).Clone();
					WindowsAPI.DestroyIcon(hIcon);
				}

			}

			return result;
			
		}
	}
}
