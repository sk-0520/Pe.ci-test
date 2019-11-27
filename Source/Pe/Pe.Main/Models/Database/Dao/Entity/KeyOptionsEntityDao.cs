using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data.Dto.Entity;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity
{
    public class KeyOptionsEntityDao : EntityDaoBase
    {
        public KeyOptionsEntityDao(IDatabaseCommander commander, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(commander, statementLoader, implementation, loggerFactory)
        { }

        #region property

        public static class Column
        {
            #region property

            public static string KeyActionId { get; } = "KeyActionId";
            public static string KeyOptionName { get; } = "KeyOptionName";
            public static string KeyOptionValue { get; } = "KeyOptionValue";

            #endregion
        }

        #endregion

        #region function

        public IEnumerable<KeyValuePair<string, string>> SelectOptions(Guid keyActionId)
        {
            var builder = CreateSelectBuilder();
            builder.AddSelect(Column.KeyActionId);
            builder.AddSelect(Column.KeyOptionName);
            builder.AddSelect(Column.KeyOptionValue);
            builder.AddValueParameter(Column.KeyActionId, keyActionId);
            return Select<KeyOptionsEntityDto>(builder)
                .Select(i => KeyValuePair.Create(i.KeyOptionName, i.KeyOptionValue))
            ;
        }

    #endregion
}
}
