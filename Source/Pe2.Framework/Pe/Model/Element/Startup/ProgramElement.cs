using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;

namespace ContentTypeTextNet.Pe.Main.Model.Element.Startup
{
    public class ProgramElement : ElementBase
    {
        public ProgramElement(FileInfo fileInfo, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            FileInfo = fileInfo;
        }

        #region property

        public FileInfo FileInfo { get; }
        public bool IsImport { get; set; }

        #endregion

        #region ContextElementBase

        protected override void InitializeImpl()
        {
            Logger.Trace("not impl");
        }

        #endregion
    }
}
