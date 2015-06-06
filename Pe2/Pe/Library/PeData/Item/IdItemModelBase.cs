namespace ContentTypeTextNet.Pe.Library.PeData.Item
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Runtime.Serialization;
	using System.Text;
	using System.Threading.Tasks;
	using System.Xml.Serialization;

	/// <summary>
	/// IDを保持する基底アイテム。
	/// <para>IDをどう扱うかは統括側に委任するが一意であることが前提。</para>
	/// </summary>
	[DataContract, Serializable]
	public abstract class TIdItemModelBase<T>: ItemModelBase
	{
		/// <summary>
		/// ID。
		/// </summary>
		[DataMember, XmlAttribute]
		public T Id { get; set; }
	}

	/// <summary>
	/// 数値IDを保持する基底アイテム。
	/// <para>IDをどう扱うかは統括側に委任するが一意であることが前提。</para>
	/// </summary>
	[DataContract, Serializable]
	public abstract class NumberIdItemModelBase: TIdItemModelBase<int>
	{
		public NumberIdItemModelBase()
			: base()
		{}
	}

	/// <summary>
	/// 文字列IDを保持する基底アイテム。
	/// <para>IDをどう扱うかは統括側に委任するが一意であることが前提。</para>
	/// </summary>
	[DataContract, Serializable]
	public abstract class StringIdItemModelBase: TIdItemModelBase<string>
	{
		public StringIdItemModelBase()
			: base()
		{}
	}
}
