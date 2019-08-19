using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;

namespace ContentTypeTextNet.Pe.Main.Model.Manager
{
    /// <summary>
    /// クリップボード操作用。
    /// <para>もっかしたら<see cref="IOrderManager"/>で完結するかも。</para>
    /// </summary>
    public interface IClipboardManager
    {
        #region function

        bool Set(IDataObject data);

        #endregion
    }

    public class ClipboardManager : ManagerBase, IClipboardManager
    {
        public ClipboardManager(IDiContainer diContainer, ILoggerFactory loggerFactory)
            : base(diContainer, loggerFactory)
        { }

        #region property
        #endregion

        #region function
        #endregion

        #region IClipboardManager

        public bool Set(IDataObject data)
        {
            if(data == null) {
                throw new ArgumentNullException(nameof(data));
            }

            try {
                Clipboard.SetDataObject(data);
                return true;
            } catch(Exception ex) {
                Logger.Error(ex);
                return false;
            }
        }

        #endregion
    }
}
