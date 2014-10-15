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
using PInvoke.Windows;

namespace PeUtility
{
	/// <summary>
	/// アイコンサイズ。
	/// </summary>
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
	
	/// <summary>
	/// アイコンのファイルパス。
	/// </summary>
	public class IconPath
	{
		/// <summary>
		/// アイコンのファイルパスを作成。
		/// </summary>
		public IconPath()
		{
			Path = string.Empty;
		}
		
		/// <summary>
		/// アイコンのファイルパスを作成。
		/// </summary>
		/// <param name="iconPath"></param>
		public IconPath(string iconPath)
		{
			var index = iconPath.LastIndexOf(',');
			if(index == -1) {
				Path = iconPath;
				Index = 0;
			} else {
				Path = iconPath.Substring(0, index);
				Index = int.Parse(iconPath.Substring(index + 1));
			}
		}
		
		/// <summary>
		/// アイコンのファイルパスを作成。
		/// </summary>
		/// <param name="path"></param>
		/// <param name="index"></param>
		public IconPath(string path, int index)
		{
			Path = path;
			Index = index;
		}
		
		/// <summary>
		/// ファイルパス。
		/// </summary>
		public string Path { get; set; }
		/// <summary>
		/// アイコンインデックス。
		/// </summary>
		public int Index { get; set; }
		
		public override string ToString()
		{
			return string.Format("{0},{1}", Path, Index);
		}

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
		
		/// <summary>
		/// 16px, 32pxアイコン取得
		/// </summary>
		/// <param name="iconPath"></param>
		/// <param name="iconScale"></param>
		/// <param name="iconIndex"></param>
		/// <param name="hasIcon"></param>
		/// <returns></returns>
		static Icon LoadNormalIcon(string iconPath, IconScale iconScale, int iconIndex, bool hasIcon)
		{
			Debug.Assert(iconScale.IsIn(IconScale.Small, IconScale.Normal), iconScale.ToString());
			
			Icon result = null;
			// 16, 32 px
			if(hasIcon) {
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
			return result;
		}
		
		// BUGS: いろいろ調べてるけどインデックス指定がわけわかめ
		static Icon LoadLargeIcon(string iconPath, IconScale iconScale, int iconIndex, bool hasIcon)
		{
			Debug.Assert(iconScale.IsIn(IconScale.Big, IconScale.Large), iconScale.ToString());
			
			Icon result = null;
			var shellImageList = iconScale == IconScale.Big ? SHIL.SHIL_EXTRALARGE : SHIL.SHIL_JUMBO;
			var fileInfo = new SHFILEINFO() {
				iIcon = iconIndex,
			};
			
			var infoFlags = SHGFI.SHGFI_SYSICONINDEX ;//| SHGFI.SHGFI_USEFILEATTRIBUTES;
			//var hImgSmall = API.SHGetFileInfo(iconPath, (int)FILE_ATTRIBUTE.FILE_ATTRIBUTE_NORMAL, ref fileInfo, (uint)Marshal.SizeOf(fileInfo), infoFlags);
			var hImgSmall = API.SHGetFileInfo(iconPath, 0, ref fileInfo, (uint)Marshal.SizeOf(fileInfo), infoFlags);

			IImageList imageList = null;
			var getImageListResult = API.SHGetImageList((int)shellImageList, ref API.IID_IImageList, out imageList);
			
			if (getImageListResult == ComResult.S_OK) {
				IntPtr hIcon = IntPtr.Zero;
				if(hasIcon) {
					int n = 0;
					imageList.GetImageCount(ref n);
					Debug.WriteLine("{0}, {1}", n, fileInfo.iIcon);
					//fileInfo.iIcon = iconIndex;
					var hResult = imageList.GetIcon(fileInfo.iIcon, (int)ImageListDrawItemConstants.ILD_TRANSPARENT, ref hIcon);
				} else {
					var hResult = imageList.GetIcon(fileInfo.iIcon, (int)ImageListDrawItemConstants.ILD_TRANSPARENT, ref hIcon);
				}
				
				result = (Icon)System.Drawing.Icon.FromHandle(hIcon).Clone();
				API.DestroyIcon(hIcon);
			}
			
			return result;
		}
		
		public static Icon Load(string iconPath, IconScale iconScale, int iconIndex)
		{
			// 実行形式
			var hasIcon = PathUtility.HasIconPath(iconPath);
			
			Icon result = null;
			if (iconScale == IconScale.Small || iconScale == IconScale.Normal) {
				result = LoadNormalIcon(iconPath, iconScale, iconIndex, hasIcon);
			} else {
				result = LoadLargeIcon(iconPath, iconScale, iconIndex, hasIcon);
			}
			
			return result;
		}
	}
	
	public class OpenIconDialog: CommonDialog
	{
		public OpenIconDialog(): base()
		{
			IconPath = new IconPath();
		}
		/*
		public string IconPath { set; get; }
		public int IconIndex { set; get; }
		 */
		
		public IconPath IconPath { set; get; }
		
		//表示
		protected override bool RunDialog(IntPtr hwndOwner)
		{
			var iconIndex = IconPath.Index;
			var sb = new StringBuilder(IconPath.Path, (int)MAX.MAX_PATH);
			var result = API.SHChangeIconDialog(hwndOwner, sb, sb.Capacity, ref iconIndex);
			if (result) {
				IconPath.Index = iconIndex;
				IconPath.Path = sb.ToString();
			}
			
			return result;
		}
		//ダイアログを初期化する。
		public override void Reset()
		{
			/*
			IconPath = string.Empty;
			IconIndex = 0;
			 */
		}
	}
}
