using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Data.Dto.Domain;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Dao.Domain
{
    public class KeyActionDomainDao : DomainDaoBase
    {
        public KeyActionDomainDao(IDatabaseCommander commander, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(commander, statementLoader, implementation, loggerFactory)
        { }

        #region function

        KeyActionData ConvertFromDto(KeyActionRowDto dto)
        {
            var keyActionKindTransfer = new EnumTransfer<KeyActionKind>();
            var modifierKeyTransfer = new EnumTransfer<ModifierKey>();
            var keyConverter = new KeyConverter();

            var result = new KeyActionData() {
                KeyActionId = dto.KeyActionId,
                KeyActionKind = keyActionKindTransfer.ToEnum(dto.KeyActionKind),
                KeyActionContent = dto.KeyActionContent,
                KeyActionOption = dto.KeyActionOption,
                Key = (Key)keyConverter.ConvertFromInvariantString(dto.Key),
                Shift = modifierKeyTransfer.ToEnum(dto.Shift),
                Contrl = modifierKeyTransfer.ToEnum(dto.Contrl),
                Alt = modifierKeyTransfer.ToEnum(dto.Alt),
                Super = modifierKeyTransfer.ToEnum(dto.Super),
            };

            return result;
        }

        public IEnumerable<KeyActionData> SelectAllKeyActionsFromKind(KeyActionKind keyActionKind)
        {
            var keyActionKindTransfer = new EnumTransfer<KeyActionKind>();

            var statement = StatementLoader.LoadStatementByCurrent(GetType());
            var parameter = new { KeyActionKind = keyActionKindTransfer.ToString(keyActionKind) };
            return Commander.Query<KeyActionRowDto>(statement, parameter)
                .Select(i => ConvertFromDto(i))
            ;
        }

        #endregion
    }
}
