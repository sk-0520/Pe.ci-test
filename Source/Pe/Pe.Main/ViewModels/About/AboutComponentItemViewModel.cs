using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Data;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels.About
{
    public class AboutComponentItemViewModel : ViewModelBase
    {
        public AboutComponentItemViewModel(AboutComponentItem item, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            Item = item;
        }

        #region property

        AboutComponentItem Item { get; }

        public AboutComponentKind Kind => Item.Kind;

        public string Name => Item.Data.Name;
        public Uri Uri => new Uri(Item.Data.Uri);

        //public bool HasLicenseName => !string.IsNullOrWhiteSpace(License);
        public bool HasLicenseUri => !string.IsNullOrWhiteSpace(Item.Data.License.Uri);

        public string License => Item.Data.License.Name;
        public Uri LicenseUri => new Uri(Item.Data.License.Uri);

        public bool HasComment => !string.IsNullOrWhiteSpace(Comment);
        public string Comment => Item.Data.Comment;

        public int Sort => Item.Sort;

        #endregion

        #region command

        #endregion

        #region function

        #endregion
    }
}
