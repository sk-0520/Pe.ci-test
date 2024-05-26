using System;
using System.Text.Json.Serialization;
using ContentTypeTextNet.Pe.Core.Models.Serialization;
using Xunit;

namespace ContentTypeTextNet.Pe.Core.Test.Models.Serialization
{
    public class JsonTextSerializerTest
    {
        #region define

        private class SerializableData_A
        {
            #region property

            public Version? DefaultVersion { get; set; }

            [JsonConverter(typeof(JsonTextSerializer.VersionConverter))]
            public Version? ConverterVersion1 { get; set; }

            [JsonConverter(typeof(JsonTextSerializer.VersionConverter))]
            public Version? ConverterVersion2 { get; set; }

            #endregion
        }

        private class SerializableData_B
        {
            #region property

            public int Int { get; set; }
            public string String { get; set; } = string.Empty;

            #endregion
        }

        #endregion

        #region function

        [Fact]
        public void VersionConverter_Read_Test()
        {
            var test = new JsonTextSerializer();

            var data = new SerializableData_A() {
                DefaultVersion = new Version(1, 2, 3, 4),
                ConverterVersion1 = new Version(4, 5, 6, 7),
                ConverterVersion2 = null,
            };

            var actual = test.Clone(data);

            Assert.Equal(actual.DefaultVersion, data.DefaultVersion);
            Assert.Equal(actual.ConverterVersion1, data.ConverterVersion1);
            Assert.Null(actual.ConverterVersion2);
        }

        [Fact]
        public void Load_Heap_Test()
        {
            var test = new JsonTextSerializer() {
                StackSize = 0,
            };

            var data = new SerializableData_B() {
                Int = 1234,
                String = "String",
            };

            var actual = test.Clone(data);

            Assert.Equal(actual.Int, data.Int);
            Assert.Equal(actual.String, data.String);
        }

        [Fact]
        public void Load_Stack_Test()
        {
            var test = new JsonTextSerializer() {
                StackSize = 1024,
            };

            var data = new SerializableData_B() {
                Int = 1234,
                String = "String",
            };

            var actual = test.Clone(data);

            Assert.Equal(actual.Int, data.Int);
            Assert.Equal(actual.String, data.String);
        }

        #endregion
    }
}
