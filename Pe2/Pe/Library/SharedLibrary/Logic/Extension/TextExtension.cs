namespace ContentTypeTextNet.Library.SharedLibrary.Logic.Extension
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	public static class TextExtension
	{
		/// <summary>
		/// 文字列を改行で区切る。
		/// </summary>
		/// <param name="lines">何らかの文字列</param>
		/// <returns>改行を含めない各行。</returns>
		public static IEnumerable<string> SplitLines(this string lines)
		{
			if(lines != null) {
				using(var stream = new StringReader(lines)) {
					string line = null;
					while((line = stream.ReadLine()) != null) {
						yield return line;
					}
				}
			}
		}
	}
}
