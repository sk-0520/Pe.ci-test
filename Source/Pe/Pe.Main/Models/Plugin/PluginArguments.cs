using ContentTypeTextNet.Pe.Bridge.Models;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin
{
    public interface IPluginArguments
    {
        #region property

        IHttpUserAgentFactory HttpUserAgentFactory { get; }
        IPlatformTheme PlatformTheme { get; }
        IImageLoader ImageLoader { get; }
        IMediaConverter MediaConverter { get; }
        IDispatcherWrapper DispatcherWrapper { get; }
        ILoggerFactory LoggerFactory { get; }

        #endregion
    }
    public class PluginArguments: IPluginArguments
    {
        public PluginArguments(IHttpUserAgentFactory httpUserAgentFactory, IPlatformTheme platformTheme, IImageLoader imageLoader, IMediaConverter mediaConverter, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
        {
            HttpUserAgentFactory = httpUserAgentFactory;
            PlatformTheme = platformTheme;
            ImageLoader = imageLoader;
            MediaConverter = mediaConverter;
            DispatcherWrapper = dispatcherWrapper;
            LoggerFactory = loggerFactory;
        }

        #region IPluginArguments

        public IHttpUserAgentFactory HttpUserAgentFactory { get; }
        public IPlatformTheme PlatformTheme { get; }
        public IImageLoader ImageLoader { get; }
        public IMediaConverter MediaConverter { get; }
        public IDispatcherWrapper DispatcherWrapper { get; }
        public ILoggerFactory LoggerFactory { get; }

        #endregion
    }
}
