/*
This file is part of Pe.

Pe is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

Pe is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with Pe.  If not, see <http://www.gnu.org/licenses/>.
*/
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
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.Pe.Library.PeData.Define;
using ContentTypeTextNet.Pe.Library.PeData.Item;
using ContentTypeTextNet.Pe.PeMain.Data;
using ContentTypeTextNet.Pe.PeMain.IF;
using ContentTypeTextNet.Pe.PeMain.Logic.Utility;

namespace ContentTypeTextNet.Pe.PeMain.ViewModel
{
    public abstract class LauncherItemViewModelBase: SingleModelWrapperViewModelBase<LauncherItemModel>, IHasAppNonProcess, IHasAppSender
    {
        #region variable

        static readonly Color defualtIconColor = Colors.Transparent;
        Color _iconColor = defualtIconColor;

        #endregion

        public LauncherItemViewModelBase(LauncherItemModel model, IAppNonProcess appNonProcess, IAppSender appSender)
            : base(model)
        {
            AppNonProcess = appNonProcess;
            AppSender = appSender;
        }

        #region property

        public override string DisplayText { get { return DisplayTextUtility.GetDisplayName(Model); } }

        public virtual LauncherKind LauncherKind
        {
            get { return Model.LauncherKind; }
            set { throw new NotSupportedException(); }
        }

        public virtual string Command
        {
            get { return Model.Command; }
            set
            {
                if(SetModelValue(value)) {
                    CallOnPropertyChangeDisplayItem();
                }
            }
        }

        public virtual string WorkDirectoryPath
        {
            get { return Model.WorkDirectoryPath; }
            set { SetModelValue(value); }
        }

        public virtual string Option
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

        public virtual bool StdStreamOutput
        {
            get { return Model.StdStream.OutputWatch; }
            set { SetPropertyValue(Model.StdStream, value, nameof(Model.StdStream.OutputWatch)); }
        }

        public virtual bool StdStreamInput
        {
            get { return Model.StdStream.InputUsing; }
            set { SetPropertyValue(Model.StdStream, value, nameof(Model.StdStream.InputUsing)); }
        }


        public virtual bool Administrator
        {
            get { return Model.Administrator; }
            set { SetModelValue(value); }
        }

        public bool IsCommandAutocomplete
        {
            get { return Model.IsCommandAutocomplete; }
            set { SetModelValue(value); }
        }

        public string Tags
        {
            get { return string.Join(", ", Model.Tag.Items.Concat(new[] { string.Empty })); }
            set
            {
                var items = value.Split(',')
                    .Where(s => !string.IsNullOrWhiteSpace(s))
                    .Select(s => s.Trim())
                ;
                Model.Tag.Items = new CollectionModel<string>(items);
                OnPropertyChanged();
            }
        }

        public BitmapSource ViewIcon
        {
            get
            {
                var element = ImageUtility.MakeOverlayImage(
                    AppResource.ApplicationIconNormal,
                    GetIcon(IconScale.Small)
                );

                // TODO: 16pxアイコンも作りたいなぁ
                var image = ImageUtility.MakeBitmapBitmapSourceDefualtDpi(element);
                FreezableUtility.SafeFreeze(image);

                return image;
            }
        }

        #endregion

        #region function

        public BitmapSource GetIcon(IconScale iconScale)
        {
            CheckUtility.DebugEnforceNotNull(AppNonProcess.LauncherIconCaching);

            return AppUtility.LoadLauncherItemIcon(iconScale, Model, AppNonProcess.LauncherIconCaching, AppNonProcess);
        }

        public Color GetIconColor(IconScale iconScale)
        {
            if(this._iconColor == Colors.Transparent) {
                var icon = GetIcon(iconScale);
                this._iconColor = AppUtility.GetHotTrackColor(icon);
            }

            return this._iconColor;
        }

        public void Execute(ScreenModel screen)
        {
            try {
                ExecuteUtility.RunItem(Model, screen, AppNonProcess, AppSender);
                SettingUtility.IncrementLauncherItem(Model, null, null, AppNonProcess);
            } catch(Exception ex) {
                AppNonProcess.Logger.Warning(ex);
            }
        }

        #endregion

        #region IHasAppNonProcess

        public IAppNonProcess AppNonProcess { get; private set; }

        #endregion

        #region IHavingAppSender

        public IAppSender AppSender { get; private set; }

        #endregion

        #region SingleModelWrapperViewModelBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                AppNonProcess = null;
                AppSender = null;
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}
