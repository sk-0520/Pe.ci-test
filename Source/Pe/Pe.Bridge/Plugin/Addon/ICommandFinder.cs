using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;

namespace ContentTypeTextNet.Pe.Bridge.Plugin.Addon
{
    /// <summary>
    /// コマンド検索。
    /// </summary>
    public interface ICommandFinder: IDisposable
    {
        #region property

        /// <summary>
        /// 初期化済みか。
        /// </summary>
        bool IsInitialized { get; }

        #endregion

        #region function

        /// <summary>
        /// 初期化処理。
        /// </summary>
        void Initialize();

        /// <summary>
        /// 現在状態を更新。
        /// </summary>
        void Refresh(IPluginContext pluginContext);

        /// <summary>
        /// 入力から<see cref="ICommandItem"/>を取得する。
        /// </summary>
        /// <param name="inputValue"></param>
        /// <param name="inputRegex"></param>
        /// <param name="hitValuesCreator"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        IAsyncEnumerable<ICommandItem> EnumerateCommandItemsAsync(string inputValue, Regex inputRegex, IHitValuesCreator hitValuesCreator, CancellationToken cancellationToken);

        #endregion
    }
}
