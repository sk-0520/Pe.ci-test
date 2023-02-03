using System;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.DependencyInjection;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element
{

    /// <summary>
    /// DataContext にあてる VM のモデルになる基底クラス。
    /// </summary>
    public abstract class ElementBase: BindModelBase
    {
        #region variable

        private bool _isInitialized;

        #endregion
        protected ElementBase(ILoggerFactory loggerFactory)
            : base(loggerFactory)
        { }

        #region property

        public bool IsInitialized
        {
            get => this._isInitialized;
            private set => SetProperty(ref this._isInitialized, value);
        }

        #endregion

        #region function

        protected abstract void InitializeImpl();

        public void Initialize()
        {
            if(IsInitialized) {
                throw new InvalidOperationException(nameof(IsInitialized));
            }

            ThrowIfDisposed();

            InitializeImpl();

            IsInitialized = true;
        }

        #endregion
    }

    /// <summary>
    /// <see cref="ElementBase"/>と同じだけど結構な長寿で DI コンテナを持ち運ぶ気持ち最上位なモデル。
    /// <para>わっけ分からんくなりそうなので極力使用しないこと！</para>
    /// </summary>
    public abstract class ServiceLocatorElementBase: ElementBase
    {
        protected ServiceLocatorElementBase(IDiContainer diContainer, ILoggerFactory loggerFactory)
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

            //childServiceLocator.RegisterLogger(LoggerFactory);

            return childServiceLocator;
        }

        #endregion
    }
}
