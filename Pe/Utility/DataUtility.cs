using System;

namespace ContentTypeTextNet.Pe.Library.Utility
{
	/// <summary>
	/// ペア。
	/// 
	/// 捨てたい……。
	/// </summary>
	/// <typeparam name="TFIRST"></typeparam>
	/// <typeparam name="TSECOND"></typeparam>
	[Serializable]
	public struct TPair<TFIRST, TSECOND>
	{
		public static TPair<TFIRST, TSECOND> Create(TFIRST first, TSECOND second)
		{
			var pair = new TPair<TFIRST, TSECOND>();

			pair.First = first;
			pair.Second = second;

			return pair;
		}

		public TFIRST First { get; set; }
		public TSECOND Second { get; set; }
	}
	/// <summary>
	/// 評価
	/// </summary>
	public class Evaluation
	{
		public bool Eval { get; set; }
		public string Info { get; set; }
		public object Tag { get; set; }
	}
	/// <summary>
	/// 評価とその結果
	/// </summary>
	public class EvaluationResult<T>: Evaluation
	{
		public T Result { get; set; }
	}
	
	/// <summary>
	/// 最小値・中間値・最大値を保持する。
	/// </summary>
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
		
		public T ToRounding(T value)
		{
			return value.Rounding(this.minimum, this.maximum);
		}
	}

	/// <summary>
	/// メッセージと例外を持つ。
	/// 
	/// ここで言うメッセージはException.Messageを指すものではなく独自のメッセージという意味。
	/// 
	/// 完全にObjectDumper対策。
	/// </summary>
	public class ExceptionMessage
	{
		public ExceptionMessage(string message, Exception exception)
		{
			Message = message;
			Exception = exception;
		}
		public string Message { get; private set; }
		public Exception Exception { get; private set; }
	}
}
