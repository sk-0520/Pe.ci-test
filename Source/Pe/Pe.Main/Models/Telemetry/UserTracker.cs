using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Telemetry
{
    public interface IUserTracker
    {
        #region function

        Task TrackAsync(string eventName);
        Task TrackAsync(string eventName, TrackProperties properties);

        #endregion
    }

    internal class UserTracker : TelemeterBase, IUserTracker
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

        public Task TrackAsync(string eventName) => TrackCoreAsync(eventName, TrackProperties.Empty);
        public Task TrackAsync(string eventName, TrackProperties properties) => TrackCoreAsync(eventName, properties);

        #endregion
    }
}
