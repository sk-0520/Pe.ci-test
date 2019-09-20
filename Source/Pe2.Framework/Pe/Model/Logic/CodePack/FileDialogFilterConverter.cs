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
    public class FileDialogFilterConverter
    {
        public FileDialogFilterConverter(ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateTartget(GetType());
        }

        #region function

        ILogger Logger { get; }

        #endregion

        #region function

        CommonFileDialogFilter ToFilter(DialogFilterItem sourceItem, bool showExtensions)
        {
            var filter = new CommonFileDialogFilter() {
                DisplayName = sourceItem.Display,
            };
            foreach(var wildcard in sourceItem.Wildcard) {
                var index = wildcard.IndexOf('.');
                if(index != -1) {
                    filter.Extensions.Add(wildcard.Substring(index + 1));
                } else {
                    Logger.Debug($"拡張子分離不可: {wildcard}");
                    filter.Extensions.Add(wildcard);
                }
            }
            filter.ShowExtensions = showExtensions;
            return filter;
        }

        public IEnumerable<CommonFileDialogFilter> ToFilters(IEnumerable<DialogFilterItem> sourceItems, bool showExtensions)
        {
            return sourceItems.Select(i => ToFilter(i, showExtensions));
        }

        #endregion
    }
}
