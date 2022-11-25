using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using WinForms = System.Windows.Forms;

namespace ContentTypeTextNet.Pe.Core.Compatibility.Forms
{
    /// <summary>
    /// <inheritdoc cref="System.Windows.Forms.Screen" />
    /// <see cref="System.Windows.Forms.Screen"/> 互換クラス。
    /// </summary>
    [Serializable]
    public class Screen: IScreen
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

        /// <inheritdoc cref="WinForms.Screen.AllScreens"/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Critical Code Smell", "S2365:Properties should not make collection or array copies")]
        public static Screen[] AllScreens
        {
            get { return GetAllScreens().ToArray(); }
        }

        /// <inheritdoc cref="WinForms.Screen.PrimaryScreen"/>
        public static Screen? PrimaryScreen
        {
            get
            {
                if(WinForms.Screen.PrimaryScreen is null) {
                    return null;
                }

                return ConvertScreen(WinForms.Screen.PrimaryScreen);
            }
        }

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

        /// <inheritdoc cref="WinForms.Screen.GetAllScreens()"/>
        static IEnumerable<Screen> GetAllScreens()
        {
            return WinForms.Screen.AllScreens
                .Select(s => ConvertScreen(s))
            ;
        }

        /// <inheritdoc cref="WinForms.Screen.FromDevicePoint(Point)"/>
        public static Screen FromDevicePoint([PixelKind(Px.Device)] Point point)
        {
            var drawingPoint = DrawingUtility.Convert(point);
            var formScreen = WinForms.Screen.FromPoint(drawingPoint);
            var result = ConvertScreen(formScreen);

            return result;
        }

        /// <inheritdoc cref="WinForms.Screen.FromDeviceRectangle(Rect)"/>
        public static Screen FromDeviceRectangle(Rect rect)
        {
            var drawingRect = DrawingUtility.Convert(rect);
            var formScreen = WinForms.Screen.FromRectangle(drawingRect);
            var result = ConvertScreen(formScreen);

            return result;
        }

        /// <inheritdoc cref="WinForms.Screen.FromHandle(IntPtr)"/>
        public static Screen FromHandle(IntPtr hWnd)
        {
            var formScreen = WinForms.Screen.FromHandle(hWnd);

            var result = ConvertScreen(formScreen);

            return result;
        }

        #endregion

        #region IScreen

        /// <inheritdoc cref="IScreen.BitsPerPixel"/>
        public int BitsPerPixel { get; }
        /// <inheritdoc cref="IScreen.DeviceBounds"/>
        [PixelKind(Px.Device)]
        public Rect DeviceBounds { get; protected internal set; }
        /// <inheritdoc cref="IScreen.DeviceName"/>
        public string DeviceName { get; protected internal set; } = string.Empty;
        /// <inheritdoc cref="IScreen.Primary"/>
        public bool Primary { get; protected internal set; }
        /// <inheritdoc cref="IScreen.DeviceWorkingArea"/>
        [PixelKind(Px.Device)]
        public Rect DeviceWorkingArea { get; protected internal set; }


        #endregion
    }
}
