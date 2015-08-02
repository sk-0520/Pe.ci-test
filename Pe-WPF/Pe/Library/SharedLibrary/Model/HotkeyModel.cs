namespace ContentTypeTextNet.Library.SharedLibrary.Model
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Runtime.Serialization;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows.Input;
	using System.Xml.Serialization;
	using ContentTypeTextNet.Library.SharedLibrary.IF;

	[Serializable]
	public class HotKeyModel: ModelBase, IDeepClone
	{
		public HotKeyModel()
			: base()
		{
			Key = Key.None;
			ModifierKeys = ModifierKeys.None;

			IsRegistered = false;
		}

		#region property

		[DataMember]
		public ModifierKeys ModifierKeys { get; set; }
		[DataMember]
		public Key Key { get; set; }

		/// <summary>
		/// 登録されているか
		/// </summary>
		[XmlIgnore, IgnoreDataMember]
		public bool IsRegistered { get; set; }

		/// <summary>
		/// 有効なキー設定か。
		/// </summary>
		[XmlIgnore, IgnoreDataMember]
		public bool Enabled
		{
			get
			{
				return Key != Key.None && ModifierKeys != ModifierKeys.None;
			}
		}
		#endregion

		#region IDeepClone

		public virtual void DeepCloneTo(IDeepClone target)
		{
			var obj = (HotKeyModel)target;

			obj.Key = Key;
			obj.ModifierKeys = ModifierKeys;
			obj.IsRegistered = IsRegistered;
		}

		public IDeepClone DeepClone()
		{
			var result = new HotKeyModel();

			DeepCloneTo(result);

			return result;
		}

		#endregion
	}
}
