namespace ContentTypeTextNet.Pe.Test.UtilityTest
{
	using System;
	using System.Linq;
	using System.Diagnostics;
	using ContentTypeTextNet.Pe.Library.Utility;
	using NUnit.Framework;

	[TestFixture]
	class T4TemplateProcessorTest
	{
		[TestCase("", "", "", true)]
		[TestCase(" ", "", "", true)]
		[TestCase("", " ", "", true)]
		[TestCase("a", "", "", true)]
		[TestCase("a", " ", "", true)]
		[TestCase("", "a", "", true)]
		[TestCase(" ", "a", "", true)]
		[TestCase("a", "a", "a", true)]
		[TestCase("a", "a", "<#@ template language=\"C#\" #>", false)]
		public void GeneratSource_ErrorTest(string name, string cls, string ts, bool isError)
		{
			var t4 = new T4TemplateProcessor() {
				NamespaceName = name,
				ClassName = cls,
				TemplateSource = ts,
			};
			bool hasError;
			try {
				t4.GeneratSource();
				hasError = t4.GeneratedErrorList.Count > 0;
			} catch(InvalidOperationException ex) {
				Debug.WriteLine(ex);
				hasError = true;
			}
			Assert.IsTrue(hasError == isError);
		}

		[TestCase(" ", true)]
		[TestCase("<#@ template language=\"C#\" #>", false)]
		public void CompileSource_ErrorTest(string ts, bool isError)
		{
			Debug.Assert(!string.IsNullOrEmpty(ts), "GeneratSource_ErrorTest!");

			var t4 = new T4TemplateProcessor() {
				NamespaceName = "a",
				ClassName = "b",
				TemplateSource = ts,
			};

			t4.GeneratSource();
			bool hasError;
			try {
				t4.CompileSource();
				hasError = t4.CompileErrorList.Count > 0;
			} catch(InvalidOperationException) {
				hasError = true;
			}
			if(hasError) {
				Debug.WriteLine("T: " +  string.Join(Environment.NewLine, t4.GeneratedErrorList.Select(e => e.ToString())));
				Debug.WriteLine("S: " + string.Join(Environment.NewLine, t4.CompileErrorList.Select(e => e.ToString())));
			}

			Assert.IsTrue(hasError == isError);
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
