using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ContentTypeTextNet.Pe.Core.Models;
using Xunit;

namespace ContentTypeTextNet.Pe.Core.Test.Models
{
    public class IDataObjectExtensionsTest
    {
        #region function

        [Fact]
        public void TryGet_int_success_Test()
        {
            var test = new DataObject(123);
            var actual = test.TryGet<int>(out var result);
            Assert.True(actual);
            Assert.Equal(123, result);
        }

        [Fact]
        public void TryGet_int_failure_Test()
        {
            var test = new DataObject("abc");
            var actual = test.TryGet<int>(out var result);
            Assert.False(actual);
            Assert.Equal(default, result);
        }

        [Fact]
        public void TryGet_string_success_Test()
        {
            var test = new DataObject("abc");
            var actual = test.TryGet<string>(out var result);
            Assert.True(actual);
            Assert.Equal("abc", result);
        }

        [Fact]
        public void TryGet_string_failure_Test()
        {
            var test = new DataObject(123);
            var actual = test.TryGet<string>(out var result);
            Assert.False(actual);
            Assert.Equal(default, result);
        }

        [Fact]
        public void IsTextPresent_UnicodeText_Test()
        {
            var test = new DataObject(DataFormats.UnicodeText, "abc");
            var actual = test.IsTextPresent();
            Assert.True(actual);
        }

        [Fact]
        public void IsTextPresent_OemText_Test()
        {
            var test = new DataObject(DataFormats.OemText, "abc");
            var actual = test.IsTextPresent();
            Assert.True(actual);
        }

        [Fact]
        public void IsTextPresent_Text_Test()
        {
            var test = new DataObject(DataFormats.Text, "abc");
            var actual = test.IsTextPresent();
            Assert.True(actual);
        }

        [Fact]
        public void IsTextPresent_Rtf_Test()
        {
            var test = new DataObject(DataFormats.Rtf, "abc");
            var actual = test.IsTextPresent();
            Assert.False(actual);
        }

        [Fact]
        public void IsTextPresent_int_Test()
        {
            var test = new DataObject(123);
            var actual = test.IsTextPresent();
            Assert.False(actual);
        }


        [Fact]
        public void RequireText_UnicodeText_Test()
        {
            var test = new DataObject(DataFormats.UnicodeText, "abc");
            var actual = test.RequireText();
            Assert.Equal("abc", actual);
        }

        [Fact]
        public void RequireText_OemText_Test()
        {
            var test = new DataObject(DataFormats.OemText, "abc");
            var actual = test.RequireText();
            Assert.Equal("abc", actual);
        }

        [Fact]
        public void RequireText_Text_Test()
        {
            var test = new DataObject(DataFormats.Text, "abc");
            var actual = test.RequireText();
            Assert.Equal("abc", actual);
        }

        [Fact]
        public void RequireText_Rtf_Test()
        {
            var test = new DataObject(DataFormats.Rtf, "abc");
            Assert.Throws<InvalidCastException>(() => test.RequireText());
        }

        [Fact]
        public void RequireText_int_Test()
        {
            var test = new DataObject(123);
            Assert.Throws<InvalidCastException>(() => test.RequireText());
        }

        #endregion
    }
}
