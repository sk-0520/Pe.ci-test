/*
This file is part of SharedLibrary.

SharedLibrary is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

SharedLibrary is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with SharedLibrary.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Extension;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;

namespace ContentTypeTextNet.Library.SharedLibrary.Logic
{
    /// <summary>
    /// 独自言語設定の管理薬。
    /// </summary>
    public class LanguageManager: SingleModelWrapperViewModelBase<LanguageCollectionModel>, ILanguage
    {
        /// <summary>
        /// 生成。
        /// </summary>
        /// <param name="model"></param>
        /// <param name="languageFilePath"></param>
        public LanguageManager(LanguageCollectionModel model, string languageFilePath)
            : base(model)
        {
            LanguageFilePath = languageFilePath;
        }

        #region property

        /// <summary>
        /// ファイルパス。
        /// </summary>
        public string LanguageFilePath { get; private set; }

        #endregion

        #region function

        static LanguageItemModel GetItem(IList<LanguageItemModel> list, string key)
        {
            var result = list
                .FirstOrDefault(l => l.Id == key)
                ?? new LanguageItemModel() {
                    Id = key,
                    Word = key,
                }
            ;

            return result;
        }

        /// <summary>
        /// キーからテキスト取得。
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetPlainText(string key)
        {
            var item = GetItem(Model.Words, key);
            return item.Word;
        }

        /// <summary>
        /// システム用
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        protected virtual IDictionary<string, string> GetSystemMap(DateTime dateTime)
        {
            return new Dictionary<string, string>();

            //return new Dictionary<string, string>() {
            //	{ "TIMESTAMP",  dateTime.ToString() },
            //	{ "Y",          dateTime.Year.ToString() },
            //	{ "YYYY",       dateTime.Year.ToString("D4") },
            //	{ "M",          dateTime.Month.ToString() },
            //	{ "MM",         dateTime.Month.ToString("D2") },
            //	{ "MMM",        dateTime.ToString("MMM") },
            //	{ "MMMM",       dateTime.ToString("MMMM") },
            //	{ "D",          dateTime.Day.ToString() },
            //	{ "DD",         dateTime.Day.ToString("D2") },
            //	{ "h",          dateTime.Hour.ToString() },
            //	{ "hh",         dateTime.Hour.ToString("D2") },
            //	{ "m",          dateTime.Minute.ToString() },
            //	{ "mm",         dateTime.Minute.ToString("D2") },
            //	{ "s",          dateTime.Second.ToString() },
            //	{ "ss",         dateTime.Second.ToString("D2") },
            //};
        }

        #endregion

        #region ILanguage

        public string CultureCode { get { return Model.CultureCode; } }

        public string GetReplacedWordText(string words, DateTime dateTime, IReadOnlyDictionary<string, string> map)
        {
            var plainText = GetPlainText(words);
            if(string.IsNullOrWhiteSpace(plainText)) {
                return plainText ?? string.Format("<{0}>", words);
            }

            var usingMap = GetSystemMap(dateTime);
            if(map != null && map.Any()) {
                foreach(var pair in map) {
                    usingMap[pair.Key] = pair.Value;
                }
            }

            var replacedSystemMapText = plainText.ReplaceRangeFromDictionary("@[", "]", usingMap);
            var replacedDefineText = replacedSystemMapText.ReplaceRange("${", "}", s => GetItem(Model.Define, s).Word);

            return replacedDefineText;
        }

        public string this[string key, IReadOnlyDictionary<string, string> map = null]
        {
            get { return GetReplacedWordText(key, DateTime.Now, map); }
        }

        public string GuiTextToPlainText(string guiText)
        {
            var reg = new Regex(@"^(?<HEAD>.*?)(?<L>\(?)(?<KEY>_\w)(?<R>\)?)(?<TAIL>.*)$", RegexOptions.ExplicitCapture);
            var match = reg.Match(guiText);
            if(match.Success) {
                if(0 < match.Groups["L"].Length && 0 < match.Groups["R"].Length) {
                    return match.Groups["HEAD"].Value + match.Groups["TAIL"].Value;
                } else {
                    var key = match.Groups["KEY"].Value.Substring(1);
                    return match.Groups["HEAD"].Value + key + match.Groups["TAIL"].Value;
                }
            }

            return guiText;
        }
        #endregion
    }
}
