namespace ContentTypeTextNet.Pe.PeMain.UI.Ex
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows.Forms;
	using ContentTypeTextNet.Pe.Library.Skin;

	public abstract class ExRadioButton: RadioButton
	{ }

	public class ToolbarPositionRadioButton: ExRadioButton
	{
		#region property

		public ToolbarPosition ToolbarPosition { get; set; }

		#endregion
	}

}
