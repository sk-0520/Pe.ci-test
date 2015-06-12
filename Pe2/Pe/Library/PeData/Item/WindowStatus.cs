namespace ContentTypeTextNet.Pe.Library.PeData.Item
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Runtime.Serialization;
	using System.Text;
	using System.Threading.Tasks;

	[Serializable]
	public class WindowStatus: ItemModelBase
	{
		public WindowStatus()
			: base()
		{ }

		/// <summary>
		/// 論理X座標。
		/// </summary>
		[DataMember]
		public double X { get; set; }
		/// <summary>
		/// 論理Y座標。
		/// </summary>
		[DataMember]
		public double Y { get; set; }
		/// <summary>
		/// 論理横幅。
		/// </summary>
		[DataMember]
		public double Width { get; set; }
		/// <summary>
		/// 論理高さ。
		/// </summary>
		[DataMember]
		public double Height { get; set; }

		[DataMember]
		public WindowStatus State { get; set; }
	}
}
