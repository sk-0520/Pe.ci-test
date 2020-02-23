using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using ContentTypeTextNet.Pe.Bridge.Models.Data;

namespace ContentTypeTextNet.Pe.Bridge.Models
{
    public interface IHitValuesCreator
    {
        #region property

        int InitialScore { get; }
        int MaximumScore { get; }
        int MinimumScore { get; }
        int GoodScore { get; }
        int BadScore { get; }
        int SeparatorScore { get; }

        #endregion

        #region function

        /// <summary>
        /// 検索条件に一致した正規表現結果を取得。
        /// </summary>
        /// <param name="regex"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        IReadOnlyList<Match> GetMatches(Regex regex, string input);

        IReadOnlyList<Range> ConvertRanges(IEnumerable<Match> matches);

        List<HitValue> ConvertHitValues(string source, IReadOnlyList<Range> hitRanges);

        int CalcScore(string source, IReadOnlyList<HitValue> hitValues);

        #endregion
    }
}
