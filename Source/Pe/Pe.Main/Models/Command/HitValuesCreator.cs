using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
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

        public int InitialScore { get; } = 0;
        public int MaximumScore { get; } = 100;
        public int MinimumScore { get; } = -100;
        public int GoodScore { get; } = 10;
        public int BadScore { get; } = -5;
        public int SeparatorScore { get; } = 5;


        public IReadOnlyList<Match> GetMatches(Regex regex, string input) => regex.Matches(input).Cast<Match>().ToList();

        public IReadOnlyList<Range> ConvertRanges(IEnumerable<Match> matches) => matches.Select(i => new Range(i.Index, i.Index + i.Length)).ToList();

        public List<HitValue> ConvertHitValues(string source, IReadOnlyList<Range> hitRanges)
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
                    i = i + hitLength;
                    if(workMatches.Count == 0) {
                        if(i <= source.Length) {
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

        public int CalcScore(string source, IReadOnlyList<HitValue> hitValues)
        {
            return 0;
        }



        #endregion
    }
}
