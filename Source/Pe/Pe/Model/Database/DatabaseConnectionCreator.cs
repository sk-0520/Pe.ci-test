using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Main.Model.Database
{

    public interface IDatabaseConnectionCreator<TDbConnection>
        where TDbConnection : IDbConnection
    {
        #region function

        TDbConnection CreateConnection();

        #endregion
    }

    public class DatabaseConnectionCreator<TDbConnection> : IDatabaseConnectionCreator<TDbConnection>
        where TDbConnection : IDbConnection, new()
    {
        #region IDatabaseConnectionCreator

        public virtual TDbConnection CreateConnection()
        {
            var con = new TDbConnection();
            return con;
        }

        #endregion
    }
}
