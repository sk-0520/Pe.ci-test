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
            public static string IsBold { get; } = "IsBold";
            public static string IsItalic { get; } = "IsItalic";
            public static string IsUnderline { get; } = "IsUnderline";
            public static string IsStrikeThrough { get; } = "IsStrikeThrough";

            #endregion
        }

        #endregion

        #region function

        FontData ConvertFromDto(FontsRowDto dto)
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

        FontsRowDto ConvertFromData(IReadOnlyFontData data, IDatabaseCommonStatus databaseCommonStatus)
        {
            var dto = new FontsRowDto() {
                FamilyName = data.FamilyName,
                Height = data.Size,
                IsBold = data.IsBold,
                IsItalic = data.IsItalic,
                IsUnderline = data.IsUnderline,
                IsStrikeThrough = data.IsStrikeThrough,
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

        public bool UpdateFamilyName(Guid fontId, string familyName, IDatabaseCommonStatus databaseCommonStatus)
        {
            var builder = CreateUpdateBuilder(databaseCommonStatus);
            builder.AddKey(Column.FontId, fontId);
            builder.AddValue(Column.FamilyName, familyName);
            return ExecuteUpdate(builder) == 1;
        }

        public bool UpdateBold(Guid fontId, bool isBold, IDatabaseCommonStatus databaseCommonStatus)
        {
            var builder = CreateUpdateBuilder(databaseCommonStatus);
            builder.AddKey(Column.FontId, fontId);
            builder.AddValue(Column.IsBold, isBold);
            return ExecuteUpdate(builder) == 1;
        }

        public bool UpdateItalic(Guid fontId, bool isItalic, IDatabaseCommonStatus databaseCommonStatus)
        {
            var builder = CreateUpdateBuilder(databaseCommonStatus);
            builder.AddKey(Column.FontId, fontId);
            builder.AddValue(Column.IsItalic, isItalic);
            return ExecuteUpdate(builder) == 1;
        }

        public bool UpdateHeight(Guid fontId, double height, IDatabaseCommonStatus databaseCommonStatus)
        {
            var builder = CreateUpdateBuilder(databaseCommonStatus);
            builder.AddKey(Column.FontId, fontId);
            builder.AddValue(Column.Height, height);
            return ExecuteUpdate(builder) == 1;
        }


        #endregion
    }
}
