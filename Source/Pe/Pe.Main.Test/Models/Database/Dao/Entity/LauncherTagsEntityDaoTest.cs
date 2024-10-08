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
    public class LauncherTagsEntityDaoTest
    {
        #region property

        private Test Test { get; } = Test.Create(TestSetup.Database);

        #endregion

        #region function

        [Fact]
        public void Insert_Select_Delete_Test()
        {
            var testItems = Test.BuildDao<LauncherItemsEntityDao>(AccessorKind.Main);
            var testTags = Test.BuildDao<LauncherTagsEntityDao>(AccessorKind.Main);

            var id = LauncherItemId.NewId();

            testItems.InsertLauncherItem(new LauncherItemData() {
                LauncherItemId = id,
                Kind = LauncherItemKind.File,
                Name = "Test",
            }, Test.DiContainer.Build<IDatabaseCommonStatus>());

            Assert.Empty(testTags.SelectTags(id));

            testTags.InsertTags(id, ["tag1", "tag2"], Test.DiContainer.Build<IDatabaseCommonStatus>());
            Assert.Equal(["tag1", "tag2"], testTags.SelectTags(id));

            testTags.InsertTags(id, ["tag0"], Test.DiContainer.Build<IDatabaseCommonStatus>());
            Assert.Equal(["tag0", "tag1", "tag2"], testTags.SelectTags(id));

            Assert.Equal(3, testTags.DeleteTagByLauncherItemId(id));
            Assert.Equal(0, testTags.DeleteTagByLauncherItemId(id));

            Assert.Empty(testTags.SelectTags(id));
        }

        [Fact]
        public void SelectUniqueTagsTest()
        {
            var testItems = Test.BuildDao<LauncherItemsEntityDao>(AccessorKind.Main);
            var testTags = Test.BuildDao<LauncherTagsEntityDao>(AccessorKind.Main);

            var id = LauncherItemId.NewId();

            testItems.InsertLauncherItem(new LauncherItemData() {
                LauncherItemId = id,
                Kind = LauncherItemKind.File,
                Name = "Test",
            }, Test.DiContainer.Build<IDatabaseCommonStatus>());

            Assert.Empty(testTags.SelectUniqueTags(id));

            testTags.InsertTags(id, ["tag0", " tag0", "tag1", "tag1 ", "tag2", " tag2 "], Test.DiContainer.Build<IDatabaseCommonStatus>());
            Assert.Equal([" tag0", " tag2 ", "tag0", "tag1", "tag1 ", "tag2"], testTags.SelectTags(id));

            Assert.Equal(["tag0", "tag1", "tag2"], testTags.SelectUniqueTags(id));
        }

        [Fact]
        public void SelectAllTagsTest()
        {
            var testItems = Test.BuildDao<LauncherItemsEntityDao>(AccessorKind.Main);
            var testTags = Test.BuildDao<LauncherTagsEntityDao>(AccessorKind.Main);

            var idA = LauncherItemId.NewId();
            var idB = LauncherItemId.NewId();
            var idC = LauncherItemId.NewId();
            var idD = LauncherItemId.NewId();

            testItems.InsertLauncherItem(new LauncherItemData() {
                LauncherItemId = idA,
                Kind = LauncherItemKind.File,
                Name = "TestA",
            }, Test.DiContainer.Build<IDatabaseCommonStatus>());
            testItems.InsertLauncherItem(new LauncherItemData() {
                LauncherItemId = idB,
                Kind = LauncherItemKind.File,
                Name = "TestB",
            }, Test.DiContainer.Build<IDatabaseCommonStatus>());
            testItems.InsertLauncherItem(new LauncherItemData() {
                LauncherItemId = idC,
                Kind = LauncherItemKind.File,
                Name = "TestC",
            }, Test.DiContainer.Build<IDatabaseCommonStatus>());
            testItems.InsertLauncherItem(new LauncherItemData() {
                LauncherItemId = idD,
                Kind = LauncherItemKind.File,
                Name = "TestD",
            }, Test.DiContainer.Build<IDatabaseCommonStatus>());

            testTags.InsertTags(idA, ["tag0", "tagA", " tagA"], Test.DiContainer.Build<IDatabaseCommonStatus>());
            testTags.InsertTags(idB, ["tag0", "tagB", "tagB "], Test.DiContainer.Build<IDatabaseCommonStatus>());
            testTags.InsertTags(idC, ["tag0", "tagC", " tagC "], Test.DiContainer.Build<IDatabaseCommonStatus>());

            var actual = testTags.SelectAllTags();
            Assert.Equal(["tag0", "tagA"], actual[idA]);
            Assert.Equal(["tag0", "tagB"], actual[idB]);
            Assert.Equal(["tag0", "tagC"], actual[idC]);
            Assert.False(actual.ContainsKey(idD));
        }

        #endregion
    }
}
