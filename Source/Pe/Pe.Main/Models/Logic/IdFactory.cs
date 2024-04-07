using System;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Logic
{
    public interface IIdFactory
    {
        #region function

        LauncherItemId CreateLauncherItemId();
        CredentialId CreateCredentialId();
        LauncherToolbarId CreateLauncherToolbarId();
        FontId CreateFontId();
        LauncherGroupId CreateLauncherGroupId();
        NoteId CreateNoteId();
        NoteFileId CreateNoteFileId();
        KeyActionId CreateKeyActionId();
        ScheduleJobId CreateScheduleJobId();
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

        public LauncherItemId CreateLauncherItemId() => LauncherItemId.NewId();
        public CredentialId CreateCredentialId() => CredentialId.NewId();
        public LauncherToolbarId CreateLauncherToolbarId() => LauncherToolbarId.NewId();
        public FontId CreateFontId() => FontId.NewId();
        public LauncherGroupId CreateLauncherGroupId() => LauncherGroupId.NewId();
        public NoteId CreateNoteId() => NoteId.NewId();
        public NoteFileId CreateNoteFileId() => NoteFileId.NewId();
        public KeyActionId CreateKeyActionId() => KeyActionId.NewId();
        public ScheduleJobId CreateScheduleJobId() => ScheduleJobId.NewId();

        #endregion
    }
}
