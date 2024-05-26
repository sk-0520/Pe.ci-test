using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Core.Views;
using Xunit;

namespace ContentTypeTextNet.Pe.Core.Test.Views
{
    public class NumericUpDownTest
    {
        #region function

        [WpfFact]
        public void Minimum_Enable_Test()
        {
            var test = new NumericUpDown() {
                Maximum = 100,
                Minimum = 0,
                Value = 50,
            };

            Assert.True(test.PART_DOWN_BUTTON.IsEnabled);
        }

        [WpfTheory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Minimum_Disable_Test(decimal value)
        {
            var test = new NumericUpDown() {
                Maximum = 100,
                Minimum = 0,
                Value = value,
            };

            Assert.False(test.PART_DOWN_BUTTON.IsEnabled);
        }

        [WpfFact]
        public void Maximum_Enable_Test()
        {
            var test = new NumericUpDown() {
                Maximum = 100,
                Minimum = 0,
                Value = 50,
            };

            Assert.True(test.PART_UP_BUTTON.IsEnabled);
        }

        [WpfTheory]
        [InlineData(100)]
        [InlineData(101)]
        public void Maximum_Disable_Test(decimal value)
        {
            var test = new NumericUpDown() {
                Maximum = 100,
                Minimum = 0,
                Value = value,
            };

            Assert.False(test.PART_UP_BUTTON.IsEnabled);
        }

        #endregion
    }
}
