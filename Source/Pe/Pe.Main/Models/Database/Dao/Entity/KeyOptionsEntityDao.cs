using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity
{
    internal class KeyOptionsEntityDto : CreateDtoBase
    {
        #region property

        public Guid KeyActionId { get; set; }
        public string KeyOptionName { get; set; } = string.Empty;
        public string KeyOptionValue { get; set; } = string.Empty;

        #endregion
    }

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
            var statement = LoadStatement();
            var parameter = new {
                KeyActionId = keyActionId,
            };
            return Commander.Query<KeyOptionsEntityDto>(statement, parameter)
                .Select(i => KeyValuePair.Create(i.KeyOptionName, i.KeyOptionValue))
            ;
        }

        public bool InsertOption(Guid keyActionId, string name, string value, IDatabaseCommonStatus commonStatus)
        {
            var statement = LoadStatement();
            var parameter = commonStatus.CreateCommonDtoMapping();
            parameter[Column.KeyActionId] = keyActionId;
            parameter[Column.KeyOptionName] = name;
            parameter[Column.KeyOptionValue] = value;
            return Commander.Execute(statement, parameter) == 1;
        }


        public int DeleteByKeyActionId(Guid keyActionId)
        {
            var statement = LoadStatement();
            var parameter = new {
                KeyActionId = keyActionId,
            };
            return Commander.Execute(statement, parameter);
        }

        #endregion
    }
}
