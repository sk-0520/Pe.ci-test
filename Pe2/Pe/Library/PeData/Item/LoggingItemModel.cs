namespace ContentTypeTextNet.Pe.Library.PeData.Item
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Runtime.Serialization;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;
	using ContentTypeTextNet.Library.SharedLibrary.Define;
	using ContentTypeTextNet.Pe.Library.PeData.IF;

	[Serializable]
	public class LoggingItemModel : ItemModelBase, IWindowStatus, IVisible
	{
		public LoggingItemModel()
			: base()
		{ }

		#region IWindowStatus

		[DataMember]
		public double WindowTop { get; set; }
		[DataMember]
		public double WindowLeft { get; set; }
		[DataMember]
		public double WindowWidth { get; set; }
		[DataMember]
		public double WindowHeight { get; set; }
		[DataMember]
		public WindowState WindowState { get; set; }

		#endregion

		#region IVisible

		[DataMember]
		public bool Visible { get; set; }

		#endregion

		[DataMember]
		public bool AddShow { get; set; }
		[DataMember]
		public bool ShowTriggerDebug { get; set; }
		[DataMember]
		public bool ShowTriggerTrace { get; set; }
		[DataMember]
		public bool ShowTriggerInformation { get; set; }
		[DataMember]
		public bool ShowTriggerWarning { get; set; }
		[DataMember]
		public bool ShowTriggerError { get; set; }
		[DataMember]
		public bool ShowTriggerFatal { get; set; }
	}
}
