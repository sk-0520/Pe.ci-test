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
		where T: IComparable
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
			where T: IComparable
		{
			return new TripleRange<T>(minimum, median, maximum);
		}
	}
}
