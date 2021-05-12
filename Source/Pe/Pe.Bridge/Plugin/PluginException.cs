using System;
using System.Runtime.Serialization;

namespace ContentTypeTextNet.Pe.Bridge.Plugin
{
    /// <summary>
    /// プラグイン特有の基底例外。
    /// <para>ファイルが見つからなければ<see cref="System.IO.IOException"/>でいいけどプラグイン処理として固有の例外はこいつを使用すること。</para>
    /// </summary>
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

    /// <summary>
    /// プラグイン使用不可例外。
    /// </summary>
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
