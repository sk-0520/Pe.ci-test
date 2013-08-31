/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/08/30
 * 時刻: 23:47
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Runtime.InteropServices;

namespace ShareLib
{
     public struct IMAGELISTDRAWPARAMS
    {
        public int cbSize;
        public IntPtr himl;
        public int i;
        public IntPtr hdcDst;
        public int x;
        public int y;
        public int cx;
        public int cy;
        public int xBitmap; // x offest from the upperleft of bitmap
        public int yBitmap; // y offset from the upperleft of bitmap
        public int rgbBk;
        public int rgbFg;
        public int fStyle;
        public int dwRop;
        public int fState;
        public int Frame;
        public int crEffect;
    }


    [StructLayout(LayoutKind.Sequential)]
    public struct IMAGEINFO
    {
        private readonly IntPtr hbmImage;
        private readonly IntPtr hbmMask;
        private readonly int Unused1;
        private readonly int Unused2;
        private readonly RECT rcImage;
    }

        [Flags]
        public enum ImageListDrawItemConstants : int
        {
                /// <summary>
                /// Draw item normally.
                /// </summary>
                ILD_NORMAL = 0x0,
                /// <summary>
                /// Draw item transparently.
                /// </summary>
                ILD_TRANSPARENT = 0x1,
                /// <summary>
                /// Draw item blended with 25% of the specified foreground colour
                /// or the Highlight colour if no foreground colour specified.
                /// </summary>
                ILD_BLEND25 = 0x2,
                /// <summary>
                /// Draw item blended with 50% of the specified foreground colour
                /// or the Highlight colour if no foreground colour specified.
                /// </summary>
                ILD_SELECTED = 0x4,
                /// <summary>
                /// Draw the icon's mask
                /// </summary>
                ILD_MASK = 0x10,
                /// <summary>
                /// Draw the icon image without using the mask
                /// </summary>
                ILD_IMAGE = 0x20,
                /// <summary>
                /// Draw the icon using the ROP specified.
                /// </summary>
                ILD_ROP = 0x40,
                /// <summary>
                /// Preserves the alpha channel in dest. XP only.
                /// </summary>
                ILD_PRESERVEALPHA = 0x1000,
                /// <summary>
                /// Scale the image to cx, cy instead of clipping it.  XP only.
                /// </summary>
                ILD_SCALE = 0x2000,
                /// <summary>
                /// Scale the image to the current DPI of the display. XP only.
                /// </summary>
                ILD_DPISCALE = 0x4000
        }

        [ComImportAttribute()]
        [GuidAttribute("46EB5926-582E-4017-9FDF-E8998DAA0950")]
        [InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
        //helpstring("Image List"),
        public interface IImageList
        {
                [PreserveSig]
                int Add(
                        IntPtr hbmImage,
                        IntPtr hbmMask,
                        ref int pi);

                [PreserveSig]
                int ReplaceIcon(
                        int i,
                        IntPtr hicon,
                        ref int pi);

                [PreserveSig]
                int SetOverlayImage(
                        int iImage,
                        int iOverlay);

                [PreserveSig]
                int Replace(
                        int i,
                        IntPtr hbmImage,
                        IntPtr hbmMask);

                [PreserveSig]
                int AddMasked(
                        IntPtr hbmImage,
                        int crMask,
                        ref int pi);

                [PreserveSig]
                int Draw(
                        ref IMAGELISTDRAWPARAMS pimldp);

                [PreserveSig]
                int Remove(
                        int i);

                [PreserveSig]
                int GetIcon(
                        int i,
                        int flags,
                        ref IntPtr picon);

                [PreserveSig]
                int GetImageInfo(
                        int i,
                        ref IMAGEINFO pImageInfo);

                [PreserveSig]
                int Copy(
                        int iDst,
                        IImageList punkSrc,
                        int iSrc,
                        int uFlags);

                [PreserveSig]
                int Merge(
                        int i1,
                        IImageList punk2,
                        int i2,
                        int dx,
                        int dy,
                        ref Guid riid,
                        ref IntPtr ppv);

                [PreserveSig]
                int Clone(
                        ref Guid riid,
                        ref IntPtr ppv);

                [PreserveSig]
                int GetImageRect(
                        int i,
                        ref RECT prc);

                [PreserveSig]
                int GetIconSize(
                        ref int cx,
                        ref int cy);

                [PreserveSig]
                int SetIconSize(
                        int cx,
                        int cy);

                [PreserveSig]
                int GetImageCount(
                        ref int pi);

                [PreserveSig]
                int SetImageCount(
                        int uNewCount);

                [PreserveSig]
                int SetBkColor(
                        int clrBk,
                        ref int pclr);

                [PreserveSig]
                int GetBkColor(
                        ref int pclr);

                [PreserveSig]
                int BeginDrag(
                        int iTrack,
                        int dxHotspot,
                        int dyHotspot);

                [PreserveSig]
                int EndDrag();

                [PreserveSig]
                int DragEnter(
                        IntPtr hwndLock,
                        int x,
                        int y);

                [PreserveSig]
                int DragLeave(
                        IntPtr hwndLock);

                [PreserveSig]
                int DragMove(
                        int x,
                        int y);

                [PreserveSig]
                int SetDragCursorImage(
                        ref IImageList punk,
                        int iDrag,
                        int dxHotspot,
                        int dyHotspot);

                [PreserveSig]
                int DragShowNolock(
                        int fShow);

                [PreserveSig]
                int GetDragImage(
                        ref POINT ppt,
                        ref POINT pptHotspot,
                        ref Guid riid,
                        ref IntPtr ppv);

                [PreserveSig]
                int GetItemFlags(
                        int i,
                        ref int dwFlags);

                [PreserveSig]
                int GetOverlayImage(
                        int iOverlay,
                        ref int piIndex);
        }
}