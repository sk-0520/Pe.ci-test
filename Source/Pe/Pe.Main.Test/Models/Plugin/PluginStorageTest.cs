using System;
using System.Collections.Generic;
using System.IO;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Standard.DependencyInjection;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Models.Plugin;
using ContentTypeTextNet.Pe.Standard.Database;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ContentTypeTextNet.Pe.Standard.Base;

namespace ContentTypeTextNet.Pe.Main.Test.Models.Plugin
{
    [TestClass]
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

        #region function

        [TestInitialize]
        public void Initialize()
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

        [TestMethod]
        public void FileTest()
        {
            var dir = new DirectoryInfo(Path.Combine(".", pluginDataDirName, "user", this.Information.PluginIdentifiers.PluginId.ToString()));
            dir.Refresh();
            if(dir.Exists) {
                var directoryCleaner = Test.DiContainer.Build<DirectoryCleaner>(dir, 10, TimeSpan.FromMilliseconds(300));
                directoryCleaner.Clear();
            }

            var file = new PluginFileStorage(dir);
            Assert.ThrowsException<ArgumentException>(() => file.Exists(":"));
            Assert.ThrowsException<ArgumentException>(() => file.Exists("\\"));
            Assert.ThrowsException<ArgumentException>(() => file.Exists("/"));
            Assert.ThrowsException<ArgumentException>(() => file.Exists("<"));
            Assert.ThrowsException<ArgumentException>(() => file.Exists(">"));
            Assert.ThrowsException<ArgumentException>(() => file.Exists("*"));
            Assert.ThrowsException<ArgumentException>(() => file.Exists("\""));

            Assert.ThrowsException<ArgumentNullException>(() => file.Exists(default!));
            Assert.ThrowsException<ArgumentException>(() => file.Exists("  "));
            Assert.ThrowsException<ArgumentException>(() => file.Exists(""));

            Assert.IsFalse(file.Exists("file"));
            using(var stream = file.Open("file", FileMode.Create)) {
                using var writer = new StreamWriter(stream);
                writer.WriteLine("ABC");
                writer.WriteLine("DEF");
                writer.WriteLine("GHI");
            }
            Assert.IsTrue(file.Exists("file"));

            file.Copy("file", "COPY", false);
            Assert.IsTrue(file.Exists("file"));
            Assert.IsTrue(file.Exists("COPY"));

            file.Delete("file");
            Assert.IsFalse(file.Exists("file"));

            file.Rename("COPY", "file", false);
            Assert.IsFalse(file.Exists("COPY"));
            Assert.IsTrue(file.Exists("file"));

            using(var stream = file.Open("file", FileMode.Open)) {
                using var writer = new StreamReader(stream);
                Assert.AreEqual("ABC", writer.ReadLine());
                Assert.AreEqual("DEF", writer.ReadLine());
                Assert.AreEqual("GHI", writer.ReadLine());
            }

        }

        void DeletePluginSetting()
        {
            using var commander = Test.DiContainer.Build<IMainDatabaseBarrier>().WaitWrite();
            commander.Execute("delete from PluginSettings where PluginId = @PluginId", new { PluginId = this.Information.PluginIdentifiers.PluginId });
            commander.Commit();
        }

