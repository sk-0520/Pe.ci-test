using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity
{
    public class KeyMappingsEntityDao : EntityDaoBase
    {
        #region define

        private class KeyMappingsEntityDto: CreateDtoBase
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

        public KeyMappingsEntityDao(IDatabaseContext context, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(context, statementLoader, implementation, loggerFactory)
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

        KeyMappingsEntityDto ConvertFromData(Guid keyActionId, KeyMappingData data, int sequence, IDatabaseCommonStatus databaseCommonStatus)
        {
            var modifierKeyTransfer = new EnumTransfer<ModifierKey>();
            var keyConverter = new KeyConverter();

            var dto = new KeyMappingsEntityDto() {
                KeyActionId = keyActionId,
                Key = keyConverter.ConvertToInvariantString(data.Key),
                Sequence = sequence,
                Shift = modifierKeyTransfer.ToString(data.Shift),
                Control = modifierKeyTransfer.ToString(data.Control),
                Alt = modifierKeyTransfer.ToString(data.Alt),
                Super = modifierKeyTransfer.ToString(data.Super),
            };
            databaseCommonStatus.WriteCreate(dto);

            return dto;
        }

        public IEnumerable<KeyMappingData> SelectMappings(Guid keyActionId)
        {
            var statement = LoadStatement();
            var parameter = new { KeyActionId = keyActionId };
            return Context.Query<KeyMappingsEntityDto>(statement, parameter)
                .Select(i => ConvertFromDto(i))
            ;
        }

        public bool InsertMapping(Guid keyActionId, KeyMappingData mapping, int sequence, IDatabaseCommonStatus commonStatus)
        {
            var statement = LoadStatement();
            var dto = ConvertFromData(keyActionId, mapping, sequence, commonStatus);
            return Context.Execute(statement, dto) == 1;
        }

        public int DeleteByKeyActionId(Guid keyActionId)
        {
            var statement = LoadStatement();
            var parameter = new {
                KeyActionId = keyActionId,
            };
            return Context.Execute(statement, parameter);
        }

        #endregion
    }
}
