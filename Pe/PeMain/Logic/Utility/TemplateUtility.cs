/**
This file is part of Pe.

Pe is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

Pe is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with Pe.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Extension;
using ContentTypeTextNet.Pe.Library.PeData.Define;
using ContentTypeTextNet.Pe.Library.PeData.Item;
using System.Diagnostics;

namespace ContentTypeTextNet.Pe.PeMain.Logic.Utility
{
    public static class TemplateUtility
    {
        internal const string textReplaceKeywordHead = "@[";
        internal const string textReplaceKeywordTail = "]";

        static IReadOnlyDictionary<string, string> GetTemplateMap(INonProcess nonProcess)
        {
            var map = new Dictionary<string, string>();

            var clipboardItem = ClipboardUtility.GetClipboardData(ClipboardType.Text | ClipboardType.Files, IntPtr.Zero);
            if(clipboardItem.Type != ClipboardType.None) {
                var clipboardText = clipboardItem.Body.Text;
                // そのまんま
                map[TemplateReplaceKey.textClipboard] = clipboardText;

                var lines = clipboardText.SplitLines().ToList();
                // 改行を削除
                map[TemplateReplaceKey.textClipboardNobreak] = string.Join(string.Empty, lines);
                // 先頭行
                map[TemplateReplaceKey.textClipboardHead] = lines.FirstOrDefault();
                // 最終行
                map[TemplateReplaceKey.textClipboardTail] = lines.LastOrDefault();
            }

            return map;
        }

        /// <summary>
        /// テンプレートアイテムからテンプレートプロセッサ作成。
        /// </summary>
        /// <param name="item">テンプレートアイテム。テンプレートプロセッサが設定される。</param>
        /// <param name="language">使用言語。</param>
        /// <returns>作成されたテンプレートプロセッサ。</returns>
        public static ProgramTemplateProcessor MakeTemplateProcessor(string source, ProgramTemplateProcessor processor, INonProcess appNonProcess)
        {
            if(processor != null) {
                //processor.Language = language;
                try {
                    processor.CultureCode = appNonProcess.Language.CultureCode;
                    processor.TemplateSource = source;
                } catch(RemotingException ex) {
                    appNonProcess.Logger.Error(ex);
                }

                return processor;
            }

            var result = new ProgramTemplateProcessor(appNonProcess) {
                CultureCode = appNonProcess.Language.CultureCode,
                TemplateSource = source,
            };

            return result;
        }

        static string ToPlainTextProgrammable(TemplateIndexItemModel indexModel, TemplateBodyItemModel bodyModel, ProgramTemplateProcessor processor, DateTime dateTime, INonProcess appNonProcess)
        {
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
        }

        public static IDictionary<string, string> GetTextTemplateMap(DateTime timestamp, INonProcess nonProcess)
        {
            var templateMap = GetTemplateMap(nonProcess);
            var appMap = AppLanguageManager.GetAppMap(DateTime.Now, nonProcess.Language);

            var result = templateMap.Concat(appMap).ToDictionary(p => p.Key, p => p.Value);

            return result;
        }

        static string ToPlainTextReplace(TemplateIndexItemModel indexModel, TemplateBodyItemModel bodyModel, DateTime dateTime, INonProcess nonProcess)
        {
            var src = bodyModel.Source ?? string.Empty;
            if(string.IsNullOrWhiteSpace(src)) {
                return src;
            }

            var map = GetTextTemplateMap(dateTime, nonProcess);

            var result = src.ReplaceRangeFromDictionary(textReplaceKeywordHead, textReplaceKeywordTail, map);

            return result;
        }

        public static string ToPlainText(TemplateIndexItemModel indexModel, TemplateBodyItemModel bodyModel, ProgramTemplateProcessor processor, DateTime dateTime, INonProcess appNonProcess)
        {
            if(indexModel.TemplateReplaceMode == TemplateReplaceMode.None) {
                return bodyModel.Source ?? string.Empty;
            }
            if(indexModel.TemplateReplaceMode == TemplateReplaceMode.Program) {
                CheckUtility.EnforceNotNull(processor);
                return ToPlainTextProgrammable(indexModel, bodyModel, processor, dateTime, appNonProcess);
            } else {
                Debug.Assert(indexModel.TemplateReplaceMode == TemplateReplaceMode.Text);
                return ToPlainTextReplace(indexModel, bodyModel, dateTime, appNonProcess);
            }
        }
    }
}
