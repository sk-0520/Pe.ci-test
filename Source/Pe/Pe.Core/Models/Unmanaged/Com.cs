using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using ContentTypeTextNet.Pe.Library.Base;

namespace ContentTypeTextNet.Pe.Core.Models.Unmanaged
{
    /// <summary>
    /// 生のCOMを管理。
    /// </summary>
    public class Com<T>: DisposerBase
        where T : class
    {
        public Com(T comInstance)
        {
            Debug.Assert(comInstance != null);

            if(!Marshal.IsComObject(comInstance)) {
                throw new ArgumentException("Marshal.IsComObject", nameof(comInstance));
            }

            Instance = comInstance;
        }

        #region property

        private object RawInstance => Instance;

        public T Instance { get; }

        #endregion

        #region function

        public Com<TCastType> Cast<TCastType>()
            where TCastType : class
        {
            var castValue = (TCastType)RawInstance;
            if(castValue == null) {
                throw new InvalidCastException($"{typeof(T).Name} -> {typeof(TCastType).Name}");
            }

            return new Com<TCastType>(castValue);
        }

        #endregion

        #region DisposerBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(Instance is not null) {
                    Marshal.ReleaseComObject(Instance);
                }
            }

            base.Dispose(disposing);
        }

        #endregion
    }

    public static class ComWrapper
    {
        public static Com<T> Create<T>(T comObject)
            where T : class
        {
            return new Com<T>(comObject);
        }
    }
}
