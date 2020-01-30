using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Linq;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using Microsoft.Extensions.Logging;
using System.Security;

namespace ContentTypeTextNet.Pe.Main.Models.Element.Accept
{
    public class AcceptElement : ElementBase
    {
        public AcceptElement(CustomConfiguration configuration, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            Configuration = configuration;
        }

        #region property
        CustomConfiguration Configuration { get; }
        public bool Accepted { get; set; }
        public UpdateKind UpdateKind { get; set; } = UpdateKind.Auto;
        public bool SendUsageStatistics { get; set; } = true;

        #endregion

        #region function

        public Stream GetAcceptDocumentXamlStream()
        {
            //リソースから読み込んで色々つけてあげる（将来用）
            var map = new Dictionary<string, string>() {
                ["APP"] = BuildStatus.Name,
                ["ACCEPT-AFFIRMATIVE"] = Properties.Resources.String_Accept_Affirmative,
                ["ACCEPT-NEGATIVE"] = Properties.Resources.String_Accept_Negative,
                ["LICENSE-NAME"] = Configuration.General.LicenseName,
                ["COPYRIGHT"] = BuildStatus.Copyright,
                ["PROJECT-URI"] = Configuration.General.ProjectRepositoryUri.ToString(),
                ["FORUM-URI"] = Configuration.General.ProjectForumUri.ToString(),
                ["WEBSITE-URI"] = Configuration.General.ProjectWebSiteUri.ToString(),
            }.ToDictionary(i => i.Key, i => SecurityElement.Escape(i.Value)!)
            ;
            var rawXml = TextUtility.ReplaceFromDictionary(Properties.Resources.File_Accept_AcceptDocument, map);

            // 受け渡し用に変更
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(rawXml));
            stream.Position = 0;
            return stream;
        }

        #endregion

        #region ElementBase

        protected override void InitializeImpl()
        { }

        #endregion
    }
}
