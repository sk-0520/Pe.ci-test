using System;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Logic
{
    public interface IIdFactory
    {
        #region function

        LauncherItemId CreateLauncherItemId();
        CredentialIdId CreateCredentialId();
        LauncherToolbarId CreateLauncherToolbarId();
        Guid CreateFontId();
        LauncherGroupId CreateLauncherGroupId();
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

        private ILogger Logger { get; }

        #endregion

        #region IIdFactory

        public LauncherItemId CreateLauncherItemId() => new LauncherItemId(Guid.NewGuid());
        public CredentialIdId CreateCredentialId() => new CredentialIdId(Guid.NewGuid());
        public LauncherToolbarId CreateLauncherToolbarId() => new LauncherToolbarId(Guid.NewGuid());
        public Guid CreateFontId() => Guid.NewGuid();
        public LauncherGroupId CreateLauncherGroupId() => new LauncherGroupId(Guid.NewGuid());
        public Guid CreateNoteId() => Guid.NewGuid();
        public Guid CreateNoteFileId() => Guid.NewGuid();
        public Guid CreateKeyActionId() => Guid.NewGuid();

        #endregion
    }
}
