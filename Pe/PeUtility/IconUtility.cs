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
using PeSkin;
using PInvoke.Windows;

namespace PeUtility
{
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
        static Icon LoadLargeIcon(string iconPath, IconScale iconScale, int iconIndex, bool hasIcon)
        {
            Debug.Assert(iconScale.IsIn(IconScale.Big, IconScale.Large), iconScale.ToString());

            Icon result = null;
            var shellImageList = iconScale == IconScale.Big ? SHIL.SHIL_EXTRALARGE : SHIL.SHIL_JUMBO;
            var fileInfo = new SHFILEINFO() {
                iIcon = iconIndex,
            };

            var infoFlags = SHGFI.SHGFI_SYSICONINDEX;//| SHGFI.SHGFI_USEFILEATTRIBUTES;
            //var hImgSmall = NativeMethods.SHGetFileInfo(iconPath, (int)FILE_ATTRIBUTE.FILE_ATTRIBUTE_NORMAL, ref fileInfo, (uint)Marshal.SizeOf(fileInfo), infoFlags);
            var hImgSmall = NativeMethods.SHGetFileInfo(iconPath, 0, ref fileInfo, (uint)Marshal.SizeOf(fileInfo), infoFlags);

            IImageList imageList = null;
            var getImageListResult = NativeMethods.SHGetImageList((int)shellImageList, ref NativeMethods.IID_IImageList, out imageList);

            if(getImageListResult == ComResult.S_OK) {
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
                NativeMethods.DestroyIcon(hIcon);
            }

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
    }
	
	


}
