using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Data.Dto.Entity;
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

        KeyMappingData ConvertFromDto(KeyMappingsEntityDto dto)
        {
            var modifierKeyTransfer = new EnumTransfer<ModifierKey>();
            var keyConverter = new KeyConverter();

            var result = new KeyMappingData() {
                Key = (Key)keyConverter.ConvertFromInvariantString(dto.Key),
                Shift = modifierKeyTransfer.ToEnum(dto.Shift),
                Control = modifierKeyTransfer.ToEnum(dto.Control),
                Alt = modifierKeyTransfer.ToEnum(dto.Alt),
                Super = modifierKeyTransfer.ToEnum(dto.Super),
            };

            return result;
        }

        public IEnumerable<KeyMappingData> SelectMappings(Guid keyActionId)
        {
            var statement = LoadStatement();
            var parameter = new { KeyActionId = keyActionId };
            return Commander.Query<KeyMappingsEntityDto>(statement, parameter)
                .Select(i => ConvertFromDto(i))
            ;
        }

        #endregion
    }
}
