namespace ContentTypeTextNet.Pe.Test.Library.PeMainTest.Logic.Property
{
	using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ContentTypeTextNet.Pe.Library.PeData.IF;
using ContentTypeTextNet.Pe.PeMain.Logic.Property;
using NUnit.Framework;

	class VisibleVisibilityPropertyTest
	{
		class IVisibleImpl: IVisible
		{
			public bool Visible { get; set; }
		}

		void DummyAction(string name) { }

		[TestCase(true, true)]
		[TestCase(false, false)]
		public void GetVisibleTest(bool result, bool test)
		{
			var testObj = new IVisibleImpl() {
				Visible = test,
			};
			Assert.AreEqual(result, VisibleVisibilityProperty.GetVisible(testObj));
		}

		[TestCase(true, true)]
		[TestCase(false, false)]
		public void SetVisibleTest(bool result, bool test)
		{
			var testObj = new IVisibleImpl();
			VisibleVisibilityProperty.SetVisible(testObj, test, DummyAction);
			Assert.AreEqual(result, testObj.Visible);
		}

		[TestCase(Visibility.Visible, true)]
		[TestCase(Visibility.Hidden, false)]
		public void GetVisibleTest(Visibility result, bool test)
		{
			var testObj = new IVisibleImpl() {
				Visible = test,
			};
			Assert.AreEqual(result, VisibleVisibilityProperty.GetVisibility(testObj));
		}

		[TestCase(true, Visibility.Visible)]
		[TestCase(true, Visibility.Collapsed)]
		[TestCase(false, Visibility.Hidden)]
		public void SetVisibleTest(bool result, Visibility test)
		{
			var testObj = new IVisibleImpl();
			VisibleVisibilityProperty.SetVisibility(testObj, test, DummyAction);
			Assert.AreEqual(result, testObj.Visible);
		}
	}
}
