using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.About
{
    public class AboutElement : ElementBase
    {
        public AboutElement(CustomConfiguration customConfiguration, ILoggerFactory loggerFactory) : base(loggerFactory)
        {
            CustomConfiguration = customConfiguration;
        }

        #region property

        CustomConfiguration CustomConfiguration { get; }


        #endregion

        #region function

        #endregion

        #region ElementBase

        protected override void InitializeImpl()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
