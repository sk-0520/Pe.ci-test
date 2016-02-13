/**
This file is part of Pe.

Pe is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

Pe is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with Pe.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ContentTypeTextNet.Pe.Library.PeData.IF;
using ContentTypeTextNet.Pe.PeMain.Logic.Property;
using NUnit.Framework;

namespace ContentTypeTextNet.Pe.Test.Library.PeMainTest.Logic.Property
{
    [TestFixture]
    class VisibleVisibilityPropertyTest
    {
        class IVisibleImpl: IVisible
        {
            public bool IsVisible { get; set; }
        }

        void DummyAction(string name) { }

        [TestCase(true, true)]
        [TestCase(false, false)]
        public void GetVisibleTest(bool test, bool arg)
        {
            var testObj = new IVisibleImpl() {
                IsVisible = arg,
            };
            Assert.AreEqual(test, VisibleVisibilityProperty.GetVisible(testObj));
        }

        [TestCase(true, true)]
        [TestCase(false, false)]
        public void SetVisibleTest(bool test, bool value)
        {
            var testObj = new IVisibleImpl();
            VisibleVisibilityProperty.SetVisible(testObj, value, DummyAction);
            Assert.AreEqual(test, testObj.IsVisible);
        }

        [TestCase(Visibility.Visible, true)]
        [TestCase(Visibility.Hidden, false)]
        public void GetVisibleTest(Visibility test, bool value)
        {
            var testObj = new IVisibleImpl() {
                IsVisible = value,
            };
            Assert.AreEqual(test, VisibleVisibilityProperty.GetVisibility(testObj));
        }

        [TestCase(true, Visibility.Visible)]
        [TestCase(false, Visibility.Collapsed)]
        [TestCase(false, Visibility.Hidden)]
        public void SetVisibleTest(bool test, Visibility value)
        {
            var testObj = new IVisibleImpl();
            VisibleVisibilityProperty.SetVisibility(testObj, value, DummyAction);
            Assert.AreEqual(test, testObj.IsVisible);
        }
    }
}
