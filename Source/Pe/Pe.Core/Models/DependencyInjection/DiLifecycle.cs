using System;
using System.Collections.Generic;
using System.Text;

namespace ContentTypeTextNet.Pe.Core.Models.DependencyInjection
{
    /// <summary>
    /// 生成タイミング。
    /// </summary>
    public enum DiLifecycle
    {
        /// <summary>
        /// 毎回作る。
        /// </summary>
        Transient,
        /// <summary>
        /// シングルトン。
        /// </summary>
        Singleton,
    }
}
