using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.PeData.Define;
using ContentTypeTextNet.Pe.Library.PeData.Item;
using NUnit.Framework;

namespace ContentTypeTextNet.Test.Library.PeDataTest.Item
{
    [TestFixture]
    class LauncherItemModelTest
    {
        [Test]
        public void DeepCloneTest()
        {
            var src = new LauncherItemModel() {
                Administrator = true,
                Command = "command",
                Comment = "comment",
                History = new LauncherHistoryItemModel() {
                    CreateTimestamp = DateTime.Now,
                    ExecuteCount = 999,
                    ExecuteTimestamp = DateTime.UtcNow,
                    UpdateCount = 888,
                    UpdateTimestamp = DateTime.MaxValue,
                },
                Icon = new IconItemModel() {
                    Index = 7,
                    Path = "path",
                },
                Id = Guid.NewGuid(),
                IsCommandAutocomplete = true,
                LauncherKind = LauncherKind.Command,
                Name = "name",
                StdStream = new LauncherStdStreamItemModel() {
                    InputUsing = true,
                    OutputWatch = true,
                },
                Option = "option",
                WorkDirectoryPath = "work",
                EnvironmentVariables = new EnvironmentVariablesItemModel() {
                    Edit = true,
                    Remove = new ContentTypeTextNet.Library.SharedLibrary.Model.CollectionModel<string>(new[] { "A", "B" }),
                },
            };
            src.History.Options.InitializeRange(new[] { "a", "b" });
            src.History.WorkDirectoryPaths.InitializeRange(new[] { "a", "b" });
            src.EnvironmentVariables.Update.InitializeRange(new[] { new EnvironmentVariableUpdateItemModel() { Id = "#", Value = "v" } });
            src.Tag.Items.InitializeRange(new[] { "aa", "bb" });

            var dst = (LauncherItemModel)src.DeepClone();

            Assert.IsTrue(src.Administrator == dst.Administrator);
            Assert.IsTrue(src.Command == dst.Command);
            Assert.IsTrue(src.Comment == dst.Comment);
            Assert.IsTrue(src.History != dst.History);
            Assert.IsTrue(src.History.CreateTimestamp == dst.History.CreateTimestamp);
            Assert.IsTrue(src.History.ExecuteCount == dst.History.ExecuteCount);
            Assert.IsTrue(src.History.ExecuteTimestamp == dst.History.ExecuteTimestamp);
            Assert.IsTrue(src.History.UpdateCount == dst.History.UpdateCount);
            Assert.IsTrue(src.History.UpdateTimestamp == dst.History.UpdateTimestamp);
            Assert.IsTrue(src.Icon != dst.Icon);
            Assert.IsTrue(src.Icon.Index == dst.Icon.Index);
            Assert.IsTrue(src.Icon.Path == dst.Icon.Path);
            Assert.IsTrue(src.Id == dst.Id);
            Assert.IsTrue(src.IsCommandAutocomplete == dst.IsCommandAutocomplete);
            Assert.IsTrue(src.LauncherKind == dst.LauncherKind);
            Assert.IsTrue(src.Name == dst.Name);
            Assert.IsTrue(src.StdStream.InputUsing == dst.StdStream.InputUsing);
            Assert.IsTrue(src.StdStream.OutputWatch == dst.StdStream.OutputWatch);
            Assert.IsTrue(src.Option == dst.Option);
            Assert.IsTrue(src.WorkDirectoryPath == dst.WorkDirectoryPath);
            Assert.IsTrue(src.StdStream != dst.StdStream);
            Assert.IsTrue(src.StdStream.InputUsing == dst.StdStream.InputUsing);
            Assert.IsTrue(src.StdStream.OutputWatch == dst.StdStream.OutputWatch);
            Assert.IsTrue(src.EnvironmentVariables != dst.EnvironmentVariables);
            Assert.IsTrue(src.EnvironmentVariables.Edit == dst.EnvironmentVariables.Edit);
            Assert.IsTrue(src.EnvironmentVariables.Remove != dst.EnvironmentVariables.Remove);
            Assert.IsTrue(src.EnvironmentVariables.Update != dst.EnvironmentVariables.Update);
            Assert.IsTrue(src.Tag != dst.Tag);
            Assert.IsTrue(src.Tag.Items != dst.Tag.Items);
            Assert.IsTrue(src.Tag.Items.SequenceEqual(dst.Tag.Items));
        }
    }
}
