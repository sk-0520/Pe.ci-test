namespace ContentTypeTextNet.Library.SharedLibrary.Model
{
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Linq;
	using System.Reflection;
	using System.Runtime.Serialization;
	using System.Text;
	using System.Threading.Tasks;
	using System.Xml.Serialization;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;

	/// <summary>
	/// コレクションデータ保持用モデル。
	/// <para>ObservableCollectionの単純ラッパー。</para>
	/// </summary>
	/// <typeparam name="TValue"></typeparam>
	[Serializable]
	public class CollectionModel<TValue> : ObservableCollection<TValue>, IIsDisposed, IModel
	{
		#region variable

		[IgnoreDataMember, XmlIgnore]
		IEnumerable<PropertyInfo> _propertyInfos = null;

		#endregion

		public CollectionModel()
			: base()
		{
			IsDisposed = false;
		}

		public CollectionModel(IEnumerable<TValue> items)
			: base(items)
		{
			IsDisposed = false;
		}

		~CollectionModel()
		{
			Dispose(false);
		}

		#region IIsDisposed

		[IgnoreDataMember, XmlIgnore]
		public bool IsDisposed { get; private set; }

		protected virtual void Dispose(bool disposing)
		{
			if (IsDisposed) {
				return;
			}

			IsDisposed = true;
			GC.SuppressFinalize(this);
		}

		public void Dispose()
		{
			Dispose(true);
		}

		#endregion

		#region IModel

		[IgnoreDataMember, XmlIgnore]
		public virtual string DisplayText
		{
			get { return GetType().FullName; }
		}

		[IgnoreDataMember, XmlIgnore]
		public IEnumerable<PropertyInfo> PropertyInfos
		{
			get
			{
				if (this._propertyInfos == null) {
					this._propertyInfos = GetType().GetProperties();
				}

				return this._propertyInfos;
			}
		}

		public IEnumerable<string> GetNameValueList()
		{
			var nameValueMap = ReflectionUtility.GetMembers(this, PropertyInfos);
			return ReflectionUtility.GetNameValueStrings(nameValueMap);
		}

		public virtual void Correction()
		{ }

		#endregion

		#region function

		/// <summary>
		/// Addを内部的に繰り返すだけ。
		/// <para>速度的にどうとかじゃなくて毎度毎度foreachするのだりぃ。</para>
		/// </summary>
		/// <param name="items"></param>
		public void AddRange(IEnumerable<TValue> items)
		{
			foreach (var item in items) {
				Add(item);
			}
		}

		#endregion
	}
}
