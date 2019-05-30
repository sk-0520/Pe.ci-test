using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Embedded.Model;

namespace ContentTypeTextNet.Pe.Library.Shared.Library.Model
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks>http://www.unicode.org/Public/UNIDATA/Blocks.txt</remarks>
    public class TextMatcher
    {
        #region property

        /// <summary>
        /// 3040..309F; Hiragana
        /// </summary>
        protected virtual Range<char> HiraganaRange { get; } = Range.Create('\u3040', '\u309F');
        /// <summary>
        /// 30A0..30FF; Katakana
        /// </summary>
        protected virtual Range<char> KatakanaRange { get; } = Range.Create('\u30A0', '\u30FF');

        #endregion

        #region function

        protected virtual bool IsHiragana(char c) => HiraganaRange.IsIn(c);
        protected virtual bool IsKatakana(char c) => KatakanaRange.IsIn(c);

        string ConvertCore(string input, Func<string, bool> checker, Action<StringBuilder, string> appender)
        {
            var sb = new StringBuilder(input.Length);
            foreach(var s in TextUtility.GetCharacters(input)) {
                if(checker(s)) {
                    appender(sb, s);
                } else {
                    sb.Append(s);
                }
            }

            return sb.ToString();
        }

        string ConvertCore(string input, Func<string, bool> checker, Func<string, char> converter)
        {
            return ConvertCore(input, checker, (sb, s) => sb.Append(converter(s)));
        }
        string ConvertCore(string input, Func<string, bool> checker, Func<string, string> converter)
        {
            return ConvertCore(input, checker, (sb, s) => sb.Append(converter(s)));
        }

        public string ConvertHiraganaToKataka(string input)
        {
            if(input == null) {
                throw new ArgumentNullException(nameof(input));
            }

            return ConvertCore(input, s => s.Length == 1 && IsHiragana(s[0]), s => (char)(s[0] + 'ァ' - 'ぁ'));
        }
        public string ConvertKatakaToHiragana(string input)
        {
            if(input == null) {
                throw new ArgumentNullException(nameof(input));
            }

            return ConvertCore(input, s => s.Length == 1 && IsKatakana(s[0]), s => (char)(s[0] + 'ぁ' - 'ァ'));
        }

        #endregion
    }
}
