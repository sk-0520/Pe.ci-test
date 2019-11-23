using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Element;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherItemCustomize;
using ContentTypeTextNet.Pe.Main.Models.Element.Setting;
using ContentTypeTextNet.Pe.Main.ViewModels.LauncherIcon;
using ContentTypeTextNet.Pe.Main.ViewModels.LauncherItemCustomize;
using Microsoft.Extensions.Logging;
using Prism.Commands;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Setting
{
    public class LauncherItemWithIconViewModel<TLauncherItemViewModel> : ViewModelBase, ILauncherItemId
        where TLauncherItemViewModel : ViewModelBase, ILauncherItemId
    {
        public LauncherItemWithIconViewModel(TLauncherItemViewModel item, LauncherIconViewModel icon, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            if(item.LauncherItemId != icon.LauncherItemId) {
                throw new ArgumentException(nameof(ILauncherItemId.LauncherItemId));
            }

            Item = item;
            Icon = icon;
        }

        #region property

        public TLauncherItemViewModel Item { get; }
        public LauncherIconViewModel Icon { get; }

        #endregion


        #region ILauncherItemId

        public Guid LauncherItemId => Item.LauncherItemId;

        #endregion

    }

    public static class LauncherItemWithIconViewModel
    {
        #region function

        public static LauncherItemWithIconViewModel<TLauncherItemViewModel> Create<TLauncherItemViewModel>(TLauncherItemViewModel item, LauncherIconViewModel icon, ILoggerFactory loggerFactory)
            where TLauncherItemViewModel : ViewModelBase, ILauncherItemId
        {
            return new LauncherItemWithIconViewModel<TLauncherItemViewModel>(item, icon, loggerFactory);
        }

        #endregion
    }

}
