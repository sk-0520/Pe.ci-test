namespace ContentTypeTextNet.Library.SharedLibrary.ViewModel
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Linq;
	using System.Runtime.CompilerServices;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows.Input;
	using ContentTypeTextNet.Library.SharedLibrary.Logic;

	public abstract class ViewModelBase: DisposeFinalizeBase, INotifyPropertyChanged
	{
		#region variable

		HashSet<DelegateCommand> _createdCommands = new HashSet<DelegateCommand>();

		#endregion

		public ViewModelBase()
			: base()
		{ }

		#region DisposeFinalizeBase

		protected override void Dispose(bool disposing)
		{
			if(!IsDisposed) {
				foreach(var command in this._createdCommands) {
					command.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		#endregion

		#region INotifyPropertyChanged

		/// <summary>
		/// プロパティが変更された際に発生。
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged = delegate { };

		/// <summary>
		/// PropertyChanged呼び出し。
		/// </summary>
		/// <param name="propertyName"></param>
		protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
		{
			this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}

		#endregion

		#region functino

		/// <summary>
		/// 変数変更のヘルパ。
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="modelProperty">格納するプロパティ。</param>
		/// <param name="value">変更データ。</param>
		/// <param name="propertyName"></param>
		/// <returns>変更があった場合は真を返す。</returns>
		protected bool SetVariableValue<T>(ref T modelProperty, T value, [CallerMemberName] string propertyName = "")
		{
			if(!IComparable<T>.Equals(modelProperty, value)) {
				modelProperty = value;
				OnPropertyChanged(propertyName);

				return true;
			}

			return false;
		}

		protected virtual ICommand CreateCommand(Action<object> executeCommand)
		{
			var result = new DelegateCommand(executeCommand);

			this._createdCommands.Add(result);

			return result;
		}

		protected virtual ICommand CreateCommand(Action<object> executeCommand, Func<object, bool> canExecuteCommand)
		{
			var result = new DelegateCommand(executeCommand, canExecuteCommand);

			this._createdCommands.Add(result);

			return result;
		}

		#endregion
	}
}
