using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Embedded.Model;

namespace ContentTypeTextNet.Pe.Library.Shared.Library.Model
{
    public interface IResultBuffer
    {
        #region function

        void Append(char c);
        void Append(string s);

        #endregion
    }

    internal class ResultBuffer : IResultBuffer
    {
        public ResultBuffer(StringBuilder buffer)
        {
            Buffer = buffer;
        }

        #region property

        StringBuilder Buffer { get; }
        public bool IsAppend { get; private set; }
        #endregion

        #region IResultBuffer
        public void Append(char c)
        {
            Buffer.Append(c);
            IsAppend = true;
        }

        public void Append(string s)
        {
            Buffer.Append(s);
            IsAppend = true;
        }
        #endregion
    }

    /// <summary>
    ///
    /// </summary>
    /// <remarks>http://www.unicode.org/Public/UNIDATA/Blocks.txt</remarks>
    public class TextConverter
    {
        #region define

        /// <summary>
        ///
        /// </summary>
        /// <param name="characterBlocks"></param>
        /// <param name="currentIndex"></param>
        /// <param name="isLastIndex"></param>
        /// <param name="currentText"></param>
        /// <param name="resultBuffer"></param>
        /// <returns>次回読み飛ばし数。<param name="resultBuffer" />に書き込んだ場合にのみ使用される。</returns>
        protected delegate int TextConvertDelegate(IReadOnlyList<string> characterBlocks, int currentIndex, bool isLastIndex, string currentText, IResultBuffer resultBuffer);

        #endregion

        #region variable

        Dictionary<char, char> _halfwidthKatakanaDakutenMap;
        Dictionary<char, char> _halfwidthKatakanaHandakutenMap;
        IDictionary<char, char> _katakanaHalfToFullMap;
        IDictionary<char, char> _katakanaFullToHalfMap;
        IDictionary<char, string> _dakutenKatakanaFullToHalfMap;
        #endregion

        #region property

        /// <summary>
        /// 3040..309F; Hiragana
        /// </summary>
        Range<char> HiraganaRange { get; } = Range.Create('\u3040', '\u309F');
        /// <summary>
        /// 30A0..30FF; Katakana
        /// </summary>
        Range<char> KatakanaRange { get; } = Range.Create('\u30A0', '\u30FF');
        /// <summary>
        /// 半角カナ
        /// </summary>
        Range<char> HalfwidthKatakanaRange { get; } = Range.Create('\uFF60', '\uFF9D');
        Range<char> HalfwidthKatakanaDakutenRange { get; } = Range.Create('ﾞ', 'ﾟ');

        protected virtual IDictionary<char, char> HalfwidthKatakanaDakutenMap
        {
            get
            {
                if(this._halfwidthKatakanaDakutenMap == null) {
                    this._halfwidthKatakanaDakutenMap = new Dictionary<char, char>() {
                        ['ｶ'] = 'ガ',
                        ['ｷ'] = 'ギ',
                        ['ｸ'] = 'グ',
                        ['ｹ'] = 'ゲ',
                        ['ｺ'] = 'ゴ',
                        ['ｻ'] = 'ザ',
                        ['ｼ'] = 'ジ',
                        ['ｽ'] = 'ズ',
                        ['ｾ'] = 'ゼ',
                        ['ｿ'] = 'ゾ',
                        ['ﾀ'] = 'ダ',
                        ['ﾁ'] = 'ヂ',
                        ['ﾂ'] = 'ヅ',
                        ['ﾃ'] = 'デ',
                        ['ﾄ'] = 'ド',
                        ['ﾊ'] = 'バ',
                        ['ﾋ'] = 'ビ',
                        ['ﾌ'] = 'ブ',
                        ['ﾍ'] = 'ベ',
                        ['ﾎ'] = 'ボ',
                        ['ｳ'] = 'ヴ',
                    };
                }

                return this._halfwidthKatakanaDakutenMap;
            }
        }

        protected virtual IDictionary<char, char> HalfwidthKatakanaHandakutenMap
        {
            get
            {
                if(this._halfwidthKatakanaHandakutenMap == null) {
                    this._halfwidthKatakanaHandakutenMap = new Dictionary<char, char>() {
                        ['ﾊ'] = 'パ',
                        ['ﾋ'] = 'ピ',
                        ['ﾌ'] = 'プ',
                        ['ﾍ'] = 'ペ',
                        ['ﾎ'] = 'ポ',
                    };
                }

                return this._halfwidthKatakanaHandakutenMap;
            }
        }

