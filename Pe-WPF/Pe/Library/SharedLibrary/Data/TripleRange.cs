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

	public struct TripleRange<T>
		where T: IComparable
	{
		public readonly T minimum, median, maximum;

		public TripleRange(T min, T median, T max)
		{
			this.minimum = min;
			this.median = median;
			this.maximum = max;
		}

		public T GetRounding(T value)
		{
			return RangeUtility.Rounding(value, this.minimum, this.maximum);
		}

	}
}
