namespace ContentTypeTextNet.Library.SharedLibrary.Logic
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.IF;

	/// <summary>
	/// 生成データを保持しておく。
	/// </summary>
	/// <typeparam name="Tkey"></typeparam>
	/// <typeparam name="TValue"></typeparam>
	public class Caching<Tkey, TValue>: Dictionary<Tkey, TValue>
	{
		#region function

		/// <summary>
		/// 指定キーからデータを取得する。
		/// <para>指定キーにデータがなければデータを生成してキャッシュに入れる。</para>
		/// </summary>
		/// <param name="key"></param>
		/// <param name="creator"></param>
		/// <returns></returns>
		public TValue Get(Tkey key, Func<TValue> creator)
		{
			TValue result;
			if(!TryGetValue(key, out result)) {
				result = creator();
				this[key] = result;
			}

			return result;
		}

		public bool ClearCache(Tkey key)
		{
			TValue result;
			if(!TryGetValue(key, out result)) {
				result = default(TValue);
				Remove(key);
				return true;
			} else {
				return false;
			}
		}

		public new void Clear()
		{
			foreach(var key in this.Keys) {
				ClearCache(key);
			}

			base.Clear();
		}

		#endregion
	}
}
