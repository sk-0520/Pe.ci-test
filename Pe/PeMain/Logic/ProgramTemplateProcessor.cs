/**
This file is part of Pe.

Pe is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

Pe is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with Pe.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.Logic.T4Template;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Extension;
using ContentTypeTextNet.Pe.PeMain.Logic.Utility;
using ContentTypeTextNet.Pe.Library.PeData.Define;

namespace ContentTypeTextNet.Pe.PeMain.Logic
{
    /// <summary>
    /// C#限定でムリくりアプリケーション用テンプレート処理。
    /// </summary>
    [Serializable]
    public class ProgramTemplateProcessor: T4TemplateProcessor, IHasNonProcess
    {
        #region define

        const string directiveLang = "LANGUAGE";

        #endregion

        public ProgramTemplateProcessor(INonProcess nonProcess)
            : base(nonProcess != null ? nonProcess.Logger: null)
        {
            NonProcess = nonProcess;
        }

        public ProgramTemplateProcessor(TextTemplatingEngineHost host, INonProcess nonProcess)
            : base(host, nonProcess != null ? nonProcess.Logger : null)
        {
            NonProcess = nonProcess;
        }

        #region property

        /// <summary>
        /// テンプレートディレクティブ。
        /// </summary>
        public string TemplateDirective { get; set; }

        /// <summary>
        /// 言語コード。
        /// </summary>
        public string CultureCode { get; set; }

        #endregion

        #region function

        protected void ResetVariable()
        {
            var clipboardData = ClipboardUtility.GetClipboardDataDefault(ClipboardType.Text, IntPtr.Zero, NonProcess);

            Variable[TemplateReplaceKey.programTimestamp] = DateTime.Now;
            Variable[TemplateReplaceKey.programClipboard] = clipboardData != null ? clipboardData.Body.Text ?? string.Empty: string.Empty;
            Variable[TemplateReplaceKey.programApplicationName] = Constants.ApplicationName;
            Variable[TemplateReplaceKey.programApplicationVersion] = Constants.ApplicationVersion;
            Variable[TemplateReplaceKey.programApplicationVersionNumber] = Constants.ApplicationVersionNumber;
            Variable[TemplateReplaceKey.programApplicationVersionRevision] = Constants.ApplicationVersionRevision;
        }

        #endregion

        #region T4TemplateProcessor

        protected override void Initialize()
        {
            base.Initialize();

            NamespaceName = "ContentTypeTextNet.Pe.PeMain.Logic.ProgramTemplateProcessor.Generator";
            ClassName = "ProgramTemplateProcessor";
            TemplateAppDomainName = "TemplateAppDomain";

            var templateDirective = new[] {
                "<#@ template language=\"C#\" hostspecific=\"true\" {{" + directiveLang + "}} #>",
                "<#",
                "var __host    = (Microsoft.VisualStudio.TextTemplating.ITextTemplatingSessionHost) Host;",
                "var app = (IReadOnlyDictionary<string, object>)__host.Session;",
                "#>",
            };
            TemplateDirective = string.Join(Environment.NewLine, templateDirective);

            FirstLineNumber = templateDirective.Length;
        }

        protected override string MakeTemplateSource()
        {
            var templateSource = new StringBuilder(TemplateSource.Length + 40);

            var map = new Dictionary<string, string>() {
                { directiveLang, string.Empty },
            };
            if(!string.IsNullOrWhiteSpace(CultureCode)) {
                map[directiveLang] = string.Format("culture=\"{0}\"", CultureCode);
            }
            var templateDirective = TemplateDirective.ReplaceRangeFromDictionary("{{", "}}", map);

            templateSource.AppendLine(templateDirective);
            templateSource.Append(base.MakeTemplateSource());

            return templateSource.ToString();
        }

        protected override string TransformText_Impl()
        {
            ResetVariable();

            return base.TransformText_Impl();
        }

        #endregion

        #region IHasNonProcess

        public INonProcess NonProcess { get; private set; }

        #endregion
    }
}
