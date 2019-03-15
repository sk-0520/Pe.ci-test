using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.View.Extend;

namespace ContentTypeTextNet.Pe.Main.Model.Logic.Designer
{
    public interface ILauncherToolbarDesigner
    {
        #region function

        [return: PixelKind(Px.Logical)]
        Thickness GetButtonPadding(AppDesktopToolbarPosition toolbarPosition, IconScale iconScale);
        [return: PixelKind(Px.Logical)]
        Thickness GetIconMargin(AppDesktopToolbarPosition toolbarPosition, IconScale iconScale, bool isIconOnly, [PixelKind(Px.Logical)] double textWidth);

        [return: PixelKind(Px.Logical)]
        Size GetDisplaySize([PixelKind(Px.Logical)] Thickness buttonPadding, [PixelKind(Px.Logical)] Thickness iconMargin, IconScale iconScale, bool isIconOnly, [PixelKind(Px.Logical)] double textWidth);
        [return: PixelKind(Px.Logical)]
        Size GetHiddenSize([PixelKind(Px.Logical)] Thickness buttonPadding, [PixelKind(Px.Logical)] Thickness iconMargin, IconScale iconScale, bool isIconOnly, [PixelKind(Px.Logical)] double textWidth);

        #endregion
    }

    public class LauncherToolbarDesigner : DesignerBase, ILauncherToolbarDesigner
    {
        public LauncherToolbarDesigner(ILoggerFactory loggerFactory)
            : base(loggerFactory)
        { }

        #region ILauncherToolbarDesigner

        [return: PixelKind(Px.Logical)]
        public Thickness GetIconMargin(AppDesktopToolbarPosition toolbarPosition, IconScale iconScale, bool isIconOnly, [PixelKind(Px.Logical)] double textWidth)
        {
            return new Thickness(2);
        }

        [return: PixelKind(Px.Logical)]
        public Thickness GetButtonPadding(AppDesktopToolbarPosition toolbarPosition, IconScale iconScale)
        {
            return new Thickness(2);
        }

        [return: PixelKind(Px.Logical)]
        public Size GetDisplaySize([PixelKind(Px.Logical)] Thickness buttonPadding, [PixelKind(Px.Logical)] Thickness iconMargin, IconScale iconScale, bool isIconOnly, [PixelKind(Px.Logical)] double textWidth)
        {
            return new Size(
                GetHorizontal(buttonPadding) + GetHorizontal(iconMargin) + (int)iconScale + (isIconOnly ? 0 : textWidth),
                GetVertical(buttonPadding) + GetVertical(iconMargin) + (int)iconScale
            );
        }

        [return: PixelKind(Px.Logical)]
        public Size GetHiddenSize([PixelKind(Px.Logical)] Thickness buttonPadding, [PixelKind(Px.Logical)] Thickness iconMargin, IconScale iconScale, bool isIconOnly, [PixelKind(Px.Logical)] double textWidth)
        {
            return new Size(4, 4);
        }


        #endregion
    }
}
