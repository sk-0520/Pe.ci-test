using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using Xunit;

namespace ContentTypeTextNet.Pe.Bridge.Test.Models.Data
{
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

        [Fact]
        public void SerializeClassDataTest()
        {
            var input = new ClassData() {
                LauncherItemId = LauncherItemId.NewId(),
            };
            var clone = JsonSerializer.Serialize(input);
            var deserialize = JsonSerializer.Deserialize<ClassData>(clone)!;
            Assert.Equal(input.LauncherItemId, deserialize.LauncherItemId);
        }

        [Fact]
        public void SerializeClassRecordDataTest()
        {
            var input = new ClassRecordData() {
                LauncherItemId = LauncherItemId.NewId(),
            };
            var clone = JsonSerializer.Serialize(input);
            var deserialize = JsonSerializer.Deserialize<ClassRecordData>(clone)!;
            Assert.Equal(input.LauncherItemId, deserialize.LauncherItemId);
        }

        [Fact]
        public void SerializeStructDataTest()
        {
            var input = new StructData() {
                LauncherItemId = LauncherItemId.NewId(),
            };
            var clone = JsonSerializer.Serialize(input);
            var deserialize = JsonSerializer.Deserialize<StructData>(clone)!;
            Assert.Equal(input.LauncherItemId, deserialize.LauncherItemId);
        }

        [Fact]
        public void SerializeStructRecordDataTest()
        {
            var input = new StructRecordData() {
                LauncherItemId = LauncherItemId.NewId(),
            };
            var clone = JsonSerializer.Serialize(input);
            var deserialize = JsonSerializer.Deserialize<StructRecordData>(clone)!;
            Assert.Equal(input.LauncherItemId, deserialize.LauncherItemId);
        }

        #endregion
    }
}
