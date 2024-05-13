using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using ContentTypeTextNet.Pe.Core.Views;
using Xunit;

namespace ContentTypeTextNet.Pe.Core.Test.Views
{
    public class PasswordBox2Test
    {
        #region function

        [WpfFact]
        public void Test()
        {
            var test = new PasswordBox2();
            test.SetValue(PasswordBox2.PasswordProperty, "abc");
            var actual = test.GetValue(PasswordBox2.PasswordProperty);
            Assert.Equal("abc", actual);
        }

        #endregion
    }
}
