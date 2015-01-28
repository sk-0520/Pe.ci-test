namespace ContentTypeTextNet.Pe.Library.Utility
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Diagnostics;
	using System.Drawing;
	using System.Drawing.Imaging;
	using System.IO;
	using System.Linq;
	using System.Runtime.InteropServices;
	using ContentTypeTextNet.Pe.Library.PlatformInvoke.Windows;
	using ContentTypeTextNet.Pe.Library.Skin;

	/// <summary>
	/// アイコン取得共通処理。
	/// </summary>
	public static class IconUtility
	{
		const int sizeofGRPICONDIR_idCount = 4;
		const int offsetGRPICONDIRENTRY_nID = 12;
		const int offsetGRPICONDIRENTRY_dwBytesInRes = 8;
		static readonly int sizeofICONDIR;
		static readonly int sizeofICONDIRENTRY;
		static readonly int sizeofGRPICONDIRENTRY;

		static IconUtility()
		{
			sizeofICONDIR = Marshal.SizeOf<ICONDIR>();
			sizeofICONDIRENTRY = Marshal.SizeOf<ICONDIRENTRY>();
			sizeofGRPICONDIRENTRY = Marshal.SizeOf<GRPICONDIRENTRY>();
		}

		/// <summary>
		/// GBITAMPからBitmap作成。
		/// 
		/// http://svn.nate.deepcreek.org.au/svn/KeyboardRedirector/trunk/IconExtractor/IconExtractor.cs
		/// </summary>
		/// <param name="hBitmap"></param>
		/// <returns></returns>
		static Bitmap BitmapFromhBitmap(IntPtr hBitmap)
		{
			var plainBitmap = Image.FromHbitmap(hBitmap);
			var bmData = new BitmapData[2];
			var bounds = new Rectangle(0, 0, plainBitmap.Width, plainBitmap.Height);
			bmData[0] = plainBitmap.LockBits(bounds, ImageLockMode.ReadOnly, plainBitmap.PixelFormat);

			var transparentBitmap = new Bitmap(bmData[0].Width, bmData[0].Height, bmData[0].Stride, System.Drawing.Imaging.PixelFormat.Format32bppArgb, bmData[0].Scan0);
			bmData[1] = transparentBitmap.LockBits(bounds, System.Drawing.Imaging.ImageLockMode.WriteOnly, transparentBitmap.PixelFormat);
			try {
				Marshal.StructureToPtr(bmData[0].Scan0, bmData[1].Scan0, true);
			} finally {
				transparentBitmap.UnlockBits(bmData[1]);
				plainBitmap.UnlockBits(bmData[0]);
			}
			var isPlain = true;
			for(int y = 0; y < transparentBitmap.Height; y++) {
				for(int x = 0; x < transparentBitmap.Width; x++) {
					if((x > 0) && (y > 0)) {
						byte alpha = transparentBitmap.GetPixel(x, y).A;
						if(alpha > 0) {
							isPlain = false;
							break;
						}
					}
				}
			}

			if(!isPlain) {
				return transparentBitmap;
			}

			return plainBitmap;
		}

		/// <summary>
		/// ファイルのサムネイルを取得。
		/// </summary>
		/// <param name="iconPath"></param>
		/// <param name="iconScale"></param>
		/// <returns></returns>
		public static Bitmap GetThumbnailImage(string iconPath, IconScale iconScale)
		{
			var hBitmap = IntPtr.Zero;
			IShellItem iShellItem = null;
			NativeMethods.SHCreateItemFromParsingName(iconPath, IntPtr.Zero, NativeMethods.IID_IShellItem, out iShellItem);
			var size = iconScale.ToSize();
			var siigbf = SIIGBF.SIIGBF_RESIZETOFIT;
			((IShellItemImageFactory)iShellItem).GetImage(new SIZE(size.Width, size.Height), siigbf, out hBitmap);
			Marshal.ReleaseComObject(iShellItem);
			return BitmapFromhBitmap(hBitmap);
		}

		/// <summary>
		/// http://hp.vector.co.jp/authors/VA016117/rsrc2icon.html
		/// </summary>
		/// <param name="hModule"></param>
		/// <param name="name"></param>
		/// <param name="resType"></param>
		/// <returns></returns>
		static byte[] GetResourceBinaryData(IntPtr hModule, IntPtr name, ResType resType)
		{
			var hGroup = NativeMethods.FindResource(hModule, name, new IntPtr((int)resType));
			if(hGroup == null) {
				Debug.WriteLine("return FindResource");
				return null;
			}

			var hLoadGroup = NativeMethods.LoadResource(hModule, hGroup);
			if(hLoadGroup == null) {
				Debug.WriteLine("return LoadResource");
				return null;
			}

			var resData = NativeMethods.LockResource(hLoadGroup);
			if(resData == null) {
				Debug.WriteLine("return LockResource");
				return null;
			}

			var resSize = NativeMethods.SizeofResource(hModule, hGroup);
			if(resSize == 0) {
				Debug.WriteLine("return SizeofResource");
				return null;
			}

			var resBinary = new byte[resSize];
			Marshal.Copy(resData, resBinary, 0, resBinary.Length);

			return resBinary;
		}
		
		/// <summary>
		/// https://github.com/TsudaKageyu/IconExtractor
		/// </summary>
		/// <param name="resourcePath"></param>
		/// <param name="iconScale"></param>
		/// <returns></returns>
		static IList<byte[]> LoadIconResource(string resourcePath, IconScale iconScale)
		{
			var hModule = NativeMethods.LoadLibraryEx(resourcePath, IntPtr.Zero, LOAD_LIBRARY.LOAD_LIBRARY_AS_DATAFILE);
			var binaryList = new List<byte[]>();
			EnumResNameProc proc = (hMod, type, name, lp) => {
				var binaryGroupIconData = GetResourceBinaryData(hMod, name, ResType.GROUP_ICON);
				if(binaryGroupIconData != null) {
					var iconCount = BitConverter.ToUInt16(binaryGroupIconData, sizeofGRPICONDIR_idCount);
					//Debug.WriteLine("iconCount = {0}", iconCount);

					var totalSize = sizeofICONDIR + sizeofICONDIRENTRY * iconCount;
					foreach(var i in Enumerable.Range(0, iconCount)) {
						var length = BitConverter.ToInt32(
							binaryGroupIconData,
							sizeofICONDIR + (sizeofGRPICONDIRENTRY * i) + offsetGRPICONDIRENTRY_dwBytesInRes
						);
						//Debug.WriteLine("[{0}] = {1} byte", i, length);
						totalSize += length;
					}
					//Debug.WriteLine("totalSize = {0}", totalSize);

					using(var stream = new BinaryWriter(new MemoryStream(totalSize))) {
						stream.Write(binaryGroupIconData, 0, sizeofICONDIR);

						var picOffset = sizeofICONDIR + sizeofICONDIRENTRY * iconCount;
						foreach(var i in Enumerable.Range(0, iconCount)) {
							stream.Seek(sizeofICONDIR + sizeofICONDIRENTRY * i, SeekOrigin.Begin);

							stream.Write(binaryGroupIconData, sizeofICONDIR + sizeofGRPICONDIRENTRY * i, offsetGRPICONDIRENTRY_nID);
							stream.Write(picOffset); 

							stream.Seek(picOffset, SeekOrigin.Begin);

							ushort id = BitConverter.ToUInt16(binaryGroupIconData, sizeofICONDIR + sizeofGRPICONDIRENTRY * i + offsetGRPICONDIRENTRY_nID);
							var pic = GetResourceBinaryData(hModule, new IntPtr(id), ResType.ICON);
							stream.Write(pic, 0, pic.Length);
							picOffset += pic.Length;
						}

						binaryList.Add(((MemoryStream)stream.BaseStream).ToArray());
					}
				}

				return true;
			};

			NativeMethods.EnumResourceNames(hModule, (int)ResType.GROUP_ICON, proc, IntPtr.Zero);

			return binaryList;
		}


		/// <summary>
		/// 16px, 32pxアイコン取得。
		/// </summary>
		/// <param name="iconPath"></param>
		/// <param name="iconScale"></param>
		/// <param name="iconIndex"></param>
		/// <param name="hasIcon"></param>
		/// <returns></returns>
		static Icon LoadNormalIcon(string iconPath, IconScale iconScale, int iconIndex, bool hasIcon)
		{
			Debug.Assert(iconScale.IsIn(IconScale.Small, IconScale.Normal), iconScale.ToString());
			Debug.Assert(0 <= iconIndex, iconIndex.ToString());

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
				if(iconScale == IconScale.Normal) {
					try {
						using(var bitmap = GetThumbnailImage(iconPath, iconScale)) {
							result = (Icon)System.Drawing.Icon.FromHandle(bitmap.GetHicon()).Clone();
						}
					} catch(Exception ex) {
						Debug.WriteLine(ex);
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
			}

			return result;
		}

		/// <summary>
		/// 48px以上のアイコン取得。
		/// </summary>
		/// <param name="iconPath"></param>
		/// <param name="iconScale"></param>
		/// <param name="iconIndex"></param>
		/// <param name="hasIcon"></param>
		/// <returns></returns>
		static Icon LoadLargeIcon(string iconPath, IconScale iconScale, int iconIndex, bool hasIcon)
		{
			Debug.Assert(iconScale.IsIn(IconScale.Big, IconScale.Large), iconScale.ToString());
			Debug.Assert(0 <= iconIndex, iconIndex.ToString());

			Icon result = null;

			//if(getImageListResult == ComResult.S_OK) {
				var hIcon = IntPtr.Zero;

				if(hasIcon) {
					try {
						var iconList = LoadIconResource(iconPath, iconScale);
						if(iconIndex < iconList.Count) {
							using(var ms = new MemoryStream(iconList[iconIndex])) {
								hIcon = new Icon(ms, iconScale.ToSize()).Handle;
							}
						}
					} catch(Exception ex) {
						Debug.WriteLine(ex);
					}
				}

				if(hIcon == IntPtr.Zero) {
					using(var bitmap = GetThumbnailImage(iconPath, iconScale)) {
						hIcon = bitmap.GetHicon();
					}
				}

				if(hIcon == IntPtr.Zero) {
					var shellImageList = iconScale == IconScale.Big ? SHIL.SHIL_EXTRALARGE : SHIL.SHIL_JUMBO;
					var fileInfo = new SHFILEINFO() {
						iIcon = iconIndex,
					};

					var infoFlags = SHGFI.SHGFI_SYSICONINDEX;
					var hImgSmall = NativeMethods.SHGetFileInfo(iconPath, (int)FILE_ATTRIBUTE.FILE_ATTRIBUTE_NORMAL, ref fileInfo, (uint)Marshal.SizeOf(fileInfo), infoFlags);

					IImageList imageList = null;
					var getImageListResult = NativeMethods.SHGetImageList((int)shellImageList, ref NativeMethods.IID_IImageList, out imageList);
					if(getImageListResult == ComResult.S_OK) {
						Debug.Assert(imageList != null);
						try {
							int n = 0;
							imageList.GetImageCount(ref n);
							var hResult = imageList.GetIcon(fileInfo.iIcon, (int)ImageListDrawItemConstants.ILD_TRANSPARENT, ref hIcon);
						} finally {
							Marshal.ReleaseComObject(imageList);
						}
					}
				}

				using(var icon = Icon.FromHandle(hIcon)) {
					result = (Icon)icon.Clone();
				}

				NativeMethods.DestroyIcon(hIcon);
				NativeMethods.SendMessage(hIcon, WM.WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
			//}

			// -----------------

			return result;
		}

		public static Icon Load(string iconPath, IconScale iconScale, int iconIndex)
		{
			// 実行形式
			var hasIcon = PathUtility.HasIconPath(iconPath);
			var useIconIndex = Math.Abs(iconIndex);

			Icon result = null;
			if(iconScale == IconScale.Small || iconScale == IconScale.Normal) {
				result = LoadNormalIcon(iconPath, iconScale, useIconIndex, hasIcon);
			} else {
				result = LoadLargeIcon(iconPath, iconScale, useIconIndex, hasIcon);
			}

			return result;
		}

		//private static System.Drawing.Bitmap GetBitmapFromHbitmap(IntPtr hbitmap)
		//{
		//	Bitmap bm1 = Image.FromHbitmap(hbitmap);

		//	var bmData = new System.Drawing.Imaging.BitmapData[2];
		//	var bounds = new System.Drawing.Rectangle(0, 0, bm1.Width, bm1.Height);
		//	bmData[0] = bm1.LockBits(bounds, ImageLockMode.ReadOnly, bm1.PixelFormat);

		//	var bm2 = new Bitmap(bmData[0].Width, bmData[0].Height, bmData[0].Stride, System.Drawing.Imaging.PixelFormat.Format32bppArgb, bmData[0].Scan0);
		//	bmData[1] = bm2.LockBits(bounds, System.Drawing.Imaging.ImageLockMode.WriteOnly, bm2.PixelFormat);

		//	Debug.WriteLine("{0}, {1}", bm1.Size, bm2.Size);

		//	Marshal.StructureToPtr(bmData[0].Scan0, bmData[1].Scan0, true);
		//	bm2.UnlockBits(bmData[1]);
		//	bm1.UnlockBits(bmData[0]);

		//	bool transparent = true;
		//	for(int y = 0; y < bm2.Height; y++) {
		//		for(int x = 0; x < bm2.Width; x++) {
		//			if((x > 0) && (y > 0)) {
		//				byte alpha = bm2.GetPixel(x, y).A;
		//				if(alpha > 0) {
		//					transparent = false;
		//					break;
		//				}
		//			}
		//		}
		//	}

		//	if(transparent == false) {
		//		return bm2;
		//	}

		//	return bm1;
		//}

		public static Image ImageFromIcon(Icon icon, IconScale iconScale)
		{
			var iconSize = iconScale.ToSize();
			using(var iconImage = new Icon(icon, iconSize)) {
				return iconImage.ToBitmap();
			}
		}
	}




}
