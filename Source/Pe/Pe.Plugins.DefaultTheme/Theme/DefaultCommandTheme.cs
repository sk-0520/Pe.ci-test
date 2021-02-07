using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin.Theme;
using ContentTypeTextNet.Pe.Core.Models;

namespace ContentTypeTextNet.Pe.Plugins.DefaultTheme.Theme
{
    internal class DefaultCommandTheme: DefaultThemeBase, ICommandTheme
    {
        public DefaultCommandTheme(IThemeParameter parameter)
            : base(parameter)
        { }

        #region ICommandTheme

        public Brush GetGripBrush(bool isActive)
        {
            //var color = PlatformTheme.GetTaskbarColor();
            //var fore = MediaUtility.GetAutoColor(color);
            //return new SolidColorBrush(fore);
            return GetResourceValue<Brush>(nameof(DefaultCommandTheme), nameof(GetGripBrush));
        }

        [return: PixelKind(Px.Logical)]
        public double GetGripWidth()
        {
            return 8;
        }

        public Thickness GetSelectedIconMargin(in IconScale iconScale) => new Thickness(1);

        public Thickness GetInputBorderThickness()
        {
            return new Thickness(2);
        }

        public Brush GetInputBorderBrush(InputState inputState)
        {
            var colors = PlatformTheme.GetApplicationThemeColors(PlatformTheme.ApplicationThemeKind);

            return inputState switch {
                InputState.Empty => FreezableUtility.GetSafeFreeze(new SolidColorBrush(colors.Border)),
                InputState.Finding => FreezableUtility.GetSafeFreeze(new SolidColorBrush(colors.Control)),
                InputState.Complete => FreezableUtility.GetSafeFreeze(new SolidColorBrush(PlatformTheme.GetAccentColors(PlatformTheme.AccentColor).Base)),
                InputState.NotFound => FreezableUtility.GetSafeFreeze(new SolidColorBrush(colors.Foreground)),
                _ => throw new NotImplementedException(),
            };
        }

        public Brush GetInputBackground(InputState inputState)
        {
            var colors = PlatformTheme.GetApplicationThemeColors(PlatformTheme.ApplicationThemeKind);
            return FreezableUtility.GetSafeFreeze(new SolidColorBrush(colors.Background));
        }

        public Brush GetInputForeground(InputState inputState)
        {
            var colors = PlatformTheme.GetApplicationThemeColors(PlatformTheme.ApplicationThemeKind);
            return FreezableUtility.GetSafeFreeze(new SolidColorBrush(colors.Foreground));
        }

        public Brush GetViewBackgroundBrush(bool isActive)
        {
            var colors = PlatformTheme.GetTaskbarColor();
            return FreezableUtility.GetSafeFreeze(new SolidColorBrush(colors));
        }

        public Thickness GetViewBorderThickness()
        {
            return new Thickness(2);
        }

        public Brush GetViewBorderBrush(bool isActive)
        {
            var color = PlatformTheme.GetTaskbarColor();
            color.A = (byte)(isActive ? 0xff : 0x80);
            return FreezableUtility.GetSafeFreeze(new SolidColorBrush(color));
        }

        public ControlTemplate GetExecuteButtonControlTemplate(in IconScale iconScale)
        {
            return GetResourceValue<ControlTemplate>(nameof(DefaultCommandTheme), nameof(GetExecuteButtonControlTemplate));
        }


        #endregion
    }
}
