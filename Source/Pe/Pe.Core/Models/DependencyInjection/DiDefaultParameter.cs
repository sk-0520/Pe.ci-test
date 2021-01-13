using System;
using System.Collections.Generic;
using System.Text;

namespace ContentTypeTextNet.Pe.Core.Models.DependencyInjection
{
    /// <summary>
    /// パラメータに型判別できない(default(T)とか)を無理やり認識させるしゃあなし対応。
    /// </summary>
    public readonly struct DiDefaultParameter
    {
        public DiDefaultParameter(Type type)
        {
            Type = type;
        }

        #region property

        public Type Type { get; }

        #endregion

        #region function

        public KeyValuePair<Type, object?> GetPair()
        {
            if(Type.IsValueType) {
                return KeyValuePair.Create(Type, Activator.CreateInstance(Type));
            }

            return new KeyValuePair<Type, object?>(Type, null);
        }

        public static DiDefaultParameter Create<T>()
        {
            return new DiDefaultParameter(typeof(T));
        }

        #endregion
    }

}
