using System.Diagnostics.CodeAnalysis;
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
        /// </summary>
        /// <remarks>
        /// <para>どんな時にできないのかは知らん。</para>
        /// </remarks>
        /// <param name="freezable"></param>
        /// <returns></returns>
        public static bool SafeFreeze(Freezable? freezable)
        {
            if(freezable == null) {
                return false;
            }

            var result = freezable.CanFreeze;
            if(result) {
                freezable.Freeze();
            }

            return result;
        }

        /// <summary>
        /// 引数のオブジェクトに対して<see cref="SafeFreeze(Freezable?)"/>を実施したオブジェクトをそのまま返す。
        /// </summary>
        /// <code>
        /// var brush = FreezableUtility.GetSafeFreeze(new SolidColorBrush(Colors.Red));
        /// </code>
        /// <typeparam name="TFreezable"></typeparam>
        /// <param name="freezable"></param>
        /// <returns></returns>
        [return: NotNullIfNotNull("freezable")]
        public static TFreezable? GetSafeFreeze<TFreezable>(TFreezable? freezable)
            where TFreezable : Freezable
        {
            SafeFreeze(freezable);
            return freezable;
        }
    }
}
