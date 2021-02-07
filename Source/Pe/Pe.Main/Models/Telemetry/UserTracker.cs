using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Telemetry
{
    /// <summary>
    /// ユーザー操作を追跡。
    /// </summary>
    public interface IUserTracker
    {
        #region function

        /// <inheritdoc cref="TrackAsync(string, TrackProperties)"/>
        Task TrackAsync(string eventName);
        /// <summary>
        /// ユーザーが操作したイベントを記録。
        /// </summary>
        /// <param name="eventName">イベント名。</param>
        /// <param name="properties">プロパティ。</param>
        /// <returns></returns>
        Task TrackAsync(string eventName, TrackProperties properties);

        #endregion
    }

    /// <inheritdoc cref="IUserTracker"/>
    internal class UserTracker: TelemeterBase, IUserTracker
    {
        public UserTracker(ILoggerFactory loggerFactory)
            : base(loggerFactory)
        { }

        #region function

        Task TrackCoreAsync(string eventName, TrackProperties properties)
        {
            Logger.LogTrace("{0}: {1}", eventName, properties);
            return Task.CompletedTask;
        }

        #endregion

        #region IUserTracker

        /// <inheritdoc cref="IUserTracker.TrackAsync(string)"/>
        public Task TrackAsync(string eventName) => TrackCoreAsync(eventName, TrackProperties.Empty);
        /// <inheritdoc cref="IUserTracker.TrackAsync(string, TrackProperties)"/>
        public Task TrackAsync(string eventName, TrackProperties properties) => TrackCoreAsync(eventName, properties);

        #endregion
    }
}
