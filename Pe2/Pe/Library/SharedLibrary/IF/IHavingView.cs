namespace ContentTypeTextNet.Library.SharedLibrary.IF
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;

	public interface IHavingView<out TView>
		where TView: UIElement
	{
		TView View { get; }
	}
}
