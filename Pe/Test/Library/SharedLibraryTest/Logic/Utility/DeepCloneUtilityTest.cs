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
            int private_target;
            [IsDeepClone]
            public int public_target;
            int private_untarget;
            public int public_untarget;
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
            s.public_target = 10;
            s.public_untarget = 20;
            var d = (PlainStruct)s.DeepClone();
            Assert.AreNotEqual(s, d);
            Assert.AreEqual(s.public_target, d.public_target);
            Assert.AreNotEqual(s.public_untarget, d.public_untarget);
        }

        class PlainClass: IDeepClone
        {
#pragma warning disable 169
            [IsDeepClone]
            int private_target;
            [IsDeepClone]
            public int public_target;
            int private_untarget;
            public int public_untarget;

#pragma warning restore 169

            public void DeepCloneTo(IDeepClone target)
            {
                DeepCloneUtility.DeepCopy(target, this);
            }

            public IDeepClone DeepClone()
            {
                var result = new PlainClass();
                DeepCloneTo(result);
                return result;
            }
        }
        [Test]
        public void DeepCopy_PlainClass_Test()
        {
            var s = new PlainClass();
            s.public_target = 10;
            s.public_untarget = 20;
            var d = s.DeepClone();
            Assert.AreNotEqual(s, d);
        }

        struct PropertyStruct: IDeepClone
        {
#pragma warning disable 169
            [IsDeepClone]
            int private_target { get; set; }
            [IsDeepClone]
            public int public_target { get; set; }
            int private_untarget { get; set; }
            public int public_untarget { get; set; }
#pragma warning restore 169

            public void DeepCloneTo(IDeepClone target)
            {
                DeepCloneUtility.DeepCopy(target, this);
            }

            public IDeepClone DeepClone()
            {
                var result = new PropertyStruct();
                DeepCloneTo(result);
                return result;
            }
        }
        [Test]
        public void DeepCopy_PropertyStruct_Test()
        {
            var s = new PropertyStruct();
            s.public_target = 10;
            s.public_untarget = 20;
            var d = s.DeepClone();
            Assert.AreNotEqual(s, d);
        }
    }
}
