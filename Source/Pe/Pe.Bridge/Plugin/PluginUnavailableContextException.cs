using System;
using System.Runtime.Serialization;

namespace ContentTypeTextNet.Pe.Bridge.Plugin
{
    [System.Serializable]
    public class PluginException: Exception
    {
        public PluginException()
        { }

        public PluginException(string? message)
            : base(message)
        { }

        public PluginException(string? message, Exception? innerException)
            : base(message, innerException)
        { }

        protected PluginException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }
    }

    [System.Serializable]
    public class PluginUnavailableContextException: PluginException
    {
        public PluginUnavailableContextException()
        { }

        public PluginUnavailableContextException(string message)
            : base(message)
        { }

        public PluginUnavailableContextException(string message, Exception inner)
            : base(message, inner)
        { }

        protected PluginUnavailableContextException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        { }
    }
}
