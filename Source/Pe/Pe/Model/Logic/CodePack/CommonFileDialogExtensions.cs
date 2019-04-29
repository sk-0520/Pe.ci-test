using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace ContentTypeTextNet.Pe.Main.Model.Logic.CodePack
{
    public static class CommonFileDialogExtensions
    {
        #region function

        public static void SetFilters(this CommonFileDialog @this, DialogFilterList dialogFilterList, bool showExtensions, ILoggerFactory loggerFactory)
        {
            var converter = new FileDialogFilterConverter(loggerFactory);
            var filters = converter.ToFilters(dialogFilterList, showExtensions);
            foreach(var filter in filters) {
                @this.Filters.Add(filter);
            }
        }

        #endregion
    }
}
