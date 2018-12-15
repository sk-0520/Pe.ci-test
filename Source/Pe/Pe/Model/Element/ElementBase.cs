using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;

namespace ContentTypeTextNet.Pe.Main.Model.Element
{
    /// <summary>
    /// DataContext にあてる VM のモデルになる基底クラス。
    /// </summary>
    public abstract class ElementBase : BindModelBase
    {
        public ElementBase(ILogger logger)
            : base(logger)
        { }
        
        public ElementBase(ILoggerFactory loggerFactory)
            : this(loggerFactory.CreateCurrentClass())
        { }
    }
}
