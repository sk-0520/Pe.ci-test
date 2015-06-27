namespace ContentTypeTextNet.Pe.PeMain.ViewModel
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows.Media;
	using System.Windows.Media.Imaging;
	using ContentTypeTextNet.Library.SharedLibrary.Define;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
	using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
	using ContentTypeTextNet.Pe.Library.PeData.Item;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.IF;
	using ContentTypeTextNet.Pe.PeMain.Logic.Utility;

	public class LauncherViewModelBase: SingleModelWrapperViewModelBase<LauncherItemModel>, IHavingNonProcess, IHavingClipboardWatcher, IHavingLauncherIconCaching
	{
		#region variable

		static readonly Color defualtIconColor = Colors.Transparent;
		Color _iconColor = defualtIconColor;

		#endregion

		public LauncherViewModelBase(LauncherItemModel model, LauncherIconCaching launcherIconCaching, INonProcess nonProcess, IClipboardWatcher clipboardWatcher)
			: base(model)
		{
			LauncherIconCaching = launcherIconCaching;
			NonProcess = nonProcess;
			ClipboardWatcher = clipboardWatcher;
		}

		#region property

		#region IHavingNonProcess

		public INonProcess NonProcess { get; private set; }

		#endregion

		#region IHavingClipboardWatcher

		public IClipboardWatcher ClipboardWatcher { get; private set; }

		#endregion

		#region IHavingLauncherIconCaching

		public LauncherIconCaching LauncherIconCaching { get; private set; }

		#endregion

		#endregion

		#region function

		public BitmapSource GetIcon(IconScale iconScale)
		{
			CheckUtility.DebugEnforceNotNull(LauncherIconCaching);

			return LauncherIconCaching[iconScale].Get(Model, () => LauncherItemUtility.GetIcon(Model, iconScale, NonProcess));
		}

		public Color GetIconColor(IconScale iconScale)
		{
			if(this._iconColor == Colors.Transparent) {
				var icon = GetIcon(iconScale);
				this._iconColor = MediaUtility.GetPredominantColorFromBitmapSource(icon);
			}

			return this._iconColor;
		}

		protected void Execute()
		{
			try {
				ExecuteUtility.RunItem(Model, NonProcess);
				SettingUtility.IncrementLauncherItem(Model, null, null, NonProcess);
			} catch (Exception ex) {
				NonProcess.Logger.Warning(ex);
			}
		}

		#endregion
	}
}
