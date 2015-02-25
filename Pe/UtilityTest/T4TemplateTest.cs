namespace ContentTypeTextNet.Pe.Test.UtilityTest
{
	using System;
	using System.Diagnostics;
	using ContentTypeTextNet.Pe.Library.Utility;
	using NUnit.Framework;

	[TestFixture]
	class T4TemplateProcessorTest
	{
		[TestCase("", "", true)]
		[TestCase(" ", "", true)]
		[TestCase("", " ", true)]
		[TestCase("a", "", true)]
		[TestCase("a", " ", true)]
		[TestCase("", "a", true)]
		[TestCase(" ", "a", true)]
		[TestCase("a", "a", false)]
		public void GeneratTemplate_ErrorTest(string name, string cls, bool throwResult)
		{
			var t4 = new T4TemplateProcessor() {
				Namespace = name,
				ClassName = cls,
			};
			bool isThrow;
			try {
				t4.GeneratTemplate();
				isThrow = false;
			} catch(InvalidOperationException ex) {
				Debug.WriteLine(ex);
				isThrow = true;
			} catch(Exception) {
				return;
			}
			Assert.IsTrue(isThrow == throwResult);
		}
	}

	[TestFixture]
	class T4TemplateTest
	{
		[Test]
		public void test()
		{
			var s = @"
<#@ template language=""C#"" debug=""true"" hostSpecific=""true"" #>
...
<#
// ""Host""変数を有効とするために、hostSpecific=""true"" とすること。
var sessionHost = (Microsoft.VisualStudio.TextTemplating.ITextTemplatingSessionHost) Host;
var mx = (int)sessionHost.Session[""maxCount""];
#>
へんすう<#= mx #>。
			";
			T4TemplateUtility.Convert(s);
		}
	}
}
