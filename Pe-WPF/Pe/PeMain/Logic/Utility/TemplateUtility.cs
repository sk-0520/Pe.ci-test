namespace ContentTypeTextNet.Pe.PeMain.Logic.Utility
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Runtime.Remoting;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows.Controls;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Pe.Library.PeData.Item;

	public static class TemplateUtility
	{
		static IReadOnlyDictionary<string, string> GetTemplateMap()
		{
			var map = new Dictionary<string, string>();

			//var clipboardItem = ClipboardUtility.CreateClipboardItem(ClipboardType.Text | ClipboardType.File, IntPtr.Zero, new NullLogger());
			//if(clipboardItem.ClipboardTypes != ClipboardType.None) {
			//	var clipboardText = clipboardItem.Text;
			//	// そのまんま
			//	map[TemplateTextLanguageName.clipboard] = clipboardText;

			//	var lines = clipboardText.SplitLines().ToList();
			//	// 改行を削除
			//	map[TemplateTextLanguageName.clipboardNobreak] = string.Join(string.Empty, lines);
			//	// 先頭行
			//	map[TemplateTextLanguageName.clipboardHead] = lines.FirstOrDefault();
			//	// 最終行
			//	map[TemplateTextLanguageName.clipboardTail] = lines.LastOrDefault();
			//}

			return map;
		}

		/// <summary>
		/// テンプレートアイテムからテンプレートプロセッサ作成。
		/// </summary>
		/// <param name="item">テンプレートアイテム。テンプレートプロセッサが設定される。</param>
		/// <param name="language">使用言語。</param>
		/// <returns>作成されたテンプレートプロセッサ。</returns>
		public static ProgramTemplateProcessor MakeTemplateProcessor(string source, ProgramTemplateProcessor processor, INonProcess nonProcess)
		{
			if(processor != null) {
				//processor.Language = language;
				try {
					processor.CultureCode = nonProcess.Language.CultureCode;
					processor.TemplateSource = source;
				} catch(RemotingException ex) {
					nonProcess.Logger.Error(ex);
				}

				return processor;
			}

			var result = new ProgramTemplateProcessor() {
				CultureCode = nonProcess.Language.CultureCode,
				TemplateSource = source,
			};

			return result;
		}

		public static string ToPlainText(TemplateIndexItemModel indexModel, TemplateBodyItemModel bodyModel, ProgramTemplateProcessor processor, DateTime dateTime, INonProcess nonProcess)
		{
			if(!indexModel.IsReplace) {
				return bodyModel.Source;
			}
			if(indexModel.IsProgrammableReplace) {
				if(processor.Compiled) {
					return processor.TransformText();
				}
				processor.AllProcess();
				if(processor.Error != null || processor.GeneratedErrorList.Any() || processor.CompileErrorList.Any()) {
					// エラーあり
					if(processor.Error != null) {
						return processor.Error.ToString() + Environment.NewLine + string.Join(Environment.NewLine, processor.GeneratedErrorList.Concat(processor.CompileErrorList).Select(e => e.ToString()));
					} else {
						return string.Join(Environment.NewLine, processor.GeneratedErrorList.Concat(processor.CompileErrorList).Select(e => string.Format("[{0},{1}] {2}: {3}", e.Line - processor.FirstLineNumber, e.Column, e.ErrorNumber, e.ErrorText)));
					}
				}
				return processor.TransformText();
			} else {
				var map = GetTemplateMap();
				var replacedText = nonProcess.Language.GetReplacedWordText(bodyModel.Source, dateTime, map);
				nonProcess.Logger.Debug("replacedText: " + replacedText);
				return replacedText;
			}
		}
	}
}
