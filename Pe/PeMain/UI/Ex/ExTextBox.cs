using System;
using System.Windows.Forms;

namespace ContentTypeTextNet.Pe.PeMain.UI
{
	using System;

	public abstract class ExTextBox: TextBox
	{ }
	
	public class IconTextBox: ExTextBox
	{
		private int _iconIndex;
		
		public int IconIndex
		{
			get
			{
				return this._iconIndex;
			}
			set
			{
				var changed = this._iconIndex != value;
				this._iconIndex = value;
				if(changed) {
					OnIconIndexChanged(EventArgs.Empty);
				}
			}
		}

		public event EventHandler IconIndexChanged;

		protected virtual void OnIconIndexChanged(EventArgs e)
		{
			var handler = IconIndexChanged;
			if (handler != null) {
				handler(this, e);
			}
		}
	}
}
