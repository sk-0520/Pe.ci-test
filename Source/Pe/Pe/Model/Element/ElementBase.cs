using System;
using System.Collections.Generic;
using System.ComponentModel;
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
            : base(loggerFactory)
        { }
    }

    /// <summary>
    /// <see cref="ElementBase"/>と同じだけど結構な長寿で DI コンテナを持ち運ぶ気持ち最上位なモデル。
    /// </summary>
    public abstract class ContextElementBase: ElementBase
    {
        public ContextElementBase(IDiContainer diContainer, ILogger logger)
            : base(logger)
        {
            ServiceLocator = diContainer;
        }

        public ContextElementBase(IDiContainer diContainer, ILoggerFactory loggerFactory)
            : this(diContainer, loggerFactory.CreateCurrentClass())
        { }

        #region property

        protected IDiContainer ServiceLocator { get; }

        #endregion
    }

}
