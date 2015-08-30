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
	using ContentTypeTextNet.Pe.PeMain.Logic.Utility;
	using ContentTypeTextNet.Pe.Library.PeData.Define;

	/// <summary>
	/// C#限定でムリくりアプリケーション用テンプレート処理。
	/// </summary>
	[Serializable]
	public class ProgramTemplateProcessor : T4TemplateProcessor
	{
		#region define

		const string directiveLang = "LANGUAGE";

		#endregion

		public ProgramTemplateProcessor(ILogger logger = null)
			: base(logger)
		{ }

		public ProgramTemplateProcessor(TextTemplatingEngineHost host, ILogger logger = null)
			: base(host, logger)
		{ }

		#region property

		/// <summary>
		/// テンプレートディレクティブ。
		/// </summary>
		public string TemplateDirective { get; set; }

		/// <summary>
		/// 言語コード。
		/// </summary>
		public string CultureCode { get; set; }

		#endregion

		#region function

		protected void ResetVariable()
		{
			var clipboardData = ClipboardUtility.GetClipboardData(ClipboardType.Text, IntPtr.Zero, Logger);

			Variable[TemplateReplaceKey.programTimestamp] = DateTime.Now;
			Variable[TemplateReplaceKey.programClipboard] = clipboardData.Body.Text ?? string.Empty;
			Variable[TemplateReplaceKey.programApplicationName] = Constants.ApplicationName;
			Variable[TemplateReplaceKey.programApplicationVersion] = Constants.ApplicationVersion;
			Variable[TemplateReplaceKey.programApplicationVersionNumber] = Constants.ApplicationVersionNumber;
			Variable[TemplateReplaceKey.programApplicationVersionRevision] = Constants.ApplicationVersionRevision;
		}

		#endregion

		#region T4TemplateProcessor

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
			if(!string.IsNullOrWhiteSpace(CultureCode)) {
				map[directiveLang] = string.Format("culture=\"{0}\"", CultureCode);
			}
			var templateDirective = TemplateDirective.ReplaceRangeFromDictionary("{{", "}}", map);

			templateSource.AppendLine(templateDirective);
			templateSource.Append(base.MakeTemplateSource());

			return templateSource.ToString();
		}

		protected override string TransformText_Impl()
		{
			ResetVariable();

			return base.TransformText_Impl();
		}

		#endregion
	}
}
