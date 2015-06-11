namespace ContentTypeTextNet.Pe.Test.Library.PeMainTest.Logic.Utility
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;
	using ContentTypeTextNet.Pe.PeMain.Logic.Utility;
	using NUnit.Framework;

	class ConvertUtilityTest
	{
		[TestCase(Visibility.Visible, true)]
		[TestCase(Visibility.Hidden, false)]
		public void To_Visibility_Test(Visibility result, bool test)
		{
			Assert.True(result == ConvertUtility.To(test));
		}

		[TestCase(true, Visibility.Visible)]
		[TestCase(true, Visibility.Collapsed)]
		[TestCase(false, Visibility.Hidden)]
		public void To_Bool_Test(bool result, Visibility test)
		{
			Assert.True(result == ConvertUtility.To(test));
		}
	}
}
