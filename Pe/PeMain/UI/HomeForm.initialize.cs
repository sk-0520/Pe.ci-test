namespace PeMain.UI
{
	using System;
	using System.Drawing;
	using System.Windows.Forms;

	partial class HomeForm
	{
		void InitializeUI()
		{
			var primaryArea = Screen.PrimaryScreen.Bounds;
			Location = new Point(
				primaryArea.Width / 2 - Width / 2,
				primaryArea.Height / 2 - Height / 2
			);
		}
		
		void Initialize()
		{
			#if DEBUG
			this.commandLauncher.Enabled = true;
			#endif
			
			InitializeUI();
		}
	}
}
