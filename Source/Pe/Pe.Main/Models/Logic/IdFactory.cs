using System;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Logic
{
    public interface IIdFactory
    {
        #region function

        Guid CreateLauncherItemId();
        Guid CreateCredentId();
        Guid CreateLauncherToolbarId();
        Guid CreateFontId();
        Guid CreateLauncherGroupId();
        Guid CreateNoteId();
        Guid CreateNoteFileId();
        Guid CreateKeyActionId();

        #endregion
    }

    internal sealed class IdFactory: IIdFactory
    {
        public IdFactory(ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger(GetType());
        }

        #region property

        ILogger Logger { get; }

        #endregion

        #region IIdFactory

        public Guid CreateLauncherItemId() => Guid.NewGuid();
        public Guid CreateCredentId() => Guid.NewGuid();
        public Guid CreateLauncherToolbarId() => Guid.NewGuid();
        public Guid CreateFontId() => Guid.NewGuid();
        public Guid CreateLauncherGroupId() => Guid.NewGuid();
        public Guid CreateNoteId() => Guid.NewGuid();
        public Guid CreateNoteFileId() => Guid.NewGuid();
        public Guid CreateKeyActionId() => Guid.NewGuid();

        #endregion
    }
}
