using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Shapes;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Library.Base;

namespace ContentTypeTextNet.Pe.Main.ViewModels
{
    /// <summary>
    /// 表示要素生成処理。
    /// </summary>
    public class ViewElementMakerBase
    {
        #region function

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
        /// </summary>
        /// <remarks>
        /// <para>同じようなものを共通の見栄えで作成するため細かい部分は内部実装に隠ぺいする。</para>
        /// </remarks>
        /// <param name="borderColor">境界線の色。</param>
        /// <param name="fillColor">背景色</param>
        /// <param name="size">サイズ。</param>
        /// <returns>生成された要素。</returns>
        public static FrameworkElement CreateBox(Color borderColor, Color fillColor, Size size)
        {
            var box = new Rectangle();
            using(Initializer.Begin(box)) {
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
