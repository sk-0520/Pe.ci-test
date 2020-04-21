using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        #endregion
    }
}
