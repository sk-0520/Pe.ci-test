using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Library.Database;
using Microsoft.Extensions.Logging;
using static ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity.AppNoteSettingEntityDao;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity
{
    public class LauncherSeparatorsEntityDao: EntityDaoBase
    {
        #region define

        private sealed class LauncherSeparatorsEntityDto: CommonDtoBase
        {
            #region property

            public Guid LauncherItemId { get; set; }

            public string Kind { get; set; } = string.Empty;
            public long Width { get; set; }

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
                Kind = launcherSeparatorKindTransfer.ToString(data.Kind),
                Width = data.Width,
            };

            commonStatus.WriteCommonTo(dto);

            return dto;
        }

        private LauncherSeparatorData ConvertFromDto(LauncherSeparatorsEntityDto dto)
        {
            var launcherSeparatorKindTransfer = new EnumTransfer<LauncherSeparatorKind>();

            var data = new LauncherSeparatorData() {
                Kind = launcherSeparatorKindTransfer.ToEnum(dto.Kind),
                Width = (int)dto.Width,
            };

            return data;
        }

        public LauncherSeparatorData SelectSeparator(LauncherItemId launcherItemId)
        {
            var statement = LoadStatement();
            var parameter = new {
                LauncherItemId = launcherItemId,
            };

            var dto = Context.QuerySingle<LauncherSeparatorsEntityDto>(statement, parameter);
            var data = ConvertFromDto(dto);
            return data;
        }

        public void InsertSeparator(LauncherItemId launcherItemId, LauncherSeparatorData data, IDatabaseCommonStatus databaseCommonStatus)
        {
            var statement = LoadStatement();
            var dto = ConvertFromData(launcherItemId, data, databaseCommonStatus);

            Context.InsertSingle(statement, dto);
        }

        public void UpdateSeparator(LauncherItemId launcherItemId, LauncherSeparatorData data, IDatabaseCommonStatus databaseCommonStatus)
        {
            var statement = LoadStatement();
            var dto = ConvertFromData(launcherItemId, data, databaseCommonStatus);

            Context.UpdateByKey(statement, dto);
        }

        public bool DeleteSeparatorByLauncherItemId(LauncherItemId launcherItemId)
        {
            var statement = LoadStatement();
            var parameter = new {
                LauncherItemId = launcherItemId,
            };

            return Context.DeleteByKeyOrNothing(statement, parameter);
        }


        #endregion
    }
}
