using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherItemCustomize;
using ContentTypeTextNet.Pe.Library.Base;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels.LauncherItemCustomize
{
    public class LauncherItemCustomizeSeparatorViewModel: LauncherItemCustomizeDetailViewModelBase
    {
        public LauncherItemCustomizeSeparatorViewModel(LauncherItemCustomizeEditorElement model, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, dispatcherWrapper, loggerFactory)
        {
            KindItems = new ObservableCollection<LauncherSeparatorKind>(
                Enum.GetValues<LauncherSeparatorKind>()
                    .Where(a => a != LauncherSeparatorKind.None)
                    .OrderBy(a => a)
            );
        }

        #region property
        private LauncherSeparatorData Separator => Model.Separator!;

        public ObservableCollection<LauncherSeparatorKind> KindItems { get; }
        public LauncherSeparatorKind SelectedKind
        {
            get => Separator.Kind;
            set => SetPropertyValue(Separator, value, nameof(Separator.Kind));
        }

        public int Width
        {
            get => Separator.Width;
            set => SetPropertyValue(Separator, value, nameof(Separator.Width));
        }

        #endregion

        #region LauncherItemCustomizeSeparatorViewModel

        protected override void InitializeImpl()
        {
            if(Model.IsLazyLoad) {
                return;
            }
        }

        #endregion
    }
}
