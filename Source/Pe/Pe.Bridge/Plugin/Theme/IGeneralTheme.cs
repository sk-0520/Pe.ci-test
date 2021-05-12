using System.Windows.Media;
using ContentTypeTextNet.Pe.Bridge.Models.Data;

namespace ContentTypeTextNet.Pe.Bridge.Plugin.Theme
{
    /// <summary>
    /// 基本テーマのパスのイメージ種別。
    /// </summary>
    public enum GeneralPathImageKind
    {
        /// <summary>
        /// メニュー。
        /// </summary>
        Menu,
    }

    /// <summary>
    /// 基本テーマ。
    /// </summary>
    public interface IGeneralTheme
    {
        #region function

        /// <summary>
        /// パスのイメージを取得。
        /// </summary>
        /// <param name="kind"></param>
        /// <param name="iconScale">イメージの基準サイズ。本体側で調整されるためどれくらいの大きさで使用されるかの指針にのみ使用する。</param>
        /// <returns></returns>
        Geometry GetPathImage(GeneralPathImageKind kind, in IconScale iconScale);

        #endregion
    }
}