        protected virtual IDictionary<char, char> KatakanaHalfToFullMap
        {
            get
            {
                if(this._katakanaHalfToFullMap == null) {
                    this._katakanaHalfToFullMap = new Dictionary<char, char>() {
                        ['ｱ'] = 'ア',
                        ['ｲ'] = 'イ',
                        ['ｳ'] = 'ウ',
                        ['ｴ'] = 'エ',
                        ['ｵ'] = 'オ',
                        ['ｶ'] = 'カ',
                        ['ｷ'] = 'キ',
                        ['ｸ'] = 'ク',
                        ['ｹ'] = 'ケ',
                        ['ｺ'] = 'コ',
                        ['ｻ'] = 'サ',
                        ['ｼ'] = 'シ',
                        ['ｽ'] = 'ス',
                        ['ｾ'] = 'セ',
                        ['ｿ'] = 'ソ',
                        ['ﾀ'] = 'タ',
                        ['ﾁ'] = 'チ',
                        ['ﾂ'] = 'ツ',
                        ['ﾃ'] = 'テ',
                        ['ﾄ'] = 'ト',
                        ['ﾅ'] = 'ナ',
                        ['ﾆ'] = 'ニ',
                        ['ﾇ'] = 'ヌ',
                        ['ﾈ'] = 'ネ',
                        ['ﾉ'] = 'ノ',
                        ['ﾊ'] = 'ハ',
                        ['ﾋ'] = 'ヒ',
                        ['ﾌ'] = 'フ',
                        ['ﾍ'] = 'ヘ',
                        ['ﾎ'] = 'ホ',
                        ['ﾏ'] = 'マ',
                        ['ﾐ'] = 'ミ',
                        ['ﾑ'] = 'ム',
                        ['ﾒ'] = 'メ',
                        ['ﾓ'] = 'モ',
                        ['ﾔ'] = 'ヤ',
                        ['ﾕ'] = 'ユ',
                        ['ﾖ'] = 'ヨ',
                        ['ﾗ'] = 'ラ',
                        ['ﾘ'] = 'リ',
                        ['ﾙ'] = 'ル',
                        ['ﾚ'] = 'レ',
                        ['ﾛ'] = 'ロ',
                        ['ﾜ'] = 'ワ',
                        ['ｦ'] = 'ヲ',
                        ['ﾝ'] = 'ン',
                        ['ｧ'] = 'ァ',
                        ['ｨ'] = 'ィ',
                        ['ｩ'] = 'ゥ',
                        ['ｪ'] = 'ェ',
                        ['ｫ'] = 'ォ',
                        ['ｬ'] = 'ャ',
                        ['ｭ'] = 'ュ',
                        ['ｮ'] = 'ョ',
                    };
                    Debug.Assert(this._katakanaHalfToFullMap.Keys.All(c => IsHalfwidthKatakana(c)));
                }

                return this._katakanaHalfToFullMap;
            }
        }

        protected virtual IDictionary<char, char> KatakanaFullToHalfMap
        {
            get
            {
                if(this._katakanaFullToHalfMap == null) {
                    this._katakanaFullToHalfMap = new Dictionary<char, char>() {
                        ['ア'] = 'ｱ',
                        ['イ'] = 'ｲ',
                        ['ウ'] = 'ｳ',
                        ['エ'] = 'ｴ',
                        ['オ'] = 'ｵ',
                        ['カ'] = 'ｶ',
                        ['キ'] = 'ｷ',
                        ['ク'] = 'ｸ',
                        ['ケ'] = 'ｹ',
                        ['コ'] = 'ｺ',
                        ['サ'] = 'ｻ',
                        ['シ'] = 'ｼ',
                        ['ス'] = 'ｽ',
                        ['セ'] = 'ｾ',
                        ['ソ'] = 'ｿ',
                        ['タ'] = 'ﾀ',
                        ['チ'] = 'ﾁ',
                        ['ツ'] = 'ﾂ',
                        ['テ'] = 'ﾃ',
                        ['ト'] = 'ﾄ',
                        ['ナ'] = 'ﾅ',
                        ['ニ'] = 'ﾆ',
                        ['ヌ'] = 'ﾇ',
                        ['ネ'] = 'ﾈ',
                        ['ノ'] = 'ﾉ',
                        ['ハ'] = 'ﾊ',
                        ['ヒ'] = 'ﾋ',
                        ['フ'] = 'ﾌ',
                        ['ヘ'] = 'ﾍ',
                        ['ホ'] = 'ﾎ',
                        ['マ'] = 'ﾏ',
                        ['ミ'] = 'ﾐ',
                        ['ム'] = 'ﾑ',
                        ['メ'] = 'ﾒ',
                        ['モ'] = 'ﾓ',
                        ['ヤ'] = 'ﾔ',
                        ['ユ'] = 'ﾕ',
                        ['ヨ'] = 'ﾖ',
                        ['ラ'] = 'ﾗ',
                        ['リ'] = 'ﾘ',
                        ['ル'] = 'ﾙ',
                        ['レ'] = 'ﾚ',
                        ['ロ'] = 'ﾛ',
                        ['ワ'] = 'ﾜ',
                        ['ヰ'] = 'ｲ',
                        ['ヱ'] = 'ｴ',
                        ['ヲ'] = 'ｦ',
                        ['ン'] = 'ﾝ',
                        ['ァ'] = 'ｧ',
                        ['ィ'] = 'ｨ',
                        ['ゥ'] = 'ｩ',
                        ['ェ'] = 'ｪ',
                        ['ォ'] = 'ｫ',
                        ['ャ'] = 'ｬ',
                        ['ュ'] = 'ｭ',
                        ['ョ'] = 'ｮ',
                        ['ﾞ'] = 'ﾞ',
                        ['ﾟ'] = 'ﾟ',
                    };
                }

                return this._katakanaFullToHalfMap;
            }
        }

