﻿namespace ContentTypeTextNet.Pe.PeMain.ViewModel
{
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows.Media;
	using System.Windows.Media.Imaging;
	using ContentTypeTextNet.Library.SharedLibrary.Define;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
	using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
	using ContentTypeTextNet.Pe.Library.PeData.Define;
	using ContentTypeTextNet.Pe.Library.PeData.Item;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.IF;
	using ContentTypeTextNet.Pe.PeMain.Logic.Utility;

	public abstract class LauncherViewModelBase: SingleModelWrapperViewModelBase<LauncherItemModel>, IHavingNonProcess, IHavingLauncherIconCaching
	{
		#region variable

		static readonly Color defualtIconColor = Colors.Transparent;
		Color _iconColor = defualtIconColor;

		#endregion

		public LauncherViewModelBase(LauncherItemModel model, LauncherIconCaching launcherIconCaching, INonProcess nonProcess)
			: base(model)
		{
			LauncherIconCaching = launcherIconCaching;
			NonProcess = nonProcess;
		}

		#region property

		#region IHavingNonProcess

		public INonProcess NonProcess { get; private set; }

		#endregion

		#region IHavingLauncherIconCaching

		public LauncherIconCaching LauncherIconCaching { get; private set; }

		#endregion

		public override string DisplayText { get { return DisplayTextUtility.GetDisplayName(Model); } }

		public virtual LauncherKind LauncherKind
		{
			get { return Model.LauncherKind; }
			set { throw new NotSupportedException(); }
		}

		public string Command
		{
			get { return Model.Command; }
			set { SetModelValue(value); }
		}

		public string WorkDirectoryPath
		{
			get { return Model.WorkDirectoryPath; }
			set { SetModelValue(value); }
		}

		public string Option
		{
			get { return Model.Option; }
			set { SetModelValue(value); }
		}

		public string Comment
		{
			get { return Model.Comment; }
			set { SetModelValue(value); }
		}

		public IconItemModel Icon
		{
			get { return Model.Icon; }
			set { SetModelValue(value); }
		}

		public bool StdStreamOutput
		{
			get { return Model.StdStream.OutputWatch; }
			set { SetPropertyValue(Model.StdStream, value, "OutputWatch"); }
		}

		public bool Administrator
		{
			get { return Model.Administrator; }
			set { SetModelValue(value); }
		}

		public string Tags
		{
			get { return string.Join(", ", Model.Tag.Items); }
			set
			{
				var items = value.Split(',')
					.Where(s => !string.IsNullOrWhiteSpace(s))
					.Select(s => s.Trim())
					.OrderBy(s => s)
					.Distinct()
				;
				Model.Tag.Items = new ObservableCollection<string>(items);
				OnPropertyChanged();
			}
		}

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
