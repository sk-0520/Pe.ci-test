using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ContentTypeTextNet.Pe.Bridge.Test.Models.Data
{
    [TestClass]
    public class IdTest
    {
        #region define

        class ClassData
        {
            public LauncherItemId LauncherItemId { get; set; }
        }

        record class ClassRecordData
        {
            public LauncherItemId LauncherItemId { get; init; }
        }

        struct StructData
        {
            public LauncherItemId LauncherItemId { get; set; }
        }

        record struct StructRecordData
        {
            public LauncherItemId LauncherItemId { get; init; }
        }

        #endregion

        #region function

        [TestMethod]
        public void SerializeClassDataTest()
        {
            var input = new ClassData() {
                LauncherItemId = new LauncherItemId(Guid.NewGuid()),
            };
            var clone = JsonSerializer.Serialize(input);
            var deserialize = JsonSerializer.Deserialize<ClassData>(clone)!;
            Assert.AreEqual(input.LauncherItemId, deserialize.LauncherItemId);
        }

        [TestMethod]
        public void SerializeClassRecordDataTest()
        {
            var input = new ClassRecordData() {
                LauncherItemId = new LauncherItemId(Guid.NewGuid()),
            };
            var clone = JsonSerializer.Serialize(input);
            var deserialize = JsonSerializer.Deserialize<ClassRecordData>(clone)!;
            Assert.AreEqual(input.LauncherItemId, deserialize.LauncherItemId);
        }

        [TestMethod]
        public void SerializeStructDataTest()
        {
            var input = new StructData() {
                LauncherItemId = new LauncherItemId(Guid.NewGuid()),
            };
            var clone = JsonSerializer.Serialize(input);
            var deserialize = JsonSerializer.Deserialize<StructData>(clone)!;
            Assert.AreEqual(input.LauncherItemId, deserialize.LauncherItemId);
        }

        [TestMethod]
        public void SerializeStructRecordDataTest()
        {
            var input = new StructRecordData() {
                LauncherItemId = new LauncherItemId(Guid.NewGuid()),
            };
            var clone = JsonSerializer.Serialize(input);
            var deserialize = JsonSerializer.Deserialize<StructRecordData>(clone)!;
            Assert.AreEqual(input.LauncherItemId, deserialize.LauncherItemId);
        }

        #endregion
    }
}
