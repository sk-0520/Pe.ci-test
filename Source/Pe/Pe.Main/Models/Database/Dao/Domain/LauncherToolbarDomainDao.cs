using System;
using System.Collections.Generic;
using System.Linq;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Library.Database;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Dao.Domain
{
    public class LauncherToolbarDomainDao: DomainDaoBase
    {
        #region define

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S3459:Unassigned members should be removed", Justification = "<保留中>")]
        private sealed class LauncherToolbarScreenRowDto: CommonDtoBase
        {
            #region property

            public LauncherToolbarId LauncherToolbarId { get; set; }

            public string ScreenName { get; set; } = string.Empty;
            [PixelKind(Px.Device)]
            public long ScreenX { get; set; }
            [PixelKind(Px.Device)]
            public long ScreenY { get; set; }
            [PixelKind(Px.Device)]
            public long ScreenWidth { get; set; }
            [PixelKind(Px.Device)]
            public long ScreenHeight { get; set; }

            #endregion
        }

        #endregion

        public LauncherToolbarDomainDao(IDatabaseContext context, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(context, statementLoader, implementation, loggerFactory)
        { }

        #region function

        private LauncherToolbarsScreenData ConvertFromDto(LauncherToolbarScreenRowDto dto)
        {
            var data = new LauncherToolbarsScreenData() {
                LauncherToolbarId = dto.LauncherToolbarId,
                ScreenName = dto.ScreenName,
                X = dto.ScreenX,
                Y = dto.ScreenY,
                Height = dto.ScreenHeight,
                Width = dto.ScreenWidth,
            };

            return data;
        }

        public LauncherToolbarsScreenData SelectScreenToolbar(LauncherToolbarId launcherToolbarId)
        {
            var statement = LoadStatement();
            var parameter = new {
                LauncherToolbarId = launcherToolbarId,
            };
            var dto = Context.QueryFirst<LauncherToolbarScreenRowDto>(statement, parameter);
            return ConvertFromDto(dto);
        }

        public IEnumerable<LauncherToolbarsScreenData> SelectAllScreenToolbars()
        {
            var statement = LoadStatement();
            return Context.Query<LauncherToolbarScreenRowDto>(statement)
                .Select(i => ConvertFromDto(i))
            ;
        }

        #endregion
    }
}
