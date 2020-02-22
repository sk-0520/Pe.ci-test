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

            HeaderValues = ConvertHitValueItems(Item.Header, Item.HeaderMatches, LoggerFactory);
            DescriptionValues = ConvertHitValueItems(Item.Description, Item.DescriptionMatches, LoggerFactory);
        }

        #region property

        ICommandItem Item { get; }
        IconBox IconBox { get; }
        IDispatcherWrapper DispatcherWrapper { get; }
        public string Header => Item.Header;
        public string Description => Item.Description;
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

        #endregion

        #region function

        public void Execute(IScreen screen)
        {
            var isExtend = Keyboard.Modifiers == ModifierKeys.Shift;
            Item.Execute(screen, isExtend);
        }

        //TODO: ViewModel の層から外したい
        private static List<HitValueItem> ConvertHitValueItems(string source, IReadOnlyList<Range> matches, ILoggerFactory loggerFactory)
        {
            if(matches.Count == 0) {
                return new List<HitValueItem>() {
                    new HitValueItem(source, false, loggerFactory),
                };
            }

            var result = new List<HitValueItem>();

            var workMatches = matches.ToDictionary(i => i.Start.Value, i => i.End.Value - i.Start.Value);
            var i = 0;
            while(true) {
                if(workMatches.TryGetValue(i, out var hitLength)) {
                    var value = source.Substring(i, hitLength);
                    var item = new HitValueItem(value, true, loggerFactory);
                    result.Add(item);
                    workMatches.Remove(i);
                    i = i + hitLength;
                    if(workMatches.Count == 0) {
                        if(i <= source.Length) {
                            result.Add(new HitValueItem(source.Substring(i), false, loggerFactory));
                        }
                        break;
                    }
                } else {
                    var minIndex = workMatches.Keys.Min();
                    var value = source.Substring(i, minIndex - i);
                    var item = new HitValueItem(value, false, loggerFactory);
                    result.Add(item);
                    i = minIndex;
                }
            }

            return result;
        }

        #endregion
    }
}
