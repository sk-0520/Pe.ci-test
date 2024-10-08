using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Library.Database;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity
{
    public class AppNoteHiddenSettingEntityDao: EntityDaoBase
    {
        #region define

        private sealed class AppNoteHiddenSettingEntityDto: CommonDtoBase
        {
            #region property
            public string HiddenMode { get; set; } = string.Empty;
            public TimeSpan WaitTime { get; set; }
            #endregion
        }

        #endregion

        public AppNoteHiddenSettingEntityDao(IDatabaseContext context, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(context, statementLoader, implementation, loggerFactory)
        { }

        #region function

        public TimeSpan SelectHiddenWaitTime(NoteHiddenMode hiddenMode)
        {
            var noteHiddenModeTransfer = new EnumTransfer<NoteHiddenMode>();

            var statement = LoadStatement();
            var parameter = new {
                HiddenMode = noteHiddenModeTransfer.ToString(hiddenMode),
            };

            return Context.QueryFirst<TimeSpan>(statement, parameter);
        }

        public IDictionary<NoteHiddenMode, TimeSpan> SelectHiddenWaitTimes()
        {
            var noteHiddenModeTransfer = new EnumTransfer<NoteHiddenMode>();

            var statement = LoadStatement();

            var items = Context.Query<AppNoteHiddenSettingEntityDto>(statement, null, false);
            return items.ToDictionary(
                k => noteHiddenModeTransfer.ToEnum(k.HiddenMode),
                v => v.WaitTime
            );
        }

        public void UpdateHiddenWaitTimes(IReadOnlyDictionary<NoteHiddenMode, TimeSpan> items, IDatabaseCommonStatus databaseCommonStatus)
        {
            foreach(var item in items) {
                UpdateHiddenWaitTime(item.Key, item.Value, databaseCommonStatus);
            }
        }

        public void UpdateHiddenWaitTime(NoteHiddenMode hiddenMode, TimeSpan waitTime, IDatabaseCommonStatus databaseCommonStatus)
        {
            var noteHiddenModeTransfer = new EnumTransfer<NoteHiddenMode>();

            var statement = LoadStatement();
            var parameter = new AppNoteHiddenSettingEntityDto() {
                HiddenMode = noteHiddenModeTransfer.ToString(hiddenMode),
                WaitTime = waitTime,
            };
            databaseCommonStatus.WriteCommonTo(parameter);

            Context.UpdateByKey(statement, parameter);
        }

        #endregion
    }
}
