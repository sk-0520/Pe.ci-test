namespace ContentTypeTextNet.Pe.Test.ApplicationTest.Logic
{
	using System;
	using ContentTypeTextNet.Pe.PeMain.Logic;
	using NUnit.Framework;

	[TestFixture]
	class TextTemplatingTest
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
			";
			TextTemplating.test(s);
		}
	}
}
