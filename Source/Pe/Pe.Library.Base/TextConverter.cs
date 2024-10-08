using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;

namespace ContentTypeTextNet.Pe.Library.Base
{
    public interface IResultBuffer
    {
        #region function

        void Append(char c);
        void Append(string s);
        void AppendFormat(string format, object? arg);
        void AppendFormat(string format, object? arg1, params object[]? args);

        #endregion
    }

    internal class ResultBuffer: IResultBuffer
    {
        public ResultBuffer(StringBuilder buffer)
        {
            Buffer = buffer;
        }

        #region property

        private StringBuilder Buffer { get; }
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

        public void AppendFormat(string format, object? arg)
        {
            Buffer.AppendFormat(CultureInfo.InvariantCulture, format, arg);
            IsAppend = true;
        }
        public void AppendFormat(string format, object? arg1, params object[]? args)
        {
            Buffer.AppendFormat(CultureInfo.InvariantCulture, format, arg1, args);
            IsAppend = true;
        }

        #endregion
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="characterBlocks">入力文字列全体の文字単位分割。</param>
    /// <param name="currentIndex">今処理すべき <paramref name="characterBlocks"/>のインデックス。</param>
    /// <param name="isLastIndex">現在処理は最終インデックスか</param>
    /// <param name="currentText">今処理する文字列。<paramref name="characterBlocks"/>[<paramref name="currentIndex"/>]と同じ</param>
    /// <param name="resultBuffer">変換後文字列。格納された場合はそのまま、格納しない場合は<paramref name="resultBuffer"/>に書き込んだ場合にのみ使用される。<paramref name="currentText"/>が格納される。</param>
    /// <returns>次回読み飛ばし数。0で次文字列へ進んでいく。</returns>
    public delegate int TextConvertDelegate(IReadOnlyList<string> characterBlocks, int currentIndex, bool isLastIndex, string currentText, IResultBuffer resultBuffer);

    /// <summary>
    ///
    /// </summary>
    /// <remarks>http://www.unicode.org/Public/UNIDATA/Blocks.txt</remarks>
    public class TextConverter
    {
        #region define

        private const char KatakanaDakuten = '゙';
        private const char KatakanaHanDakuten = '゚';

        #endregion

        #region variable

        private IDictionary<char, char>? _halfwidthKatakanaDakutenMap;
        private IDictionary<char, char>? _halfwidthKatakanaHandakutenMap;
        private IDictionary<char, char>? _katakanaHalfToFullMap;
        private IDictionary<char, char>? _katakanaFullToHalfMap;
        private IDictionary<char, string>? _dakutenKatakanaFullToHalfMap;

        private IDictionary<char, string>? _hiraganaToRomeMap;
        private IDictionary<string, string>? _hiraganaExToRomeMap;

        #endregion

        #region property

        /// <summary>
        /// 3040..309F; Hiragana
        /// </summary>
        private IReadOnlyMinMax<char> HiraganaRange { get; } = MinMax.Create('\u3040', '\u309F');
        /// <summary>
        /// 30A0..30FF; Katakana
        /// </summary>
        private IReadOnlyMinMax<char> KatakanaRange { get; } = MinMax.Create('\u30A0', '\u30FF');
        /// <summary>
        /// 半角カナ
        /// </summary>
        private IReadOnlyMinMax<char> HalfwidthKatakanaRange { get; } = MinMax.Create('\uFF60', '\uFF9D');
        private IReadOnlyMinMax<char> HalfwidthKatakanaDakutenRange { get; } = MinMax.Create('ﾞ', 'ﾟ');

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
                        ['ｯ'] = 'ッ',
                        ['ｬ'] = 'ャ',
                        ['ｭ'] = 'ュ',
                        ['ｮ'] = 'ョ',
                        ['ｰ'] = 'ー',
                        ['･'] = '・',
                        ['､'] = '、',
                        ['｡'] = '。',
                        ['｢'] = '「',
                        ['｣'] = '」',
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
                        ['ッ'] = 'ｯ',
                        ['ャ'] = 'ｬ',
                        ['ュ'] = 'ｭ',
                        ['ョ'] = 'ｮ',
                        ['ー'] = 'ｰ',
                        ['ﾞ'] = 'ﾞ',
                        ['ﾟ'] = 'ﾟ',
                        ['・'] = '･',
                        ['、'] = '､',
                        ['。'] = '｡',
                        ['「'] = '｢',
                        ['」'] = '｣',
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

