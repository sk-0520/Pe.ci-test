using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin
{
#pragma warning disable S3925 // "ISerializable" should be implemented correctly

    /// <summary>
    /// プラグイン処理にて明示的に発生させる基底の例外。
    /// </summary>
    public abstract class PluginException: Exception
    {
        protected PluginException()
        { }

        protected PluginException(string? message) : base(message)
        { }

        protected PluginException(SerializationInfo info, StreamingContext context) : base(info, context)
        { }

        protected PluginException(string? message, Exception? innerException) : base(message, innerException)
        { }
    }

    /// <summary>
    /// プラグインが存在しない場合に投げられる。
    /// </summary>
    [Serializable]
    public sealed class PluginNotFoundException: PluginException
    {
        public PluginNotFoundException()
        { }

        public PluginNotFoundException(string? message) : base(message)
        { }

        public PluginNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        { }

        public PluginNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        { }
    }

    /// <summary>
    /// プラグインアーカイブの種別不明時に投げられる。
    /// </summary>
    [Serializable]
    public sealed class PluginInvalidArchiveKindException: PluginException
    {
        public PluginInvalidArchiveKindException()
        { }

        public PluginInvalidArchiveKindException(string? message) : base(message)
        { }

        public PluginInvalidArchiveKindException(SerializationInfo info, StreamingContext context) : base(info, context)
        { }

        public PluginInvalidArchiveKindException(string? message, Exception? innerException) : base(message, innerException)
        { }
    }

#pragma warning restore S3925 // "ISerializable" should be implemented correctly
}
