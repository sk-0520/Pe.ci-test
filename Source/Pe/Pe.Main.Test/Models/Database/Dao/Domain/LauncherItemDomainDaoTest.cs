using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Domain;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Library.DependencyInjection;
using Xunit;

namespace ContentTypeTextNet.Pe.Main.Test.Models.Database.Dao.Domain
{
    public class LauncherItemDomainDaoTest
    {
        #region property

        private Test Test { get; } = Test.Create(TestSetup.Database);

        #endregion

        #region function

        [Fact]
        public void SelectFileIcon_NoFile_Test()
        {
            var testDomain = Test.BuildDao<LauncherItemDomainDao>(AccessorKind.Main);
            var testItems = Test.BuildDao<LauncherItemsEntityDao>(AccessorKind.Main);
            //var testFiles = Test.BuildDao<LauncherFilesEntityDao>(AccessorKind.Main);

            var launcherItemId = LauncherItemId.NewId();

            testItems.InsertLauncherItem(
                new LauncherItemData() {
                    LauncherItemId = launcherItemId,
                    Kind = LauncherItemKind.File,
                    Icon = new IconData() {
                        Index = 1,
                        Path = "Path"
                    }
                },
                Test.DiContainer.Build<IDatabaseCommonStatus>()
            );

            var actual = testDomain.SelectFileIcon(launcherItemId);
            Assert.Equal(LauncherItemKind.File, actual.Kind);
            Assert.Equal("", actual.Path.Path);
            Assert.Equal(0, actual.Path.Index);
            Assert.Equal("Path", actual.Icon.Path);
            Assert.Equal(1, actual.Icon.Index);
        }

        [Fact]
        public void SelectFileIcon_File_Test()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var testDomain = Test.BuildDao<LauncherItemDomainDao>(AccessorKind.Main);
            var testItems = Test.BuildDao<LauncherItemsEntityDao>(AccessorKind.Main);
            var testFiles = Test.BuildDao<LauncherFilesEntityDao>(AccessorKind.Main);

            var launcherItemId = LauncherItemId.NewId();

            testItems.InsertLauncherItem(
                new LauncherItemData() {
                    LauncherItemId = launcherItemId,
                    Kind = LauncherItemKind.File,
                    Icon = new IconData() {
                        Index = 1,
                        Path = "Path"
                    }
                },
                Test.DiContainer.Build<IDatabaseCommonStatus>()
            );

            testFiles.InsertFile(
                launcherItemId,
                new LauncherExecutePathData() {
                    Path = "File",
                },
                Test.DiContainer.Build<IDatabaseCommonStatus>()
            );

            var actual = testDomain.SelectFileIcon(launcherItemId);
            Assert.Equal(LauncherItemKind.File, actual.Kind);
            Assert.Equal("File", actual.Path.Path);
            Assert.Equal(0, actual.Path.Index);
            Assert.Equal("Path", actual.Icon.Path);
            Assert.Equal(1, actual.Icon.Index);
        }

        #endregion
    }
}
