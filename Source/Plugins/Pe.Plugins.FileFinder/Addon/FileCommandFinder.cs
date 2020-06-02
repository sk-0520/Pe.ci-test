using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Plugins.FileFinder.Addon
{
    /// <summary>
    /// 入力からファイル検索を行う。
    /// <para>もうちと簡単にやるなら Pe.Core から IconLoader を参照するべきかなぁ。</para>
    /// </summary>
    internal class FileCommandFinder: ICommandFinder, IDisposable
    {
        public FileCommandFinder(IAddonParameter parameter)
        {
            Logger = parameter.LoggerFactory.CreateLogger(GetType());
            DispatcherWrapper = parameter.DispatcherWrapper;
        }

        #region property

        ILogger Logger { get; }
        IDispatcherWrapper DispatcherWrapper { get; }

        #endregion

        #region ICommandFinder

        /// <inheritdoc cref="ICommandFinder.IsInitialize"/>
        public bool IsInitialize { get; private set; }

        /// <inheritdoc cref="ICommandFinder.Initialize"/>
        public void Initialize()
        {
            if(IsInitialize) {
                throw new InvalidOperationException(nameof(IsInitialize));
            }

            IsInitialize = true;
        }

        /// <inheritdoc cref="ICommandFinder.ListupCommandItems(string, Regex, IHitValuesCreator, CancellationToken)"/>
        public IEnumerable<ICommandItem> ListupCommandItems(string inputValue, Regex inputRegex, IHitValuesCreator hitValuesCreator, CancellationToken cancellationToken)
        {
            yield break;
        }

        /// <inheritdoc cref="ICommandFinder.Refresh"/>
        public void Refresh()
        { }

        #endregion

        #region IDisposable

        private bool disposedValue;
        protected virtual void Dispose(bool disposing)
        {
            if(!this.disposedValue) {
                if(disposing) {
                    // TODO: マネージド状態を破棄します (マネージド オブジェクト)
                }

                // TODO: アンマネージド リソース (アンマネージド オブジェクト) を解放し、ファイナライザーをオーバーライドします
                // TODO: 大きなフィールドを null に設定します
                this.disposedValue = true;
            }
        }

        // // TODO: 'Dispose(bool disposing)' にアンマネージド リソースを解放するコードが含まれる場合にのみ、ファイナライザーをオーバーライドします
        // ~FileCommandFinder()
        // {
        //     // このコードを変更しないでください。クリーンアップ コードを 'Dispose(bool disposing)' メソッドに記述します
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // このコードを変更しないでください。クリーンアップ コードを 'Dispose(bool disposing)' メソッドに記述します
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
