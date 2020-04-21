using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Ink;

namespace ContentTypeTextNet.Pe.Core.Models
{
    public class NumericRange
    {
        public NumericRange()
            : this(true, ",", "-")
        { }
        public NumericRange(bool separatorSpace, string valueSeparator, string rangeSeparator)
        {
            if(valueSeparator == rangeSeparator) {
                throw new ArgumentException($"{nameof(valueSeparator)} == {nameof(rangeSeparator)}");
            }
            SeparatorSpace = separatorSpace;
            ValueSeparator = valueSeparator ?? throw new ArgumentNullException(nameof(valueSeparator));
            RangeSeparator = rangeSeparator ?? throw new ArgumentNullException(nameof(rangeSeparator));
        }

        #region property

        public bool SeparatorSpace { get; }
        public string ValueSeparator { get; }
        public string RangeSeparator { get; }

        #endregion

        #region function

        public string ToString(IEnumerable<int>? values)
        {
            if(values == null) {
                return string.Empty;
            }

            var orderdValues = values
                .OrderBy(i => i)
                .ToList()
            ;

            if(orderdValues.Count == 0) {
                return string.Empty;
            }

            var builder = new StringBuilder();
            var prevValue = orderdValues.First();
            builder.Append(prevValue);
            var nowRange = false;
            foreach(var value in orderdValues.Skip(1)) {
                if(prevValue == value) {
                    continue;
                }

                if(prevValue + 1 == value) {
                    if(!nowRange) {
                        builder.Append(RangeSeparator);
                        nowRange = true;
                    }
                } else {
                    if(nowRange) {
                        builder.Append(prevValue);
                        nowRange = false;
                    }
                    builder.Append(ValueSeparator);
                    if(SeparatorSpace) {
                        builder.Append(' ');
                    }
                    builder.Append(value);
                }

                prevValue = value;
            }
            if(nowRange) {
                builder.Append(prevValue);
            }

            return builder.ToString();
        }

        bool ParseCore(string? s, out List<int> result, out Exception? ex)
        {
            ex = null;
            if(string.IsNullOrWhiteSpace(s)) {
                result = new List<int>();
                return true;
            }

            var values = s.Split(ValueSeparator);
            if(values.Length == 0) {
                result = new List<int>();
                return true;
            }

            var reg = new Regex(@"\s*(?<HEAD>[+\-]?\d+)\s*((?<RANGE>.+?)(?<TAIL>[+\-]?\d+))?\s*");

            result = new List<int>();

            foreach(var value in values) {
                var match = reg.Match(value);
                if(match.Success) {
                    var head = int.Parse(match.Groups["HEAD"].Value);
                    var range = match.Groups["RANGE"];
                    if(range.Success) {
                        if(range.Value == RangeSeparator) {
                            var tail = int.Parse(match.Groups["TAIL"].Value);
                            if(head < tail) {
                                for(var i = head; i <= tail; i++) {
                                    result.Add(i);
                                }
                            } else {
                                ex = new Exception($"error: {head} < {tail}");
                                return false;
                            }
                        } else {
                            ex = new Exception("range separator");
                            return false;
                        }
                    } else {
                        result.Add(head);
                    }
                } else {
                    ex = new Exception("unmatch");
                    return false;
                }
            }

            result = result
                .OrderBy(i => i)
                .Distinct()
                .ToList()
            ;

            return true;
        }

        public List<int> Parse(string? s)
        {
            if(ParseCore(s, out var result, out var ex)) {
                Debug.Assert(ex == null);
                return result;
            } else {
                Debug.Assert(ex != null);
                throw ex;
            }
        }

        public bool TryParse(string? s, List<int> result)
        {
            return ParseCore(s, out result, out _);
        }


        #endregion
    }
}
