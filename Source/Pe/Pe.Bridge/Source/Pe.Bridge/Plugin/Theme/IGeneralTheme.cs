using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using ContentTypeTextNet.Pe.Bridge.Models.Data;

namespace ContentTypeTextNet.Pe.Bridge.Plugin.Theme
{
    public enum GeneralGeometryImageKind
    {
        Menu,
    }

    public interface IGeneralTheme
    {
        #region function

        /// <summary>
        /// パスのイメージを取得。
        /// </summary>
        /// <param name="kind"></param>
        /// <param name="iconBox">イメージの基準サイズ。本体側で調整されるためどれくらいの大きさで使用されるかの指針にのみ使用する。</param>
        /// <returns></returns>
        Geometry GetGeometryImage(GeneralGeometryImageKind kind, IconBox iconBox);

        #endregion
    }
}
