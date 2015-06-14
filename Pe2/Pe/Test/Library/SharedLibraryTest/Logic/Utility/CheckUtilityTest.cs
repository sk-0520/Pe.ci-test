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
	class CheckUtilityTest
	{
		[TestCase(true, true)]
		[TestCase(false, false)]
		public void EnforceTest(bool success, bool test)
		{
			if(success) {
				Assert.DoesNotThrow(() => CheckUtility.Enforce(test));
			} else {
				Assert.Throws<Exception>(() => CheckUtility.Enforce(test));
			}
		}

		[Test]
		public void EnforceNotNull_class_Test()
		{
			var test = new Object();
			Assert.DoesNotThrow(() => CheckUtility.EnforceNotNull(test));

			test = null;
			Assert.Throws<ArgumentNullException>(() => CheckUtility.EnforceNotNull(test));

			Assert.DoesNotThrow(() => CheckUtility.EnforceNotNull(new Exception()));
			Assert.Throws<ArgumentNullException>(() => CheckUtility.EnforceNotNull(default(Exception)));
		}

		[Test]
		public void EnforceNotNull_Nullable_Test()
		{
			int? test1 = 1;
			Assert.DoesNotThrow(() => CheckUtility.EnforceNotNull(test1));
			test1 = null;
			Assert.Throws<ArgumentNullException>(() => CheckUtility.EnforceNotNull(test1));
		}

		[TestCase(true, "b")]
		[TestCase(true, " c")]
		[TestCase(true, "d ")]
		[TestCase(true, " e ")]
		[TestCase(true, " ")]
		[TestCase(false, null)]
		[TestCase(false, "")]
		public void EnforceNotNullAndNotEmptyTest(bool success, string test)
		{
			if(success) {
				Assert.DoesNotThrow(() => CheckUtility.EnforceNotNullAndNotEmpty(test));
			} else {
				Assert.Throws<ArgumentException>(() => CheckUtility.EnforceNotNullAndNotEmpty(test));
			}
		}

		[TestCase(true, "b")]
		[TestCase(true, " c")]
		[TestCase(true, "d ")]
		[TestCase(true, " e ")]
		[TestCase(false, " ")]
		[TestCase(false, null)]
		[TestCase(false, "")]
		public void EnforceNotNullAndNotWhiteSpaceTest(bool success, string test)
		{
			if(success) {
				Assert.DoesNotThrow(() => CheckUtility.EnforceNotNullAndNotWhiteSpace(test));
			} else {
				Assert.Throws<ArgumentException>(() => CheckUtility.EnforceNotNullAndNotWhiteSpace(test));
			}
		}

		[TestCase(true, 1)]
		[TestCase(false, 0)]
		public void EnforceNotZeroTest(bool success, int arg)
		{
			var test = new IntPtr(arg);
			if(success) {
				Assert.DoesNotThrow(() => CheckUtility.EnforceNotZero(test));
			} else {
				Assert.Throws<ArgumentException>(() => CheckUtility.EnforceNotZero(test));
			}
		}

		[Test]
		public void EnforceNotZero_val_Test()
		{
			Assert.Throws<ArgumentException>(() => CheckUtility.EnforceNotZero(IntPtr.Zero));
			Assert.Throws<ArgumentException>(() => CheckUtility.EnforceNotZero((IntPtr)null));
		}
	}
}
