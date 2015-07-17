namespace ContentTypeTextNet.Pe.Library.PeData.Item
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Runtime.Serialization;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;
	using System.Windows.Media;
	using System.Xml.Serialization;
	using ContentTypeTextNet.Library.SharedLibrary.Attribute;
	using ContentTypeTextNet.Library.SharedLibrary.Define;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.Model;
	using ContentTypeTextNet.Pe.Library.PeData.Define;
	using ContentTypeTextNet.Pe.Library.PeData.IF;

	/// <summary>
	/// 
	/// </summary>
	[Serializable]
	public class NoteIndexItemModel: IndexItemModelBase, IWindowArea, ITopMost, IVisible, IColorPair
	{
		public NoteIndexItemModel()
			: base()
		{
			NoteKind = NoteKind.Plain;
		}

		#region property

		[DataMember]
		public NoteKind NoteKind { get; set; }

		[DataMember]
		public bool IsLocked { get; set; }
		[DataMember]
		public bool IsCompacted { get; set; }

		#endregion

		#region IColorPair

		[DataMember]
		public Color ForeColor { get; set; }
		[DataMember]
		public Color BackColor { get; set; }

		#endregion

		#region IWindowArea

		[DataMember]
		[PixelKind(Px.Logical)]
		public double WindowTop { get; set; }
		[DataMember]
		[PixelKind(Px.Logical)]
		public double WindowLeft { get; set; }
		[DataMember]
		[PixelKind(Px.Logical)]
		public double WindowWidth { get; set; }
		[DataMember]
		[PixelKind(Px.Logical)]
		public double WindowHeight { get; set; }

		#endregion

		#region ITopMost

		[DataMember]
		public bool TopMost { get; set; }

		#endregion

		#region IVisible

		[DataMember]
		public bool Visible { get; set; }

		#endregion
	}
}
