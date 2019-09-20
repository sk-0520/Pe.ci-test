using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;

namespace ContentTypeTextNet.Pe.Main.Model.Data
{
    [DataContract, Serializable]
    public class IconData : DataBase
    {
        #region property

        public string Path { get; set; }
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
