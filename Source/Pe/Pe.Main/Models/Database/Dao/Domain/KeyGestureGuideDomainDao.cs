using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Library.Database;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Dao.Domain
{
    public class KeyGestureGuideDomainDao: DomainDaoBase
    {
        #region define

        private class KeyGestureGuidRowDto: RowDtoBase
        {
            #region property

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S3459:Unassigned members should be removed")]
            public KeyActionId KeyActionId { get; set; }
            public long Sequence { get; set; }

            public string Key { get; set; } = string.Empty;
            public string Shift { get; set; } = string.Empty;
            public string Control { get; set; } = string.Empty;
            public string Alt { get; set; } = string.Empty;
            public string Super { get; set; } = string.Empty;

            #endregion
        }

        #endregion

        public KeyGestureGuideDomainDao(IDatabaseContext context, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(context, statementLoader, implementation, loggerFactory)
        { }

        #region function

        private KeyMappingData ConvertFromDto(KeyGestureGuidRowDto dto)
        {
            var keyConverter = new KeyConverter();
            var modifierKeyTransfer = new EnumTransfer<ModifierKey>();
            var result = new KeyMappingData() {
                Key = (Key)keyConverter.ConvertFromInvariantString(dto.Key)!,
                Shift = modifierKeyTransfer.ToEnum(dto.Shift),
                Control = modifierKeyTransfer.ToEnum(dto.Control),
                Alt = modifierKeyTransfer.ToEnum(dto.Alt),
                Super = modifierKeyTransfer.ToEnum(dto.Super),
            };

            return result;
        }

        public KeyGestureSetting SelectKeyMappings(KeyActionKind keyActionKind, string keyActionContent)
        {
            var keyActionKindTransfer = new EnumTransfer<KeyActionKind>();

            var statement = LoadStatement();
            var parameter = new {
                KeyActionKind = keyActionKindTransfer.ToString(keyActionKind),
                KeyActionContent = keyActionContent,
            };

            var map = new Dictionary<Guid, KeyGestureSetting>();
            var keyGestureGuidRows = Context.Query<KeyGestureGuidRowDto>(statement, parameter);
            var keyGestureGuidGroups = keyGestureGuidRows.GroupBy(i => i.KeyActionId, i => i);

            var items = keyGestureGuidGroups.Select(g => new KeyGestureItem(g.Key, g.Select(i => ConvertFromDto(i)).ToArray())).ToArray();
            return new KeyGestureSetting(items);
        }

        public KeyGestureSetting SelectLauncherKeyMappings(LauncherItemId launcherItemId)
        {
            var keyActionKindTransfer = new EnumTransfer<KeyActionKind>();

            var statement = LoadStatement();
            var parameter = new {
                KeyActionKind = keyActionKindTransfer.ToString(KeyActionKind.LauncherItem),
                KeyActionContents = Enum.GetValues<KeyActionContentLauncherItem>().Select(i => i.ToString()).ToArray(),
                LauncherItemId = launcherItemId.ToString(),
            };

            var map = new Dictionary<Guid, KeyGestureSetting>();
            var keyGestureGuidRows = Context.Query<KeyGestureGuidRowDto>(statement, parameter);
            var keyGestureGuidGroups = keyGestureGuidRows.GroupBy(i => i.KeyActionId, i => i);

            var items = keyGestureGuidGroups.Select(g => new KeyGestureItem(g.Key, g.Select(i => ConvertFromDto(i)).ToArray())).ToArray();
            return new KeyGestureSetting(items);
        }

        #endregion
    }
}
