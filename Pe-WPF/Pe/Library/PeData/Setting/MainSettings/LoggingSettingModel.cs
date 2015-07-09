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
	using ContentTypeTextNet.Pe.Library.PeData.IF;
	using ContentTypeTextNet.Pe.Library.PeData.Item;

	[Serializable]
	public class LoggingSettingModel : SettingModelBase, IWindowStatus
	{
		public LoggingSettingModel()
			: base()
		{ }

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

		#region property

		/// <summary>
		/// ログ追加時にUIを表示るか。
		/// </summary>
		[DataMember]
		public bool AddShow { get; set; }
		/// <summary>
		/// デバッグ時に表示する
		/// </summary>
		[DataMember]
		public bool ShowTriggerDebug { get; set; }
		/// <summary>
		/// トレース時に表示する
		/// </summary>
		[DataMember]
		public bool ShowTriggerTrace { get; set; }
		/// <summary>
		/// 情報時に表示する
		/// </summary>
		[DataMember]
		public bool ShowTriggerInformation { get; set; }
		/// <summary>
		/// 警告時に表示する
		/// </summary>
		[DataMember]
		public bool ShowTriggerWarning { get; set; }
		/// <summary>
		/// エラー時に表示する
		/// </summary>
		[DataMember]
		public bool ShowTriggerError { get; set; }
		/// <summary>
		/// 致命時に表示する
		/// </summary>
		[DataMember]
		public bool ShowTriggerFatal { get; set; }

		#endregion
	}
}
