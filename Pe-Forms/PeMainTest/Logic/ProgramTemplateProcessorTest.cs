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
	class ProgramTemplateProcessorTest
	{
		[TestCase("", "")]
		[TestCase("<#= app[\"APPLICATION\"] #>", "Pe")]
		public void SimpleTest(string src, string result)
		{
			var pp = new ProgramTemplateProcessor();
			pp.TemplateSource = src;
			pp.AllProcess();
			var output = pp.TransformText();
			Assert.IsTrue(result == output);
		}
	}
}
