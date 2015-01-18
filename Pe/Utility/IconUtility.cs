using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using ContentTypeTextNet.Pe.Library.PlatformInvoke.Windows;
using ContentTypeTextNet.Pe.Library.Skin;

namespace ContentTypeTextNet.Pe.Library.Utility
{
	public static class IconUtility
	{
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
					NativeMethods.ExtractIconEx(iconPath, iconIndex, null, iconHandle, 1);
				} else {
					Debug.Assert(iconScale == IconScale.Normal);
					NativeMethods.ExtractIconEx(iconPath, iconIndex, iconHandle, null, 1);
				}
				if(iconHandle[0] != IntPtr.Zero) {
					result = (Icon)System.Drawing.Icon.FromHandle(iconHandle[0]).Clone();
					NativeMethods.DestroyIcon(iconHandle[0]);
				}
			}
			if(result == null) {
				var fileInfo = new SHFILEINFO();
				SHGFI flag = SHGFI.SHGFI_ICON;
				if(iconScale == IconScale.Small) {
					flag |= SHGFI.SHGFI_SMALLICON;
				} else {
					Debug.Assert(iconScale == IconScale.Normal);
					flag |= SHGFI.SHGFI_LARGEICON;
				}
				var fileInfoResult = NativeMethods.SHGetFileInfo(iconPath, 0, ref fileInfo, (uint)Marshal.SizeOf(fileInfo), flag);
				if(fileInfoResult != IntPtr.Zero) {
					result = (Icon)System.Drawing.Icon.FromHandle(fileInfo.hIcon).Clone();
					NativeMethods.DestroyIcon(fileInfo.hIcon);
				}
			}
			return result;
		}

		// BUGS: いろいろ調べてるけどインデックス指定がわけわかめ
		unsafe static Icon LoadLargeIcon(string iconPath, IconScale iconScale, int iconIndex, bool hasIcon)
		{
			Debug.Assert(iconScale.IsIn(IconScale.Big, IconScale.Large), iconScale.ToString());

			Icon result = null;
			var shellImageList = iconScale == IconScale.Big ? SHIL.SHIL_EXTRALARGE : SHIL.SHIL_JUMBO;
			var fileInfo = new SHFILEINFO() {
				iIcon = iconIndex,
			};

			var infoFlags = SHGFI.SHGFI_SYSICONINDEX;//| SHGFI.SHGFI_USEFILEATTRIBUTES;
			//var hImgSmall = NativeMethods.SHGetFileInfo(iconPath, (int)FILE_ATTRIBUTE.FILE_ATTRIBUTE_NORMAL, ref fileInfo, (uint)Marshal.SizeOf(fileInfo), infoFlags);
			var hImgSmall = NativeMethods.SHGetFileInfo(iconPath, (int)FILE_ATTRIBUTE.FILE_ATTRIBUTE_NORMAL, ref fileInfo, (uint)Marshal.SizeOf(fileInfo), infoFlags);

			IImageList imageList = null;
			var getImageListResult = NativeMethods.SHGetImageList((int)shellImageList, ref NativeMethods.IID_IImageList, out imageList);

			if(getImageListResult == ComResult.S_OK) {
				var hIcon = IntPtr.Zero;
				if(hasIcon) {
					int n = 0;
					imageList.GetImageCount(ref n);
					Debug.WriteLine("{0}, {1}, {2}, {3}", iconPath, n, fileInfo.iIcon, hasIcon);
					//fileInfo.iIcon = iconIndex;
					var hResult = imageList.GetIcon(fileInfo.iIcon, (int)ImageListDrawItemConstants.ILD_TRANSPARENT, ref hIcon);
				}
				if(hIcon == IntPtr.Zero) {

					//var hResult = imageList.GetIcon(fileInfo.iIcon, (int)ImageListDrawItemConstants.ILD_TRANSPARENT, ref hIcon);
					var hBitmap = IntPtr.Zero;
					IShellItem iShellItem = null;
					NativeMethods.SHCreateItemFromParsingName(iconPath, IntPtr.Zero, NativeMethods.IID_IShellItem, out iShellItem);
					var size = iconScale.ToSize();
					((IShellItemImageFactory)iShellItem).GetImage(new SIZE(size.Width, size.Height), 0x0, out hBitmap);

					var dibsection = new DIBSECTION();
					NativeMethods.GetObject((IntPtr)hBitmap, Marshal.SizeOf(dibsection), ref dibsection);
					int width = dibsection.dsBm.bmWidth;
					int height = dibsection.dsBm.bmHeight;
					Debug.WriteLine("{0}, {1}, {2}, {3}", iconPath, dibsection.dsBm.bmHeight, dibsection.dsBmih.biHeight, hasIcon);
					using(var bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb)) {

						for(int x = 0; x < dibsection.dsBmih.biWidth; x++) {
							for(int y = 0; y < dibsection.dsBmih.biHeight; y++) {
								int i = y * dibsection.dsBmih.biWidth + x;
								IntPtr ptr = dibsection.dsBm.bmBits + (i * Marshal.SizeOf(typeof(RGBQUAD)));
								var rgbquad = (RGBQUAD)Marshal.PtrToStructure(ptr, typeof(RGBQUAD));

								if(rgbquad.rgbReserved != 0)
									bitmap.SetPixel(x, y, Color.FromArgb(rgbquad.rgbReserved, rgbquad.rgbRed, rgbquad.rgbGreen, rgbquad.rgbBlue));
							}
						}
						/*
						{
							// create the destination Bitmap object
							Bitmap bmp = new Bitmap(width, height, PixelFormat.Format32bppArgb);

							// get the HDCs and select the HBITMAP
							Graphics graphics = Graphics.FromImage(bmp);

							IntPtr hdcDest = graphics.GetHdc();
							IntPtr hdcSrc = NativeMethods.CreateCompatibleDC(hdcDest);
							IntPtr hobjOriginal = NativeMethods.SelectObject(hdcSrc, (IntPtr)hBitmap);

							// render the bmp using AlphaBlend
							BLENDFUNCTION blendfunction = new BLENDFUNCTION(AC.AC_SRC_OVER, 0, 0xFF, AC.AC_SRC_ALPHA);
							NativeMethods.AlphaBlend(hdcDest, 0, 0, width, height, hdcSrc, 0, 0, width, height, blendfunction);

							// clean up
							NativeMethods.SelectObject(hdcSrc, hobjOriginal);
							NativeMethods.DeleteDC(hdcSrc);
							graphics.ReleaseHdc(hdcDest);
							graphics.Dispose();
						}
						*/
						hIcon = bitmap.GetHicon();
					}
				}

				using(var icon = System.Drawing.Icon.FromHandle(hIcon)) {
					result = (Icon)icon.Clone();
				}
				NativeMethods.DestroyIcon(hIcon);
				NativeMethods.SendMessage(hIcon, WM.WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
			}

			// -----------------

			return result;
		}

		public static Icon Load(string iconPath, IconScale iconScale, int iconIndex)
		{
			// 実行形式
			var hasIcon = PathUtility.HasIconPath(iconPath);

			Icon result = null;
			if(iconScale == IconScale.Small || iconScale == IconScale.Normal) {
				result = LoadNormalIcon(iconPath, iconScale, iconIndex, hasIcon);
			} else {
				result = LoadLargeIcon(iconPath, iconScale, iconIndex, hasIcon);
			}

			return result;
		}

		public static Image ImageFromIcon(Icon icon, IconScale iconScale)
		{
			var iconSize = iconScale.ToSize();
			using(var iconImage = new Icon(icon, iconSize)) {
				return iconImage.ToBitmap();
			}
		}
	}




}
