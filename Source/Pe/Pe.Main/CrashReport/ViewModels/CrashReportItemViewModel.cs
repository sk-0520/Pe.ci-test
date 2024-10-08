using System.Collections.Generic;
using System.Linq;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Library.Base;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.CrashReport.ViewModels
{
    public class CrashReportItemViewModel: ViewModelBase
    {
        #region variable

        private bool _isExpanded = true;

        #endregion

        public CrashReportItemViewModel(ObjectDumpItem item, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            Item = item;
            Name = Item.MemberInfo.Name;
            Value = Item.Value;
            Children = Item.Children
                .Select(i => new CrashReportItemViewModel(i, LoggerFactory))
                .ToList()
            ;
        }

        #region property

        private ObjectDumpItem Item { get; }
        public object? Value { get; }
        public string Name { get; }

        public IReadOnlyList<CrashReportItemViewModel> Children { get; } = new List<CrashReportItemViewModel>();

        public bool IsExpanded
        {
            get => this._isExpanded;
            set => SetProperty(ref this._isExpanded, value);
        }

        #endregion
    }
}
