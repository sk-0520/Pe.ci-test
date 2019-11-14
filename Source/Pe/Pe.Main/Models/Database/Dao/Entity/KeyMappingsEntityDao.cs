using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Core.Models.Database;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity
{
    public class KeyMappingsEntityDao : EntityDaoBase
    {
        public KeyMappingsEntityDao(IDatabaseCommander commander, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(commander, statementLoader, implementation, loggerFactory)
        { }

        #region property

        public static class Column
        {
            #region property

            public static string KeyActionId { get; } = "KeyActionId";
            public static string Sequence { get; } = "Sequence";

            public static string Key { get; } = "Key";
            public static string Shift { get; } = "Shift";
            public static string Control { get; } = "Control";
            public static string Alt { get; } = "Alt";
            public static string Super { get; } = "Super";

            #endregion
        }

        #endregion

        #region function
        #endregion
    }
}
