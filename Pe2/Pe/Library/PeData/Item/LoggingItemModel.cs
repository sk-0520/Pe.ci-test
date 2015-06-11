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

	[Serializable]
	public class LoggingItemModel: ItemModelBase
	{
		public LoggingItemModel()
			: base()
		{ }

		[DataMember]
		public bool Visible { get; set; }
		[DataMember]
		public double Left { get; set; }
		[DataMember]
		public double Top { get; set; }
		[DataMember]
		public double Width { get; set; }
		[DataMember]
		public double Height { get; set; }
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
