namespace ContentTypeTextNet.Library.SharedLibrary.Logic.Utility
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.Logic.T4Template;
	using Mono.TextTemplating;

	/// <summary>
	/// T4Templateの便利処理。
	/// </summary>
	public static class T4TemplateUtility
	{
		public static string TransformText(string templateContent)
		{
			var host = new TextTemplatingEngineHost();
			host.Session = new TextTemplatingSession();

			var engine = new Microsoft.VisualStudio.TextTemplating.Engine();
			return engine.ProcessTemplate(templateContent, host);
		}

		public static string TransformTextWidthVariable(string templateContent, IDictionary<string, object> variable)
		{
			var host = new TextTemplatingEngineHost();
			host.Session = new TextTemplatingSession();
			foreach (var pair in variable) {
				host.Session.Add(pair);
			}

			var engine = new Microsoft.VisualStudio.TextTemplating.Engine();
			return engine.ProcessTemplate(templateContent, host);
		}
	}
}
