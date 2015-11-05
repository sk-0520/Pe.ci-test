/**
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
    using ContentTypeTextNet.Library.SharedLibrary.Attribute;
    using ContentTypeTextNet.Library.SharedLibrary.IF;
    using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
    using NUnit.Framework;

    [TestFixture]
    class DeepCloneUtilityTest
    {
        struct PlainStruct:IDeepClone
        {
#pragma warning disable 169
            [IsDeepClone]
            int private_field_target;
            [IsDeepClone]
            public int public_field_target;
            int private_field_untarget;
            public int public_field_untarget;
#pragma warning restore 169

            public void DeepCloneTo(IDeepClone target)
            {
                DeepCloneUtility.DeepCopy(target, this);
            }

            public IDeepClone DeepClone()
            {
                var result = new PlainStruct();
                DeepCloneTo(result);
                return result;
            }
        }
        [Test]
        public void DeepCopy_PlainStruct_Test()
        {
            var s = new PlainStruct();
            s.public_field_target = 10;
            s.public_field_untarget = 20;
            var d = s.DeepClone();
            Assert.AreEqual(s, d);
        }
    }
}
