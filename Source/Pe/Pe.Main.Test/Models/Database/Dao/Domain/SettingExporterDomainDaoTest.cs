using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Domain;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Library.DependencyInjection;
using Xunit;

namespace ContentTypeTextNet.Pe.Main.Test.Models.Database.Dao.Domain
{
    public class SettingExporterDomainDaoTest
    {
        #region property

        private Test Test { get; } = Test.Create(TestSetup.Database);

        #endregion

        #region function

        [Fact]
        public void SelectSettingGroups_Empty_Test()
        {
            var testDomain = Test.BuildDao<SettingExporterDomainDao>(AccessorKind.Main);

            var actual = testDomain.SelectSettingGroups();
            Assert.Empty(actual);
        }

        [Fact]
        public void SelectSettingLauncherItems_Empty_Test()
        {
            var testDomain = Test.BuildDao<SettingExporterDomainDao>(AccessorKind.Main);

            var actual = testDomain.SelectSettingLauncherItems();
            Assert.Empty(actual);
        }

        [Fact]
        public void SelectSettingNotes_Empty_Test()
        {
            var testDomain = Test.BuildDao<SettingExporterDomainDao>(AccessorKind.Main);

            var actual = testDomain.SelectSettingNotes();
            Assert.Empty(actual);
        }

        #endregion
    }
}
