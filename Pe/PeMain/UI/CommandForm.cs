using System;
using System.Collections.Generic;
using System.ComponentModel;
namespace ContentTypeTextNet.Pe.PeMain.UI
{
	using System.Data;
	using System.Drawing;
	using System.Text;
	using System.Windows.Forms;
	using ContentTypeTextNet.Pe.Library.Utility;
	using ContentTypeTextNet.Pe.PeMain.UI.Ex;

	public partial class CommandForm: CommonForm
	{
		public CommandForm()
		{
			InitializeComponent();
		}

		#region functino

		public void SetCurrentLocation()
		{
			Location = Cursor.Position;
		}

		#endregion
	}
}
