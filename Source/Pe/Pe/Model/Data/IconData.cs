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

        public static IconData None { get; } = new IconData() {
            Path = string.Empty,
            Index = 0,
        };

        public string Path { get; set; }
        public int Index { get; set; }

        #endregion
    }
}
