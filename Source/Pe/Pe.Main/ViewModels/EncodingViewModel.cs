using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.ViewModels;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels
{
    public class EncodingViewModel : ViewModelBase
    {
        public EncodingViewModel(EncodingInfo encodingInfo, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            Name = encodingInfo.Name;
            DisplayName = encodingInfo.DisplayName;
            CodePage = encodingInfo.CodePage;
        }

        public EncodingViewModel(Encoding encoding, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            Name = EncodingUtility.ToString(encoding);
            DisplayName = encoding.EncodingName;
            CodePage = encoding.CodePage;
        }

        #region property

        public string Name { get; }
        public string DisplayName { get; }
        public int CodePage { get; }

        #endregion
    }
}
