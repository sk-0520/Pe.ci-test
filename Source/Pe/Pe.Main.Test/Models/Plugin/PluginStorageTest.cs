using System;
using System.Collections.Generic;
using System.IO;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Library.DependencyInjection;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Models.Plugin;
using ContentTypeTextNet.Pe.Library.Database;
using Microsoft.Extensions.Logging;
using Xunit;
using ContentTypeTextNet.Pe.Library.Base;

namespace ContentTypeTextNet.Pe.Main.Test.Models.Plugin
{
    public class PluginPersistenceStorageTest
    {
        #region define

        const string pluginDataDirName = "__test-plugin__";

        readonly PluginInformation Information = new PluginInformation(
            new PluginIdentifiers(new PluginId(new Guid("00000000-1111-2222-3333-444444444444")), "test-plugin"),
            new PluginVersions(new Version(1, 2, 3), new Version(), new Version(), Array.Empty<string>()),
            new PluginAuthors(new Author("testman"), PluginLicense.Unknown)
        );

        [Serializable]
        class Data
        {
            public int Int { get; set; }
            public long Long { get; set; }
            public string String { get; set; } = string.Empty;
            public int[] Array { get; set; } = System.Array.Empty<int>();
            public List<int> List { get; set; } = new List<int>();
            public Dictionary<string, string> Dictionary { get; set; } = new Dictionary<string, string>();
        }

        #endregion

        #region property

        private Test Test { get; } = Test.Create(TestSetup.Database);

        #endregion

        public PluginPersistenceStorageTest()
        {
            using var context = Test.DiContainer.Build<IMainDatabaseBarrier>().WaitWrite();
            var pluginsEntityDao = Test.DiContainer.Build<PluginsEntityDao>(context, context.Implementation);
            if(pluginsEntityDao.SelectExistsPlugin(this.Information.PluginIdentifiers.PluginId)) {
                return;
            }
            pluginsEntityDao.InsertPluginStateData(
                new Main.Models.Data.PluginStateData() {
                    PluginId = this.Information.PluginIdentifiers.PluginId,
                    PluginName = this.Information.PluginIdentifiers.PluginName,
                    State = Main.Models.Data.PluginState.Enable,
                },
                DatabaseCommonStatus.CreateCurrentAccount()
            );
            context.Commit();
        }

        #region function

        [Fact]
        public void FileTest()
        {
            var dir = new DirectoryInfo(Path.Combine(".", pluginDataDirName, "user", this.Information.PluginIdentifiers.PluginId.ToString()));
            dir.Refresh();
            if(dir.Exists) {
                var directoryCleaner = Test.DiContainer.Build<DirectoryCleaner>(dir, 10, TimeSpan.FromMilliseconds(300));
                directoryCleaner.Clear();
            }

            var file = new PluginFileStorage(dir);
            Assert.Throws<ArgumentException>(() => file.Exists(":"));
            Assert.Throws<ArgumentException>(() => file.Exists("\\"));
            Assert.Throws<ArgumentException>(() => file.Exists("/"));
            Assert.Throws<ArgumentException>(() => file.Exists("<"));
            Assert.Throws<ArgumentException>(() => file.Exists(">"));
            Assert.Throws<ArgumentException>(() => file.Exists("*"));
            Assert.Throws<ArgumentException>(() => file.Exists("\""));

            Assert.Throws<ArgumentNullException>(() => file.Exists(default!));
            Assert.Throws<ArgumentException>(() => file.Exists("  "));
            Assert.Throws<ArgumentException>(() => file.Exists(""));

            Assert.False(file.Exists("file"));
            using(var stream = file.Open("file", FileMode.Create)) {
                using var writer = new StreamWriter(stream);
                writer.WriteLine("ABC");
                writer.WriteLine("DEF");
                writer.WriteLine("GHI");
            }
            Assert.True(file.Exists("file"));

            file.Copy("file", "COPY", false);
            Assert.True(file.Exists("file"));
            Assert.True(file.Exists("COPY"));

            file.Delete("file");
            Assert.False(file.Exists("file"));

            file.Rename("COPY", "file", false);
            Assert.False(file.Exists("COPY"));
            Assert.True(file.Exists("file"));

            using(var stream = file.Open("file", FileMode.Open)) {
                using var writer = new StreamReader(stream);
                Assert.Equal("ABC", writer.ReadLine());
                Assert.Equal("DEF", writer.ReadLine());
                Assert.Equal("GHI", writer.ReadLine());
            }

        }

