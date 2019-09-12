using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Shapes;
using ContentTypeTextNet.Pe.Bridge.Model;
using ContentTypeTextNet.Pe.Core.Model;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Model.Theme
{
    internal abstract class ThemeBase
    {

        public ThemeBase(IDispatcherWapper dispatcherWapper, ILogger logger)
        {
            DispatcherWapper = dispatcherWapper;
            Logger = logger;
        }

        public ThemeBase(IDispatcherWapper dispatcherWapper, ILoggerFactory loggerFactory)
        {
            DispatcherWapper = dispatcherWapper;
            Logger = loggerFactory.CreateLogger(GetType());
        }

        #region property

        protected ILogger Logger { get; }
        /// <summary>
        /// <see cref="DependencyObject"/>作成用のディスパッチャー。
        /// <para>だけどまず呼び出し側で UI スレッドであることを保証するので内部的にどうこうする場合にしゃあなし使うのであって原則使用しない。</para>
        /// </summary>
        protected IDispatcherWapper DispatcherWapper { get; }

        #endregion

        #region function

        protected double GetHorizontal(Thickness thickness) => thickness.Left + thickness.Right;

        protected double GetVertical(Thickness thickness) => thickness.Top + thickness.Bottom;

        protected Color GetMenuImageColor(bool getBoxColor, bool isActiveColor)
        {
            byte alpha = 80;

            if(getBoxColor) {
                if(isActiveColor) {
                    return SystemColors.ActiveCaptionColor;
                } else {
                    var color = SystemColors.InactiveCaptionColor;
                    return Color.FromArgb(alpha, color.R, color.G, color.B);
                }
            } else {
                if(isActiveColor) {
                    return SystemColors.ActiveCaptionTextColor;
                } else {
                    var color = SystemColors.InactiveCaptionTextColor;
                    return Color.FromArgb(alpha, color.R, color.G, color.B);
                }
            }
        }

        /// <summary>
        /// 箱形の要素を作成する。
        /// <para>同じようなものを共通の見栄えで作成するため細かい部分は内部実装に隠ぺいする。</para>
        /// </summary>
        /// <param name="borderColor">境界線の色。</param>
        /// <param name="fillColor">背景色</param>
        /// <param name="size">サイズ。</param>
        /// <returns>生成された要素。</returns>
        public static FrameworkElement CreateBox(Color borderColor, Color fillColor, Size size)
        {
            var box = new Rectangle();
            using(Initializer.BeginInitialize(box)) {
                box.Width = size.Width;
                box.Height = size.Height;
                box.Stroke = FreezableUtility.GetSafeFreeze(new SolidColorBrush(borderColor));
                box.StrokeThickness = 1;
                box.Fill = FreezableUtility.GetSafeFreeze(new SolidColorBrush(fillColor));
            }
            return box;
        }

        protected Effect GetStrongEffect() => (Effect)Application.Current.Resources["Effect-Strong"];


        #endregion
    }
}
