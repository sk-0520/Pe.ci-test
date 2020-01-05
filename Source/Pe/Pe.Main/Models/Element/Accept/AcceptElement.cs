using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using ContentTypeTextNet.Pe.Core.Models;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.Accept
{
    public class AcceptElement : ContextElementBase
    {
        public AcceptElement(IDiContainer diContainer, ILoggerFactory loggerFactory)
            : base(diContainer, loggerFactory)
        { }

        #region property

        internal bool HasSetting { get; set; }

        public bool Accepted { get; set; }

        public bool CheckUpdate { get; set; }
        public bool SendUsageStatistics { get; set; }

        #endregion

        #region function

        public Stream GetAcceptDocumentXamlStream()
        {
            //リソースから読み込んで色々つけてあげる（将来用）
            var xml = XDocument.Parse(Properties.Resources.File_Accept_AcceptDocument);

            // 受け渡し用に変更
            var stream = new MemoryStream();
            xml.Save(stream);
            stream.Position = 0;
            return stream;
        }

        #endregion

        #region ContextElementBase

        protected override void InitializeImpl()
        {
            Logger.LogTrace("not impl");
        }

        #endregion
    }
}
