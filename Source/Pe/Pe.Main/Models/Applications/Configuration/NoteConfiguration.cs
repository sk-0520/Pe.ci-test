using System;
using System.Windows;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Library.Base;
using Microsoft.Extensions.Configuration;

namespace ContentTypeTextNet.Pe.Main.Models.Applications.Configuration
{
    /// <summary>
    /// アプリケーション構成: ノート。
    /// </summary>
    public class NoteConfiguration: ConfigurationBase
    {
        public NoteConfiguration(IConfigurationSection section)
            : base(section)
        { }

        #region property

        /// <summary>
        /// 移動時の透明度。
        /// </summary>
        [Configuration]
        public double MovingOpacity { get; }

        /// <summary>
        /// 絶対座標での標準サイズ。
        /// </summary>
        [Configuration(rootConvertMethodName: nameof(ConvertSize))]
        public Size LayoutAbsoluteSize { get; }
        /// <summary>
        /// 相対座標での標準サイズ。
        /// </summary>
        [Configuration(rootConvertMethodName: nameof(ConvertSize))]
        public Size LayoutRelativeSize { get; }
        /// <summary>
        /// フォント最小最大サイズ。
        /// </summary>
        [Configuration(rootConvertMethodName: nameof(ConvertMinMaxDefault))]
        public MinMaxDefault<double> FontSize { get; }

        /// <summary>
        /// 自動的に最小化するまでの時間。
        /// </summary>
        [Configuration(rootConvertMethodName: nameof(ConvertMinMax))]
        public MinMax<TimeSpan> HiddenCompactWaitTime { get; }
        /// <summary>
        /// 自動的に隠すまでの時間。
        /// </summary>
        [Configuration(rootConvertMethodName: nameof(ConvertMinMax))]
        public MinMax<TimeSpan> HiddenBlindWaitTime { get; }

        #endregion
    }
}
