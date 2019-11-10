using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Core.ViewModels;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels
{
    public class EncodingViewModel: ViewModelBase
    {
        public EncodingViewModel(EncodingInfo encodingInfo, ILoggerFactory loggerFactory)
            :base(loggerFactory)
        {
            EncodingInfo = encodingInfo;
        }

        #region property

        public EncodingInfo EncodingInfo { get; }

        public string Name => EncodingInfo.Name;
        public string DisplayName => EncodingInfo.DisplayName;
        public int CodePage => EncodingInfo.CodePage;

        #endregion
    }
}
