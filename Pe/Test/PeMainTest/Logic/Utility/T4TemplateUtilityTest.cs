/*
This file is part of SharedLibrary.

SharedLibrary is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

SharedLibrary is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with SharedLibrary.  If not, see <http://www.gnu.org/licenses/>.
*/
namespace ContentTypeTextNet.Test.Library.SharedLibraryTest.Logic.Utility
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
    using NUnit.Framework;

    [TestFixture]
    class T4TemplateUtilityTest
    {
        [TestCase("abc", "abc")]
        [TestCase("<#= 1 + 1 #>", "2")]
        public void TransformTextTest(string src, string result)
        {
            var s = @"<#@ template language=""C#"" hostSpecific=""true"" #>" + Environment.NewLine + src;
            var output = T4TemplateUtility.TransformText(s);
            Assert.IsTrue(output == result);
        }
    }
}
