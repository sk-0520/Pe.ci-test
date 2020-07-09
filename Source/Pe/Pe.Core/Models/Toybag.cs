using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Threading;

namespace ContentTypeTextNet.Pe.Core.Models
{
    internal abstract class ToybagBase<TResult>: IDisposable
    {
        protected ToybagBase(ManualResetEventSlim resetEvent, CancellationToken cancellationToken)
        {
            Event = resetEvent;
            CancellationToken = cancellationToken;
        }

        #region property

        public ManualResetEventSlim Event { get; }
        public CancellationToken CancellationToken { get; }

        [MaybeNull]
        public TResult Result { get; set; } = default!;

        #endregion

        #region IDisposable

        private bool _disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if(!this._disposedValue) {
                if(disposing) {
                    Event.Dispose();
                }

                // TODO: アンマネージド リソース (アンマネージド オブジェクト) を解放し、ファイナライザーをオーバーライドします
                // TODO: 大きなフィールドを null に設定します
                this._disposedValue = true;
            }
        }

        public void Dispose()
        {
            // このコードを変更しないでください。クリーンアップ コードを 'Dispose(bool disposing)' メソッドに記述します
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }

    internal abstract class ToybagBase<TParameter, TResult>: ToybagBase<TResult>
    {
        protected ToybagBase(ManualResetEventSlim resetEvent, TParameter parameter, CancellationToken cancellationToken)
            : base(resetEvent, cancellationToken)
        {
            Parameter = parameter;
        }

        #region property

        public TParameter Parameter { get; }

        #endregion
    }

    internal class Toybag<TResult>: ToybagBase<TResult>
    {
        public Toybag(Func<TResult> method, ManualResetEventSlim resetEvent, CancellationToken cancellationToken) : base(resetEvent, cancellationToken)
        {
            Method = method;
        }

        #region property

        public Func<TResult> Method { get; }

        #endregion
    }

    internal class Toybag<TParameter, TResult>: ToybagBase<TParameter, TResult>
    {
        public Toybag(Func<TParameter, TResult> method, ManualResetEventSlim resetEvent, TParameter parameter, CancellationToken cancellationToken)
            : base(resetEvent, parameter, cancellationToken)
        {
            Method = method;
        }

        #region property

        public Func<TParameter, TResult> Method { get; }

        #endregion
    }

}
