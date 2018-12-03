using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Library.Shared.Library.Model.Database
{

    public interface IDatabaseFactory
    {
        #region function

        IDbConnection CreateConnection();

        IDbDataAdapter CreateDataAdapter();

        IDatabaseImplementation CreateImplementation();

        #endregion
    }

}