        protected virtual IDictionary<char, string> HiraganaToRomeMap => this._hiraganaToRomeMap ??= new Dictionary<char, string>() {
            ['あ'] = "a",
            ['い'] = "i",
            ['う'] = "u",
            ['え'] = "e",
            ['お'] = "o",
            ['か'] = "ka",
            ['き'] = "ki",
            ['く'] = "ku",
            ['け'] = "ke",
            ['こ'] = "ko",
            ['さ'] = "sa",
            ['し'] = "shi",
            ['す'] = "su",
            ['せ'] = "se",
            ['そ'] = "so",
            ['た'] = "ta",
            ['ち'] = "chi",
            ['つ'] = "tsu",
            ['て'] = "te",
            ['と'] = "to",
            ['な'] = "na",
            ['に'] = "ni",
            ['ぬ'] = "nu",
            ['ね'] = "ne",
            ['の'] = "no",
            ['は'] = "ha",
            ['ひ'] = "hi",
            ['ふ'] = "fu",
            ['へ'] = "he",
            ['ほ'] = "ho",
            ['ま'] = "ma",
            ['み'] = "mi",
            ['む'] = "mu",
            ['め'] = "me",
            ['も'] = "mo",
            ['や'] = "ya",
            //['ゐ'] = "",
            ['ゆ'] = "yu",
            //['ゑ'] = "",
            ['よ'] = "yo",
            ['ら'] = "ra",
            ['り'] = "ri",
            ['る'] = "ru",
            ['れ'] = "re",
            ['ろ'] = "ro",
            ['わ'] = "wa",
            ['を'] = "wo",
            ['ん'] = "n",
            ['ぁ'] = "xa",
            ['ぃ'] = "xi",
            ['ぅ'] = "xu",
            ['ぇ'] = "xe",
            ['ぉ'] = "xo",
            ['っ'] = "xtu",
            ['ゃ'] = "xya",
            ['ゅ'] = "xyu",
            ['ょ'] = "xyo",
            ['が'] = "ga",
            ['ぎ'] = "gi",
            ['ぐ'] = "gu",
            ['げ'] = "ge",
            ['ご'] = "go",
            ['ざ'] = "za",
            ['じ'] = "ji",
            ['ず'] = "zu",
            ['ぜ'] = "ze",
            ['ぞ'] = "zo",
            ['だ'] = "da",
            ['ぢ'] = "di",
            ['づ'] = "zu",
            ['で'] = "de",
            ['ど'] = "do",
            ['ば'] = "ba",
            ['び'] = "bi",
            ['ぶ'] = "bu",
            ['べ'] = "be",
            ['ぼ'] = "bo",
            ['ぱ'] = "pa",
            ['ぴ'] = "pi",
            ['ぷ'] = "pu",
            ['ぺ'] = "pe",
            ['ぽ'] = "po",
        };

        protected virtual IDictionary<string, string> HiraganaExToRomeMap => this._hiraganaExToRomeMap ??= new Dictionary<string, string>() {
            ["いぇ"] = "ye",
            ["うぃ"] = "wi",
            ["うぇ"] = "we",
            //["うぉ"] = "wo",
            ["きゃ"] = "kya",
            ["きゅ"] = "kyu",
            ["きょ"] = "kyo",
            ["ぎゃ"] = "gya",
            ["ぎゅ"] = "gyu",
            ["ぎょ"] = "gyo",
            ["しゃ"] = "sha",
            ["しゅ"] = "shu",
            ["しょ"] = "sho",
            ["じゃ"] = "ja",
            ["じゅ"] = "ju",
            ["じょ"] = "jo",
            ["ちゃ"] = "cha",
            ["ちゅ"] = "chu",
            ["ちょ"] = "cho",
            ["にゃ"] = "nya",
            ["にゅ"] = "nyu",
            ["にょ"] = "nyo",
            ["ひゃ"] = "hya",
            ["ひゅ"] = "hyu",
            ["ひょ"] = "hyo",
            ["びゃ"] = "bya",
            ["びゅ"] = "byu",
            ["びょ"] = "byo",
            ["ぴゃ"] = "pya",
            ["ぴゅ"] = "pyu",
            ["ぴょ"] = "pyo",
            ["みゃ"] = "mya",
            ["みゅ"] = "myu",
            ["みょ"] = "myo",
            ["りゃ"] = "rya",
            ["りゅ"] = "ryu",
            ["りょ"] = "ryo",
        };

        #endregion

        #region function

