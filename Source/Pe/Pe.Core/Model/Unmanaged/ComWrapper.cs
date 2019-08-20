using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace ContentTypeTextNet.Pe.Core.Model.Unmanaged
{
    /// <summary>
    /// 生のCOMを管理。
    /// </summary>
    public class ComWrapper<T> : UnmanagedModelBase<T>
    {
        public ComWrapper(T comObject)
            : base(comObject)
        {
            if(!Marshal.IsComObject(Com!)) {
                throw new ArgumentException(nameof(comObject));
            }
        }

        #region property

        public T Com => Raw;

        #endregion

        #region function

        #endregion

        #region UnmanagedModelBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                Marshal.ReleaseComObject(Raw!);
            }

            base.Dispose(disposing);
        }

        #endregion
    }

    public static class ComWrapper
    {
        public static ComWrapper<T> Create<T>(T comObject)
        {
            return new ComWrapper<T>(comObject);
        }
    }
}
