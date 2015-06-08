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

	[DataContract, Serializable]
	public class LoggingItemModel : ItemModelBase
	{
		public LoggingItemModel()
			: base()
		{ }

		public bool Visible { get; set; }
		public Point Location { get; set; }
		public Size Size { get; set; }
		public bool AddShow { get; set; }
		public bool ShowTriggerDebug { get; set; }
		public bool ShowTriggerTrace { get; set; }
		public bool ShowTriggerInformation { get; set; }
		public bool ShowTriggerWarning { get; set; }
		public bool ShowTriggerError { get; set; }
		public bool ShowTriggerFatal { get; set; }
	}
}
