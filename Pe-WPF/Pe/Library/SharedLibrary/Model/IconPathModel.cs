namespace ContentTypeTextNet.Library.SharedLibrary.Model
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Runtime.Serialization;
	using System.Text;
	using System.Threading.Tasks;

	/// <summary>
	/// アイコンのパスを保持。
	/// </summary>
	[Serializable]
	public class IconPathModel: ModelBase
	{
		#region property

		/// <summary>
		/// パス。
		/// </summary>
		[DataMember]
		public string Path { get; set; }
		/// <summary>
		/// アイコンインデックス。
		/// </summary>
		[DataMember]
		public int Index { get; set; }

		#endregion

		#region ModelBase

		public override string DisplayText
		{
			get
			{
				if(string.IsNullOrWhiteSpace(Path)) {
					if(Index > 0) {
						return string.Format(":Index = {0}", Index);
					} else {
						return string.Empty;
					}
				} else {
					return string.Format("{0},{1}", Path, Index);
				}
			}
		}

		#endregion
	}
}
