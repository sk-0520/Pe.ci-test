using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using ContentTypeTextNet.Pe.Bridge.Models.Data;

namespace ContentTypeTextNet.Pe.Bridge.Models
{
    /// <summary>
    /// スコア種別。
    /// </summary>
    public enum ScoreKind
    {
        /// <summary>
        /// 初期スコア。
        /// </summary>
        Initial,
        /// <summary>
        /// 最大スコア。
        /// </summary>
        Maximum,
        /// <summary>
        /// 最少スコア。
        /// </summary>
        Minimum,
        /// <summary>
        /// 完全一致。
        /// </summary>
        Perfect,
        /// <summary>
        /// 良い。
        /// </summary>
        Good,
        /// <summary>
        /// 悪い。
        /// </summary>
        Bad,

        GoodBonus,

    }

    /// <summary>
    /// <para>Pe から提供される。</para>
    /// </summary>
    public interface IHitValuesCreator
    {
        #region property

        double NoBonus { get; }

        #endregion

        #region function

        /// <summary>
        /// スコアの取得。
        /// </summary>
        /// <param name="scoreKind"></param>
        /// <returns></returns>
        int GetScore(ScoreKind scoreKind, double bonus);

        /// <summary>
        /// 検索条件に一致した正規表現結果を取得。
        /// </summary>
        /// <param name="input">入力値。</param>
        /// <param name="regex">Pe から提供される正規表現。</param>
        /// <returns></returns>
        IReadOnlyList<Match> GetMatches(string input, Regex regex);

        IReadOnlyList<Range> ConvertRanges(string input, IEnumerable<Match> matches);

        List<HitValue> ConvertHitValues(string input, string source, IReadOnlyList<Range> hitRanges);

        int CalcScore(string input, string source, IReadOnlyList<HitValue> hitValues);

        #endregion
    }
}
