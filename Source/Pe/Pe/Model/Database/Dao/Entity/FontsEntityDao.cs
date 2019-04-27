using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model.Database;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Data;
using ContentTypeTextNet.Pe.Main.Model.Data.Dto.Entity;

namespace ContentTypeTextNet.Pe.Main.Model.Database.Dao.Entity
{
    public class FontsEntityDao : EntityDaoBase
    {
        public FontsEntityDao(IDatabaseCommander commander, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(commander, statementLoader, implementation, loggerFactory)
        { }

        #region property

        public static class Column
        {
            #region property

            public static string FontId { get; } = "FontId";
            public static string FamilyName { get; } = "FamilyName";
            public static string Height { get; } = "Height";
            public static string Bold { get; } = "Bold";
            public static string Italic { get; } = "Italic";
            public static string Underline { get; } = "Underline";
            public static string Strike { get; } = "Strike";

            #endregion
        }

        #endregion

        #region function

        FontData ConvertFromDto(FontsRowDto dto)
        {
            var data = new FontData() {
                Family = dto.FamilyName,
                Size = dto.Height,
                Bold = dto.Bold,
                Italic = dto.Italic,
                Underline = dto.Underline,
                LineThrough = dto.Strike,
            };

            return data;
        }

        FontsRowDto ConvertFromData(IReadOnlyFontData data, IDatabaseCommonStatus databaseCommonStatus)
        {
            var dto = new FontsRowDto() {
                FamilyName = data.Family,
                Height = data.Size,
                Bold = data.Bold,
                Italic = data.Italic,
                Underline = data.Underline,
                Strike = data.LineThrough,
            };
            databaseCommonStatus.WriteCommon(dto);

            return dto;
        }

        public FontData SelectFont(Guid fontId)
        {
            var statement = StatementLoader.LoadStatementByCurrent();
            var param = new {
                FontId = fontId,
            };
            var dto = Commander.QueryFirstOrDefault<FontsRowDto>(statement, param);
            if(dto != null) {
                return ConvertFromDto(dto);
            }

            return null;
        }

        public bool InsertFont(Guid fontId, FontData fontData, IDatabaseCommonStatus databaseCommonStatus)
        {
            var statement = StatementLoader.LoadStatementByCurrent();
            var param = ConvertFromData(fontData, databaseCommonStatus);
            param.FontId = fontId;
            return Commander.Execute(statement, param) == 1;
        }

        public bool UpdateFontFamily(Guid fontId, string fontFamily, IDatabaseCommonStatus databaseCommonStatus)
        {
            var statement = StatementLoader.LoadStatementByCurrent();
            var param = databaseCommonStatus.CreateCommonDtoMapping();
            param[Column.FontId] = fontId;
            param[Column.FamilyName] = fontFamily;
            return Commander.Execute(statement, param) == 1;
        }

        public bool UpdateBold(Guid fontId, bool isBold, IDatabaseCommonStatus databaseCommonStatus)
        {
            var builder = CreateUpdateBuilder(databaseCommonStatus);
            builder.AddKey(Column.FontId, fontId);
            builder.AddValue(Column.Bold, isBold);
            return ExecuteUpdate(builder) == 1;
        }

        #endregion
    }
}