        protected virtual IDictionary<char, string> DakutenKatakanaFullToHalfMap
        {
            get
            {
                if(this._dakutenKatakanaFullToHalfMap == null) {
                    this._dakutenKatakanaFullToHalfMap = new Dictionary<char, string>() {
                        ['ガ'] = "ｶﾞ",
                        ['ギ'] = "ｷﾞ",
                        ['グ'] = "ｸﾞ",
                        ['ゲ'] = "ｹﾞ",
                        ['ゴ'] = "ｺﾞ",
                        ['ザ'] = "ｻﾞ",
                        ['ジ'] = "ｼﾞ",
                        ['ズ'] = "ｽﾞ",
                        ['ゼ'] = "ｾﾞ",
                        ['ゾ'] = "ｿﾞ",
                        ['ダ'] = "ﾀﾞ",
                        ['ヂ'] = "ﾁﾞ",
                        ['ヅ'] = "ﾂﾞ",
                        ['デ'] = "ﾃﾞ",
                        ['ド'] = "ﾄﾞ",
                        ['バ'] = "ﾊﾞ",
                        ['ビ'] = "ﾋﾞ",
                        ['ブ'] = "ﾌﾞ",
                        ['ベ'] = "ﾍﾞ",
                        ['ボ'] = "ﾎﾞ",
                        ['パ'] = "ﾊﾟ",
                        ['ピ'] = "ﾋﾟ",
                        ['プ'] = "ﾌﾟ",
                        ['ペ'] = "ﾍﾟ",
                        ['ポ'] = "ﾎﾟ",
                        ['ヴ'] = "ｳﾞ",
                    };
                }

                return this._dakutenKatakanaFullToHalfMap;
            }
        }
        #endregion

        #region function

        protected virtual bool IsHiragana(char c) => HiraganaRange.IsIn(c);
        protected virtual bool IsKatakana(char c) => KatakanaRange.IsIn(c);
        protected virtual bool IsHalfwidthKatakana(char c) => HalfwidthKatakanaRange.IsIn(c) || HalfwidthKatakanaDakutenRange.IsIn(c);
        protected virtual bool IsHalfwidthKatakanaDakutenParent(char c) => ('ｶ' <= c && c <= 'ｺ') || ('ｻ' <= c && c <= 'ｿ') || ('ﾀ' <= c && c <= 'ﾄ') || ('ﾊ' <= c && c <= 'ﾎ') || (c == 'ｳ');
        protected virtual bool IsHalfwidthKatakanaHandakutenParent(char c) => ('ﾊ' <= c && c <= 'ﾎ');


        string ConvertCore(string input, IEnumerable<TextConvertDelegate> converters)
        {
            var sb = new StringBuilder(input.Length);
            var chars = TextUtility.GetCharacters(input).ToArray();
            for(var i = 0; i < chars.Length; i++) {
                var isLastIndex = i == chars.Length - 1;
                var skip = 0;
                var resultBuffer = new ResultBuffer(sb);
                foreach(var converter in converters) {
                    skip = converter(chars, i, isLastIndex, chars[i], resultBuffer);
                    if(resultBuffer.IsAppend) {
                        break;
                    }
                }
                if(!resultBuffer.IsAppend) {
                    sb.Append(chars[i]);
                }
                i += skip;
            }

            return sb.ToString();
        }

