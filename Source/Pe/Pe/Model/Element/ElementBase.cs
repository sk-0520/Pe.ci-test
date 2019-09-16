using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Core.Model;
using ContentTypeTextNet.Pe.Main.Model.Applications;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Model.Element
{
    /// <summary>
    /// DataContext にあてる VM のモデルになる基底クラス。
    /// </summary>
    public abstract class ElementBase : BindModelBase
    {
        public ElementBase(ILoggerFactory loggerFactory)
            : base(loggerFactory)
        { }

        #region function

        protected abstract void InitializeImpl();

        public void Initialize()
        {
            InitializeImpl();
        }

        #endregion
    }

    /// <summary>
    /// <see cref="ElementBase"/>と同じだけど結構な長寿で DI コンテナを持ち運ぶ気持ち最上位なモデル。
    /// <para>わっけ分からんくなりそうなので極力使用しないこと！</para>
    /// </summary>
    public abstract class ContextElementBase : ElementBase
    {
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

            childServiceLocator.RegisterLogger(LoggerFactory);

            return childServiceLocator;
        }

        #endregion
    }
}
