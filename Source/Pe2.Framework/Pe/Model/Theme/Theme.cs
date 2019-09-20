using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Main.Model.Theme
{
    public interface IReadOnlyColorPair<out T>
    {
        #region property

        T Foreground { get; }
        T Background { get; }

        #endregion
    }

    internal class ColorPair<T>: IReadOnlyColorPair<T>
    {
        public ColorPair(T foreground, T background)
        {
            Foreground = foreground;
            Background = background;
        }

        #region property

        public T Foreground { get; }
        public T Background { get; }

        #endregion
    }

    internal static class ColorPair
    {
        #region function
        public static ColorPair<T> Create<T>(T foreground, T background)
        {
            return new ColorPair<T>(foreground, background);
        }

        #endregion
    }

}
