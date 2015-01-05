using System.ComponentModel;

namespace ContentTypeTextNet.Pe.Applications.Hash
{
	public abstract class BaseViewModel: INotifyPropertyChanged
	{
		#region INotifyPropertyChanged

		public event PropertyChangedEventHandler PropertyChanged = delegate { };

		protected void OnPropertyChanged(string name)
		{
			PropertyChanged(this, new PropertyChangedEventArgs(name));
		}

		#endregion
	}
}
