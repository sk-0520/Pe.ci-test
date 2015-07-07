namespace ContentTypeTextNet.Library.SharedLibrary.Model
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Runtime.Serialization;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows.Input;
using ContentTypeTextNet.Library.SharedLibrary.IF;

	[Serializable]
	public class HotkeyModel: ModelBase, IDeepClone
	{
		public HotkeyModel()
			: base()
		{ }

		[DataMember]
		public ModifierKeys ModifierKeys { get; set; }
		[DataMember]
		public Key Key { get; set; }

		#region IDeepClone

		public virtual IDeepClone DeepClone()
		{
			var result = new HotkeyModel() {
				Key = this.Key,
				ModifierKeys = this.ModifierKeys,
			};

			return result;
		}

		#endregion
	}
}
