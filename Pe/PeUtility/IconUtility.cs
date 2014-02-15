/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/12/15
 * 時刻: 20:14
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using PI.Windows;

namespace PeUtility
{
	public enum IconScale
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
	
	public static class IconUtility
	{
		public static int ToHeight(this IconScale iconScale)
		{
			return (int)iconScale;
		}
		public static Size ToSize(this IconScale iconScale)
		{
			var size = iconScale.ToHeight();
			return new Size(size, size);
		}
		public static Icon Load(string iconPath, IconScale iconScale, int iconIndex)
		{
			// 実行形式
			var dotExt = Path.GetExtension(iconPath);
			var isBin = dotExt.IsIn(".exe", ".dll");
			
			Icon result = null;
			if (iconScale == IconScale.Small || iconScale == IconScale.Normal) {
				// 16, 32 px
				if(isBin) {
					var iconHandle = new IntPtr[1];
					if(iconScale == IconScale.Small) {
						API.ExtractIconEx(iconPath, iconIndex, null, iconHandle, 1);
					} else {
						Debug.Assert(iconScale == IconScale.Normal);
						API.ExtractIconEx(iconPath, iconIndex, iconHandle, null, 1);
					}
					if(iconHandle[0] != IntPtr.Zero) {
						result = (Icon)System.Drawing.Icon.FromHandle(iconHandle[0]).Clone();
						API.DestroyIcon(iconHandle[0]);
					}
				}
				if(result == null){
					var fileInfo = new SHFILEINFO();
					SHGFI flag = SHGFI.SHGFI_ICON;
					if(iconScale == IconScale.Small) {
						flag |= SHGFI.SHGFI_SMALLICON;
					} else {
						Debug.Assert(iconScale == IconScale.Normal);
						flag |= SHGFI.SHGFI_LARGEICON;
					}
					var fileInfoResult = API.SHGetFileInfo(iconPath, 0, ref fileInfo, (uint)Marshal.SizeOf(fileInfo), flag);
					if (fileInfoResult != IntPtr.Zero) {
						result = (Icon)System.Drawing.Icon.FromHandle(fileInfo.hIcon).Clone();
						API.DestroyIcon(fileInfo.hIcon);
					}
				}
			} else {
				var shellImageList = iconScale == IconScale.Big ? SHIL.SHIL_EXTRALARGE : SHIL.SHIL_JUMBO;
				var fileInfo = new SHFILEINFO();
				var hImgSmall = API.SHGetFileInfo(iconPath, 0, ref fileInfo, (uint)Marshal.SizeOf(fileInfo), SHGFI.SHGFI_SYSICONINDEX);

				IImageList imageList = null;
				var getImageListResult = API.SHGetImageList((int)shellImageList, ref API.IID_IImageList, ref imageList);
				
				if (getImageListResult == ComResult.S_OK) {
					IntPtr hIcon = IntPtr.Zero;
					if(isBin) {
						fileInfo.iIcon = iconIndex;
						var hResult = imageList.GetIcon(fileInfo.iIcon, (int)ImageListDrawItemConstants.ILD_TRANSPARENT, ref hIcon);
					} else {
						var hResult = imageList.GetIcon(fileInfo.iIcon, (int)ImageListDrawItemConstants.ILD_TRANSPARENT, ref hIcon);
					}
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
