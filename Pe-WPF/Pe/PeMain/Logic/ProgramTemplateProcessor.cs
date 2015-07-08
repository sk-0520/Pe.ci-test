namespace ContentTypeTextNet.Pe.PeMain.Logic
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.Logic.T4Template;
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Extension;

	/// <summary>
	/// C#限定でムリくりアプリケーション用テンプレート処理。
	/// TODO: 既存の処理を暫定的に外してる
	/// </summary>
	[Serializable]
	public class ProgramTemplateProcessor : T4TemplateProcessor
	{
		const string directiveLang = "LANGUAGE";

		public ProgramTemplateProcessor()
			: base()
		{ }

		public ProgramTemplateProcessor(TextTemplatingEngineHost host)
			: base(host)
		{ }

		/// <summary>
		/// テンプレートディレクティブ。
		/// </summary>
		public string TemplateDirective { get; set; }

		/// <summary>
		/// 言語コード。
		/// </summary>
		public string CultureCode { get; set; }

		protected override void Initialize()
		{
			base.Initialize();

			NamespaceName = "ContentTypeTextNet.Pe.PeMain.Logic.ProgramTemplateProcessor.Generator";
			ClassName = "ProgramTemplateProcessor";
			TemplateAppDomainName = "TemplateAppDomain";

			var templateDirective = new[] {
				"<#@ template language=\"C#\" hostspecific=\"true\" {{" + directiveLang + "}} #>",
				"<#",
				"var __host    = (Microsoft.VisualStudio.TextTemplating.ITextTemplatingSessionHost) Host;",
				"var app = (IReadOnlyDictionary<string, object>)__host.Session;",
				"#>",
			};
			TemplateDirective = string.Join(Environment.NewLine, templateDirective);

			FirstLineNumber = templateDirective.Length;
		}

		protected override string MakeTemplateSource()
		{
			var templateSource = new StringBuilder(TemplateSource.Length + 40);

			var map = new Dictionary<string, string>() {
				{ directiveLang, string.Empty },
			};
			if (!string.IsNullOrWhiteSpace(CultureCode)) {
				map[directiveLang] = string.Format("culture=\"{0}\"", CultureCode);
			}
			var templateDirective = TemplateDirective.ReplaceRangeFromDictionary("{{", "}}", map);

			templateSource.AppendLine(templateDirective);
			templateSource.Append(base.MakeTemplateSource());

			return templateSource.ToString();
		}

		protected void ResetVariable()
		{
			//var clipboardItem = ClipboardUtility.CreateClipboardItem(ClipboardType.Text, IntPtr.Zero, new NullLogger());

			//Variable[TemplateProgramLanguageName.timestamp] = DateTime.Now;
			//Variable[TemplateProgramLanguageName.clipboard] = clipboardItem.Text ?? string.Empty;
			//Variable[TemplateProgramLanguageName.application] = Literal.programName;
			//Variable[TemplateProgramLanguageName.versionFull] = Literal.ApplicationVersion;
			//Variable[TemplateProgramLanguageName.versionNumber] = Literal.Version.FileVersion;
			//Variable[TemplateProgramLanguageName.versionHash] = Literal.Version.ProductVersion;
		}

		protected override string TransformText_Impl()
		{
			ResetVariable();

			return base.TransformText_Impl();
		}
	}
}
