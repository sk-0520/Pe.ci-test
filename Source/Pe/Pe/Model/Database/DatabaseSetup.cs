using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model.Database;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;

namespace ContentTypeTextNet.Pe.Main.Model.Database
{
    public class DatabaseSetup
    {
        public DatabaseSetup(ILogger logger)
        { }

        #region property

        ILogger Logger { get; }

        #endregion

        #region function

        public DatabasePack Execute()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