        void DeletePluginSetting()
        {
            using var commander = Test.DiContainer.Build<IMainDatabaseBarrier>().WaitWrite();
            commander.Execute("delete from PluginSettings where PluginId = @PluginId", new { PluginId = this.Information.PluginIdentifiers.PluginId });
            commander.Commit();
        }

        [Fact]
        public void PersistenceNormalTest()
        {
            DeletePluginSetting();

            using var writableCommandsPack = Test.DiContainer.Build<IDatabaseBarrierPack>().WaitWrite();

            var persistenceNormal = new PluginPersistenceStorage(this.Information.PluginIdentifiers, this.Information.PluginVersions, writableCommandsPack.Main, Test.DiContainer.Build<IDatabaseStatementLoader>(), false, Test.DiContainer.Build<ILoggerFactory>());

            Assert.False(persistenceNormal.Exists(""));
            Assert.False(persistenceNormal.TryGet<string>("", out _));
            Assert.True(persistenceNormal.Set("", "test", PluginPersistenceFormat.Text));
            Assert.True(persistenceNormal.Exists(""));
            Assert.True(persistenceNormal.Exists(" "));

            Assert.False(persistenceNormal.TryGet<string>("x", out _));
            if(persistenceNormal.TryGet<string>("", out var test1)) {
                Assert.Equal("test", test1);
            } else {
                Assert.Fail();
            }

            Assert.False(persistenceNormal.Delete("x"));
            Assert.True(persistenceNormal.Delete(""));
            Assert.False(persistenceNormal.Exists(""));


            var data = new Data() {
                Int = int.MinValue,
                Long = long.MaxValue,
                String = "STRING",
                Array = new[] { int.MinValue, 0, int.MaxValue },
                List = new List<int>() {
                    int.MaxValue, 0, int.MinValue
                },
                Dictionary = new Dictionary<string, string>() {
                    ["key"] = "value",
                }
            };

            Assert.True(persistenceNormal.Set("data", data));
            if(persistenceNormal.TryGet<Data>("   data   ", out var test2)) {
                Assert.False(object.ReferenceEquals(data, test2));

                Assert.Equal(data.Int, test2.Int);
                Assert.Equal(data.Long, test2.Long);
                Assert.Equal(data.String, test2.String);
                Assert.Equal(data.Array, test2.Array);
                Assert.Equal(data.List, test2.List);
                Assert.Equal(data.Dictionary, test2.Dictionary);

            } else {
                Assert.Fail();
            }
        }

        [Fact]
        public void PersistenceReadOnlyTest()
        {
            DeletePluginSetting();

            using(var writableCommandsPack = Test.DiContainer.Build<IDatabaseBarrierPack>().WaitWrite()) {
                var persistenceNormal = new PluginPersistenceStorage(this.Information.PluginIdentifiers, this.Information.PluginVersions, writableCommandsPack.Main, Test.DiContainer.Build<IDatabaseStatementLoader>(), false, Test.DiContainer.Build<ILoggerFactory>());
                Assert.True(persistenceNormal.Set("", "test", PluginPersistenceFormat.Text));
                Test.DiContainer.Build<IDatabaseBarrierPack>().Save();
            }

            using(var readonlyCommandsPack = Test.DiContainer.Build<IDatabaseBarrierPack>().WaitRead()) {
                var persistenceNormal = new PluginPersistenceStorage(this.Information.PluginIdentifiers, this.Information.PluginVersions, readonlyCommandsPack.Main, Test.DiContainer.Build<IDatabaseStatementLoader>(), true, Test.DiContainer.Build<ILoggerFactory>());
                if(persistenceNormal.TryGet<string>("", out var test1)) {
                    Assert.Equal("test", test1);
                } else {
                    Assert.Fail();
                }
                Assert.Throws<InvalidOperationException>(() => persistenceNormal.Set("", "test!", PluginPersistenceFormat.Text));
                Assert.Throws<InvalidOperationException>(() => persistenceNormal.Delete(""));
            }

            // 書き込み可能トランザクションでも IsReadOnly が優先される
            using(var writableCommandsPack = Test.DiContainer.Build<IDatabaseBarrierPack>().WaitWrite()) {
                var persistenceNormal = new PluginPersistenceStorage(this.Information.PluginIdentifiers, this.Information.PluginVersions, writableCommandsPack.Main, Test.DiContainer.Build<IDatabaseStatementLoader>(), true, Test.DiContainer.Build<ILoggerFactory>());
                if(persistenceNormal.TryGet<string>("", out var test1)) {
                    Assert.Equal("test", test1);
                } else {
                    Assert.Fail();
                }
                Assert.Throws<InvalidOperationException>(() => persistenceNormal.Set("", "test!", PluginPersistenceFormat.Text));
                Assert.Throws<InvalidOperationException>(() => persistenceNormal.Delete(""));
            }

        }

