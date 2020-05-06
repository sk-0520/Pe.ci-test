using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Command
{
    internal class HitValuesCreator : IHitValuesCreator
    {
        public HitValuesCreator(ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger(GetType());
        }

        #region property

        ILogger Logger { get; }

        #endregion

        #region IHitValuesCreator

        public double NoBonus => 1;

        public int GetScore(ScoreKind scoreKind, double bonus)
        {
            return scoreKind switch
            {
                ScoreKind.Initial => 0,
                ScoreKind.Maximum => 1000,
                ScoreKind.Minimum => -1000,
                ScoreKind.Perfect => 800,
                ScoreKind.Good => (int)Math.Round(10 * bonus),
                ScoreKind.Bad => (int)Math.Round(-10 * bonus),
                _ => throw new NotImplementedException(),
            };
        }


        public IReadOnlyList<Match> GetMatches(string input, Regex regex) => regex.Matches(input).Cast<Match>().ToList();

        public IReadOnlyList<Range> ConvertRanges(string input, IEnumerable<Match> matches) => matches.Select(i => new Range(i.Index, i.Index + i.Length)).ToList();

        public List<HitValue> ConvertHitValues(string input, string source, IReadOnlyList<Range> hitRanges)
        {
            if(hitRanges.Count == 0) {
                return new List<HitValue>() {
                    new HitValue(source, false),
                };
            }

            var result = new List<HitValue>();

            var workMatches = hitRanges.ToDictionary(i => i.Start.Value, i => i.End.Value - i.Start.Value);
            var i = 0;
            while(true) {
                if(workMatches.TryGetValue(i, out var hitLength)) {
                    var value = source.Substring(i, hitLength);
                    var item = new HitValue(value, true);
                    result.Add(item);
                    workMatches.Remove(i);
                    i += hitLength;
                    if(workMatches.Count == 0) {
                        if(i < source.Length) {
                            result.Add(new HitValue(source.Substring(i), false));
                        }
                        break;
                    }
                } else {
                    var minIndex = workMatches.Keys.Min();
                    var value = source.Substring(i, minIndex - i);
                    var item = new HitValue(value, false);
                    result.Add(item);
                    i = minIndex;
                }
            }

            return result;
        }

        public int CalcScore(string input, string source, IReadOnlyList<HitValue> hitValues)
        {
            Logger.LogDebug(">> {0}, {1}", source, string.Join(",", hitValues.Select(i => $"{(i.IsHit ? 'O' : 'X')}:{i.Value}")));
            if(hitValues.Count == 1 && hitValues.All(i => i.IsHit)) {
                // 完全一致
                Logger.LogInformation(source);
                return GetScore(ScoreKind.Perfect, NoBonus);
            }
            var scrore = GetScore(ScoreKind.Initial, NoBonus);
            var first = hitValues.First();
            if(first.IsHit) {
                if(source.StartsWith(first.Value)) {
                    scrore += GetScore(ScoreKind.Good, 10);
                }
            }

            var nextItems = hitValues.Skip(1).Counting().ToList();
            foreach(var item in nextItems) {
                var hitValue = item.Value;
                if(hitValue.IsHit) {
                    scrore += GetScore(ScoreKind.Good, (nextItems.Count - item.Number) * 0.5);
                } else {
                    scrore += GetScore(ScoreKind.Bad, item.Number / 10.0);
                }
            }


            return scrore;
        }



        #endregion
    }
}
