using System.Collections.Generic;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using System.Linq;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Library.Database;
using Microsoft.Extensions.Logging;
using ContentTypeTextNet.Pe.Main.Models.Data;
using System.Text;
using ContentTypeTextNet.Pe.Core.Models;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Dao.Domain
{
    public class SettingExporterDomainDao: DomainDaoBase
    {
        #region define

        private sealed record class SettingGroupDto(
            LauncherGroupId LauncherGroupId,
            string LauncherGroupName,
            LauncherItemId LauncherItemId,
            string LauncherItemName,
            string Kind
        );

        private sealed class SettingLauncherItemDto
        {
            public LauncherItemId LauncherItemId { get; set; } = LauncherItemId.Empty;
            public string LauncherItemName { get; set; } = string.Empty;
            public string LauncherItemKind { get; set; } = string.Empty;
            public string IconPath { get; set; } = string.Empty;
            public long IconIndex { get; set; } = 0;
            public string FilePath { get; set; } = string.Empty;
            public string FileOption { get; set; } = string.Empty;
            public string FileWorkDirectory { get; set; } = string.Empty;
            public string Comment { get; set; } = string.Empty;
        }

        public record class SettingNoteDto(
            NoteId NoteId,
            string Title,
            string ScreenName,
            string ForegroundColor,
            string BackgroundColor,
            bool IsLocked,
            bool IsTopmost,
            bool IsCompact,
            string ContentKind,
            string HiddenMode,
            string CaptionPosition,
            string Encoding,
            string Content
        );

        #endregion

        public SettingExporterDomainDao(IDatabaseContext context, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(context, statementLoader, implementation, loggerFactory)
        { }

        #region property
        #endregion

        #region function

        public IEnumerable<SettingGroup> SelectSettingGroups()
        {
            var launcherItemKindTransfer = new EnumTransfer<LauncherItemKind>();

            var statement = LoadStatement();
            var groups = Context.Query<SettingGroupDto>(statement)
                .GroupBy(a => (a.LauncherGroupId, a.LauncherGroupName))
            ;

            foreach(var group in groups) {
                yield return new SettingGroup(
                    group.Key.LauncherGroupId,
                    group.Key.LauncherGroupName,
                    group.Select(a => new SettingGroupItem(
                        a.LauncherItemId,
                        a.LauncherItemName,
                        launcherItemKindTransfer.ToEnum(a.Kind)
                    )).ToArray()
                );
            }
        }

        public IEnumerable<SettingLauncherItem> SelectSettingLauncherItems()
        {
            var launcherItemKindTransfer = new EnumTransfer<LauncherItemKind>();

            var statement = LoadStatement();
            return Context.SelectOrdered<SettingLauncherItemDto>(statement)
                .Select(a => new SettingLauncherItem(
                    a.LauncherItemId,
                    a.LauncherItemName,
                    launcherItemKindTransfer.ToEnum(a.LauncherItemKind),
                    a.IconPath,
                    a.IconIndex,
                    a.FilePath,
                    a.FileOption,
                    a.FileWorkDirectory,
                    a.Comment
                ))
            ;
        }

        public IEnumerable<SettingNote> SelectSettingNotes()
        {
            var noteContentKindTransfer = new EnumTransfer<NoteContentKind>();
            var noteHiddenModeTransfer = new EnumTransfer<NoteHiddenMode>();
            var noteCaptionPositionTransfer = new EnumTransfer<NoteCaptionPosition>();

            var statement = LoadStatement();
            return Context.SelectOrdered<SettingNoteDto>(statement)
                .Select(a => new SettingNote(
                    a.NoteId,
                    a.Title,
                    a.ScreenName,
                    a.ForegroundColor,
                    a.BackgroundColor,
                    a.IsLocked,
                    a.IsTopmost,
                    a.IsCompact,
                    noteContentKindTransfer.ToEnum(a.ContentKind),
                    noteHiddenModeTransfer.ToEnum(a.HiddenMode),
                    noteCaptionPositionTransfer.ToEnum(a.CaptionPosition),
                    EncodingUtility.Parse(a.Encoding),
                    a.Content
                ))
            ;
        }

        #endregion
    }
}