        //string ConvertCore(string input, Func<string, bool> checker, Action<StringBuilder, string> appender)
        //{
        //    var sb = new StringBuilder(input.Length);
        //    foreach(var s in TextUtility.GetCharacters(input)) {
        //        if(checker(s)) {
        //            appender(sb, s);
        //        } else {
        //            sb.Append(s);
        //        }
        //    }

        //    return sb.ToString();
        //}

        //string ConvertCore(string input, Func<string, bool> checker, Func<string, char> converter)
        //{
        //    return ConvertCore(input, checker, (sb, s) => sb.Append(converter(s)));
        //}
        //string ConvertCore(string input, Func<string, bool> checker, Func<string, string> converter)
        //{
        //    return ConvertCore(input, checker, (sb, s) => sb.Append(converter(s)));
        //}

        int ConvertHiraganaToKatakaCore(IReadOnlyList<string> characterBlocks, int currentIndex, bool isLastIndex, string currentText, IResultBuffer resultBuffer)
        {
            if(currentText.Length == 1 && IsHiragana(currentText[0])) {
                resultBuffer.Append((char)(currentText[0] + 'ァ' - 'ぁ'));
            }

            return 0;
        }
        /// <summary>
        /// 平仮名からカタカナへ変換。
        /// </summary>
        /// <param name="input"></param>
        /// <returns>平仮名以外はそのまま。</returns>
        public string ConvertHiraganaToKataka(string input)
        {
            if(input == null) {
                throw new ArgumentNullException(nameof(input));
            }

            return ConvertCore(input, new TextConvertDelegate[] {
                ConvertHiraganaToKatakaCore
            });
            //return ConvertCore(input, s => s.Length == 1 && IsHiragana(s[0]), s => (char)(s[0] + 'ァ' - 'ぁ'));
        }

        int ConvertKatakaToHiraganaCore(IReadOnlyList<string> characterBlocks, int currentIndex, bool isLastIndex, string currentText, IResultBuffer resultBuffer)
        {
            if(currentText.Length == 1 && IsKatakana(currentText[0])) {
                resultBuffer.Append((char)(currentText[0] + 'ぁ' - 'ァ'));
            }

            return 0;
        }

        /// <summary>
        /// カタカナから平仮名へ変換。
        /// </summary>
        /// <param name="input"></param>
        /// <returns>カタカナ以外はそのまま。</returns>
        public string ConvertKatakaToHiragana(string input)
        {
            if(input == null) {
                throw new ArgumentNullException(nameof(input));
            }

            return ConvertCore(input, new TextConvertDelegate[] {
                ConvertKatakaToHiraganaCore
            });
            //return ConvertCore(input, s => s.Length == 1 && IsKatakana(s[0]), s => (char)(s[0] + 'ぁ' - 'ァ'));
        }



        int ConvertHankakuKatakanaToZenkakuKatakanaCore(IReadOnlyList<string> characterBlocks, int currentIndex, bool isLastIndex, string currentText, IResultBuffer resultBuffer)
        {
            var last = isLastIndex;
            var s = currentText;
            var chars = characterBlocks;
            var i = currentIndex;
            var skip = 0;
            if(s.Length == 1 && IsHalfwidthKatakana(s[0])) {
                if(last) {
                    if(HalfwidthKatakanaDakutenRange.IsIn(s[0])) {
                        switch(s[0]) {
                            case 'ﾞ':
                                resultBuffer.Append('ﾞ');
                                break;

                            case 'ﾟ':
                                resultBuffer.Append('ﾟ');
                                break;

                            default:
                                throw new NotImplementedException();
                        }
                    } else {
                        resultBuffer.Append(KatakanaHalfToFullMap[s[0]]);
                    }
                } else if(chars[i + 1].Length == 1 && HalfwidthKatakanaDakutenRange.IsIn(chars[i + 1][0])) {
                    // 合体する必要あるかも！
                    switch(chars[i + 1][0]) {
                        case 'ﾟ':
                            if(IsHalfwidthKatakanaHandakutenParent(s[0])) {
                                resultBuffer.Append(HalfwidthKatakanaHandakutenMap[s[0]]);
                            } else {
                                resultBuffer.Append(KatakanaHalfToFullMap[s[0]]);
                                resultBuffer.Append('\u309A'); // 結合文字
                            }
                            break;

                        case 'ﾞ':
                            if(IsHalfwidthKatakanaDakutenParent(s[0])) {
                                resultBuffer.Append(HalfwidthKatakanaDakutenMap[s[0]]);
                            } else {
                                resultBuffer.Append(KatakanaHalfToFullMap[s[0]]);
                                resultBuffer.Append('\u3099'); // 結合文字
                            }
                            break;

                        default:
                            throw new NotImplementedException();
                    }
                    skip = 1;
                } else {
                    resultBuffer.Append(KatakanaHalfToFullMap[s[0]]);
                }
            }

            return skip;
        }

