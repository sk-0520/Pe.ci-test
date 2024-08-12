using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Core.Models;
using Xunit;

namespace ContentTypeTextNet.Pe.Core.Test.Models
{
    public class EncodingUtilityTest
    {
        #region function

        public static TheoryData<Encoding, string> ParseData => new() {
            {
                new ASCIIEncoding(),
                "ascii"
            },
            {
                new ASCIIEncoding(),
                "ASCII"
            },
            {
                new UTF8Encoding(),
                "UTF-8"
            },
            {
                new UTF8Encoding(),
                "utf-8"
            },
            {
                new UnicodeEncoding(),
                "UNICODE"
            },
            {
                new UnicodeEncoding(),
                "unicode"
            },
            {
                new UTF32Encoding(),
                "UTF-32"
            },
            {
                new UTF32Encoding(),
                "utf-32"
            },
        };

        [Theory]
        [MemberData(nameof(ParseData))]
        public void ParseTest(Encoding expected, string encodingName)
        {
            var actual = EncodingUtility.Parse(encodingName);
            Assert.Equal(expected.WebName, actual.WebName);
        }

        [Fact]
        public void Parse_Utf8Bom_Test()
        {
            var actual = (UTF8Encoding)EncodingUtility.Parse("utf-8bom");
            Assert.Equal("utf-8", actual.WebName);
            Assert.Equal(3, actual.GetPreamble().Length);
            Assert.Equal("utf-8bom", EncodingUtility.ToString(actual));
        }

        [Fact]
        public void Parse_Utf8n_Test()
        {
            var actual = (UTF8Encoding)EncodingUtility.Parse("utf-8n");
            Assert.Equal("utf-8", actual.WebName);
            Assert.Empty(actual.GetPreamble());
            Assert.Equal("utf-8", EncodingUtility.ToString(actual));
        }

        #endregion
    }
}
