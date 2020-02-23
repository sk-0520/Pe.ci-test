using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
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

        public CommandItemViewModel(ICommandItem item, IconBox iconBox, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            Item = item;
            IconBox = iconBox;
            DispatcherWrapper = dispatcherWrapper;

            HeaderValues = Item.HeaderMatches.Select(i => new HitValueItem(i, LoggerFactory)).ToList();
            DescriptionValues = Item.DescriptionMatches.Select(i => new HitValueItem(i, LoggerFactory)).ToList();
        }

        #region property

        ICommandItem Item { get; }
        IconBox IconBox { get; }
        IDispatcherWrapper DispatcherWrapper { get; }
        public CommandItemKind Kind => Item.Kind;
        public double Score => Item.Score;

        public IReadOnlyList<HitValueItem> HeaderValues { get; }
        public IReadOnlyList<HitValueItem> DescriptionValues { get; }

        public bool ShowDescription
        {
            get
            {
                switch(Kind) {
                    case CommandItemKind.LauncherItem:
                    case CommandItemKind.LauncherItemName:
                        return false;

                    default:
                        return true;
                }
            }
        }

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

        public string Header => string.Join(string.Empty, HeaderValues.Select(i => i.Value));

        #endregion

        #region function

        public void Execute(IScreen screen)
        {
            var isExtend = Keyboard.Modifiers == ModifierKeys.Shift;
            Item.Execute(screen, isExtend);
        }


        #endregion
    }
}
