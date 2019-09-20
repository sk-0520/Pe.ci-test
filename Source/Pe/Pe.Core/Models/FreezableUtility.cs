using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace ContentTypeTextNet.Pe.Core.Models
{
    /// <summary>
    /// <see cref="Freezable"/>に対するあれ。
    /// </summary>
    public static class FreezableUtility
    {
        /// <summary>
        /// 安全に<see cref="Freezable.Freeze()"/>する。
        /// <para>どんな時にできないのかは知らん。</para>
        /// </summary>
        /// <param name="freezable"></param>
        /// <returns></returns>
        public static bool SafeFreeze(Freezable freezable)
        {
            var result = freezable.CanFreeze;
            if(result) {
                freezable.Freeze();
            }

            return result;
        }

        public static TFreezable GetSafeFreeze<TFreezable>(TFreezable freezable)
            where TFreezable : Freezable
        {
            SafeFreeze(freezable);
            return freezable;
        }
    }
}
