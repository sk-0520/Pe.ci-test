using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Logic
{
    /// <summary>
    /// 本体アプリケーション起動処理。
    /// </summary>
    public class ApplicationBoot
    {
        public ApplicationBoot(ILoggerFactory loggerFactory)
        {
            LoggerFactory = loggerFactory;
            Logger = LoggerFactory.CreateLogger(GetType());
        }

        #region property

        /// <inheritdoc cref="ILoggerFactory"/>
        ILoggerFactory LoggerFactory { get; }
        /// <inheritdoc cref="ILogger"/>
        ILogger Logger { get; }

        /// <summary>
        /// 本体起動コマンドパス。
        /// </summary>
        public static string CommandPath { get; } = Path.ChangeExtension(Assembly.GetExecutingAssembly().Location, "exe");

        #endregion
    }
}
