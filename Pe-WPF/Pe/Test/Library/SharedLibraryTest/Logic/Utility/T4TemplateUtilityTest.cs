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
	class T4TemplateUtilityTest
	{
		[TestCase("abc", "abc")]
		[TestCase("<#= 1 + 1 #>", "2")]
		public void TransformTextTest(string src, string result)
		{
			var s = @"<#@ template language=""C#"" hostSpecific=""true"" #>" + Environment.NewLine + src;
			var output = T4TemplateUtility.TransformText(s);
			Assert.IsTrue(output == result);
		}
	}
}
