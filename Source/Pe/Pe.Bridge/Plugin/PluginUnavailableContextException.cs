using System;
using System.Collections.Generic;
using System.Text;

namespace ContentTypeTextNet.Pe.Bridge.Plugin
{
    [System.Serializable]
    public class PluginUnavailableContextException: Exception
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
