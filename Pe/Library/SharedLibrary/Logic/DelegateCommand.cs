namespace ContentTypeTextNet.Library.SharedLibrary.Logic
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows.Input;
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;

	/// <summary>
	/// コマンド。
	/// </summary>
	public class DelegateCommand : DisposeFinalizeBase, ICommand
	{
		//#region variable

		//EventDisposer<Action<object>> _executeCommandEvent;
		//EventDisposer<Func<object, bool>> _canExecuteCommandEvent;

		//#endregion

		public DelegateCommand()
		{ }

		public DelegateCommand(Action<object> executeCommand)
			: this()
		{
			if(executeCommand == null) {
				throw new ArgumentNullException("executeCommand");
			}
			ExecuteCommand = executeCommand;
		}

		public DelegateCommand(Action<object> command, Func<object, bool> canExecuteCommand)
			: this(command)
		{
			if(canExecuteCommand == null) {
				throw new ArgumentNullException("canExecuteCommand");
			}

			CanExecuteCommand = canExecuteCommand;
		}

		#region property

		/// <summary>
		/// コマンド。
		/// </summary>
		public Action<object> ExecuteCommand { get; set; }
		/// <summary>
		/// 実行可否。
		/// </summary>
		public Func<object, bool> CanExecuteCommand { get; set; }

		#endregion

		#region DisposeFinalizeBase

		protected override void Dispose(bool disposing)
		{
			if(!IsDisposed) {
				CanExecuteCommand = null;
				ExecuteCommand = null;
			}
			base.Dispose(disposing);
		}

		#endregion

		#region ICommand

		public void Execute(object parameter)
		{
			ExecuteCommand(parameter);
		}

		public bool CanExecute(object parameter)
		{
			if(CanExecuteCommand != null) {
				return CanExecuteCommand(parameter);
			} else {
				return true;
			}
		}

		event EventHandler ICommand.CanExecuteChanged
		{
			add { CommandManager.RequerySuggested += value; }
			remove { CommandManager.RequerySuggested -= value; }
		}

		#endregion
	}


}
