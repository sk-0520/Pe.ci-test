using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Theme;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin.Theme
{
    public class ThemeContainer
    {
        public ThemeContainer(ILoggerFactory loggerFactory)
        {
            LoggerFactory = loggerFactory;
            Logger = LoggerFactory.CreateLogger(GetType());
        }

        #region property

        ILogger Logger { get; }
        ILoggerFactory LoggerFactory { get; }

        ISet<ITheme> Themes { get; } = new HashSet<ITheme>();

        #endregion

        #region function

        public void Add(ITheme theme)
        {
            Themes.Add(theme);
        }

        public ILauncherGroupTheme GetLauncherGroupTheme()
        {
            throw new NotImplementedException();
        }

        public ILauncherToolbarTheme GetLauncherToolbarTheme()
        {
            throw new NotImplementedException();
        }

        public INoteTheme GetNoteTheme()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
