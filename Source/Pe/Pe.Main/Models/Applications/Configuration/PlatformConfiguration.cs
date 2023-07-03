using System;
using System.Collections.Generic;
using ContentTypeTextNet.Pe.Main.Models.Platform;
using Microsoft.Extensions.Configuration;

namespace ContentTypeTextNet.Pe.Main.Models.Applications.Configuration
{
    /// <summary>
    /// アプリケーション構成: 実行環境。
    /// </summary>
    public class PlatformConfiguration: ConfigurationBase
    {
        #region define

        public class PlatformFullscreenConfiguration: ConfigurationBase
        {
            public PlatformFullscreenConfiguration(IConfigurationSection section)
                : base(section)
            { }

            #region property

            [Configuration("ignore_window_class")]
            public IReadOnlyList<string> IgnoreWindowClasses { get; } = default!;
            [Configuration("ignore_window_class_text", nestConvertMethodName: nameof(ConvertClassAndText))]
            public IReadOnlyList<ClassAndText> IgnoreClassAndTexts { get; } = default!;

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
        /// <summary>
        /// エクスプローラ 補正 インターバル。
        /// </summary>
        [Configuration]
        public TimeSpan ExplorerSupporterRefreshTime { get; }
        /// <summary>
        /// エクスプローラ 補正 インターバル。
        /// </summary>
        [Configuration]
        public int ExplorerSupporterCacheSize { get; }
        /// <summary>
        /// エクスプローラ 補正 キュー。
        /// </summary>
        [Configuration]
        public TimeSpan ScreenElementsResetWaitTime { get; }

        /// <summary>
        /// アイドル抑制実施時間間隔。
        /// <para>前回抑制時間を超過している場合に抑制実施。</para>
        /// </summary>
        [Configuration]
        public TimeSpan IdleDisableCycleTime { get; }
        /// <summary>
        /// <see cref="IdleDisableCycleTime"/>の確認周期。
        /// </summary>
        /// NOTE: 正直 <see cref="IdleDisableCycleTime"/> 一個だけでよかったと思ってる。
        [Configuration]
        public TimeSpan IdleCheckCycleTime { get; }

        [Configuration]
        public PlatformFullscreenConfiguration Fullscreen { get; } = default!;

        #endregion
    }
}
