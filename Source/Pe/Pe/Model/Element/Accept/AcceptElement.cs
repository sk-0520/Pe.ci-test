using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Core.Model;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Model.Element.Accept
{
    public class AcceptElement : ContextElementBase
    {
        public AcceptElement(IDiContainer diContainer, ILoggerFactory loggerFactory)
            : base(diContainer, loggerFactory)
        { }

        #region property

        public bool Accepted { get; set; }

        #endregion

        #region function
        #endregion

        #region ContextElementBase

        protected override void InitializeImpl()
        {
            Logger.LogTrace("not impl");
        }

        #endregion
    }
}
