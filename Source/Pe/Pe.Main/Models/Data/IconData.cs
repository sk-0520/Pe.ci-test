using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using ContentTypeTextNet.Pe.Core.Models.Data;

namespace ContentTypeTextNet.Pe.Main.Models.Data
{
    [DataContract, Serializable]
    public class IconData : DataBase
    {
        public IconData()
        { }

        public IconData(IconData? source)
        {
            Path = source?.Path ?? string.Empty;
            Index = source?.Index ?? 0;
        }

        #region property

        public string? Path { get; set; }
        public int Index { get; set; }

        #endregion

        #region function

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
