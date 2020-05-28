using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin.Theme;
using ContentTypeTextNet.Pe.Embedded.Abstract;

namespace ContentTypeTextNet.Pe.Plugins.ClassicTheme.Theme
{
    internal class ClassicCommandTheme: ThemeDetailBase, ICommandTheme
    {
        public ClassicCommandTheme(IThemeParameter parameter)
            : base(parameter)
        { }

        #region ICommandTheme

        public Brush GetGripBrush(bool isActive)
        {
            return SystemColors.ActiveCaptionBrush;
        }

        [return: PixelKind(Px.Logical)]
        public double GetGripWidth()
        {
            return 8;
        }

        public Thickness GetSelectedIconMargin(IconBox iconBox) => new Thickness(1);

        public Thickness GetInputBorderThickness()
        {
            return new Thickness(1);
        }

        public Brush GetInputBorderBrush(InputState inputState)
        {
            return SystemColors.ControlDarkDarkBrush;
        }

        public Brush GetInputBackground(InputState inputState)
        {
            return SystemColors.WindowBrush;
        }

        public Brush GetInputForeground(InputState inputState)
        {
            return SystemColors.WindowTextBrush;
        }

        public Brush GetViewBackgroundBrush(bool isActive)
        {
            return SystemColors.ControlBrush;
        }

        public Thickness GetViewBorderThickness()
        {
            return new Thickness(1);
        }

        public Brush GetViewBorderBrush(bool isActive)
        {
            return SystemColors.ControlDarkBrush;
        }

        public ControlTemplate GetExecuteButtonControlTemplate(IconBox icon)
        {
            return GetResourceValue<ControlTemplate>(nameof(ClassicCommandTheme), nameof(GetExecuteButtonControlTemplate));
        }


        #endregion
    }
}
