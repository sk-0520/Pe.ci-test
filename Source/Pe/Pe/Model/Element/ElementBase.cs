using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Applications;

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
    /// <para>わっけ分からんくなりそうなので極力使用しないこと！</para>
    /// </summary>
    public abstract class ContextElementBase : ElementBase, ILoggerFactory
    {
        public ContextElementBase(IDiContainer diContainer, ILogger logger)
            : base(logger)
        {
            ServiceLocator = diContainer;
        }

        public ContextElementBase(IDiContainer diContainer, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            ServiceLocator = diContainer;
        }

        #region property

        protected IDiContainer ServiceLocator { get; }

        #endregion

        #region function

        protected IScopeDiContainer UsingChildServiceLocator()
        {
            var childServiceLocator = ServiceLocator.CreateChildContainer();

            childServiceLocator.RegisterLogger(Logger);

            return childServiceLocator;
        }


        #endregion

        #region ILoggerFactory

        public ILogger CreateLogger(string header) => Logger.Factory.CreateLogger(header);

        #endregion
    }

}
