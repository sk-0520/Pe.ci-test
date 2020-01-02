using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Plugin.Theme;

namespace ContentTypeTextNet.Pe.Plugins.DefaultTheme.Theme
{
    internal class CommandTheme : ThemeBase, ICommandTheme
    {
        public CommandTheme(IThemeParameter parameter)
            : base(parameter)
        { }

        #region ICommandTheme

        public DependencyObject GetExecuteButton()
        {
            throw new NotImplementedException();
        }

        public Brush GetGripBrush(bool isActive)
        {
            throw new NotImplementedException();
        }

        [return: PixelKind(Px.Logical)]
        public double GetGripWidth()
        {
            throw new NotImplementedException();
        }

        public Brush GetInputBackground(InputState inputState)
        {
            throw new NotImplementedException();
        }

        public Border GetInputBorder(InputState inputState)
        {
            throw new NotImplementedException();
        }

        public Brush GetInputForeground(InputState inputState)
        {
            throw new NotImplementedException();
        }

        public Brush GetViewBackgroundBrush(bool isActive)
        {
            throw new NotImplementedException();
        }

        public Border GetViewBorder(bool isActive)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
