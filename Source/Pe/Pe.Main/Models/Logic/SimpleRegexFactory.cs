using System;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Logic
{
    /// <summary>
    /// 共通使用する正規表現生成所。
    /// </summary>
    public sealed class SimpleRegexFactory
    {
        public SimpleRegexFactory(ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger(GetType());
        }

        #region property

        private ILogger Logger { get; }

        public Regex AllMatchRegex { get; } = new Regex("");

        #endregion

        #region function

        private bool IsCaseSensitivePattern(string pattern)
        {
            foreach(var c in pattern) {
                if(char.IsLetter(c)) {
                    if(char.IsUpper(c)) {
                        // 最初に見つかった英字が大文字であれば大文字小文字を区別する
                        // それが正規表現のなんかであっても気にしない
                        return true;
                    }
                    return false;
                }
            }

            return false;
        }

        /// <summary>
        /// フィルタリング処理用正規表現作成。
        /// </summary>
        /// <param name="pattern">
        ///     <para>/で始まる場合に正規表現パターンとして扱う。</para>
        ///     <para>正規表現ではなく ? * を含む場合にワイルドカードとして扱う。</para>
        ///     <para>正規表現・ワイルドカードでない場合は部分一致としてあつかう。</para>
        ///     <para>最初に見つかったアルファベットが大文字の場合に大文字小文字を区別する。</para>
        /// </param>
        /// <returns></returns>
        public Regex CreateFilterRegex(string pattern)
        {
            if(string.IsNullOrWhiteSpace(pattern)) {
                return AllMatchRegex;
            }

            // / から始まるのは正規表現扱い
            if(1 < pattern.Length && pattern[0] == '/') {
                var regOption = RegexOptions.Singleline;
                var patternBody = pattern.Substring(1);
                if(!IsCaseSensitivePattern(patternBody)) {
                    regOption |= RegexOptions.IgnoreCase;
                }
                try {
                    return new Regex(patternBody, regOption);
                } catch(Exception ex) {
                    // 正規表現が変な場合は全件一致させる
                    Logger.LogWarning(ex, "正規表現パターン異常: {0}", ex.Message);
                    return AllMatchRegex;
                }
            }

            // 文字列にワイルドカードっぽいのがあればワイルドカード判定
            if(pattern.IndexOfAny(new[] { '*', '?' }) != -1) {
                var wildcard = "^" + Regex.Escape(pattern).Replace("\\?", ".").Replace("\\*", ".*") + "$";
                var regOption = RegexOptions.Singleline;
                if(!IsCaseSensitivePattern(wildcard)) {
                    regOption |= RegexOptions.IgnoreCase;
                }

                return new Regex(wildcard, regOption);
            }

            // とりあえずの部分一致
            var option = IsCaseSensitivePattern(pattern)
                ? RegexOptions.None
                : RegexOptions.IgnoreCase
            ;
            return new Regex(Regex.Escape(pattern), option);
        }

        #endregion
    }
}
