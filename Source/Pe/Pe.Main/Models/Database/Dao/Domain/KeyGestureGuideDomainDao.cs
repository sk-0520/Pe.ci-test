using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Dao.Domain
{
    public class KeyGestureGuideDomainDao: DomainDaoBase
    {
        #region define

        class KeyGestureGuidRowDto: RowDtoBase
        {
            #region property

            public Guid KeyActionId { get; set; }
            public long Sequence { get; set; }

            public string Key { get; set; } = string.Empty;
            public string Shift { get; set; } = string.Empty;
            public string Control { get; set; } = string.Empty;
            public string Alt { get; set; } = string.Empty;
            public string Super { get; set; } = string.Empty;

            #endregion
        }

        #endregion

        public KeyGestureGuideDomainDao(IDatabaseCommander commander, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(commander, statementLoader, implementation, loggerFactory)
        { }

        #region function

        public void SelectKeyMappings(KeyActionKind keyActionKind, string keyActionContent)
        {
            var statement = LoadStatement();
            var parameter = new {
                KeyActionKind = keyActionKind,
                KeyActionContent = keyActionContent,
            };

            var rows = Commander.Query<KeyGestureGuidRowDto>(statement, parameter);
            rows.GroupBy(i => i.KeyActionId, i => i);
        }

        #endregion
    }
}
