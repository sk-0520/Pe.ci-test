namespace ContentTypeTextNet.Library.SharedLibrary.Logic
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows.Input;

    public class DelegateCommand : ICommand
    {
		/// <summary>
		/// コマンド。
		/// </summary>
		public Action<object> Command;
		/// <summary>
		/// 実行可否。
		/// </summary>
		private Func<object, bool> CanExecute;

        // コンストラクタ
        public DelegateCommand(Action<object> command, Func<object,bool> canExecute)
        {
			if(command == null) {
				throw new ArgumentNullException("command");
			}

			Command = command;
			CanExecute = canExecute;
        }

		#region ICommand

		void ICommand.Execute(object parameter)
		{
			Command(parameter);
		}

		bool ICommand.CanExecute(object parameter)
		{
			if(CanExecute != null) {
				return CanExecute(parameter);
			} else {
				return false;
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
