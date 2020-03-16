using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.ViewModels;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.CrashReport.ViewModels
{
    public class CrashReportItemViewModel : ViewModelBase
    {
        public CrashReportItemViewModel(ObjectDumpItem item, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            Item = item;
            Name = Item.MemberInfo.Name;
            Value = Item.Value;
            switch(Item.Value) {
                case string _:
                    break;
                case IEnumerable enumerable:
                    var objectDumper = new ObjectDumper();
                    Children = objectDumper.Dump(enumerable)
                        .Select(i => new CrashReportItemViewModel(i, LoggerFactory))
                        .ToList()
                    ;
                    break;
                default:
                    break;
            }
            
        }

        #region property

        ObjectDumpItem Item { get; }
        public object? Value { get; }
        public string Name { get; }

        public IReadOnlyList<CrashReportItemViewModel> Children { get; } = new List<CrashReportItemViewModel>();

        #endregion
    }
}
