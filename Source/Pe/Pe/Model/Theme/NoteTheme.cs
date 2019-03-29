using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;

namespace ContentTypeTextNet.Pe.Main.Model.Theme
{
    public interface INoteTheme
    {
        #region function
        #endregion
    }

    public class NoteTheme : ThemeBase, INoteTheme
    {
        public NoteTheme(IDispatcherWapper dispatcherWapper, ILoggerFactory loggerFactory)
            : base(dispatcherWapper, loggerFactory)
        { }

        #region property
        #endregion

        #region function
        #endregion

        #region INoteTheme
        #endregion
    }
}
