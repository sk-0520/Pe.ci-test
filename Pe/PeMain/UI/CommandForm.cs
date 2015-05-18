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

		#region override

		#endregion

		#region functino

		public void SetCurrentLocation()
		{
			Location = Cursor.Position;
		}

		void ExecuteCommand()
		{}

		#endregion

		private void CommandForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			if(e.CloseReason == CloseReason.UserClosing) {
				e.Cancel = true;
				Visible = false;
			}
		}

		private void commandExecute_Click(object sender, EventArgs e)
		{
			ExecuteCommand();
		}
	}
}
