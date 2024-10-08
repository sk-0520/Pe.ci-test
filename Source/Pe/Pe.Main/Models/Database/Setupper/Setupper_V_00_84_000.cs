using System;
using System.Collections.Generic;
using System.Windows;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Library.Database;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Setupper
{
    /// <summary>
    /// 誰が何と言おうと新生初期バージョン。
    /// </summary>
    [DatabaseSetupVersion(0, 84, 0)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S101:Types should be named in PascalCase")]
    public class Setupper_V_00_84_000: SetupperBase
    {
        public Setupper_V_00_84_000(IIdFactory idFactory, IDatabaseStatementLoader statementLoader, ILoggerFactory loggerFactory)
            : base(idFactory, statementLoader, loggerFactory)
        { }

        #region SetupBase

        public override void ExecuteMainDDL(IDatabaseContext context, IReadOnlySetupDto dto)
        {
            ExecuteStatement(context, StatementLoader.LoadStatementByCurrent(GetType()), dto);
        }

        public override void ExecuteMainDML(IDatabaseContext context, IReadOnlySetupDto dto)
        {
            var fc = new FontConverter(Logger);

            //BUGS: FirstVersion おかしくね？

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

            ExecuteStatement(context, StatementLoader.LoadStatementByCurrent(GetType()), dto, parameters);
        }

        public override void ExecuteFileDDL(IDatabaseContext context, IReadOnlySetupDto dto)
        {
            ExecuteStatement(context, StatementLoader.LoadStatementByCurrent(GetType()), dto);
        }

        public override void ExecuteFileDML(IDatabaseContext context, IReadOnlySetupDto dto)
        { }

        public override void ExecuteTemporaryDDL(IDatabaseContext context, IReadOnlySetupDto dto)
        { }

        public override void ExecuteTemporaryDML(IDatabaseContext context, IReadOnlySetupDto dto)
        { }

        #endregion
    }
}
