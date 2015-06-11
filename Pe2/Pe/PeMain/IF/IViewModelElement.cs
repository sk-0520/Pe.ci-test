namespace ContentTypeTextNet.Pe.PeMain.IF
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.ViewModel;

	/// <summary>
	/// ViewModelを保持する要素。
	/// </summary>
	/// <typeparam name="TViewModel"></typeparam>
	public interface IViewModelElement<TViewModel>
		where TViewModel: ViewModelBase
	{
		/// <summary>
		/// 
		/// </summary>
		TViewModel ViewModel { get; }
	}
}
