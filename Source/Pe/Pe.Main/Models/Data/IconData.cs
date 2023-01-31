using System;
using System.Runtime.Serialization;

namespace ContentTypeTextNet.Pe.Main.Models.Data
{
    public interface IReadOnlyIconData
    {
        #region property

        /// <summary>
        /// アイコンパス。
        /// </summary>
        string Path { get; }
        /// <summary>
        /// アイコンインデックス。
        /// </summary>
        int Index { get; }

        #endregion
    }

    [DataContract, Serializable]
    public class IconData: IReadOnlyIconData
    {
        public IconData()
        { }

        #region property
        #endregion

        #region function

        #endregion

        #region IReadOnlyIconData

        /// <inheritdoc cref="IReadOnlyIconData.Path"/>
        public string Path { get; set; } = string.Empty;
        /// <inheritdoc cref="IReadOnlyIconData.Index"/>
        public int Index { get; set; }

        #endregion

        #region DataBase

        /// <summary>
        /// アイコンのアドレスをインデックス付きで文字列化。
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if(string.IsNullOrWhiteSpace(Path)) {
                if(Index > 0) {
                    return $":{nameof(Index)} = {Index}";
                } else {
                    return string.Empty;
                }
            } else {
                return $"{Path},{Index}";
            }
        }

        #endregion
    }
}
