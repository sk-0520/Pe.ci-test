using System;
using System.Collections.Generic;
using System.Text;

namespace ContentTypeTextNet.Pe.Library.DependencyInjection
{
    /// <summary>
    /// DI を通した Factory パターンの基底。
    /// </summary>
    public abstract class DiObjectFactoryBase
    {
        protected DiObjectFactoryBase(IDiContainer diContainer)
        {
            DiContainer = diContainer;
        }

        #region property

        protected IDiContainer DiContainer { get; }

        #endregion
    }
}
