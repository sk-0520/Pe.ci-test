namespace ContentTypeTextNet.Pe.PeMain.Logic
{
	using System;
using System.Text;
using ContentTypeTextNet.Pe.Library.Utility;
using ContentTypeTextNet.Pe.PeMain.Data;
using ContentTypeTextNet.Pe.PeMain.IF;

	/// <summary>
	/// C#限定でムリくりアプリケーション用テンプレート処理。
	/// </summary>
	[Serializable]
	public class ProgramTemplateProcessor: T4TemplateProcessor, ILanguage
	{
		public ProgramTemplateProcessor()
			: base()
		{ }

		public ProgramTemplateProcessor(TextTemplatingEngineHost host)
			: base(host)
		{ }

		/// <summary>
		/// テンプレートディレクティブ。
		/// </summary>
		public string TemplateDdirective {get;set;}

		public Language Language { get; set; }

		protected override void Initialize()
		{
			NamespaceName = "ContentTypeTextNet.Pe.PeMain.Logic.ProgramTemplateProcessor.Generator";
			ClassName = "ProgramTemplateProcessor";
			TemplateAppDomainName = "TemplateAppDomain";

			TemplateDdirective = string.Join(Environment.NewLine, new[] {
				"<#@ template language=\"C#\" hostspecific=\"true\" #>",
				"<#",
				"var __host    = (Microsoft.VisualStudio.TextTemplating.ITextTemplatingSessionHost) Host;",
				"var app = (IReadOnlyDictionary<string, object>)__host.Session;",
				"#>",
			});

			base.Initialize();
		}

		protected override string MakeTemplateSource()
		{
			var templateSource = new StringBuilder(TemplateSource.Length + 40);

			templateSource.AppendLine(TemplateDdirective);
			templateSource.Append(base.MakeTemplateSource());

			return templateSource.ToString();
		}

		protected void ResetVariable()
		{
			var clipboardItem = ClipboardUtility.CreateClipboardItem(ClipboardType.Text, IntPtr.Zero, new NullLogger());

			Variable[TemplateProgramLanguageName.timestamp] = DateTime.Now;
			Variable[TemplateProgramLanguageName.clipboard] = clipboardItem.Text ?? string.Empty;
			Variable[TemplateProgramLanguageName.application] = Literal.programName;
			Variable[TemplateProgramLanguageName.versionFull] = Literal.ApplicationVersion;
			Variable[TemplateProgramLanguageName.versionNumber] = Literal.Version.FileVersion;
			Variable[TemplateProgramLanguageName.versionHash] = Literal.Version.ProductVersion;
		}

		protected override string TransformText_Impl()
		{
			ResetVariable();

			return base.TransformText_Impl();
		}
	}
}
