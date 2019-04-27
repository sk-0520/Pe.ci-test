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

        #endregion
    }
}
