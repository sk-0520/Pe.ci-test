namespace ContentTypeTextNet.Library.SharedLibrary.Logic.Utility
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	public static class CheckUtility
	{
		/// <summary>
		/// 真を強制させる。
		/// </summary>
		/// <typeparam name="TException">失敗時に投げられる例外。</typeparam>
		/// <param name="test">テスト。</param>
		/// <exception cref="TException">テスト失敗時に投げられる。</exception>
		public static void Enforce<TException>(bool test)
			where TException: Exception, new()
		{
			if(!test) {
				throw new TException();
			}
		}
		/// <summary>
		/// 真を強制させる。
		/// </summary>
		/// <param name="test">テスト。</param>
		/// <exception cref="Exception">テスト失敗時に投げられる。</exception>
		public static void Enforce(bool test)
		{
			Enforce<Exception>(test);
		}

		/// <summary>
		/// 非nullを強制。
		/// </summary>
		/// <typeparam name="TClass"></typeparam>
		/// <param name="obj"></param>
		/// <exception cref="ArgumentNullException">null。</exception>
		public static void EnforceNotNull<TClass>(TClass obj)
			where TClass: class
		{
			Enforce<ArgumentNullException>(obj != null);
		}

		/// <summary>
		/// 非nullを強制。
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="nullable"></param>
		public static void EnforceNotNull<T>(Nullable<T> nullable)
			where T: struct
		{
			Enforce<ArgumentNullException>(nullable.HasValue);
		}

		/// <summary>
		/// 文字列が非nullで長さ0でないことを強制。
		/// </summary>
		/// <param name="s"></param>
		public static void EnforceNotNullAndNotEmpty(string s)
		{
			Enforce<ArgumentException>(!string.IsNullOrEmpty(s));
		}

		/// <summary>
		/// 文字列が非nullでホワイトスペースのみでないことを強制。
		/// </summary>
		/// <param name="s"></param>
		public static void EnforceNotNullAndNotWhiteSpace(string s)
		{
			Enforce<ArgumentException>(!string.IsNullOrWhiteSpace(s));
		}
	}
}
