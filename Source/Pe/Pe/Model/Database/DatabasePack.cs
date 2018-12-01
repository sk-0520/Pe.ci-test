using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model.Database;

namespace ContentTypeTextNet.Pe.Main.Model.Database
{
    public sealed class DatabasePack
    {
        #region property

        public DatabaseAccessorBase Main { get; }
        public DatabaseAccessorBase Image { get; }
        public DatabaseAccessorBase Temporary { get; }

        #endregion
    }
}
