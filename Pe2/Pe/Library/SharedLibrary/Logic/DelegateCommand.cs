namespace ContentTypeTextNet.Library.SharedLibrary.Logic
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows.Input;

	/// <summary>
	/// コマンド。
	/// </summary>
	public class DelegateCommand : ICommand
	{
		static bool DefaultExecute(object o)
		{
			return true;
		}

		/// <summary>
		/// コマンド。
		/// </summary>
		public Action<object> Command;
		/// <summary>
		/// 実行可否。
		/// </summary>
		public Func<object, bool> CanExecute;

		public DelegateCommand()
		{
			CanExecute = DefaultExecute;
		}

		public DelegateCommand(Action<object> command)
			: this()
		{
			if(command == null) {
				throw new ArgumentNullException("command");
			}

			Command = command;
		}

		public DelegateCommand(Action<object> command, Func<object, bool> canExecute)
			: this(command)
		{
			if(canExecute == null) {
				throw new ArgumentNullException("canExecute");
			}

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
