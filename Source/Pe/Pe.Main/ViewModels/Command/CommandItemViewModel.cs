using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherIcon;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.ViewModels.IconViewer;
using ContentTypeTextNet.Pe.Main.ViewModels.LauncherIcon;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Command
{
    public class CommandItemViewModel : ViewModelBase
    {
        #region variable
        #endregion

        public CommandItemViewModel(ICommandItem item, IconBox iconBox, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory) : base(loggerFactory)
        {
            Item = item;
            IconBox = iconBox;
            DispatcherWrapper = dispatcherWrapper;
        }

        #region property

        ICommandItem Item { get; }
        IconBox IconBox { get; }
        IDispatcherWrapper DispatcherWrapper { get; }
        public string Header => Item.Header;
        public string Description => Item.Description;
        public string Kind => Item.Kind;
        public double Score => Item.Score;

        public object Icon
        {
            get
            {
                var icon = Item.GetIcon(IconBox);
                if(icon is IconImageLoaderBase iconLoader) {
                    return new IconViewerViewModel(iconLoader, DispatcherWrapper, LoggerFactory) {
                        UseCache = true,
                    };
                }
                return icon;
            }
        }

        #endregion

    }
}
