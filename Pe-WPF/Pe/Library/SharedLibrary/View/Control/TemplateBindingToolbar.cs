namespace ContentTypeTextNet.Library.SharedLibrary.View.Control
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows.Controls;
	using System.Windows.Threading;

	/// <summary>
	/// http://stackoverflow.com/questions/1589034/how-to-get-a-wpf-toolbar-to-bind-to-a-collection-in-my-vm-without-using-expander?answertab=votes#tab-top
	/// </summary>
	public class TemplateBindingToolBar: ToolBar
	{
		#region define
		
		delegate void IvalidateMeasureJob();
		
		#endregion

		public TemplateBindingToolBar()
			: base()
		{ }

		#region function

		public override void OnApplyTemplate()
		{
			Dispatcher.BeginInvoke(
				new IvalidateMeasureJob(InvalidateMeasure),
				DispatcherPriority.Background,
				null
			);
			base.OnApplyTemplate();
		}

		#endregion
	}
}
