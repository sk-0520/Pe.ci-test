using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Standard.Database;
using Microsoft.Extensions.Logging;
using static ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity.AppNoteSettingEntityDao;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity
{
    public class LauncherSeparatorsEntityDao: EntityDaoBase
    {
        #region define

        private class LauncherSeparatorsEntityDto: CommonDtoBase
        {
            #region property

            public Guid LauncherItemId { get; set; }

            public string SeparatorKind { get; set; } = string.Empty;
            public long SeparatorWidth { get; set; }

            #endregion
        }

        #endregion

        public LauncherSeparatorsEntityDao(IDatabaseContext context, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
           : base(context, statementLoader, implementation, loggerFactory)
        { }

        #region function

        private LauncherSeparatorsEntityDto ConvertFromData(LauncherItemId launcherItemId, LauncherSeparatorData data, IDatabaseCommonStatus commonStatus)
        {
            var launcherSeparatorKindTransfer = new EnumTransfer<LauncherSeparatorKind>();

            var dto = new LauncherSeparatorsEntityDto() {
                LauncherItemId = launcherItemId.Id,
                SeparatorKind = launcherSeparatorKindTransfer.ToString(data.Kind),
                SeparatorWidth = data.Width,
            };

            commonStatus.WriteCommonTo(dto);

            return dto;
        }

        private LauncherSeparatorData ConvertFromDto(LauncherSeparatorsEntityDto dto)
        {
            var launcherSeparatorKindTransfer = new EnumTransfer<LauncherSeparatorKind>();

            var data = new LauncherSeparatorData() {
                Kind = launcherSeparatorKindTransfer.ToEnum(dto.SeparatorKind),
                Width = (int)dto.SeparatorWidth,
            };

            return data;
        }


        #endregion
    }
}