        protected virtual bool IsHiragana(char c) => HiraganaRange.IsIn(c);
        protected virtual bool IsKatakana(char c) => KatakanaRange.IsIn(c);
        protected virtual bool IsHalfwidthKatakana(char c) => HalfwidthKatakanaRange.IsIn(c) || HalfwidthKatakanaDakutenRange.IsIn(c);
        protected virtual bool IsHalfwidthKatakanaDakutenParent(char c) => 'ｶ' <= c && c <= 'ｺ' || 'ｻ' <= c && c <= 'ｿ' || 'ﾀ' <= c && c <= 'ﾄ' || 'ﾊ' <= c && c <= 'ﾎ' || c == 'ｳ';
        protected virtual bool IsHalfwidthKatakanaHandakutenParent(char c) => 'ﾊ' <= c && c <= 'ﾎ';

        protected virtual bool IsAsciiAlphabetUpper(char c) => 'A' <= c && c <= 'Z';
        protected virtual bool IsAsciiAlphabetLower(char c) => 'a' <= c && c <= 'z';
        protected virtual bool IsAsciiAlphabet(char c) => IsAsciiAlphabetUpper(c) || IsAsciiAlphabetLower(c);
        protected virtual bool IsFullAlphabetUpper(char c) => 'Ａ' <= c && c <= 'Ｚ';
        protected virtual bool IsFullAlphabetLower(char c) => 'ａ' <= c && c <= 'ｚ';
        protected virtual bool IsFullAlphabet(char c) => IsFullAlphabetUpper(c) || IsFullAlphabetLower(c);

        protected virtual bool IsAsciiDigit(char c) => '0' <= c && c <= '9';
        protected virtual bool IsFullDigit(char c) => '０' <= c && c <= '９';

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S127:\"for\" loop stop conditions should be invariant", Justification = "<保留中>")]
        protected string ConvertCore(string input, IEnumerable<TextConvertDelegate> converters)
        {
            var sb = new StringBuilder(input.Length);
            var chars = TextUtility.GetCharacters(input).ToArray();
            for(var i = 0; i < chars.Length; i++) {
                var currentText = chars[i];
                var isLastIndex = i == chars.Length - 1;
                var skip = 0;
                var resultBuffer = new ResultBuffer(sb);
                foreach(var converter in converters) {
                    skip = converter(chars, i, isLastIndex, currentText, resultBuffer);
                    if(resultBuffer.IsAppend) {
                        break;
                    }
                }
                if(!resultBuffer.IsAppend) {
                    sb.Append(currentText);
                }
                i += skip;
            }

            return sb.ToString();
        }

        private int ConvertHiraganaToKatakanaCore(IReadOnlyList<string> characterBlocks, int currentIndex, bool isLastIndex, string currentText, IResultBuffer resultBuffer)
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
        public string ConvertHiraganaToKatakana(string input)
        {
            if(input == null) {
                throw new ArgumentNullException(nameof(input));
            }

            return ConvertCore(input, new TextConvertDelegate[] {
                ConvertHiraganaToKatakanaCore
            });
        }

        private int ConvertKatakanaToHiraganaCore(IReadOnlyList<string> characterBlocks, int currentIndex, bool isLastIndex, string currentText, IResultBuffer resultBuffer)
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
        public string ConvertKatakanaToHiragana(string input)
        {
            if(input == null) {
                throw new ArgumentNullException(nameof(input));
            }

            return ConvertCore(input, new TextConvertDelegate[] {
                ConvertKatakanaToHiraganaCore
            });
        }

