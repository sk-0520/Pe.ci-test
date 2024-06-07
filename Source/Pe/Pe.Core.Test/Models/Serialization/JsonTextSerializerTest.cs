using System;
using System.Text.Json.Serialization;
using ContentTypeTextNet.Pe.Core.Models.Serialization;
using Xunit;

namespace ContentTypeTextNet.Pe.Core.Test.Models.Serialization
{
    public class JsonTextSerializerTest
    {
        #region define

        private class SerializableData
        {
            #region property

            public Version? DefaultVersion { get; set; }

            [JsonConverter(typeof(JsonTextSerializer.VersionConverter))]
            public Version? ConverterVersion1 { get; set; }

            [JsonConverter(typeof(JsonTextSerializer.VersionConverter))]
            public Version? ConverterVersion2 { get; set; }

            #endregion
        }

        #endregion

        #region function

        [Fact]
        public void VersionConverter_Read_Test()
        {
            var test = new JsonTextSerializer();

            var data = new SerializableData() {
                DefaultVersion = new Version(1, 2, 3, 4),
                ConverterVersion1 = new Version(4, 5, 6, 7),
                ConverterVersion2 = null,
            };

            var actual = test.Clone(data);

            Assert.Equal(actual.DefaultVersion, data.DefaultVersion);
            Assert.Equal(actual.ConverterVersion1, data.ConverterVersion1);
            Assert.Null(actual.ConverterVersion2);
        }

        #endregion
    }
}
