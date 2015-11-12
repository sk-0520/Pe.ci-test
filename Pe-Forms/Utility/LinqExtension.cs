namespace ContentTypeTextNet.Pe.Library.Utility
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	public static class LinqExtension
	{
		public static IEnumerable<TResult> IfElse<T, TResult>(this IEnumerable<T> seq, bool cond, Func<IEnumerable<T>, IEnumerable<TResult>> t, Func<IEnumerable<T>, IEnumerable<TResult>> f)
		{
			if(cond) {
				return t(seq);
			} else {
				return f(seq);
			}
		}

		public static IEnumerable<T> IfRevese<T>(this IEnumerable<T> seq, bool cond)
		{
			return IfElse(seq, cond, s => s.Reverse(), s => s);
		}

	}
}