        /// <summary>
        /// 半角カタカナを全角カタカナに変換。
        /// </summary>
        /// <param name="input"></param>
        /// <returns>半角カタカナ以外はそのまま。</returns>
        public string ConvertHankakuKatakanaToZenkakuKatakana(string input)
        {
            if(input == null) {
                throw new ArgumentNullException(nameof(input));
            }

            return ConvertCore(input, new TextConvertDelegate[] {
                ConvertHankakuKatakanaToZenkakuKatakanaCore
            });

            //var sb = new StringBuilder(input.Length);
            //var chars = TextUtility.GetCharacters(input).ToArray();
            //for(var i = 0; i < chars.Length; i++) {
            //    var last = i == chars.Length - 1;
            //    var s = chars[i];
            //    if(s.Length == 1 && IsHalfwidthKatakana(s[0])) {
            //        if(last) {
            //            if(HalfwidthKatakanaDakutenRange.IsIn(s[0])) {
            //                switch(s[0]) {
            //                    case 'ﾞ':
            //                        sb.Append('ﾞ');
            //                        break;

            //                    case 'ﾟ':
            //                        sb.Append('ﾟ');
            //                        break;

            //                    default:
            //                        throw new NotImplementedException();
            //                }
            //            } else {
            //                sb.Append(KatakanaHalfToFullMap[s[0]]);
            //            }
            //        } else if(chars[i + 1].Length == 1 && HalfwidthKatakanaDakutenRange.IsIn(chars[i + 1][0])) {
            //            // 合体する必要あるかも！
            //            switch(chars[i + 1][0]) {
            //                case 'ﾟ':
            //                    if(IsHalfwidthKatakanaHandakutenParent(s[0])) {
            //                        sb.Append(HalfwidthKatakanaHandakutenMap[s[0]]);
            //                    } else {
            //                        sb.Append(KatakanaHalfToFullMap[s[0]]);
            //                        sb.Append('\u309A'); // 結合文字
            //                    }
            //                    break;

            //                case 'ﾞ':
            //                    if(IsHalfwidthKatakanaDakutenParent(s[0])) {
            //                        sb.Append(HalfwidthKatakanaDakutenMap[s[0]]);
            //                    } else {
            //                        sb.Append(KatakanaHalfToFullMap[s[0]]);
            //                        sb.Append('\u3099'); // 結合文字
            //                    }
            //                    break;

            //                default:
            //                    throw new NotImplementedException();
            //            }
            //            i += 1;
            //        } else {
            //            sb.Append(KatakanaHalfToFullMap[s[0]]);
            //        }
            //    } else {
            //        sb.Append(s);
            //    }
            //}

            //return sb.ToString();
        }


        int ConvertZenkakuKatakanaToHankakuKatakanaCore(IReadOnlyList<string> characterBlocks, int currentIndex, bool isLastIndex, string currentText, IResultBuffer resultBuffer)
        {
            if(currentText.Length == 1) {
                if(KatakanaFullToHalfMap.TryGetValue(currentText[0], out var normal)) {
                    resultBuffer.Append(normal);
                } else if(DakutenKatakanaFullToHalfMap.TryGetValue(currentText[0], out var dakuten)) {
                    resultBuffer.Append(dakuten);
                }
            }

            return 0;
        }

        public string ConvertZenkakuKatakanaToHankakuKatakana(string input)
        {
            if(input == null) {
                throw new ArgumentNullException(nameof(input));
            }

            return ConvertCore(input, new TextConvertDelegate[] {
                ConvertZenkakuKatakanaToHankakuKatakanaCore
            });

            //var sb = new StringBuilder(input.Length);
            //foreach(var s in TextUtility.GetCharacters(input)) {
            //    if(s.Length == 1) {
            //        if(KatakanaFullToHalfMap.TryGetValue(s[0], out var normal)) {
            //            sb.Append(normal);
            //        } else if(DakutenKatakanaFullToHalfMap.TryGetValue(s[0], out var dakuten)) {
            //            sb.Append(dakuten);
            //        } else {
            //            sb.Append(s);
            //        }
            //    } else {
            //        sb.Append(s);
            //    }
            //}

            //return sb.ToString();
        }
        #endregion
    }
}
