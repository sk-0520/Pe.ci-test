using System.Windows.Media;

namespace ContentTypeTextNet.Pe.Bridge.Models
{
    /// <summary>
    /// 色とかその辺の変換処理。
    /// </summary>
    /// <remarks>
    /// <para>Pe から提供される。</para>
    /// </remarks>
    public interface IMediaConverter
    {
        #region function

        /// <summary>
        /// 色反転。
        /// </summary>
        /// <remarks>
        /// <para>透明度は保ったまま。</para>
        /// </remarks>
        /// <param name="color"></param>
        /// <returns></returns>
        Color GetNegativeColor(Color color);

        /// <summary>
        /// 補色。
        /// </summary>
        /// <remarks>
        /// <para>透明度は保ったまま。</para>
        /// </remarks>
        /// <param name="color"></param>
        /// <returns></returns>
        Color GetComplementaryColor(Color color);

        /// <summary>
        /// 明るさを算出。
        /// </summary>
        /// <param name="color">指定色。</param>
        /// <returns></returns>
        double GetBrightness(Color color);

        /// <summary>
        /// 指定色から自動的に見やすそうな色を算出。
        /// </summary>
        /// <param name="color">指定色。</param>
        /// <returns></returns>
        Color GetAutoColor(Color color);

        /// <summary>
        /// 指定色を非透明にする。
        /// </summary>
        /// <param name="value">指定色。</param>
        /// <returns></returns>
        Color GetNonTransparentColor(Color value);

        /// <summary>
        /// 色を加算する。
        /// </summary>
        /// <param name="baseColor">基本色。</param>
        /// <param name="plusColor">加算する色。</param>
        /// <returns></returns>
        Color AddColor(Color baseColor, Color plusColor);

        /// <summary>
        /// 明るさを加算する。
        /// </summary>
        /// <param name="baseColor">基本色。</param>
        /// <param name="brightness">明るさ</param>
        /// <returns></returns>
        Color AddBrightness(Color baseColor, double brightness);

        #endregion
    }
}
