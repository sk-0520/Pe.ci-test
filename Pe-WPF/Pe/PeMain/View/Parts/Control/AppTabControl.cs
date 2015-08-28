namespace ContentTypeTextNet.Pe.PeMain.View.Parts.Control
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows.Controls;

	public class AppTabControl: TabControl
	{
		#region event

		public event EventHandler Rendered = delegate { };

		#endregion

		protected override void OnRender(System.Windows.Media.DrawingContext drawingContext)
		{
			base.OnRender(drawingContext);

			Rendered(this, EventArgs.Empty);
		}
	}
}
