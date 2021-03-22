using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace ContentTypeTextNet.Pe.Core.Models.Unmanaged
{
    /// <summary>
    /// 生のCOMを管理。
    /// </summary>
    public class ComWrapper<T>: UnmanagedWrapperBase<T>
        where T : class
    {
        public ComWrapper(T comObject)
            : base(comObject)
        {
            Debug.Assert(Raw != null);

            if(!Marshal.IsComObject(Raw)) {
                throw new ArgumentException(nameof(comObject));
            }
        }

        #region function

        public ComWrapper<TCastType> Cast<TCastType>()
            where TCastType : class
        {
            var castValue = (TCastType)BaseRawObject;
            if(castValue == null) {
                throw new InvalidCastException($"{typeof(T).Name} -> {typeof(TCastType).Name}");
            }

            return new ComWrapper<TCastType>(castValue);
        }

        #endregion

        #region UnmanagedModelBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                Marshal.ReleaseComObject(Raw);
            }

            base.Dispose(disposing);
        }

        #endregion
    }

    public static class ComWrapper
    {
        public static ComWrapper<T> Create<T>(T comObject)
            where T : class
        {
            return new ComWrapper<T>(comObject);
        }
    }
}
