using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Test;
using Xunit;

namespace ContentTypeTextNet.Pe.Core.Test.Models
{
    public class SerializerTest
    {
        #region define

        [DataContract]
        public class SerializableData
        {
            #region property

            [DataMember]
            public int Int { get; set; }
            [DataMember]
            public decimal Decimal { get; set; }
            [DataMember]
            public string? String { get; set; }

            [DataMember]
            public TimeSpan TimeSpan { get; set; }
            [DataMember]
            public DateTime DateTime { get; set; }

            #endregion
        }

        #endregion

        #region function

        private static SerializableData Create()
        {
            return new SerializableData() {
                Int = 123,
                Decimal = 3.14M,
                String = "TEXT",
                TimeSpan = TimeSpan.FromSeconds(1),
                DateTime = new DateTime(2024, 5, 12, 23, 19, 12),
            };
        }

        [Theory]
        [InlineData(typeof(JsonTextSerializer))]
        //[InlineData(typeof(JsonDataSerializer))]
        //[InlineData(typeof(XmlSerializer))]
      // [InlineData(typeof(XmlDataContractSerializer))]
        //BinaryDataContractSerializer
        public void Test(Type serializerType)
        {
            var data = Create();
            var test = (SerializerBase)Activator.CreateInstance(serializerType)!;

            var dir = TestIO.InitializeMethod(this);

            var saveFile = TestIO.CreateEmptyFile(dir, serializerType.Name + ".dat");

            using(var stream = saveFile.OpenWrite()) {
                test.Save(data, stream);
            }

            string s;
            using(var reader = saveFile.OpenText()) {
                s = reader.ReadToEnd();
            }

                using(var stream = saveFile.OpenRead()) {
                var actual = test.Load<SerializableData>(stream);
                Assert.Equal(data.Int, actual.Int);
                Assert.Equal(data.Decimal, actual.Decimal);
                Assert.Equal(data.TimeSpan, actual.TimeSpan);
                Assert.Equal(data.DateTime, actual.DateTime);
            }
        }

        // これするには例外投げるところを統一する必要あり
        //[Theory]
        //[InlineData(typeof(JsonTextSerializer))]
        //public void Throw_Test(Type serializerType)
        //{
        //    var data = Create();
        //    var test = (SerializerBase)Activator.CreateInstance(serializerType)!;
        //    using var stream = new MemoryStream();
        //    Assert.Throws<SerializationException>(() => test.Load<object>(stream));
        //}


        #endregion
    }
}
