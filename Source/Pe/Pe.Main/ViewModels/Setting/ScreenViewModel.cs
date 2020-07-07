using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Core.Compatibility.Forms;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Setting
{
    public class ScreenViewModel : ViewModelBase
    {
        #region variable

        Color _foregroundColor;
        Color _backgroundColor;

        #endregion
        public ScreenViewModel(Screen screen, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            Screen = screen;

            byte alpha = 180;
            if(Screen.Primary) {
               BackgroundColor  = Color.FromArgb(alpha, 0xff, 0xff, 0xff);
            } else {
                var rand = new Random(screen.DeviceName.GetHashCode());
                BackgroundColor = Color.FromArgb(
                    alpha,
                    (byte)rand.Next(0x00, 0xff),
                    (byte)rand.Next(0x00, 0xff),
                    (byte)rand.Next(0x00, 0xff)
                );
            }
            ForegroundColor = MediaUtility.GetAutoColor(BackgroundColor);
        }

        #region property

        public Screen Screen { get; }

        public Color ForegroundColor {
            get => this._foregroundColor;
            set => SetProperty(ref this._foregroundColor, value);
        }

        public Color BackgroundColor {
            get => this._backgroundColor;
            set => SetProperty(ref this._backgroundColor, value);
        }

        public string ScreenDisplayName
        {
            get
            {
                return ScreenUtility.GetName(Screen, LoggerFactory);
            }
        }
        public string ScreenDeviceName => Screen.DeviceName;
        public bool IsPrimaryScreen => Screen.Primary;
        #endregion
    }
}
