using System;
using System.Collections.Generic;
using System.Linq;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Library.Database;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity
{
    public class KeyOptionsEntityDao: EntityDaoBase
    {
        #region define

        private sealed class KeyOptionsEntityDto: CreateDtoBase
        {
            #region property

            public Guid KeyActionId { get; set; }
            public string KeyOptionName { get; set; } = string.Empty;
            public string KeyOptionValue { get; set; } = string.Empty;

            #endregion
        }

        private static class Column
        {
            #region property

            public static string KeyActionId { get; } = "KeyActionId";
            public static string KeyOptionName { get; } = "KeyOptionName";
            public static string KeyOptionValue { get; } = "KeyOptionValue";

            #endregion
        }

        #endregion

        public KeyOptionsEntityDao(IDatabaseContext context, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(context, statementLoader, implementation, loggerFactory)
        { }

        #region function

        public IEnumerable<KeyValuePair<string, string>> SelectOptions(KeyActionId keyActionId)
        {
            var statement = LoadStatement();
            var parameter = new {
                KeyActionId = keyActionId,
            };
            return Context.Query<KeyOptionsEntityDto>(statement, parameter)
                .Select(i => KeyValuePair.Create(i.KeyOptionName, i.KeyOptionValue))
            ;
        }

        public void InsertOption(KeyActionId keyActionId, string name, string value, IDatabaseCommonStatus commonStatus)
        {
            var statement = LoadStatement();
            var parameter = commonStatus.CreateCommonDtoMapping();
            parameter[Column.KeyActionId] = keyActionId;
            parameter[Column.KeyOptionName] = name;
            parameter[Column.KeyOptionValue] = value;

            Context.InsertSingle(statement, parameter);
        }


        public int DeleteByKeyActionId(KeyActionId keyActionId)
        {
            var statement = LoadStatement();
            var parameter = new {
                KeyActionId = keyActionId,
            };

            return Context.Delete(statement, parameter);
        }

        #endregion
    }
}
