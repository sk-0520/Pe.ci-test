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
	}
}
