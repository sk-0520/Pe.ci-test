namespace ContentTypeTextNet.Pe.PeMain.View.Parts.Control
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows.Controls;

	public class AppDataGrid: DataGrid
	{
		#region event

		public event EventHandler Rendered = delegate { };

		#endregion

		

		protected override void OnRender(System.Windows.Media.DrawingContext drawingContext)
		{
			base.OnRender(drawingContext);

			Debug.WriteLine("-.-");
			Rendered(this, EventArgs.Empty);
		}
	}
}
