namespace ContentTypeTextNet.Pe.PeMain
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	public static class TemplateReplaceKey
	{
		public const string clipboard = "CLIP";
		public const string clipboardNobreak = "CLIP:NOBREAK";
		public const string clipboardHead = "CLIP:HEAD";
		public const string clipboardTail = "CLIP:TAIL";

		public static IReadOnlyList<string> GetMembersList()
		{
			return new[] {
				clipboard,
				clipboardNobreak,
				clipboardHead,
				clipboardTail,
			};
		}
	}
}