        [Fact]
        public void PersistenceBarrierTest()
        {
            DeletePluginSetting();

            var persistenceNormal = new PluginPersistenceStorage(this.Information.PluginIdentifiers, this.Information.PluginVersions, Test.DiContainer.Build<IMainDatabaseBarrier>(), Test.DiContainer.Build<IDatabaseStatementLoader>(), false, Test.DiContainer.Build<ILoggerFactory>());

            Assert.False(persistenceNormal.Exists(""));
            Assert.False(persistenceNormal.TryGet<string>("", out _));
            Assert.True(persistenceNormal.Set("", "test", PluginPersistenceFormat.Text));
            Assert.True(persistenceNormal.Exists(""));
            Assert.True(persistenceNormal.Exists(" "));

            Assert.False(persistenceNormal.TryGet<string>("x", out _));
            if(persistenceNormal.TryGet<string>("", out var test1)) {
                Assert.Equal("test", test1);
            } else {
                Assert.Fail();
            }

            Assert.False(persistenceNormal.Delete("x"));
            Assert.True(persistenceNormal.Delete(""));
            Assert.False(persistenceNormal.Exists(""));


            var data = new Data() {
                Int = int.MinValue,
                Long = long.MaxValue,
                String = "STRING",
                Array = new[] { int.MinValue, 0, int.MaxValue },
                List = new List<int>() {
                    int.MaxValue, 0, int.MinValue
                },
                Dictionary = new Dictionary<string, string>() {
                    ["key"] = "value",
                }
            };

            Assert.True(persistenceNormal.Set("data", data));
            if(persistenceNormal.TryGet<Data>("   data   ", out var test2)) {
                Assert.False(object.ReferenceEquals(data, test2));

                Assert.Equal(data.Int, test2.Int);
                Assert.Equal(data.Long, test2.Long);
                Assert.Equal(data.String, test2.String);
                Assert.Equal(data.Array, test2.Array);
                Assert.Equal(data.List, test2.List);
                Assert.Equal(data.Dictionary, test2.Dictionary);

            } else {
                Assert.Fail();
            }
        }


        [Fact]
        public void PersistenceLazyTest()
        {
            DeletePluginSetting();

            var persistenceNormal = new PluginPersistenceStorage(this.Information.PluginIdentifiers, this.Information.PluginVersions, Test.DiContainer.Build<IMainDatabaseBarrier>(), Test.DiContainer.Build<IMainDatabaseDelayWriter>(), Test.DiContainer.Build<IDatabaseStatementLoader>(), Test.DiContainer.Build<ILoggerFactory>());

            Assert.False(persistenceNormal.Exists(""));
            Assert.False(persistenceNormal.TryGet<string>("", out _));
            Assert.True(persistenceNormal.Set("", "test", PluginPersistenceFormat.Text));
            Assert.True(persistenceNormal.Exists(""));
            Assert.True(persistenceNormal.Exists(" "));

            Assert.False(persistenceNormal.TryGet<string>("x", out _));
            if(persistenceNormal.TryGet<string>("", out var test1)) {
                Assert.Equal("test", test1);
            } else {
                Assert.Fail();
            }

            Assert.False(persistenceNormal.Delete("x"));
            Assert.False(persistenceNormal.Delete("")); // 遅延処理時は成功状態不明
            Assert.False(persistenceNormal.Exists("")); // 遅延処理フラッシュにより存在しないことを検知


            var data = new Data() {
                Int = int.MinValue,
                Long = long.MaxValue,
                String = "STRING",
                Array = new[] { int.MinValue, 0, int.MaxValue },
                List = new List<int>() {
                    int.MaxValue, 0, int.MinValue
                },
                Dictionary = new Dictionary<string, string>() {
                    ["key"] = "value",
                }
            };

            Assert.True(persistenceNormal.Set("data", data));
            if(persistenceNormal.TryGet<Data>("   data   ", out var test2)) {
                Assert.False(object.ReferenceEquals(data, test2));

                Assert.Equal(data.Int, test2.Int);
                Assert.Equal(data.Long, test2.Long);
                Assert.Equal(data.String, test2.String);
                Assert.Equal(data.Array, test2.Array);
                Assert.Equal(data.List, test2.List);
                Assert.Equal(data.Dictionary, test2.Dictionary);

            } else {
                Assert.Fail();
            }
        }

        #endregion
    }
}
