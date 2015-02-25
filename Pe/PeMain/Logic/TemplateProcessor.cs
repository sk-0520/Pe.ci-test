namespace ContentTypeTextNet.Pe.PeMain.Logic
{
	using System.Text;
	using ContentTypeTextNet.Pe.Library.Utility;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.IF;

	/// <summary>
	/// C#限定でムリくりアプリケーション用テンプレート処理。
	/// </summary>
	public class TemplateProcessor: T4TemplateProcessor, ILanguage
	{
		private string _templateDdirective = "<#@ template language=\"C#\" hostspecific=\"true\" #>";

		public TemplateProcessor()
			: base()
		{ }

		public TemplateProcessor(TextTemplatingEngineHost host)
			: base(host)
		{ }

		/// <summary>
		/// テンプレートディレクティブ。
		/// </summary>
		public string TemplateDdirective
		{
			get { return this._templateDdirective; }
			set { this._templateDdirective = value; }
		}

		public Language Language { get; set; }

		protected override void Initialize()
		{
			NamespaceName = "ContentTypeTextNet.Pe.PeMain.Logic.TemplateProcessorGenerator";
			ClassName = "TextProcessor";

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

		}

		protected override string TransformTextImpl()
		{
			ResetVariable();

			return base.TransformTextImpl();
		}
	}
}
