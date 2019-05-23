using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Data;
using ContentTypeTextNet.Pe.Main.Model.Element.CustomizeLauncherItem;
using ContentTypeTextNet.Pe.Main.Model.Element.LauncherItem;

namespace ContentTypeTextNet.Pe.Main.ViewModel.CustomizeLauncherItem
{
    public class CustomizeLauncherCommonViewModel : CustomizeLauncherDetailViewModelBase
    {
        #region variable

        string _name;
        string _code;

        #endregion

        public CustomizeLauncherCommonViewModel(CustomizeLauncherItemElement model, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        { }

        #region property

        public Guid LauncherItemId => Model.LauncherItemId;
        public LauncherItemKind Kind => Model.Kind;

        public string Name
        {
            get => this._name;
            set => SetProperty(ref this._name, value);
        }
        public string Code
        {
            get => this._code;
            set => SetProperty(ref this._code, value);
        }

        #endregion

        #region command
        #endregion

        #region function

        #endregion

        #region CustomizeLauncherDetailViewModelBase

        protected override void InitializeImpl()
        {
            Name = Model.Name;
            Code = Model.Code;
        }

        #endregion
    }
}
