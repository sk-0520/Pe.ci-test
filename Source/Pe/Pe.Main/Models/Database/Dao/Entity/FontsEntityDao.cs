using System;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Library.Database;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity
{
    public class FontsEntityDao: EntityDaoBase
    {
        #region define

        private sealed class FontsRowDto: CommonDtoBase
        {
            #region property

            public FontId FontId { get; set; }
            public string FamilyName { get; set; } = string.Empty;
            public double Height { get; set; }
            public bool IsBold { get; set; }
            public bool IsItalic { get; set; }
            public bool IsUnderline { get; set; }
            public bool IsStrikeThrough { get; set; }

            #endregion
        }

        private static class Column
        {
            #region property

            public static string FontId { get; } = "FontId";
            public static string FamilyName { get; } = "FamilyName";
            public static string Height { get; } = "Height";
            public static string IsBold { get; } = "IsBold";
            public static string IsItalic { get; } = "IsItalic";
            public static string IsUnderline { get; } = "IsUnderline";
            public static string IsStrikeThrough { get; } = "IsStrikeThrough";

            #endregion
        }

        #endregion

        public FontsEntityDao(IDatabaseContext context, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(context, statementLoader, implementation, loggerFactory)
        { }

        #region function

        private FontData ConvertFromDto(FontsRowDto dto)
        {
            var data = new FontData() {
                FamilyName = dto.FamilyName,
                Size = dto.Height,
                IsBold = dto.IsBold,
                IsItalic = dto.IsItalic,
                IsUnderline = dto.IsUnderline,
                IsStrikeThrough = dto.IsStrikeThrough,
            };

            return data;
        }

        private FontsRowDto ConvertFromData(IFont font, IDatabaseCommonStatus databaseCommonStatus)
        {
            var dto = new FontsRowDto() {
                FamilyName = font.FamilyName,
                Height = font.Size,
                IsBold = font.IsBold,
                IsItalic = font.IsItalic,
                IsUnderline = font.IsUnderline,
                IsStrikeThrough = font.IsStrikeThrough,
            };
            databaseCommonStatus.WriteCommonTo(dto);

            return dto;
        }

        public FontData SelectFont(FontId fontId)
        {
            var statement = LoadStatement();
            var param = new {
                FontId = fontId,
            };
            var dto = Context.QueryFirst<FontsRowDto>(statement, param);
            return ConvertFromDto(dto);
        }

        public void InsertFont(FontId fontId, IFont font, IDatabaseCommonStatus databaseCommonStatus)
        {
            var statement = LoadStatement();
            var param = ConvertFromData(font, databaseCommonStatus);
            param.FontId = fontId;

            Context.InsertSingle(statement, param);
        }

        public void InsertCopyFont(FontId sourceFontId, FontId destinationFontId, IDatabaseCommonStatus commonStatus)
        {
            var statement = LoadStatement();
            var parameter = commonStatus.CreateCommonDtoMapping();
            parameter["SrcFontId"] = sourceFontId;
            parameter["DstFontId"] = destinationFontId;

            Context.InsertSingle(statement, parameter);
        }


        public void UpdateFamilyName(FontId fontId, string familyName, IDatabaseCommonStatus databaseCommonStatus)
        {
            var statement = LoadStatement();
            var parameter = databaseCommonStatus.CreateCommonDtoMapping();
            parameter[Column.FontId] = fontId;
            parameter[Column.FamilyName] = familyName;

            Context.UpdateByKey(statement, parameter);
        }

        public void UpdateBold(FontId fontId, bool isBold, IDatabaseCommonStatus databaseCommonStatus)
        {
            var statement = LoadStatement();
            var parameter = databaseCommonStatus.CreateCommonDtoMapping();
            parameter[Column.FontId] = fontId;
            parameter[Column.IsBold] = isBold;

            Context.UpdateByKey(statement, parameter);
        }

        public void UpdateItalic(FontId fontId, bool isItalic, IDatabaseCommonStatus databaseCommonStatus)
        {
            var statement = LoadStatement();
            var parameter = databaseCommonStatus.CreateCommonDtoMapping();
            parameter[Column.FontId] = fontId;
            parameter[Column.IsItalic] = isItalic;

            Context.UpdateByKey(statement, parameter);
        }

        public void UpdateHeight(FontId fontId, double height, IDatabaseCommonStatus databaseCommonStatus)
        {
            var statement = LoadStatement();
            var parameter = databaseCommonStatus.CreateCommonDtoMapping();
            parameter[Column.FontId] = fontId;
            parameter[Column.Height] = height;

            Context.UpdateByKey(statement, parameter);
        }

        public void UpdateFont(FontId fontId, IFont font, IDatabaseCommonStatus commonStatus)
        {
            var statement = LoadStatement();
            var parameter = ConvertFromData(font, commonStatus);
            parameter.FontId = fontId;

            Context.UpdateByKey(statement, parameter);
        }

        #endregion
    }
}
