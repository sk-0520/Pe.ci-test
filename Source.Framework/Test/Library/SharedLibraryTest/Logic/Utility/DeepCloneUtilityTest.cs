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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Attribute;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using NUnit.Framework;

namespace ContentTypeTextNet.Test.Library.SharedLibraryTest.Logic.Utility
{
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

            //public void DeepCloneTo(IDeepClone target)
            //{
            //    DeepCloneUtility.DeepCopy(target, this);
            //}

            public IDeepClone DeepClone()
            {
                //var result = new PlainStruct();
                //DeepCloneTo(result);
                //return result;
                return DeepCloneUtility.Copy(this);
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

            //public void DeepCloneTo(IDeepClone target)
            //{
            //    DeepCloneUtility.DeepCopy(target, this);
            //}

            public IDeepClone DeepClone()
            {
                //var result = new PlainClass();
                //DeepCloneTo(result);
                //return result;
                return DeepCloneUtility.Copy(this);
            }
        }
        [Test]
        public void DeepCopy_PlainClass_Test()
        {
            var s = new PlainClass();
            s.public_target = 10;
            s.public_untarget = 20;
            var d = (PlainClass)s.DeepClone();
            Assert.AreNotEqual(s, d);
            Assert.AreEqual(s.public_target, d.public_target);
            Assert.AreNotEqual(s.public_untarget, d.public_untarget);
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

            //public void DeepCloneTo(IDeepClone target)
            //{
            //    DeepCloneUtility.DeepCopy(target, this);
            //}

            public IDeepClone DeepClone()
            {
                //var result = new PropertyStruct();
                //DeepCloneTo(result);
                //return result;
                return DeepCloneUtility.Copy(this);
            }
        }
        [Test]
        public void DeepCopy_PropertyStruct_Test()
        {
            var s = new PropertyStruct();
            s.public_target = 10;
            s.public_untarget = 20;
            var d = (PropertyStruct)s.DeepClone();
            Assert.AreNotEqual(s, d);
            Assert.AreEqual(s.public_target, d.public_target);
            Assert.AreNotEqual(s.public_untarget, d.public_untarget);
        }

        struct NestStruct: IDeepClone
        {
#pragma warning disable 649
            [IsDeepClone]
            public PlainStruct plainStruct_target;

            public PlainStruct plainStruct_untarget;
#pragma warning restore 649

            public IDeepClone DeepClone()
            {
                return DeepCloneUtility.Copy(this);
            }
        }
        [Test]
        public void DeepCopy_NestStruct_Test()
        {
            var s = new NestStruct();
            s.plainStruct_target.public_target = 10;
            s.plainStruct_target.public_untarget = 20;
            s.plainStruct_untarget.public_target = 100;
            s.plainStruct_untarget.public_untarget = 200;
            var d = (NestStruct)s.DeepClone();
            Assert.AreNotEqual(s, d);
            Assert.AreEqual(s.plainStruct_target.public_target, d.plainStruct_target.public_target);
            Assert.AreNotEqual(s.plainStruct_target.public_untarget, d.plainStruct_target.public_untarget);

            Assert.AreNotEqual(s.plainStruct_untarget.public_target, d.plainStruct_untarget.public_target);
            Assert.AreNotEqual(s.plainStruct_untarget.public_untarget, d.plainStruct_untarget.public_untarget);
        }

        class NestClass: IDeepClone
        {
            public NestClass()
            {
                plainClass_target = new PlainClass();
                plainClass_untarget = new PlainClass();
            }
#pragma warning disable 649
            [IsDeepClone]
            public PlainClass plainClass_target;

            public PlainClass plainClass_untarget;
#pragma warning restore 649

            public IDeepClone DeepClone()
            {
                return DeepCloneUtility.Copy(this);
            }
        }
        [Test]
        public void DeepCopy_NestClass_Test()
        {
            var s = new NestClass();
            s.plainClass_target.public_target = 10;
            s.plainClass_target.public_untarget = 20;
            s.plainClass_untarget.public_target = 100;
            s.plainClass_untarget.public_untarget = 200;
            var d = (NestClass)s.DeepClone();
            Assert.AreEqual(s.plainClass_target.public_target, d.plainClass_target.public_target);
            Assert.AreNotEqual(s.plainClass_target.public_untarget, d.plainClass_target.public_untarget);

            Assert.AreNotEqual(s.plainClass_untarget.public_target, d.plainClass_untarget.public_target);
            Assert.AreNotEqual(s.plainClass_untarget.public_untarget, d.plainClass_untarget.public_untarget);
        }
    }
}
