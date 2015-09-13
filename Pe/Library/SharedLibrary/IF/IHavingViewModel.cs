namespace ContentTypeTextNet.Library.SharedLibrary.IF
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;
	using ContentTypeTextNet.Library.SharedLibrary.ViewModel;

	public interface IHavingViewModel<out TViewModel>
		where TViewModel : ViewModelBase
	{
		TViewModel ViewModel { get; }
	}
}