        [TestMethod]
        public void PersistenceNormalTest()
        {
            DeletePluginSetting();

            using var writableCommandsPack = Test.DiContainer.Build<IDatabaseBarrierPack>().WaitWrite();

            var persistenceNormal = new PluginPersistenceStorage(this.Information.PluginIdentifiers, this.Information.PluginVersions, writableCommandsPack.Main, Test.DiContainer.Build<IDatabaseStatementLoader>(), false, Test.DiContainer.Build<ILoggerFactory>());

            Assert.IsFalse(persistenceNormal.Exists(""));
            Assert.IsFalse(persistenceNormal.TryGet<string>("", out _));
            Assert.IsTrue(persistenceNormal.Set("", "test", PluginPersistenceFormat.Text));
            Assert.IsTrue(persistenceNormal.Exists(""));
            Assert.IsTrue(persistenceNormal.Exists(" "));

            Assert.IsFalse(persistenceNormal.TryGet<string>("x", out _));
            if(persistenceNormal.TryGet<string>("", out var test1)) {
                Assert.AreEqual("test", test1);
            } else {
                Assert.Fail();
            }

            Assert.IsFalse(persistenceNormal.Delete("x"));
            Assert.IsTrue(persistenceNormal.Delete(""));
            Assert.IsFalse(persistenceNormal.Exists(""));


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

            Assert.IsTrue(persistenceNormal.Set("data", data));
            if(persistenceNormal.TryGet<Data>("   data   ", out var test2)) {
                Assert.IsFalse(object.ReferenceEquals(data, test2));

                Assert.AreEqual(data.Int, test2.Int);
                Assert.AreEqual(data.Long, test2.Long);
                Assert.AreEqual(data.String, test2.String);
                CollectionAssert.AreEqual(data.Array, test2.Array);
                CollectionAssert.AreEqual(data.List, test2.List);
                CollectionAssert.AreEqual(data.Dictionary, test2.Dictionary);

            } else {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void PersistenceReadOnlyTest()
        {
            DeletePluginSetting();

            using(var writableCommandsPack = Test.DiContainer.Build<IDatabaseBarrierPack>().WaitWrite()) {
                var persistenceNormal = new PluginPersistenceStorage(this.Information.PluginIdentifiers, this.Information.PluginVersions, writableCommandsPack.Main, Test.DiContainer.Build<IDatabaseStatementLoader>(), false, Test.DiContainer.Build<ILoggerFactory>());
                Assert.IsTrue(persistenceNormal.Set("", "test", PluginPersistenceFormat.Text));
                Test.DiContainer.Build<IDatabaseBarrierPack>().Save();
            }

            using(var readonlyCommandsPack = Test.DiContainer.Build<IDatabaseBarrierPack>().WaitRead()) {
                var persistenceNormal = new PluginPersistenceStorage(this.Information.PluginIdentifiers, this.Information.PluginVersions, readonlyCommandsPack.Main, Test.DiContainer.Build<IDatabaseStatementLoader>(), true, Test.DiContainer.Build<ILoggerFactory>());
                if(persistenceNormal.TryGet<string>("", out var test1)) {
                    Assert.AreEqual("test", test1);
                } else {
                    Assert.Fail();
                }
                Assert.ThrowsException<InvalidOperationException>(() => persistenceNormal.Set("", "test!", PluginPersistenceFormat.Text));
                Assert.ThrowsException<InvalidOperationException>(() => persistenceNormal.Delete(""));
            }

            // 書き込み可能トランザクションでも IsReadOnly が優先される
            using(var writableCommandsPack = Test.DiContainer.Build<IDatabaseBarrierPack>().WaitWrite()) {
                var persistenceNormal = new PluginPersistenceStorage(this.Information.PluginIdentifiers, this.Information.PluginVersions, writableCommandsPack.Main, Test.DiContainer.Build<IDatabaseStatementLoader>(), true, Test.DiContainer.Build<ILoggerFactory>());
                if(persistenceNormal.TryGet<string>("", out var test1)) {
                    Assert.AreEqual("test", test1);
                } else {
                    Assert.Fail();
                }
                Assert.ThrowsException<InvalidOperationException>(() => persistenceNormal.Set("", "test!", PluginPersistenceFormat.Text));
                Assert.ThrowsException<InvalidOperationException>(() => persistenceNormal.Delete(""));
            }

        }

        [TestMethod]
        public void PersistenceBarrierTest()
        {
            DeletePluginSetting();

            var persistenceNormal = new PluginPersistenceStorage(this.Information.PluginIdentifiers, this.Information.PluginVersions, Test.DiContainer.Build<IMainDatabaseBarrier>(), Test.DiContainer.Build<IDatabaseStatementLoader>(), false, Test.DiContainer.Build<ILoggerFactory>());

            Assert.IsFalse(persistenceNormal.Exists(""));
            Assert.IsFalse(persistenceNormal.TryGet<string>("", out _));
            Assert.IsTrue(persistenceNormal.Set("", "test", PluginPersistenceFormat.Text));
            Assert.IsTrue(persistenceNormal.Exists(""));
            Assert.IsTrue(persistenceNormal.Exists(" "));

            Assert.IsFalse(persistenceNormal.TryGet<string>("x", out _));
            if(persistenceNormal.TryGet<string>("", out var test1)) {
                Assert.AreEqual("test", test1);
            } else {
                Assert.Fail();
            }

            Assert.IsFalse(persistenceNormal.Delete("x"));
            Assert.IsTrue(persistenceNormal.Delete(""));
            Assert.IsFalse(persistenceNormal.Exists(""));


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

            Assert.IsTrue(persistenceNormal.Set("data", data));
            if(persistenceNormal.TryGet<Data>("   data   ", out var test2)) {
                Assert.IsFalse(object.ReferenceEquals(data, test2));

                Assert.AreEqual(data.Int, test2.Int);
                Assert.AreEqual(data.Long, test2.Long);
                Assert.AreEqual(data.String, test2.String);
                CollectionAssert.AreEqual(data.Array, test2.Array);
                CollectionAssert.AreEqual(data.List, test2.List);
                CollectionAssert.AreEqual(data.Dictionary, test2.Dictionary);

            } else {
                Assert.Fail();
            }
        }


        [TestMethod]
        public void PersistenceLazyTest()
        {
            DeletePluginSetting();

            var persistenceNormal = new PluginPersistenceStorage(this.Information.PluginIdentifiers, this.Information.PluginVersions, Test.DiContainer.Build<IMainDatabaseBarrier>(), Test.DiContainer.Build<IMainDatabaseLazyWriter>(), Test.DiContainer.Build<IDatabaseStatementLoader>(), Test.DiContainer.Build<ILoggerFactory>());

            Assert.IsFalse(persistenceNormal.Exists(""));
            Assert.IsFalse(persistenceNormal.TryGet<string>("", out _));
            Assert.IsTrue(persistenceNormal.Set("", "test", PluginPersistenceFormat.Text));
            Assert.IsTrue(persistenceNormal.Exists(""));
            Assert.IsTrue(persistenceNormal.Exists(" "));

            Assert.IsFalse(persistenceNormal.TryGet<string>("x", out _));
            if(persistenceNormal.TryGet<string>("", out var test1)) {
                Assert.AreEqual("test", test1);
            } else {
                Assert.Fail();
            }

            Assert.IsFalse(persistenceNormal.Delete("x"));
            Assert.IsFalse(persistenceNormal.Delete("")); // 遅延処理時は成功状態不明
            Assert.IsFalse(persistenceNormal.Exists("")); // 遅延処理フラッシュにより存在しないことを検知


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

            Assert.IsTrue(persistenceNormal.Set("data", data));
            if(persistenceNormal.TryGet<Data>("   data   ", out var test2)) {
                Assert.IsFalse(object.ReferenceEquals(data, test2));

                Assert.AreEqual(data.Int, test2.Int);
                Assert.AreEqual(data.Long, test2.Long);
                Assert.AreEqual(data.String, test2.String);
                CollectionAssert.AreEqual(data.Array, test2.Array);
                CollectionAssert.AreEqual(data.List, test2.List);
                CollectionAssert.AreEqual(data.Dictionary, test2.Dictionary);

            } else {
                Assert.Fail();
            }
        }

        #endregion
    }
}
