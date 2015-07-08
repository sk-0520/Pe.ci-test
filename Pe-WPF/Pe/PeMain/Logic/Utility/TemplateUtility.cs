using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.PeMain.Logic.Utility
{
	//public static class TemplateUtility
	//{
	//	/// <summary>
	//	/// テンプレートアイテムからテンプレートプロセッサ作成。
	//	/// </summary>
	//	/// <param name="item">テンプレートアイテム。テンプレートプロセッサが設定される。</param>
	//	/// <param name="language">使用言語。</param>
	//	/// <returns>作成されたテンプレートプロセッサ。</returns>
	//	public static ProgramTemplateProcessor MakeTemplateProcessor(TemplateItem item, Language language, ILogger logger)
	//	{
	//		Debug.Assert(item.ReplaceMode);
	//		Debug.Assert(item.Program);

	//		if (item.Processor != null) {
	//			item.Processor.Language = language;
	//			try {
	//				item.Processor.TemplateSource = item.Source;
	//			} catch (RemotingException ex) {
	//				logger.Puts(LogType.Error, ex.Message, ex);
	//			}

	//			return item.Processor;
	//		}

	//		var processor = new ProgramTemplateProcessor() {
	//			Language = language,
	//			TemplateSource = item.Source,
	//		};

	//		item.Processor = processor;

	//		return processor;
	//	}
	//}
}
