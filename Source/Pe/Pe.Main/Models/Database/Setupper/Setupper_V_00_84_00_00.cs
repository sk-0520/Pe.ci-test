using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Data.Dto;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Setupper
{
    /// <summary>
    /// 誰が何と言おうと新生初期バージョン。
    /// </summary>
    public class Setupper_V_00_84_00_00 : SetupperBase
    {
        public Setupper_V_00_84_00_00(IIdFactory idFactory, IDatabaseStatementLoader statementLoader, ILoggerFactory loggerFactory)
            : base(idFactory, statementLoader, loggerFactory)
        { }

        #region SetupBase

        public override Version Version { get; } = new Version(0, 84, 0, 0);

        public override void ExecuteMainDDL(IDatabaseCommander commander, IReadOnlySetupDto dto)
        {
            ExecuteStatement(commander, StatementLoader.LoadStatementByCurrent(GetType()), dto);
        }

        public override void ExecuteMainDML(IDatabaseCommander commander, IReadOnlySetupDto dto)
        {
            var fc = new FontConverter(Logger);

            var parameters = new Dictionary<string, object>() {
                // NoteDefault
                ["NoteDefaultFontId"] = IdFactory.CreateFontId(),
                ["NoteDefaultFamilyName"] = fc.GetOriginalFontFamilyName(SystemFonts.MessageFontFamily),
                ["NoteDefaultHeight"] = SystemFonts.MessageFontSize,
                ["NoteDefaultIsBold"] = fc.IsBold(SystemFonts.MessageFontWeight),
                ["NoteDefaultIsItalic"] = fc.IsItalic(SystemFonts.MessageFontStyle),
                ["NoteDefaultIsUnderline"] = false,
                ["NoteDefaultIsStrikeThrough"] = false,
                // command
                ["CommandFontId"] = IdFactory.CreateFontId(),
                ["CommandFamilyName"] = fc.GetOriginalFontFamilyName(SystemFonts.MessageFontFamily),
                ["CommandHeight"] = SystemFonts.MessageFontSize,
                ["CommandIsBold"] = fc.IsBold(SystemFonts.MessageFontWeight),
                ["CommandIsItalic"] = fc.IsItalic(SystemFonts.MessageFontStyle),
                ["CommandIsUnderline"] = false,
                ["CommandIsStrikeThrough"] = false,
                // stdio
                ["StdioFontId"] = IdFactory.CreateFontId(),
                ["StdioFamilyName"] = fc.GetOriginalFontFamilyName(SystemFonts.MessageFontFamily),
                ["StdioHeight"] = SystemFonts.MessageFontSize,
                ["StdioIsBold"] = fc.IsBold(SystemFonts.MessageFontWeight),
                ["StdioIsItalic"] = fc.IsItalic(SystemFonts.MessageFontStyle),
                ["StdioIsUnderline"] = false,
                ["StdioIsStrikeThrough"] = false,
                // launcher toolbar
                ["LauncherToolbarFontId"] = IdFactory.CreateFontId(),
                ["LauncherToolbarFamilyName"] = fc.GetOriginalFontFamilyName(SystemFonts.MessageFontFamily),
                ["LauncherToolbarHeight"] = SystemFonts.MessageFontSize,
                ["LauncherToolbarIsBold"] = fc.IsBold(SystemFonts.MessageFontWeight),
                ["LauncherToolbarIsItalic"] = fc.IsItalic(SystemFonts.MessageFontStyle),
                ["LauncherToolbarIsUnderline"] = false,
                ["LauncherToolbarIsStrikeThrough"] = false,
            };

            ExecuteStatement(commander, StatementLoader.LoadStatementByCurrent(GetType()), dto, parameters);
        }

        public override void ExecuteFileDDL(IDatabaseCommander commander, IReadOnlySetupDto dto)
        {
            ExecuteStatement(commander, StatementLoader.LoadStatementByCurrent(GetType()), dto);
        }

        public override void ExecuteFileDML(IDatabaseCommander commander, IReadOnlySetupDto dto)
        { }

        public override void ExecuteTemporaryDDL(IDatabaseCommander commander, IReadOnlySetupDto dto)
        { }

        public override void ExecuteTemporaryDML(IDatabaseCommander commander, IReadOnlySetupDto dto)
        { }

        #endregion
    }
}
