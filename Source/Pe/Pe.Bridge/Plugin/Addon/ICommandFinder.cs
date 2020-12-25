using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;

namespace ContentTypeTextNet.Pe.Bridge.Plugin.Addon
{
    public interface ICommandFinder: IDisposable
    {
        #region property

        bool IsInitialize { get; }

        #endregion

        #region function

        void Initialize();

        /// <summary>
        /// 現在状態を更新。
        /// </summary>
        void Refresh(IPluginContext pluginContext);

        IEnumerable<ICommandItem> EnumerateCommandItems(string inputValue, Regex inputRegex, IHitValuesCreator hitValuesCreator, CancellationToken cancellationToken);

        #endregion
    }
}
