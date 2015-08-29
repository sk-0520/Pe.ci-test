namespace ContentTypeTextNet.Pe.PeMain
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	public static class TemplateReplaceKey
	{
		public const string textClipboard = "CLIP";
		public const string textClipboardNobreak = "CLIP:NOBREAK";
		public const string textClipboardHead = "CLIP:HEAD";
		public const string textClipboardTail = "CLIP:TAIL";

		public const string programCode = "<#  #>";
		public const string programExpr = "<#=  #>";
		public const string programDefine = "<#+  #>";

		public static IReadOnlyList<string> TextKeyList = new List<string>() {
			textClipboard,
			textClipboardNobreak,
			textClipboardHead,
			textClipboardTail,

			LanguageKey.timestamp,
			LanguageKey.year,
			LanguageKey.year04,
			LanguageKey.month,
			LanguageKey.month02,
			LanguageKey.monthShortName,
			LanguageKey.monthLongName,
			LanguageKey.day,
			LanguageKey.day02,
			LanguageKey.hour,
			LanguageKey.hour02,
			LanguageKey.minute,
			LanguageKey.minute02,
			LanguageKey.second,
			LanguageKey.second02,

			LanguageKey.application,
			LanguageKey.applicationVersion,
			LanguageKey.applicationRevision,
		};

		public static IReadOnlyList<string> ProgramKeyList = new List<string>() {
			programCode,
			programExpr,

			LanguageKey.timestamp,
			textClipboard,
			LanguageKey.application,
			LanguageKey.applicationVersion,
			LanguageKey.applicationRevision,

			programDefine,
		};

		public static IReadOnlyDictionary<string, Type> ProgramTypes = new Dictionary<string, Type>() {
			{ programCode, null },
			{ programExpr, null },
				
			{ LanguageKey.timestamp, typeof(DateTime) },
			{ textClipboard, typeof(string) },
			{ LanguageKey.application, typeof(string) },
			{ LanguageKey.applicationVersion, typeof(string) },
			{ LanguageKey.applicationRevision, typeof(string) },

			{ programDefine, null },
		};

		public static IReadOnlyList<string> caretInSpaceKeys = new List<string>() {
			programCode,
			programExpr,
			programDefine,
		};

	}
}
