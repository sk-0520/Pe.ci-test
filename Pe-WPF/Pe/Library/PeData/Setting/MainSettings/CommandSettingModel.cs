namespace ContentTypeTextNet.Pe.Library.PeData.Setting.MainSettings
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Runtime.Serialization;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;
	using ContentTypeTextNet.Library.SharedLibrary.Attribute;
	using ContentTypeTextNet.Library.SharedLibrary.Define;
	using ContentTypeTextNet.Library.SharedLibrary.Model;
	using ContentTypeTextNet.Pe.Library.PeData.IF;
	using ContentTypeTextNet.Pe.Library.PeData.Item;

	[Serializable]
	public class CommandSettingModel : SettingModelBase, IWindowStatus
	{
		public CommandSettingModel()
			: base()
		{
			ShowHotkey = new HotkeyModel();
		}

		#region property

		/// <summary>
		/// アイコンサイズ。
		/// </summary>
		[DataMember]
		public IconScale IconScale { get; set; }
		/// <summary>
		/// 非表示になるまでの時間。
		/// </summary>
		[DataMember]
		public TimeSpan HideTime { get; set; }

		[DataMember]
		public HotkeyModel ShowHotkey { get; set; }

		#endregion

		#region IWindowStatus

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
		[DataMember]
		[PixelKind(Px.Logical)]
		public WindowState WindowState { get; set; }

		#region ITopMost

		[DataMember]
		public bool TopMost { get; set; }

		#endregion

		#region IVisible

		[DataMember]
		public bool Visible { get; set; }

		#endregion

		#endregion

	}
}
