namespace ContentTypeTextNet.Library.SharedLibrary.Model
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Runtime.Serialization;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows.Input;

	[Serializable]
	public class HotkeyModel: ModelBase
	{
		public HotkeyModel()
			: base()
		{ }

		[DataMember]
		public ModifierKeys ModifierKeys { get; set; }
		[DataMember]
		public Key Key { get; set; }
	}
}
