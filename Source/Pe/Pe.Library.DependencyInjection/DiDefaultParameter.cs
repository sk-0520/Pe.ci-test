using System;
using System.Collections.Generic;

namespace ContentTypeTextNet.Pe.Library.DependencyInjection
{
    /// <summary>
    /// パラメータに型判別できない(<c>default(T)</c>とか)を無理やり認識させるしゃあなし対応。
    /// </summary>
    public readonly struct DiDefaultParameter
    {
        public DiDefaultParameter(Type type)
        {
            Type = type;
        }

        #region property

        /// <summary>
        /// 対象の型。
        /// </summary>
        public Type Type { get; }

        #endregion

        #region function

        /// <summary>
        /// 型と初期データを生成。
        /// </summary>
        /// <returns></returns>
        public KeyValuePair<Type, object?> GetPair()
        {
            if(Type.IsValueType) {
                var instance = Activator.CreateInstance(Type);
                return new KeyValuePair<Type, object?>(Type, instance);
            }

            return new KeyValuePair<Type, object?>(Type, null);
        }

        /// <summary>
        /// <see cref="DiDefaultParameter"/>を指定型で生成。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>生成した<see cref="DiDefaultParameter"/>。</returns>
        public static DiDefaultParameter Create<T>()
        {
            return new DiDefaultParameter(typeof(T));
        }

        #endregion
    }
}
