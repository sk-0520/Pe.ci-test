using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models;
using Microsoft.Extensions.Logging;
using ContentTypeTextNet.Pe.Library.Base.Linq;

namespace ContentTypeTextNet.Pe.Main.Models.Command
{
    internal class HitValuesCreator: IHitValuesCreator
    {
        public HitValuesCreator(ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger(GetType());
        }

        #region property

        private ILogger Logger { get; }

        #endregion

        #region IHitValuesCreator

        /// <inheritdoc cref="IHitValuesCreator.NoBonus"/>
        public double NoBonus => 1;

        /// <inheritdoc cref="IHitValuesCreator.GetScore(ScoreKind, double)"/>
        public int GetScore(ScoreKind scoreKind, double bonus)
        {
            return scoreKind switch {
                ScoreKind.Initial => 0,
                ScoreKind.Maximum => 1000,
                ScoreKind.Minimum => -1000,
                ScoreKind.Perfect => 800,
                ScoreKind.Good => (int)Math.Round(10 * bonus),
                ScoreKind.Bad => (int)Math.Round(-10 * bonus),
                ScoreKind.GoodStep => 15,
                ScoreKind.BadStep => -15,
                _ => throw new NotImplementedException(),
            };
        }

        /// <inheritdoc cref="IHitValuesCreator.GetMatches(string, Regex)"/>
        public IReadOnlyList<Match> GetMatches(string source, Regex regex) => regex.Matches(source).Cast<Match>().ToList();

        /// <inheritdoc cref="IHitValuesCreator.ConvertRanges(IEnumerable{Match})"/>
        public IReadOnlyList<Range> ConvertRanges(IEnumerable<Match> matches) => matches.Select(i => new Range(i.Index, i.Index + i.Length)).ToList();

        /// <inheritdoc cref="IHitValuesCreator.ConvertHitValues(ReadOnlySpan{char}, IReadOnlyList{Range})"/>
        public IReadOnlyList<HitValue> ConvertHitValues(ReadOnlySpan<char> source, IReadOnlyList<Range> hitRanges)
        {
            if(hitRanges.Count == 0) {
                return new List<HitValue>() {
                    new HitValue(source.ToString(), false),
                };
            }

            var result = new List<HitValue>();

            var workMatches = hitRanges
                .Select(i => (value: i.Start.Value, length: i.End.Value - i.Start.Value))
                .Where(i => 0 < i.length)
                .ToDictionary(i => i.value, i => i.length)
            ;

            var i = 0;
            while(true) {
                if(workMatches.TryGetValue(i, out var hitLength)) {
                    var value = source.Slice(i, hitLength);
                    var item = new HitValue(value.ToString(), true);
                    result.Add(item);
                    workMatches.Remove(i);
                    i += hitLength;
                    if(workMatches.Count == 0) {
                        if(i < source.Length) {
                            result.Add(new HitValue(source.Slice(i).ToString(), false));
                        }
                        break;
                    }
                } else {
                    if(workMatches.Count != 0) {
                        var minIndex = workMatches.Keys.Min();
                        var value = source[i..minIndex];
                        var item = new HitValue(value.ToString(), false);
                        result.Add(item);
                        i = minIndex;
                    }
                }
            }

            return result;
        }

        /// <inheritdoc cref="IHitValuesCreator.CalcScore(ReadOnlySpan{char}, ReadOnlySpan{char}, IReadOnlyList{HitValue})"/>
        public int CalcScore(ReadOnlySpan<char> source, IReadOnlyList<HitValue> hitValues)
        {
            if(hitValues.Count == 0) {
                return GetScore(ScoreKind.Initial, NoBonus);
            }

            if(hitValues.Count == 1 && hitValues.All(i => i.IsHit)) {
                // 完全一致
                return GetScore(ScoreKind.Perfect, NoBonus);
            }
            var score = GetScore(ScoreKind.Initial, NoBonus);
            var first = hitValues.First();
            if(first.IsHit) {
                if(source.StartsWith(first.Value)) {
                    score += GetScore(ScoreKind.Good, 10);
                }
            }

            var nextItems = hitValues.Skip(1).Counting().ToList();
            foreach(var item in nextItems) {
                var hitValue = item.Value;
                if(hitValue.IsHit) {
                    score += GetScore(ScoreKind.Good, (nextItems.Count - item.Number) * 0.5);
                } else {
                    score += GetScore(ScoreKind.Bad, item.Number / 10.0);
                }
            }


            return score;
        }



        #endregion
    }
}
