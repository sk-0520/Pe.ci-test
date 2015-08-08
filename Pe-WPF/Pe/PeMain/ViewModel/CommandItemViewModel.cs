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

		public CommandItemViewModel(LauncherItemModel launcherItem, IAppNonProcess appNonProcess)
		{
			CommandKind = CommandKind.LauncherItemName;
			LauncherItemModel = launcherItem;
			AppNonProcess = appNonProcess;
		}

		public CommandItemViewModel(LauncherItemModel launcherItem, string tag, IAppNonProcess appNonProcess)
		{
			CommandKind = CommandKind.LauncherItemTag;
			LauncherItemModel = launcherItem;
			Tag = tag;
			AppNonProcess = appNonProcess;
		}

		public CommandItemViewModel(string filePath, bool isDirectory, bool isHideFile, IAppNonProcess appNonProcess)
		{
			CommandKind = CommandKind.File;
			FilePath = filePath;
			IsDirectory = isDirectory;
			IsHideFile = isHideFile;
			AppNonProcess = appNonProcess;
		}

		#region property

		public CommandKind CommandKind { get; private set; }
		public LauncherItemModel LauncherItemModel { get; private set; }
		public string Tag { get; private set; }
		public string FilePath { get; private set; }
		public bool IsDirectory { get; private set; }
		public bool IsHideFile { get; private set; }

		#endregion

		#region IHavingAppNonProcess

		public IAppNonProcess AppNonProcess { get; private set; }

		#endregion

		#region ViewModelBase

		public override string DisplayText
		{
			get
			{
				switch (CommandKind) {
					case Define.CommandKind.LauncherItemName:
						return LauncherItemModel.Name;

					case Define.CommandKind.LauncherItemTag:
						return Tag;

					case Define.CommandKind.File:
						return FilePath;

					default:
						throw new NotImplementedException();
				}
			}
		}

		#endregion
	}
}
