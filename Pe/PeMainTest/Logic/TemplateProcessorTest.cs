namespace ContentTypeTextNet.Pe.Test.ApplicationTest.Logic
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Pe.PeMain.Logic;
	using NUnit.Framework;

	[TestFixture]
	class TemplateProcessorTest
	{
		[TestCase("", "")]
		public void SimpleTest(string src, string result)
		{
			var tp = new TemplateProcessor();
			tp.TemplateSource = src;
			tp.AllProcess();
			var output = tp.TransformText();
			Assert.IsTrue(result == output);
		}
	}
}
