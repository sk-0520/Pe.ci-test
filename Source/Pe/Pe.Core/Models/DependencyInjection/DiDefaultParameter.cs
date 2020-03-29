using System;
using System.Collections.Generic;
using System.Text;

namespace ContentTypeTextNet.Pe.Core.Models.DependencyInjection
{
    /// <summary>
    /// パラメータに型判別できない(default(T)とか)を無理やり認識させるしゃあなし対応。
    /// </summary>
    public struct DiDefaultParameter
    {
        public DiDefaultParameter(Type type)
        {
            Type = type;
        }

        #region property

        public Type Type { get; }

        #endregion

        #region function

        public KeyValuePair<Type, object> GetPair()
        {
            if(Type.IsValueType) {
#pragma warning disable CS8604 // Null 参照引数の可能性があります。
                return new KeyValuePair<Type, object>(Type, Activator.CreateInstance(Type));
#pragma warning restore CS8604 // Null 参照引数の可能性があります。
            }

#pragma warning disable CS8625 // null リテラルを null 非許容参照型に変換できません。
            return new KeyValuePair<Type, object>(Type, null);
#pragma warning restore CS8625 // null リテラルを null 非許容参照型に変換できません。
        }

        public static DiDefaultParameter Create<T>()
        {
            return new DiDefaultParameter(typeof(T));
        }

        #endregion
    }

}
