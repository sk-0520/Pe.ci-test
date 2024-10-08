using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Library.DependencyInjection;
using Xunit;

namespace ContentTypeTextNet.Pe.Main.Test.Models.Database.Dao.Entity
{
    public class LauncherFilesEntityDaoTest
    {
        #region property

        private Test Test { get; } = Test.Create(TestSetup.Database);

        #endregion

        #region function

        [Fact]
        public void Insert_Select_Update_Delete_Test()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var testItem = Test.BuildDao<LauncherItemsEntityDao>(AccessorKind.Main);
            var testFile = Test.BuildDao<LauncherFilesEntityDao>(AccessorKind.Main);
            var item = new LauncherItemData() {
                LauncherItemId = LauncherItemId.NewId(),
                Name = "Data",
                Kind = LauncherItemKind.File,
            };
            testItem.InsertLauncherItem(item, Test.DiContainer.Build<IDatabaseCommonStatus>());

            var file = new LauncherFileData() {
                Caption = "Caption",
                IsEnabledCustomEnvironmentVariable = true,
                IsEnabledStandardInputOutput = true,
                Option = "Option",
                Path = "Path",
                RunAdministrator = true,
                ShowMode = ShowMode.Normal,
                WorkDirectoryPath = "Dir",
            };
            testFile.InsertFile(item.LauncherItemId, file, Test.DiContainer.Build<IDatabaseCommonStatus>());

            var actualPath = testFile.SelectPath(item.LauncherItemId);
            Assert.Equal(file.Option, actualPath.Option);
            Assert.Equal(file.Path, actualPath.Path);
            Assert.Equal(file.WorkDirectoryPath, actualPath.WorkDirectoryPath);

            var actualFile = testFile.SelectFile(item.LauncherItemId);
            Assert.Equal(file.Option, actualFile.Option);
            Assert.Equal(file.Path, actualFile.Path);
            Assert.Equal(file.WorkDirectoryPath, actualFile.WorkDirectoryPath);
            // RunAdministrator は挿入時 False なのだ。。。なんでだこれ
            Assert.NotEqual(file.RunAdministrator, actualFile.RunAdministrator);
            Assert.Equal(file.ShowMode, actualFile.ShowMode);
            Assert.NotEqual(file.Caption, actualFile.Caption);
            // IsEnabledCustomEnvironmentVariable は挿入時 False なのだ。。。なんでだこれ
            Assert.NotEqual(file.IsEnabledCustomEnvironmentVariable, actualFile.IsEnabledCustomEnvironmentVariable);
            // IsEnabledStandardInputOutput は挿入時 False なのだ。。。なんでだこれ
            Assert.NotEqual(file.IsEnabledStandardInputOutput, actualFile.IsEnabledStandardInputOutput);

            var updateData = new LauncherFileData() {
                Path = "Path2",
                Option = "Option2",
                WorkDirectoryPath = "WorkDirectoryPath2",
                Caption = "Caption2",
                IsEnabledCustomEnvironmentVariable = false,
                IsEnabledStandardInputOutput = false,
                RunAdministrator = false,
                ShowMode = ShowMode.Maximized,
            };
            testFile.UpdateCustomizeLauncherFile(
                item.LauncherItemId,
                updateData,
                updateData,
                Test.DiContainer.Build<IDatabaseCommonStatus>()
            );

            testFile.DeleteFileByLauncherItemId(item.LauncherItemId);
            Assert.Throws<InvalidOperationException>(() => testFile.SelectFile(item.LauncherItemId));
        }

        #endregion
    }
}
