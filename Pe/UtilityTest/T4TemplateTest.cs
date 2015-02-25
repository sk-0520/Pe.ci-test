namespace ContentTypeTextNet.Pe.Test.UtilityTest
{
	using System;
	using ContentTypeTextNet.Pe.Library.Utility;
	using NUnit.Framework;

	[TestFixture]
	class T4TemplateTest
	{
		//[Test]
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
			";
			T4Template.test(s);
		}
	}
}
