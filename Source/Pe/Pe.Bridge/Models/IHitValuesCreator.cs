using System;
using System.Collections.Generic;
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
        /// <summary>
        /// 良い感じの一つ上げ。
        /// </summary>
        GoodStep,
        /// <summary>
        /// 悪い感じの一つ上げ。
        /// </summary>
        BadStep,
    }

    /// <summary>
    /// </summary>
    /// <remarks>
    /// <para>Pe から提供される。</para>
    /// </remarks>
    public interface IHitValuesCreator
    {
        #region property

        /// <summary>
        /// ボーナス無効。
        /// </summary>
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
        /// <param name="source">対象値。</param>
        /// <param name="regex">Pe から提供される正規表現。</param>
        /// <returns></returns>
        IReadOnlyList<Match> GetMatches(string source, Regex regex);

        /// <summary>
        /// 検索結果から元データに対する一致箇所の<see cref="Range"/>一覧を取得。
        /// </summary>
        /// <param name="matches"><see cref="GetMatches"/>の対象値に対する検索結果。</param>
        /// <returns>><see cref="GetMatches"/>の対象値に対する該当箇所一覧。</returns>
        IReadOnlyList<Range> ConvertRanges(IEnumerable<Match> matches);

        /// <summary>
        ///検索一致箇所から検索該当・非該当で構成されたデータを取得。
        /// </summary>
        /// <param name="source"><see cref="GetMatches"/>/<see cref="ConvertRanges"/>で使用した対象値。</param>
        /// <param name="hitRanges"><see cref="ConvertRanges"/>で取得した該当箇所一覧。</param>
        /// <returns>該当・非該当の一覧。</returns>
        IReadOnlyList<HitValue> ConvertHitValues(ReadOnlySpan<char> source, IReadOnlyList<Range> hitRanges);

        /// <summary>
        /// スコア算出。
        /// </summary>
        /// <param name="source"><see cref="GetMatches"/>の対象値。</param>
        /// <param name="hitValues">該当・非該当の一覧。</param>
        /// <returns></returns>
        int CalcScore(ReadOnlySpan<char> source, IReadOnlyList<HitValue> hitValues);

        #endregion
    }

    public static class IHitValuesCreatorExtensions
    {
        #region function

        /// <summary>
        /// 対象値から該当・非該当の一覧を取得。
        /// </summary>
        /// <param name="hitValuesCreator"></param>
        /// <param name="source"></param>
        /// <param name="regex"></param>
        /// <returns></returns>
        public static IReadOnlyList<HitValue> GetHitValues(this IHitValuesCreator hitValuesCreator, string source, Regex regex)
        {
            var matches = hitValuesCreator.GetMatches(source, regex);
            if(matches.Count == 0) {
                return Array.Empty<HitValue>();
            }
            var sourceSpan = source.AsSpan();
            var ranges = hitValuesCreator.ConvertRanges(matches);
            var hitValue = hitValuesCreator.ConvertHitValues(sourceSpan, ranges);
            return hitValue;
        }

        #endregion
    }
}
