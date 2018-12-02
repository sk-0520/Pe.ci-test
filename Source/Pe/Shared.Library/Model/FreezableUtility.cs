using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ContentTypeTextNet.Pe.Library.Shared.Library.Model
{
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
    }
}
