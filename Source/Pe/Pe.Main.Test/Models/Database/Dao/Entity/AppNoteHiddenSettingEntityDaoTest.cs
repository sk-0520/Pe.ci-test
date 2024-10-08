using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Library.Database;
using ContentTypeTextNet.Pe.Library.DependencyInjection;
using Xunit;

namespace ContentTypeTextNet.Pe.Main.Test.Models.Database.Dao.Entity
{
    public class AppNoteHiddenSettingEntityDaoTest
    {
        #region property

        private Test Test { get; } = Test.Create(TestSetup.Database);

        #endregion

        #region function

        [Fact]
        public void SelectHiddenWaitTimeTest()
        {
            var test = Test.BuildDao<AppNoteHiddenSettingEntityDao>(AccessorKind.Main);

            var actualCompact = test.SelectHiddenWaitTime(NoteHiddenMode.Compact);
            Assert.Equal(TimeSpan.FromMinutes(1), actualCompact);

            var actualBlind = test.SelectHiddenWaitTime(NoteHiddenMode.Blind);
            Assert.Equal(TimeSpan.FromSeconds(30), actualBlind);
        }

        [Fact]
        public void SelectHiddenWaitTimesTest()
        {
            var test = Test.BuildDao<AppNoteHiddenSettingEntityDao>(AccessorKind.Main);
            var actual = test.SelectHiddenWaitTimes();
            Assert.Equal(TimeSpan.FromMinutes(1), actual[NoteHiddenMode.Compact]);
            Assert.Equal(TimeSpan.FromSeconds(30), actual[NoteHiddenMode.Blind]);
        }

        [Fact]
        public void UpdateHiddenWaitTimeTest()
        {
            var test = Test.BuildDao<AppNoteHiddenSettingEntityDao>(AccessorKind.Main);
            test.UpdateHiddenWaitTime(NoteHiddenMode.Compact, TimeSpan.FromMinutes(10), Test.DiContainer.Build<IDatabaseCommonStatus>());
            test.UpdateHiddenWaitTime(NoteHiddenMode.Blind, TimeSpan.FromMinutes(20), Test.DiContainer.Build<IDatabaseCommonStatus>());

            var actualCompact = test.SelectHiddenWaitTime(NoteHiddenMode.Compact);
            Assert.Equal(TimeSpan.FromMinutes(10), actualCompact);

            var actualBlind = test.SelectHiddenWaitTime(NoteHiddenMode.Blind);
            Assert.Equal(TimeSpan.FromMinutes(20), actualBlind);
        }

        [Fact]
        public void UpdateHiddenWaitTimesTest()
        {
            var test = Test.BuildDao<AppNoteHiddenSettingEntityDao>(AccessorKind.Main);
            test.UpdateHiddenWaitTimes(
                new Dictionary<NoteHiddenMode, TimeSpan>() {
                    [NoteHiddenMode.Compact] = TimeSpan.FromHours(1),
                    [NoteHiddenMode.Blind] = TimeSpan.FromHours(2),
                },
                Test.DiContainer.Build<IDatabaseCommonStatus>()
            );

            var actual = test.SelectHiddenWaitTimes();
            Assert.Equal(TimeSpan.FromHours(1), actual[NoteHiddenMode.Compact]);
            Assert.Equal(TimeSpan.FromHours(2), actual[NoteHiddenMode.Blind]);
        }

        #endregion
    }
}
