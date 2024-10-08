using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Media.Imaging;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Compatibility.Forms;
using ContentTypeTextNet.Pe.Core.Compatibility.Windows;
using ContentTypeTextNet.Pe.Core.Models.Unmanaged;
using ContentTypeTextNet.Pe.Core.Models.Unmanaged.Gdi;
using ContentTypeTextNet.Pe.PInvoke.Windows;
using ContentTypeTextNet.Pe.Library.Base;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Core.Models
{
    /// <summary>
    /// アイコンファイルの読み込み処理。
    /// </summary>
    public class IconLoader
    {
        #region define

        private const int SIZEOF_GRPICONDIR_idCount = 4;
        private const int OFFSET_GRPICONDIRENTRY_nID = 12;
        private const int OFFSET_GRPICONDIRENTRY_dwBytesInRes = 8;
        private static readonly int SIZEOF_ICONDIR = Marshal.SizeOf<ICONDIR>();
        private static readonly int SIZEOF_ICONDIRENTRY = Marshal.SizeOf<ICONDIRENTRY>();
        private static readonly int SIZEOF_GRPICONDIRENTRY = Marshal.SizeOf<GRPICONDIRENTRY>();

        #endregion


        public IconLoader(ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger(GetType());
        }

        #region property

        private ILogger Logger { get; }

        #endregion

        #region function

        private BitmapSource GetThumbnailImageCore(string iconPath, in IconSize iconSize)
        {
            NativeMethods.SHCreateItemFromParsingName(iconPath, IntPtr.Zero, NativeMethods.IID_IShellItem, out var iShellItem);

            using var shellItem = ComWrapper.Create(iShellItem);
            var size = iconSize.ToSize();
            var siigbf = SIIGBF.SIIGBF_RESIZETOFIT;

            IntPtr hResultBitmap;
            using(var imageFactory = shellItem.Cast<IShellItemImageFactory>()) {
                imageFactory.Instance.GetImage(PodStructUtility.Convert(size), siigbf, out hResultBitmap);
            }

            using(var hBitmap = new BitmapHandleWrapper(hResultBitmap)) {
                var result = hBitmap.MakeBitmapSource();
                return result;
            }
        }

        /// <summary>
        /// ファイルのサムネイルを取得。
        /// </summary>
        /// <param name="iconPath">ファイルパス。</param>
        /// <param name="iconSize">アイコンサイズ。</param>
        /// <returns>成功した場合にイメージ。失敗すれば<see langword="null" />を返す。</returns>
        public BitmapSource? GetThumbnailImage(string iconPath, in IconSize iconSize)
        {
            try {
                return GetThumbnailImageCore(iconPath, iconSize);
            } catch(COMException ex) {
                Logger.LogWarning(ex, ex.Message);
                return null;
            } catch(ArgumentException ex) {
                Logger.LogWarning(ex, ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 実行モジュールのリソースを取得。
        /// </summary>
        /// <param name="hModule">モジュールハンドル。</param>
        /// <param name="name">リソース名。</param>
        /// <param name="resType"><inheritdoc cref="RT"/></param>
        /// <returns>取得成功した場合のリソースバイナリ。</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S1168:Empty arrays and collections should be returned instead of null")]
        private byte[]? GetResourceBinaryData(IntPtr hModule, IntPtr name, RT resType)
        {
            var hGroup = NativeMethods.FindResource(hModule, name, new IntPtr((int)resType));
            if(hGroup == IntPtr.Zero) {
                Logger.LogTrace("return {FUNC}", nameof(NativeMethods.FindResource));
                return null;
            }

            var hLoadGroup = NativeMethods.LoadResource(hModule, hGroup);
            if(hLoadGroup == IntPtr.Zero) {
                Logger.LogTrace("return {FUNC}", nameof(NativeMethods.LoadResource));
                return null;
            }

            var resData = NativeMethods.LockResource(hLoadGroup);
            if(resData == IntPtr.Zero) {
                Logger.LogTrace("return {FUNC}", nameof(NativeMethods.LockResource));
                return null;
            }

            var resSize = NativeMethods.SizeofResource(hModule, hGroup);
            if(resSize == 0) {
                Logger.LogTrace("return {FUNC}", nameof(NativeMethods.SizeofResource));
                return null;
            }

            var resBinary = new byte[resSize];
            Marshal.Copy(resData, resBinary, 0, resBinary.Length);

            return resBinary;
        }

        /// <summary>
        /// アイコンリソースの一覧を取得。
        /// </summary>
        /// <param name="resourcePath"></param>
        /// <returns>取得したリソースのバイナリ群。</returns>
        private IList<byte[]> LoadIconResource(string resourcePath)
        {
            var hModule = NativeMethods.LoadLibraryEx(resourcePath, IntPtr.Zero, LOAD_LIBRARY.LOAD_LIBRARY_AS_DATAFILE);
            if(hModule == IntPtr.Zero) {
                Logger.LogError("{0}: {1}, {2}", nameof(NativeMethods.LoadLibraryEx), NativeMethods.GetLastError(), resourcePath);
                return new List<byte[]>();
            }

            var binaryList = new List<byte[]>();
            EnumResNameProc proc = (hMod, type, name, lp) => {
                var binaryGroupIconData = GetResourceBinaryData(hMod, name, RT.RT_GROUP_ICON);
                if(binaryGroupIconData == null) {
                    return true;
                }

                var iconCount = BitConverter.ToUInt16(binaryGroupIconData, SIZEOF_GRPICONDIR_idCount);

                var totalSize = SIZEOF_ICONDIR + SIZEOF_ICONDIRENTRY * iconCount;
                for(var i = 0; i < iconCount; i++) {
                    var readOffset = SIZEOF_ICONDIR + (SIZEOF_GRPICONDIRENTRY * i) + OFFSET_GRPICONDIRENTRY_dwBytesInRes;
                    if(binaryGroupIconData.Length == 0 && readOffset + sizeof(Int32) < binaryGroupIconData.Length) {
                        break;
                    }
                    if(binaryGroupIconData.Length < readOffset) {
                        break;
                    }

                    var length = BitConverter.ToInt32(
                        binaryGroupIconData,
                        readOffset
                    );
                    totalSize += length;
                }

                using(var stream = new MemoryReleaseStream(totalSize))
                using(var writer = new BinaryWriter(stream)) {
                    writer.Write(binaryGroupIconData, 0, SIZEOF_ICONDIR);

                    var picOffset = SIZEOF_ICONDIR + SIZEOF_ICONDIRENTRY * iconCount;
                    foreach(var i in Enumerable.Range(0, iconCount)) {
                        writer.Seek(SIZEOF_ICONDIR + SIZEOF_ICONDIRENTRY * i, SeekOrigin.Begin);
                        var offsetWrite = SIZEOF_ICONDIR + SIZEOF_GRPICONDIRENTRY * i;
                        if(binaryGroupIconData.Length <= offsetWrite + OFFSET_GRPICONDIRENTRY_nID) {
                            continue;
                        }
                        writer.Write(binaryGroupIconData, offsetWrite, OFFSET_GRPICONDIRENTRY_nID);
                        writer.Write(picOffset);

                        writer.Seek(picOffset, SeekOrigin.Begin);

                        ushort id = BitConverter.ToUInt16(binaryGroupIconData, SIZEOF_ICONDIR + SIZEOF_GRPICONDIRENTRY * i + OFFSET_GRPICONDIRENTRY_nID);
                        var pic = GetResourceBinaryData(hMod, new IntPtr(id), RT.RT_ICON);
                        if(pic != null) {
                            writer.Write(pic, 0, pic.Length);
                            picOffset += pic.Length;
                        }
                    }

                    binaryList.Add(stream.ToArray());
                }

                return true;
            };

            NativeMethods.EnumResourceNames(hModule, (int)RT.RT_GROUP_ICON, proc, IntPtr.Zero);

            NativeMethods.FreeLibrary(hModule);

            return binaryList;
        }

        /// <summary>
        /// 16px, 32pxアイコン取得。
        /// </summary>
        /// <param name="iconPath"></param>
        /// <param name="iconSize"></param>
        /// <param name="iconIndex"></param>
        /// <param name="hasIcon"></param>
        /// <returns></returns>
        private BitmapSource? LoadNormalIcon(string iconPath, int iconIndex, bool hasIcon, IconSize iconSize)
        {
            Debug.Assert(iconSize.Width == (int)IconBox.Small || iconSize.Width == (int)IconBox.Normal);
            Debug.Assert(0 <= iconIndex, iconIndex.ToString(CultureInfo.InvariantCulture));

            // 16, 32 px
            if(hasIcon) {
                var iconHandle = new IntPtr[1];
                if(iconSize.Width == (int)IconBox.Small) {
#pragma warning disable CS8625 // null リテラルを null 非許容参照型に変換できません。
                    _ = NativeMethods.ExtractIconEx(iconPath, iconIndex, null, iconHandle, 1);
#pragma warning restore CS8625 // null リテラルを null 非許容参照型に変換できません。
                } else {
                    Debug.Assert(iconSize.Width == (int)IconBox.Normal);
#pragma warning disable CS8625 // null リテラルを null 非許容参照型に変換できません。
                    NativeMethods.ExtractIconEx(iconPath, iconIndex, iconHandle, null, 1);
#pragma warning restore CS8625 // null リテラルを null 非許容参照型に変換できません。
                }
                if(iconHandle[0] != IntPtr.Zero) {
                    using(var hIcon = new IconHandleWrapper(iconHandle[0])) {
                        return hIcon.MakeBitmapSource();
                    }
                }
            }

            if(iconSize.Width == (int)IconBox.Normal) {
                try {
                    var thumbnailImage = GetThumbnailImage(iconPath, iconSize);
                    if(thumbnailImage != null) {
                        return thumbnailImage;
                    }
                } catch(Exception ex) {
                    Logger.LogWarning(ex, ex.Message);
                }
            }

            var fileInfo = new SHFILEINFO();
            SHGFI flag = SHGFI.SHGFI_ICON;
            if(iconSize.Width == (int)IconBox.Small) {
                flag |= SHGFI.SHGFI_SMALLICON;
            } else {
                Debug.Assert(iconSize.Width == (int)IconBox.Normal);
                flag |= SHGFI.SHGFI_LARGEICON;
            }
            var fileInfoResult = NativeMethods.SHGetFileInfo(iconPath, 0, ref fileInfo, (uint)Marshal.SizeOf(fileInfo), flag);
            if(fileInfo.hIcon != IntPtr.Zero) {
                using(var hIcon = new IconHandleWrapper(fileInfo.hIcon)) {
                    return hIcon.MakeBitmapSource();
                }
            }

            return null;
        }

        /// <summary>
        /// 48px以上のアイコン取得。
        /// </summary>
        /// <param name="iconPath"></param>
        /// <param name="iconSize"></param>
        /// <param name="iconIndex"></param>
        /// <param name="hasIcon"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        private BitmapSource? LoadLargeIcon(string iconPath, int iconIndex, bool hasIcon, IconSize iconSize)
        {
            Debug.Assert(0 <= iconIndex, iconIndex.ToString(CultureInfo.InvariantCulture));

            if(hasIcon) {
                try {
                    var iconList = LoadIconResource(iconPath);
                    if(iconIndex < iconList.Count) {
                        var binary = iconList[iconIndex];
                        iconList.Clear();
                        var image = DrawingUtility.ImageSourceFromBinaryIcon(binary, iconSize.ToSize());
                        return image;
                    }
                } catch(Exception ex) {
                    Logger.LogDebug(ex, ex.Message);
                }
            }

            var thumbnailImage = GetThumbnailImage(iconPath, iconSize);
            if(thumbnailImage != null) {
                return thumbnailImage;
            }

            var shellImageList = iconSize.Width == (int)IconBox.Big ? SHIL.SHIL_EXTRALARGE : SHIL.SHIL_JUMBO;
            var fileInfo = new SHFILEINFO() {
                iIcon = iconIndex,
            };

            var infoFlags = SHGFI.SHGFI_SYSICONINDEX;
            var hImgSmall = NativeMethods.SHGetFileInfo(iconPath, (int)FILE_ATTRIBUTE.FILE_ATTRIBUTE_NORMAL, ref fileInfo, (uint)Marshal.SizeOf(fileInfo), infoFlags);

            IImageList? resultImageList = null;
            try {
                var iidImageList = NativeMethods.IID_IImageList;
                var getImageListResult = NativeMethods.SHGetImageList((int)shellImageList, ref iidImageList, out resultImageList);

                if(getImageListResult == HRESULT.S_OK) {
                    Debug.Assert(resultImageList != null);
                    using(var imageList = new Com<IImageList>(resultImageList)) {
                        int n = 0;
                        imageList.Instance.GetImageCount(ref n);

                        var hResultIcon = IntPtr.Zero;
                        var hResult = imageList.Instance.GetIcon(fileInfo.iIcon, (int)ImageListDrawItemConstants.ILD_TRANSPARENT, ref hResultIcon);
                        if(hResultIcon != IntPtr.Zero) {
                            using(var hIcon = new IconHandleWrapper(hResultIcon)) {
                                return hIcon.MakeBitmapSource();
                            }
                        }
                    }
                }
            } catch(InvalidCastException ex) {
                Logger.LogWarning(ex, ex.Message);
            }

            return null;
        }

        /// <summary>
        /// アイコンを取得。
        /// </summary>
        /// <param name="iconPath">対象ファイルパス。</param>
        /// <param name="iconSize">アイコンサイズ。</param>
        /// <param name="iconIndex">アイコンインデックス。</param>
        /// <returns>取得したアイコン。呼び出し側で破棄が必要。</returns>
        public BitmapSource? Load(string iconPath, int iconIndex, IconSize iconSize)
        {
            // 実行形式
            var hasIcon = PathUtility.HasIconPath(iconPath);
            var useIconIndex = Math.Abs(iconIndex);

            BitmapSource result;
            if(iconSize.Width == (int)IconBox.Small || iconSize.Width == (int)IconBox.Normal) {
                result = LoadNormalIcon(iconPath, useIconIndex, hasIcon, iconSize)!;
            } else {
                result = LoadLargeIcon(iconPath, useIconIndex, hasIcon, iconSize)!;
            }

            return result;
        }

        #endregion
    }
}
