using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;

namespace ContentTypeTextNet.Pe.Main.Model.ViewElement
{
    /// <summary>
    /// ビューの DataContext になる基底クラス。
    /// </summary>
    public abstract class ViewElementBase : BindModelBase
    {
        public ViewElementBase(ILogger logger)
            : base(logger)
        { }
    }
}