        private int ConvertHankakuKatakanaToZenkakuKatakanaCore(IReadOnlyList<string> characterBlocks, int currentIndex, bool isLastIndex, string currentText, IResultBuffer resultBuffer)
        {
            var skip = 0;
            if(currentText.Length == 1 && IsHalfwidthKatakana(currentText[0])) {
                if(isLastIndex) {
                    if(HalfwidthKatakanaDakutenRange.IsIn(currentText[0])) {
                        switch(currentText[0]) {
                            case 'ﾞ':
                                resultBuffer.Append('゛');
                                break;

                            case 'ﾟ':
                                resultBuffer.Append('゜');
                                break;

                            default:
                                throw new NotImplementedException();
                        }
                    } else {
                        resultBuffer.Append(KatakanaHalfToFullMap[currentText[0]]);
                    }
                } else if(characterBlocks[currentIndex + 1].Length == 1 && HalfwidthKatakanaDakutenRange.IsIn(characterBlocks[currentIndex + 1][0])) {
                    // 合体する必要あるかも！
                    switch(characterBlocks[currentIndex + 1][0]) {
                        case 'ﾟ':
                            if(IsHalfwidthKatakanaHandakutenParent(currentText[0])) {
                                resultBuffer.Append(HalfwidthKatakanaHandakutenMap[currentText[0]]);
                            } else {
                                resultBuffer.Append(KatakanaHalfToFullMap[currentText[0]]);
                                resultBuffer.Append(KatakanaHanDakuten); // 結合文字
                            }
                            break;

                        case 'ﾞ':
                            if(IsHalfwidthKatakanaDakutenParent(currentText[0])) {
                                resultBuffer.Append(HalfwidthKatakanaDakutenMap[currentText[0]]);
                            } else {
                                resultBuffer.Append(KatakanaHalfToFullMap[currentText[0]]);
                                resultBuffer.Append(KatakanaDakuten); // 結合文字
                            }
                            break;

                        default:
                            throw new NotImplementedException();
                    }
                    skip = 1;
                } else {
                    resultBuffer.Append(KatakanaHalfToFullMap[currentText[0]]);
                }
            } else if(currentText.Length == 2) {
                foreach(var pair in DakutenKatakanaFullToHalfMap) {
                    if(pair.Value == currentText) {
                        resultBuffer.Append(pair.Key);
                        break;
                    }
                }

                //if(IsHalfwidthKatakana(currentText[0])) {
                //    resultBuffer.Append(KatakanaHalfToFullMap[currentText[0]]);
                //    if(currentText[1] == 'ﾞ') {
                //        resultBuffer.Append('\u3099'); // 結合文字
                //    } else if(currentText[1] == 'ﾟ') {
                //        resultBuffer.Append('\u309A'); // 結合文字
                //    }
                //}
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
        }

        private int ConvertZenkakuKatakanaToHankakuKatakanaCore(IReadOnlyList<string> characterBlocks, int currentIndex, bool isLastIndex, string currentText, IResultBuffer resultBuffer)
        {
            if(currentText.Length == 1) {
                if(KatakanaFullToHalfMap.TryGetValue(currentText[0], out var normal)) {
                    resultBuffer.Append(normal);
                } else if(DakutenKatakanaFullToHalfMap.TryGetValue(currentText[0], out var dakuten)) {
                    resultBuffer.Append(dakuten);
                } else {
                    var map = new Dictionary<char, char>() {
                        ['゜'] = 'ﾟ',
                        ['゛'] = 'ﾞ',
                    };
                    if(map.TryGetValue(currentText[0], out var c)) {
                        resultBuffer.Append(c);
                    }
                }
            } else {
                var map = new Dictionary<char, char>() {
                    [KatakanaHanDakuten] = 'ﾟ',
                    [KatakanaDakuten] = 'ﾞ',
                };
                foreach(var pair in map) {
                    var index = currentText.IndexOf(pair.Key);
                    if(index != -1) {
                        var s = currentText.Substring(0, index);
                        if(s.Length == 1) {
                            if(KatakanaFullToHalfMap.TryGetValue(s[0], out var normal)) {
                                resultBuffer.Append(normal);
                            } else if(DakutenKatakanaFullToHalfMap.TryGetValue(s[0], out var dakuten)) {
                                resultBuffer.Append(dakuten);
                            }
                            resultBuffer.Append(pair.Value);
                            break;
                        }
                    }
                }
            }

            return 0;
        }

        /// <summary>
        /// 全角カタカナを半角カタカナに変換。
        /// </summary>
        /// <param name="input"></param>
        /// <returns>全角カタカナ以外はそのまま。</returns>
        public string ConvertZenkakuKatakanaToHankakuKatakana(string input)
        {
            if(input == null) {
                throw new ArgumentNullException(nameof(input));
            }

            return ConvertCore(input, new TextConvertDelegate[] {
                ConvertZenkakuKatakanaToHankakuKatakanaCore
            });
        }

        private int ConvertAsciiAlphabetToZenkakuAlphabetCore(IReadOnlyList<string> characterBlocks, int currentIndex, bool isLastIndex, string currentText, IResultBuffer resultBuffer)
        {
            if(currentText.Length == 1) {
                var c = currentText[0];
                if(IsAsciiAlphabet(c)) {
                    if(IsAsciiAlphabetUpper(c)) {
                        resultBuffer.Append((char)(c - 'A' + 'Ａ'));
                    } else {
                        Debug.Assert(IsAsciiAlphabetLower(c));
                        resultBuffer.Append((char)(c - 'a' + 'ａ'));
                    }
                }
            }

            return 0;
        }

        /// <summary>
        /// 半角アルファベットを全角アルファベットに変換。
        /// </summary>
        /// <param name="input"></param>
        /// <returns>半角アルファベット以外はそのまま。</returns>
        public string ConvertAsciiAlphabetToZenkakuAlphabet(string input)
        {
            if(input == null) {
                throw new ArgumentNullException(nameof(input));
            }

            return ConvertCore(input, new TextConvertDelegate[] {
                ConvertAsciiAlphabetToZenkakuAlphabetCore
            });
        }

        private int ConvertZenkakuAlphabetToAsciiAlphabetCore(IReadOnlyList<string> characterBlocks, int currentIndex, bool isLastIndex, string currentText, IResultBuffer resultBuffer)
        {
            if(currentText.Length == 1) {
                var c = currentText[0];
                if(IsFullAlphabet(c)) {
                    if(IsFullAlphabetUpper(c)) {
                        resultBuffer.Append((char)(c - 'Ａ' + 'A'));
                    } else {
                        Debug.Assert(IsFullAlphabetLower(c));
                        resultBuffer.Append((char)(c - 'ａ' + 'a'));
                    }
                }
            }

            return 0;
        }

        /// <summary>
        /// 全角アルファベットを半角アルファベットに変換。
        /// </summary>
        /// <param name="input"></param>
        /// <returns>全角アルファベット以外はそのまま。</returns>
        public string ConvertZenkakuAlphabetToAsciiAlphabet(string input)
        {
            if(input == null) {
                throw new ArgumentNullException(nameof(input));
            }

            return ConvertCore(input, new TextConvertDelegate[] {
                ConvertZenkakuAlphabetToAsciiAlphabetCore
            });
        }

        private int ConvertAsciiDigitToZenkakuDigitCore(IReadOnlyList<string> characterBlocks, int currentIndex, bool isLastIndex, string currentText, IResultBuffer resultBuffer)
        {
            if(currentText.Length == 1) {
                var c = currentText[0];
                if(IsAsciiDigit(c)) {
                    resultBuffer.Append((char)(c - '0' + '０'));
                }
            }

            return 0;
        }

        /// <summary>
        /// 半角数値を全角数値に変換。
        /// </summary>
        /// <param name="input"></param>
        /// <returns>半角数値以外はそのまま。</returns>
        public string ConvertAsciiDigitToZenkakuDigit(string input)
        {
            if(input == null) {
                throw new ArgumentNullException(nameof(input));
            }

            return ConvertCore(input, new TextConvertDelegate[] {
                ConvertAsciiDigitToZenkakuDigitCore
            });
        }

        private int ConvertZenkakuDigitToAsciiDigitCore(IReadOnlyList<string> characterBlocks, int currentIndex, bool isLastIndex, string currentText, IResultBuffer resultBuffer)
        {
            if(currentText.Length == 1) {
                var c = currentText[0];
                if(IsFullDigit(c)) {
                    resultBuffer.Append((char)(c - '０' + '0'));
                }
            }

            return 0;
        }

        /// <summary>
        /// 全角数値を半角数値に変換。
        /// </summary>
        /// <param name="input"></param>
        /// <returns>全角数値以外はそのまま。</returns>
        public string ConvertZenkakuDigitToAsciiDigit(string input)
        {
            if(input == null) {
                throw new ArgumentNullException(nameof(input));
            }

            return ConvertCore(input, new TextConvertDelegate[] {
                ConvertZenkakuDigitToAsciiDigitCore
            });
        }

        private int ConvertHiraganaToAsciiRomeCore(IReadOnlyList<string> characterBlocks, int currentIndex, bool isLastIndex, string currentText, IResultBuffer resultBuffer)
        {
            if(currentText.Length != 1) {
                return 0;
            }

            var c = currentText[0];
            if(!IsHiragana(c)) {
                return 0;
            }

            if(HiraganaToRomeMap.TryGetValue(c, out var s)) {
                if(!isLastIndex) {
                    var next = characterBlocks[currentIndex + 1];
                    if(next.Length == 1 && IsHiragana(next[0])) {
                        var exKey = new string(new[] { c, next[0] });
                        if(HiraganaExToRomeMap.TryGetValue(exKey, out var exString)) {
                            resultBuffer.Append(exString);
                            return 1;
                        }
                    }
                }
                resultBuffer.Append(s);
            }

            return 0;
        }

        /// <summary>
        /// 平仮名をローマ字(半角アルファベット)に変換。
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public string ConvertHiraganaToAsciiRome(string input)
        {
            if(input == null) {
                throw new ArgumentNullException(nameof(input));
            }

            return ConvertCore(input, new TextConvertDelegate[] {
                ConvertHiraganaToAsciiRomeCore
            });
        }

        /// <summary>
        /// もう好きにせぇや。
        /// </summary>
        /// <param name="input"></param>
        /// <param name="converter"></param>
        /// <returns></returns>
        public string ConvertToCustom(string input, TextConvertDelegate converter)
        {
            return ConvertCore(input, new TextConvertDelegate[] {
                converter
            });
        }

        #endregion
    }
}
