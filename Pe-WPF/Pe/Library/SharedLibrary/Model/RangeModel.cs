using ContentTypeTextNet.Library.SharedLibrary.Model;
namespace ContentTypeTextNet.Library.SharedLibrary.IF
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Runtime.Serialization;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.Model;

	/// <summary>
	/// 範囲持ちアイテム。
	/// </summary>
	/// <typeparam name="T"></typeparam>
	[Serializable]
	public class RangeModel<T>: ModelBase
	{
		public RangeModel()
		{ }

		#region propert

		/// <summary>
		/// 範囲の開始点。
		/// </summary>
		[DataMember]
		public T Head { get; set; }
		/// <summary>
		/// 範囲の終了点。
		/// </summary>
		[DataMember]
		public T Tail { get; set; }

		#endregion
	}
}
