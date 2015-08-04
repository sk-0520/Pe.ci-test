namespace ContentTypeTextNet.Pe.PeMain.ViewModel
{
	using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.Pe.Library.PeData.Item;
using ContentTypeTextNet.Pe.PeMain.Define;
using ContentTypeTextNet.Pe.PeMain.IF;

	/// <summary>
	/// <para>モデルはなし。</para>
	/// </summary>
	public class CommandItemViewModel : ViewModelBase, IHavingAppNonProcess
	{
		#region variable

		#endregion

		public CommandItemViewModel(CommandKind commandKind, LauncherItemModel launcherItem, IAppNonProcess appNonProcess)
		{
			CommandKind = commandKind;
			LauncherItemModel = launcherItem;
			AppNonProcess = appNonProcess;
		}

		public CommandItemViewModel(string filePath)
		{
			CommandKind = CommandKind.File;
			FilePath = filePath;
		}

		#region property

		public CommandKind CommandKind { get; private set; }
		public LauncherItemModel LauncherItemModel { get; private set; }
		public string FilePath { get; private set; }

		#endregion

		#region IHavingAppNonProcess

		public IAppNonProcess AppNonProcess { get; private set; }

		#endregion
	}
}
