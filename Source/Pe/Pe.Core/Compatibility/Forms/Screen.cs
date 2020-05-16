using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models;
using WinForms = System.Windows.Forms;

namespace ContentTypeTextNet.Pe.Core.Compatibility.Forms
{
    /// <summary>
    /// <see cref="System.Windows.Forms.Screen"/> 互換クラス。
    /// </summary>
    [Serializable]
    public class Screen : IScreen
    {
        internal Screen(WinForms.Screen screen)
        {
            BitsPerPixel = screen.BitsPerPixel;
            DeviceBounds = DrawingUtility.Convert(screen.Bounds);
            DeviceName = screen.DeviceName;
            Primary = screen.Primary;
            DeviceWorkingArea = DrawingUtility.Convert(screen.WorkingArea);
        }

        #region property

        /// <summary>
        /// システム上のすべてのディスプレイの配列を取得します。
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Critical Code Smell", "S2365:Properties should not make collection or array copies")]
        public static Screen[] AllScreens
        {
            get { return GetAllScreens().ToArray(); }
        }

        /// <summary>
        /// プライマリ ディスプレイを取得します。
        /// </summary>
        public static Screen PrimaryScreen
        {
            get { return ConvertScreen(WinForms.Screen.PrimaryScreen); }
        }

        /// <summary>
        /// 1 ピクセルのデータに関連付けられているメモリのビット数を取得します。
        /// </summary>
        public int BitsPerPixel { get; }
        /// <summary>
        /// ディスプレイの範囲を取得します。
        /// </summary>
        [PixelKind(Px.Device)]
        public Rect DeviceBounds { get; protected internal set; }
        /// <summary>
        /// ディスプレイに関連付けられているデバイス名を取得します。
        /// </summary>
        public string DeviceName { get; protected internal set; } = string.Empty;
        /// <summary>
        /// 特定のディスプレイがプライマリ デバイスかどうかを示す値を取得します。
        /// </summary>
        public bool Primary { get; protected internal set; }
        /// <summary>
        /// ディスプレイの作業領域を取得します。 作業領域とは、ディスプレイのデスクトップ領域からタスクバー、ドッキングされたウィンドウ、およびドッキングされたツール バーを除いた部分です。
        /// </summary>
        [PixelKind(Px.Device)]
        public Rect DeviceWorkingArea { get; protected internal set; }

        #endregion

        #region function

        /// <summary>
        /// <see cref="System.Windows.Forms.Screen"/> を <see cref="Screen"/> に変換。
        /// </summary>
        /// <param name="screen"></param>
        /// <returns></returns>
        static Screen ConvertScreen(WinForms.Screen screen)
        {
            return new Screen(screen);
        }

        static IEnumerable<Screen> GetAllScreens()
        {
            return WinForms.Screen.AllScreens
                .Select(s => ConvertScreen(s))
            ;
        }

        /// <summary>
        /// 指定したポイントを保持するディスプレイを表す <see cref="Screen"/> を取得します。
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public static Screen FromDevicePoint([PixelKind(Px.Device)] Point point)
        {
            var drawingPoint = DrawingUtility.Convert(point);
            var formScreen = WinForms.Screen.FromPoint(drawingPoint);
            var result = ConvertScreen(formScreen);

            return result;
        }

        /// <summary>
        /// 四角形の最大部分を保持する <see cref="Screen"/> を取得します。
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        public static Screen FromDeviceRectangle(Rect rect)
        {
            var drawingRect = DrawingUtility.Convert(rect);
            var formScreen = WinForms.Screen.FromRectangle(drawingRect);
            var result = ConvertScreen(formScreen);

            return result;
        }

        /// <summary>
        /// 指定したハンドルによって参照されているオブジェクトの最大領域を保持するディスプレイを表す <see cref="Screen"/> を取得します。
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        public static Screen FromHandle(IntPtr hWnd)
        {
            var formScreen = WinForms.Screen.FromHandle(hWnd);

            var result = ConvertScreen(formScreen);

            return result;
        }

        #endregion
    }
}
