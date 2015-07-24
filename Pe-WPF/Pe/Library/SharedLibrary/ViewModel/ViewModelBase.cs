namespace ContentTypeTextNet.Library.SharedLibrary.ViewModel
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Linq;
	using System.Reflection;
	using System.Runtime.CompilerServices;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows.Input;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.Logic;
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;

	public abstract class ViewModelBase : DisposeFinalizeBase, INotifyPropertyChanged, IDisplayText
	{
		#region variable

		HashSet<DelegateCommand> _createdCommands = new HashSet<DelegateCommand>();
		bool _isChanged = false;

		#endregion

		public ViewModelBase()
			: base()
		{
			ResetChangeFlag();
		}

		#region property

		public bool IsChanged {
			get { return this._isChanged; }
			private set { SetVariableValue(ref this._isChanged, value); }
		}

		#endregion

		#region IDisplayText

		public virtual string DisplayText { get { return ToString(); } }

		#endregion

		#region DisposeFinalizeBase

		protected override void Dispose(bool disposing)
		{
			if (!IsDisposed) {
				foreach (var command in this._createdCommands) {
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
			IsChanged = true;
			this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}

		#endregion

		#region function

		/// <summary>
		/// 変数変更用ヘルパ。
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="variable">格納する変数。</param>
		/// <param name="value">変更データ。</param>
		/// <param name="propertyName"></param>
		/// <returns>変更があった場合は真を返す。</returns>
		protected bool SetVariableValue<T>(ref T variable, T value, [CallerMemberName] string propertyName = "")
		{
			if (!IComparable<T>.Equals(variable, value)) {
				variable = value;
				OnPropertyChanged(propertyName);

				return true;
			}

			return false;
		}

		/// <summary>
		/// プロパティ変更用ヘルパ。
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="obj">対象オブジェクト。</param>
		/// <param name="value">変更データ。</param>
		/// <param name="targetMemberName">対象オブジェクトのメンバ名。</param>
		/// <param name="propertyName"></param>
		/// <returns>変更があった場合は真を返す。</returns>
		protected bool SetPropertyValue<T>(object obj, T value, [CallerMemberName] string targetMemberName = "", [CallerMemberName] string callerPropertyName = "")
		{
			CheckUtility.DebugEnforceNotNull(obj);

			var type = obj.GetType();
			var propertyInfo = type.GetProperty(targetMemberName);

			var nowValue = (T)propertyInfo.GetValue(obj);

			if (!IComparable<T>.Equals(nowValue, value)) {
				propertyInfo.SetValue(obj, value);
				OnPropertyChanged(callerPropertyName);

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

		protected void ResetChangeFlag()
		{
			IsChanged = false;
		}

		#endregion
	}
}
