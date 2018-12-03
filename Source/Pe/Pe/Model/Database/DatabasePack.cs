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

        public DatabaseAccessor Main { get; }
        public DatabaseAccessor Image { get; }
        public DatabaseAccessor Temporary { get; }

        #endregion
    }
}
