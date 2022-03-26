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
        FontId CreateFontId();
        LauncherGroupId CreateLauncherGroupId();
        NoteId CreateNoteId();
        NoteFileId CreateNoteFileId();
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
        public FontId CreateFontId() => new FontId(Guid.NewGuid());
        public LauncherGroupId CreateLauncherGroupId() => new LauncherGroupId(Guid.NewGuid());
        public NoteId CreateNoteId() => new NoteId(Guid.NewGuid());
        public NoteFileId CreateNoteFileId() => new NoteFileId(Guid.NewGuid());
        public Guid CreateKeyActionId() => Guid.NewGuid();

        #endregion
    }
}
