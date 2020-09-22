using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ContentTypeTextNet.Pe.Main.Models.Platform;
using Microsoft.Extensions.Configuration;

namespace ContentTypeTextNet.Pe.Main.Models.Applications.Configuration
{
    public class PlatformConfiguration: ConfigurationBase
    {
        #region define

        public class PlatformFullscreenConfiguration: ConfigurationBase
        {
            public PlatformFullscreenConfiguration(IConfigurationSection section)
                : base(section)
            {
                //IgnoreWindowClasses = section.GetSection("ignore_window_class").Get<string[]>();

                var classTextSection = section.GetSection("ignore_window_class_text");
                IgnoreClassAndTexts = classTextSection.GetChildren().Select(i => new ClassAndText(i.GetValue<string>("class"), i.GetValue<string>("text"))).ToArray();

                //TopmostOnly = section.GetValue<bool>("topmost_only");
                //ExcludeNoActive = section.GetValue<bool>("exclude_noactive");
                //ExcludeToolWindow = section.GetValue<bool>("exclude_toolwindow");
            }

            #region proeprty

            [Configuration("ignore_window_class")]
            public IReadOnlyList<string> IgnoreWindowClasses { get; } = default!;
            public IReadOnlyList<ClassAndText> IgnoreClassAndTexts { get; }

            [Configuration]
            public bool TopmostOnly { get; }
            [Configuration("exclude_noactive")]
            public bool ExcludeNoActive { get; }
            [Configuration("exclude_toolwindow")]
            public bool ExcludeToolWindow { get; }

            #endregion
        }

        #endregion

        public PlatformConfiguration(IConfigurationSection section)
            : base(section)
        {
            //ThemeAccentColorMinimumAlpha = section.GetValue<byte>("theme_accent_color_minimum_alpha");
            //ThemeAccentColorDefaultAlpha = section.GetValue<byte>("theme_accent_color_default_alpha");

            //ExplorerSupporterRefreshTime = section.GetValue<TimeSpan>("explorer_supporter_refresh_time");
            //ExplorerSupporterCacheSize = section.GetValue<int>("explorer_supporter_cache_size");

            //ScreenElementsResetWaitTime = section.GetValue<TimeSpan>("screen_elements_reset_wait_time");

            //Fullscreen = new PlatformFullscreenConfiguration(section.GetSection("fullscreen"));
        }

        #region property

        /// <summary>
        /// アクセントカラーの透明度を無効と判断する最低A値。
        /// <para>この値未満であれば無効。</para>
        /// </summary>
        [Configuration]
        public byte ThemeAccentColorMinimumAlpha { get; }
        /// <summary>
        /// アクセントカラーの透明度が<see cref="ThemeAccentColorMinimumAlpha"/>で無効判定なら使用するA値。
        /// </summary>
        [Configuration]
        public byte ThemeAccentColorDefaultAlpha { get; }
        [Configuration]
        public TimeSpan ExplorerSupporterRefreshTime { get; }
        [Configuration]
        public int ExplorerSupporterCacheSize { get; }

        [Configuration]
        public TimeSpan ScreenElementsResetWaitTime { get; }

        [Configuration]
        public PlatformFullscreenConfiguration Fullscreen { get; } = default!;

        #endregion
    }
}
