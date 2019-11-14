using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Core.Models.Database;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity
{
    public class KeyActionsEntityDao : EntityDaoBase
    {
        public KeyActionsEntityDao(IDatabaseCommander commander, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(commander, statementLoader, implementation, loggerFactory)
        { }

        #region property

        public static class Column
        {
            #region property

            public static string KeyMappingId { get; } = "KeyMappingId";
            public static string KeyActionKind { get; } = "KeyActionKind";
            public static string KeyActionContent { get; } = "KeyActionContent";
            public static string Comment { get; } = "Comment";

            #endregion
        }

        #endregion

        #region function
        #endregion
    }
}
