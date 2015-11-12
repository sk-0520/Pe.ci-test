/**
This file is part of SharedLibrary.

SharedLibrary is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

SharedLibrary is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with SharedLibrary.  If not, see <http://www.gnu.org/licenses/>.
*/
namespace ContentTypeTextNet.Library.SharedLibrary.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.Serialization;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Serialization;
    using ContentTypeTextNet.Library.SharedLibrary.IF;
    using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;

    /// <summary>
    /// 最小値・中間値・最大値を保持するデータ。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public struct TripleRange<T>
        where T : IComparable
    {
        #region variable

        public readonly T minimum, median, maximum;

        #endregion

        public TripleRange(T minimum, T median, T maximum)
        {
            this.minimum = minimum;
            this.median = median;
            this.maximum = maximum;
        }

        #region function

        public T GetClamp(T value)
        {
            return RangeUtility.Clamp(value, this.minimum, this.maximum);
        }

        #endregion
    }

    /// <summary>
    /// ラッパー。
    /// </summary>
    public static class TripleRange
    {
        public static TripleRange<T> Create<T>(T minimum, T median, T maximum)
            where T : IComparable
        {
            return new TripleRange<T>(minimum, median, maximum);
        }
    }
}
