namespace ContentTypeTextNet.Pe.PeMain.ViewModel
{
	using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.Pe.Library.PeData.Item;
using ContentTypeTextNet.Pe.PeMain.IF;

	public class WindowItemCollectionViewModel : SingleModelWrapperViewModelBase<WindowItemCollectionModel>, IMenuItem
	{
		public WindowItemCollectionViewModel(WindowItemCollectionModel model)
			:base(model)
		{ }

		#region IMenuItem

		public ImageSource MenuImage {get { return null; }}

		public override string DisplayText
		{
			get
			{
				return Model.Name;
			}
		}

		#endregion

		#region command

		public ICommand WindowMenuSelectedCommand
		{
			get
			{
				var result = CreateCommand(
					o => {

					}
				);

				return result;
			}
		}
		#endregion
	}
}
