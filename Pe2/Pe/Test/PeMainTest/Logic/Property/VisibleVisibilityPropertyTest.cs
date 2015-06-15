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

	[TestFixture]
	class VisibleVisibilityPropertyTest
	{
		class IVisibleImpl: IVisible
		{
			public bool Visible { get; set; }
		}

		void DummyAction(string name) { }

		[TestCase(true, true)]
		[TestCase(false, false)]
		public void GetVisibleTest(bool test, bool arg)
		{
			var testObj = new IVisibleImpl() {
				Visible = arg,
			};
			Assert.AreEqual(test, VisibleVisibilityProperty.GetVisible(testObj));
		}

		[TestCase(true, true)]
		[TestCase(false, false)]
		public void SetVisibleTest(bool test, bool value)
		{
			var testObj = new IVisibleImpl();
			VisibleVisibilityProperty.SetVisible(testObj, value, DummyAction);
			Assert.AreEqual(test, testObj.Visible);
		}

		[TestCase(Visibility.Visible, true)]
		[TestCase(Visibility.Hidden, false)]
		public void GetVisibleTest(Visibility test, bool value)
		{
			var testObj = new IVisibleImpl() {
				Visible = value,
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
			Assert.AreEqual(test, testObj.Visible);
		}
	}
}
