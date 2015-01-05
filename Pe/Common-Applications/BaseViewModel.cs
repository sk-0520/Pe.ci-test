namespace ContentTypeTextNet.Pe.Applications
{
	using System.ComponentModel;
	using System.Runtime.CompilerServices;

	public abstract class BaseViewModel: INotifyPropertyChanged
	{
		#region INotifyPropertyChanged
		/// <summary>
		/// プロパティ値が変更
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged = delegate { };
		#endregion

		/// <summary>
		/// PropertyChanged イベント を発生させます。
		/// </summary>
		/// <param name="propertyName">変更されたプロパティの名前</param>
		protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
		{
			this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
