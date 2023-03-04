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
    public class PluginPersistentStorageTest
    {
        #region define

        const string pluginDataDirName = "__test-plugin__";

        readonly PluginInformations Informations = new PluginInformations(
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

        //var pluginStorage = new PluginStorage(
        //    new PluginFile(
        //        new PluginFileStorage(new DirectoryInfo(Path.Combine(".", pluginDataDirName, "user", this.Informations.PluginIdentifiers.PluginId.ToString("D")))),
        //        new PluginFileStorage(new DirectoryInfo(Path.Combine(".", pluginDataDirName, "machine", this.Informations.PluginIdentifiers.PluginId.ToString("D")))),
        //        new PluginFileStorage(new DirectoryInfo(Path.Combine(".", pluginDataDirName, "temp", this.Informations.PluginIdentifiers.PluginId.ToString("D"))))
        //    ),
        //    new PluginPersistent(
        //        new PluginPersistentStorage(this.Informations.PluginIdentifiers, this.Informations.PluginVersions, writableCommandsPack.Main, Test.DiContainer.Build<IDatabaseStatementLoader>(), false, Test.DiContainer.Build<ILoggerFactory>()),
        //        new PluginPersistentStorage(this.Informations.PluginIdentifiers, this.Informations.PluginVersions, writableCommandsPack.File, Test.DiContainer.Build<IDatabaseStatementLoader>(), false, Test.DiContainer.Build<ILoggerFactory>()),
        //        new PluginPersistentStorage(this.Informations.PluginIdentifiers, this.Informations.PluginVersions, writableCommandsPack.Temporary, Test.DiContainer.Build<IDatabaseStatementLoader>(), false, Test.DiContainer.Build<ILoggerFactory>())
        //    )
        //);

        [TestInitialize]
        public void Initialize()
        {
            using var context = Test.DiContainer.Build<IMainDatabaseBarrier>().WaitWrite();
            var pluginsEntityDao = Test.DiContainer.Build<PluginsEntityDao>(context, context.Implementation);
            if(pluginsEntityDao.SelecteExistsPlugin(this.Informations.PluginIdentifiers.PluginId)) {
                return;
            }
            pluginsEntityDao.InsertPluginStateData(
                new Main.Models.Data.PluginStateData() {
                    PluginId = this.Informations.PluginIdentifiers.PluginId,
                    PluginName = this.Informations.PluginIdentifiers.PluginName,
                    State = Main.Models.Data.PluginState.Enable,
                },
                DatabaseCommonStatus.CreateCurrentAccount()
            );
            context.Commit();
        }

        [TestMethod]
        public void FileTest()
        {
            var dir = new DirectoryInfo(Path.Combine(".", pluginDataDirName, "user", this.Informations.PluginIdentifiers.PluginId.ToString()));
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
            using var commadner = Test.DiContainer.Build<IMainDatabaseBarrier>().WaitWrite();
            commadner.Execute("delete from PluginSettings where PluginId = @PluginId", new { PluginId = this.Informations.PluginIdentifiers.PluginId });
            commadner.Commit();
        }

        [TestMethod]
        public void PersistentNormalTest()
        {
            DeletePluginSetting();

            using var writableCommandsPack = Test.DiContainer.Build<IDatabaseBarrierPack>().WaitWrite();

            var persistentNormal = new PluginPersistentStorage(this.Informations.PluginIdentifiers, this.Informations.PluginVersions, writableCommandsPack.Main, Test.DiContainer.Build<IDatabaseStatementLoader>(), false, Test.DiContainer.Build<ILoggerFactory>());

            Assert.IsFalse(persistentNormal.Exists(""));
            Assert.IsFalse(persistentNormal.TryGet<string>("", out _));
            Assert.IsTrue(persistentNormal.Set("", "test", PluginPersistentFormat.Text));
            Assert.IsTrue(persistentNormal.Exists(""));
            Assert.IsTrue(persistentNormal.Exists(" "));

            Assert.IsFalse(persistentNormal.TryGet<string>("x", out _));
            if(persistentNormal.TryGet<string>("", out var test1)) {
                Assert.AreEqual("test", test1);
            } else {
                Assert.Fail();
            }

            Assert.IsFalse(persistentNormal.Delete("x"));
            Assert.IsTrue(persistentNormal.Delete(""));
            Assert.IsFalse(persistentNormal.Exists(""));


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

            Assert.IsTrue(persistentNormal.Set("data", data));
            if(persistentNormal.TryGet<Data>("   data   ", out var test2)) {
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
        public void PersistentReadOnlyTest()
        {
            DeletePluginSetting();

            using(var writableCommandsPack = Test.DiContainer.Build<IDatabaseBarrierPack>().WaitWrite()) {
                var persistentNormal = new PluginPersistentStorage(this.Informations.PluginIdentifiers, this.Informations.PluginVersions, writableCommandsPack.Main, Test.DiContainer.Build<IDatabaseStatementLoader>(), false, Test.DiContainer.Build<ILoggerFactory>());
                Assert.IsTrue(persistentNormal.Set("", "test", PluginPersistentFormat.Text));
                Test.DiContainer.Build<IDatabaseBarrierPack>().Save();
            }

            using(var readonlyCommandsPack = Test.DiContainer.Build<IDatabaseBarrierPack>().WaitRead()) {
                var persistentNormal = new PluginPersistentStorage(this.Informations.PluginIdentifiers, this.Informations.PluginVersions, readonlyCommandsPack.Main, Test.DiContainer.Build<IDatabaseStatementLoader>(), true, Test.DiContainer.Build<ILoggerFactory>());
                if(persistentNormal.TryGet<string>("", out var test1)) {
                    Assert.AreEqual("test", test1);
                } else {
                    Assert.Fail();
                }
                Assert.ThrowsException<InvalidOperationException>(() => persistentNormal.Set("", "test!", PluginPersistentFormat.Text));
                Assert.ThrowsException<InvalidOperationException>(() => persistentNormal.Delete(""));
            }

            // 書き込み可能トランザクションでも IsReadOnly が優先される
            using(var writableCommandsPack = Test.DiContainer.Build<IDatabaseBarrierPack>().WaitWrite()) {
                var persistentNormal = new PluginPersistentStorage(this.Informations.PluginIdentifiers, this.Informations.PluginVersions, writableCommandsPack.Main, Test.DiContainer.Build<IDatabaseStatementLoader>(), true, Test.DiContainer.Build<ILoggerFactory>());
                if(persistentNormal.TryGet<string>("", out var test1)) {
                    Assert.AreEqual("test", test1);
                } else {
                    Assert.Fail();
                }
                Assert.ThrowsException<InvalidOperationException>(() => persistentNormal.Set("", "test!", PluginPersistentFormat.Text));
                Assert.ThrowsException<InvalidOperationException>(() => persistentNormal.Delete(""));
            }

        }

        [TestMethod]
        public void PersistentBarrierTest()
        {
            DeletePluginSetting();

            var persistentNormal = new PluginPersistentStorage(this.Informations.PluginIdentifiers, this.Informations.PluginVersions, Test.DiContainer.Build<IMainDatabaseBarrier>(), Test.DiContainer.Build<IDatabaseStatementLoader>(), false, Test.DiContainer.Build<ILoggerFactory>());

            Assert.IsFalse(persistentNormal.Exists(""));
            Assert.IsFalse(persistentNormal.TryGet<string>("", out _));
            Assert.IsTrue(persistentNormal.Set("", "test", PluginPersistentFormat.Text));
            Assert.IsTrue(persistentNormal.Exists(""));
            Assert.IsTrue(persistentNormal.Exists(" "));

            Assert.IsFalse(persistentNormal.TryGet<string>("x", out _));
            if(persistentNormal.TryGet<string>("", out var test1)) {
                Assert.AreEqual("test", test1);
            } else {
                Assert.Fail();
            }

            Assert.IsFalse(persistentNormal.Delete("x"));
            Assert.IsTrue(persistentNormal.Delete(""));
            Assert.IsFalse(persistentNormal.Exists(""));


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

            Assert.IsTrue(persistentNormal.Set("data", data));
            if(persistentNormal.TryGet<Data>("   data   ", out var test2)) {
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
        public void PersistentLazyTest()
        {
            DeletePluginSetting();

            var persistentNormal = new PluginPersistentStorage(this.Informations.PluginIdentifiers, this.Informations.PluginVersions, Test.DiContainer.Build<IMainDatabaseBarrier>(), Test.DiContainer.Build<IMainDatabaseLazyWriter>(), Test.DiContainer.Build<IDatabaseStatementLoader>(), Test.DiContainer.Build<ILoggerFactory>());

            Assert.IsFalse(persistentNormal.Exists(""));
            Assert.IsFalse(persistentNormal.TryGet<string>("", out _));
            Assert.IsTrue(persistentNormal.Set("", "test", PluginPersistentFormat.Text));
            Assert.IsTrue(persistentNormal.Exists(""));
            Assert.IsTrue(persistentNormal.Exists(" "));

            Assert.IsFalse(persistentNormal.TryGet<string>("x", out _));
            if(persistentNormal.TryGet<string>("", out var test1)) {
                Assert.AreEqual("test", test1);
            } else {
                Assert.Fail();
            }

            Assert.IsFalse(persistentNormal.Delete("x"));
            Assert.IsFalse(persistentNormal.Delete("")); // 遅延処理時は成功状態不明
            Assert.IsFalse(persistentNormal.Exists("")); // 遅延処理フラッシュにより存在しないことを検知


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

            Assert.IsTrue(persistentNormal.Set("data", data));
            if(persistentNormal.TryGet<Data>("   data   ", out var test2)) {
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
