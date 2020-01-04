using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin.Theme;
using ContentTypeTextNet.Pe.Core.Models;

namespace ContentTypeTextNet.Pe.Plugins.DefaultTheme.Theme
{
    internal class CommandTheme : ThemeBase, ICommandTheme
    {
        public CommandTheme(IThemeParameter parameter)
            : base(parameter)
        { }

        #region ICommandTheme

        public Brush GetGripBrush(bool isActive)
        {
            //var color = PlatformTheme.GetTaskbarColor();
            //var fore = MediaUtility.GetAutoColor(color);
            //return new SolidColorBrush(fore);
            return (Brush)Application.Current.Resources["ICommandTheme-GetGripBrush"];
        }

        [return: PixelKind(Px.Logical)]
        public double GetGripWidth()
        {
            return 8;
        }

        public Thickness GetSelectedIconMargin(IconBox iconBox) => new Thickness(1);

        public Thickness GetInputBorderThickness()
        {
            return new Thickness(2);
        }

        public Brush GetInputBorderBrush(InputState inputState)
        {
            return inputState switch
            {
                InputState.Empty => Brushes.AliceBlue,
                InputState.Finding => Brushes.Lime,
                InputState.Listup => Brushes.Yellow,
                InputState.NotFound => Brushes.Red,
                _ => throw new NotImplementedException(),
            };
        }

        public Brush GetInputBackground(InputState inputState)
        {
            return Brushes.White;
        }

        public Brush GetInputForeground(InputState inputState)
        {
            return Brushes.Black;
        }

        public Brush GetViewBackgroundBrush(bool isActive)
        {
            var color = PlatformTheme.GetTaskbarColor();
            return new SolidColorBrush(color);
        }

        public Thickness GetViewBorderThickness()
        {
            return new Thickness(2);
        }

        public Brush GetViewBorderBrush(bool isActive)
        {
            var color = PlatformTheme.GetTaskbarColor();
            color.A = (byte)(isActive ? 0xff : 0x80);
            return new SolidColorBrush(color);
        }

        public ControlTemplate GetExecuteButtonControlTemplate(IconBox icon)
        {
            return (ControlTemplate)Application.Current.Resources["ICommandTheme-ExecuteButton"];
        }

        #endregion
    }
}
