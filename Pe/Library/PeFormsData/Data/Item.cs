namespace ContentTypeTextNet.Pe.PeMain.Data
{
	using System;
	using System.Xml.Serialization;
	using ContentTypeTextNet.Pe.PeMain.IF;

	/// <summary>
	/// なんかを保持するアイテムの基底。
	/// </summary>
	[Serializable]
	public abstract class Item: IItem, ICorrectionItem
	{
		/// <summary>
		/// データ補正を行う。
		/// </summary>
		public virtual void CorrectionValue()
		{ }
	}

	/// <summary>
	/// 解放が必要なアイテムの基底。
	/// </summary>
	public abstract class DisposableItem: ICorrectionItem, IDisposable
	{
		protected DisposableItem()
		{
			IsDisposed = false;
		}

		/// <summary>
		/// データ補正を行う。
		/// </summary>
		public virtual void CorrectionValue()
		{ }

		/// <summary>
		/// 破棄されたか。
		/// </summary>
		[XmlIgnore]
		public bool IsDisposed { get; protected set; }

		protected virtual void Dispose(bool disposing)
		{
			IsDisposed = true;
		}

		/// <summary>
		/// 解放。
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
		}
	}

	/// <summary>
	/// 名前付きアイテム
	/// </summary>
	[Serializable]
	public abstract class NameItem: Item, INameItem
	{
		/// <summary>
		/// 名前
		/// </summary>
		[System.Xml.Serialization.XmlAttribute("Name")]
		public string Name { get; set; }
	}

	/// <summary>
	/// 解放が必要な名前月アイテム
	/// </summary>
	[Serializable]
	public abstract class DisposableNameItem: DisposableItem, INameItem
	{
		/// <summary>
		/// 名前
		/// </summary>
		[System.Xml.Serialization.XmlAttribute("Name")]
		public string Name { get; set; }
	}

	[Serializable]
	public class RangeItem<T>: IRangeItem<T>
	{
		public T Start { get; set; }
		public T End { get; set; }

		public void SetRange(T value)
		{
			Start = value;
			End = value;
		}
	}
}
