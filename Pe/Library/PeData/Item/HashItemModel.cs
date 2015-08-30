namespace ContentTypeTextNet.Pe.Library.PeData.Item
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Runtime.Serialization;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Pe.Library.PeData.Define;

	[Serializable]
	public class HashItemModel : ItemModelBase, IDeepClone, IIsEqual
	{
		public HashItemModel()
			: base()
		{ }

		#region property

		/// <summary>
		/// ハッシュ関数。
		/// <para>SHA-1 一択だけど将来変更する場合の保険。</para>
		/// </summary>
		[DataMember]
		public HashType Type { get; set; }
		/// <summary>
		/// ハッシュ値。
		/// </summary>
		[DataMember]
		public byte[] Code { get; set; }

		#endregion

		#region IDeepClone

		public void DeepCloneTo(IDeepClone target)
		{
			var obj = (HashItemModel)target;

			obj.Type = Type;
			if(Code != null && Code.Any()) {
				obj.Code = new byte[Code.Length];
				Code.CopyTo(obj.Code, 0);
			}
		}

		public IDeepClone DeepClone()
		{
			var result = new HashItemModel();

			DeepCloneTo(result);

			return result;
		}

		#endregion

		#region IIsEqual

		public bool IsEqual(IIsEqual target)
		{
			var obj = target as HashItemModel;
			if (obj == null) {
				return false;
			}

			if (Type != obj.Type) {
				return false;
			}
			if (Code == obj.Code) {
				return true;
			}

			if (Code == null || obj.Code == null) {
				return false;
			}

			if (Code.Length != obj.Code.Length) {
				return false;
			}

			return Code.SequenceEqual(obj.Code);
		}

		#endregion
	}
}
