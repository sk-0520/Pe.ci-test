using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
using ContentTypeTextNet.Pe.Core.Models.Serialization;
using ContentTypeTextNet.Pe.Test;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions.Interfaces;
using Xunit;

namespace ContentTypeTextNet.Pe.Core.Test.Models.Serialization
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

        private class ThrowSerializer<TException>: SerializerBase
            where TException : Exception, new()
        {
            protected override TResult LoadImpl<TResult>(Stream stream)
            {
                throw new TException();
            }

            protected override void SaveImpl(object value, Stream stream)
            {
                throw new TException();
            }
        }

        #endregion

        #region function

        private static SerializableData CreateData()
        {
            return new SerializableData() {
                Int = 123,
                Decimal = 3.14M,
                String = "TEXT",
                TimeSpan = TimeSpan.FromSeconds(1),
                DateTime = new DateTime(2024, 5, 12, 23, 19, 12),
            };
        }

        private static SerializerBase CreateSerializer(Type type)
        {
            var obj = Activator.CreateInstance(type);
            var serializer = (SerializerBase)obj!;
            return serializer;
        }

        [Theory]
        [InlineData(typeof(JsonTextSerializer))]
        [InlineData(typeof(JsonDataSerializer))]
        [InlineData(typeof(XmlSerializer))]
        [InlineData(typeof(XmlDataContractSerializer))]
        [InlineData(typeof(BinaryDataContractSerializer))]
        public void Test(Type serializerType)
        {
            var data = CreateData();
            var test = CreateSerializer(serializerType);

            var dir = TestIO.InitializeMethod(this, serializerType.Name);

            var saveFile = TestIO.CreateEmptyFile(dir, "data.dat");

            using(var stream = saveFile.OpenWrite()) {
                test.Save(data, stream);
            }

            using(var stream = saveFile.OpenRead()) {
                var actual = test.Load<SerializableData>(stream);
                Assert.Equal(data.Int, actual.Int);
                Assert.Equal(data.Decimal, actual.Decimal);
                Assert.Equal(data.TimeSpan, actual.TimeSpan);
                Assert.Equal(data.DateTime, actual.DateTime);
            }
        }

        [Theory]
        [InlineData(typeof(JsonTextSerializer))]
        [InlineData(typeof(JsonDataSerializer))]
        [InlineData(typeof(XmlSerializer))]
        [InlineData(typeof(XmlDataContractSerializer))]
        [InlineData(typeof(BinaryDataContractSerializer))]
        public void Load_Throw_Test(Type serializerType)
        {
            var data = CreateData();
            var test = CreateSerializer(serializerType);
            using var stream = new MemoryStream();
            Assert.Throws<SerializationException>(() => test.Load<object>(stream));
        }

        [Theory]
        [InlineData(typeof(JsonTextSerializer))]
        [InlineData(typeof(JsonDataSerializer))]
        [InlineData(typeof(XmlSerializer))]
        [InlineData(typeof(XmlDataContractSerializer))]
        [InlineData(typeof(BinaryDataContractSerializer))]
        public void CloneTest(Type serializerType)
        {
            var data = CreateData();
            var test = CreateSerializer(serializerType);

            var actual = test.Clone(data);
            Assert.Equal(data.Int, actual.Int);
            Assert.Equal(data.Decimal, actual.Decimal);
            Assert.Equal(data.TimeSpan, actual.TimeSpan);
            Assert.Equal(data.DateTime, actual.DateTime);
        }

        [Theory]
        [InlineData(typeof(JsonTextSerializer))]
        [InlineData(typeof(JsonDataSerializer))]
        [InlineData(typeof(XmlSerializer))]
        [InlineData(typeof(XmlDataContractSerializer))]
        [InlineData(typeof(BinaryDataContractSerializer))]
        public void Clone_throw_Test(Type serializerType)
        {
            var data = CreateData();
            var test = CreateSerializer(serializerType);

            Assert.Throws<ArgumentNullException>(() => test.Clone<object>(null!));
        }

        [Fact]
        public void Load_throw_Test()
        {
            var test1 = new ThrowSerializer<Exception>();
            var exception1 = Assert.Throws<SerializationException>(() => test1.Load<object>(new MemoryStream()));
            Assert.IsType<Exception>(exception1.InnerException);

            var test2 = new ThrowSerializer<NotImplementedException>();
            var exception2 = Assert.Throws<SerializationException>(() => test2.Load<object>(new MemoryStream()));
            Assert.IsType<NotImplementedException>(exception2.InnerException);

            var test3 = new ThrowSerializer<SerializationException>();
            var exception3 = Assert.Throws<SerializationException>(() => test3.Load<object>(new MemoryStream()));
            Assert.Null(exception3.InnerException);
        }

        [Fact]
        public void Save_throw_Test()
        {
            var test1 = new ThrowSerializer<Exception>();
            var exception1 = Assert.Throws<SerializationException>(() => test1.Save(default!, new MemoryStream()));
            Assert.IsType<Exception>(exception1.InnerException);

            var test2 = new ThrowSerializer<NotImplementedException>();
            var exception2 = Assert.Throws<SerializationException>(() => test2.Save(default!, new MemoryStream()));
            Assert.IsType<NotImplementedException>(exception2.InnerException);

            var test3 = new ThrowSerializer<SerializationException>();
            var exception3 = Assert.Throws<SerializationException>(() => test3.Save(default!, new MemoryStream()));
            Assert.Null(exception3.InnerException);
        }

        #endregion
    }
}
