/*
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
using NUnit.Framework;
using System.Windows;
using ContentTypeTextNet.Pe.Library.PeData.Setting.MainSettings;

namespace ContentTypeTextNet.Test.Library.PeDataTest.Setting.MainSettings
{
    [TestFixture]
    class LoggingSettingModelTest
    {
        [Test]
        public void DeepCloneTest()
        {
            var src = new LoggingSettingModel() {
                AddShow = true,
                IsTopmost = true,
                IsVisible = true,
                ShowTriggerDebug = true,
                ShowTriggerError = true,
                ShowTriggerFatal = true,
                ShowTriggerInformation = true,
                ShowTriggerTrace = true,
                ShowTriggerWarning = true,
                WindowWidth = 100,
                WindowHeight = 200,
                WindowLeft = 300,
                WindowTop = 400,
                WindowState = WindowState.Maximized,
                DetailWordWrap = true,
            };

            var dst = (LoggingSettingModel)src.DeepClone();
            Assert.AreEqual(src.AddShow, dst.AddShow);
            Assert.AreEqual(src.IsTopmost, dst.IsTopmost);
            Assert.AreEqual(src.IsVisible, dst.IsVisible);
            Assert.AreEqual(src.ShowTriggerDebug, dst.ShowTriggerDebug);
            Assert.AreEqual(src.ShowTriggerError, dst.ShowTriggerError);
            Assert.AreEqual(src.ShowTriggerFatal, dst.ShowTriggerFatal);
            Assert.AreEqual(src.ShowTriggerInformation, dst.ShowTriggerInformation);
            Assert.AreEqual(src.ShowTriggerTrace, dst.ShowTriggerTrace);
            Assert.AreEqual(src.ShowTriggerWarning, dst.ShowTriggerWarning);
            Assert.AreEqual(src.WindowWidth, dst.WindowWidth);
            Assert.AreEqual(src.WindowHeight, dst.WindowHeight);
            Assert.AreEqual(src.WindowLeft, dst.WindowLeft);
            Assert.AreEqual(src.WindowTop, dst.WindowTop);
            Assert.AreEqual(src.WindowState, dst.WindowState);
            Assert.AreEqual(src.DetailWordWrap, dst.DetailWordWrap);
        }
    }
}
