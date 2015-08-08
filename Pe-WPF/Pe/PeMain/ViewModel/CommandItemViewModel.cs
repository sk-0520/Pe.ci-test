namespace ContentTypeTextNet.Pe.PeMain.ViewModel
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows.Media.Imaging;
	using ContentTypeTextNet.Library.SharedLibrary.Define;
	using ContentTypeTextNet.Library.SharedLibrary.Model;
	using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
	using ContentTypeTextNet.Pe.Library.PeData.Item;
	using ContentTypeTextNet.Pe.PeMain.Define;
	using ContentTypeTextNet.Pe.PeMain.IF;
	using ContentTypeTextNet.Pe.PeMain.Logic.Utility;

	/// <summary>
	/// <para>モデルはなし。</para>
	/// </summary>
	public class CommandItemViewModel: ViewModelBase, IHavingAppNonProcess, IHavingAppSender
	{
		#region variable

		BitmapSource _image;

		#endregion

		CommandItemViewModel(CommandKind commandKind, IconScale iconScale, IAppNonProcess appNonProcess, IAppSender appSender)
		{
			CommandKind = commandKind;
			IconScale = iconScale;
			AppNonProcess = appNonProcess;
			AppSender = appSender;
		}

		public CommandItemViewModel(IconScale iconScale, LauncherItemModel launcherItem, IAppNonProcess appNonProcess, IAppSender appSender)
			: this(CommandKind.LauncherItemName, iconScale, appNonProcess, appSender)
		{
			LauncherItemModel = launcherItem;
		}

		public CommandItemViewModel(IconScale iconScale, LauncherItemModel launcherItem, string tag, IAppNonProcess appNonProcess, IAppSender appSender)
			: this(CommandKind.LauncherItemTag, iconScale, appNonProcess, appSender)
		{
			LauncherItemModel = launcherItem;
			Tag = tag;
		}

		public CommandItemViewModel(IconScale iconScale, string filePath, bool isDirectory, bool isHideFile, IAppNonProcess appNonProcess, IAppSender appSender)
			: this(CommandKind.File, iconScale, appNonProcess, appSender)
		{
			FilePath = filePath;
			IsDirectory = isDirectory;
			IsHideFile = isHideFile;
		}

		#region property

		public CommandKind CommandKind { get; private set; }
		public IconScale IconScale { get; private set; }
		public LauncherItemModel LauncherItemModel { get; private set; }
		public string Tag { get; private set; }
		public string FilePath { get; private set; }
		public bool IsDirectory { get; private set; }
		public bool IsHideFile { get; private set; }

		public BitmapSource Image
		{
			get
			{
				if(this._image == null) {
					if(CommandKind == CommandKind.File) {
						var iconPath = new IconPathModel() {
							Path = FilePath,
							Index = 0,
						};
						this._image = AppUtility.LoadIconDefault(iconPath, IconScale, AppNonProcess.Logger);
					} else {
						Debug.Assert(CommandKind == CommandKind.LauncherItemName || CommandKind == CommandKind.LauncherItemTag);
						var viewModel = new LauncherItemSimpleViewModel(LauncherItemModel, AppNonProcess, AppSender);
						this._image = viewModel.GetIcon(IconScale);
					}
					//AppUtility.LoadIconDefault()
				}

				return this._image;
			}
		}
		#endregion

		#region IHavingAppNonProcess

		public IAppNonProcess AppNonProcess { get; private set; }

		#endregion

		#region IHavingAppSender

		public IAppSender AppSender { get; private set; }

		#endregion

		#region ViewModelBase

		public override string DisplayText
		{
			get
			{
				switch(CommandKind) {
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
