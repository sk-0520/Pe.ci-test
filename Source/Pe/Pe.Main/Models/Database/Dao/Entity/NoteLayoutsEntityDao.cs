using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity
{
    public class NoteLayoutsEntityDao : EntityDaoBase
    {
        #region define

        private class NoteLayoutsEntityDto: CommonDtoBase
        {
            #region property

            public Guid NoteId { get; set; }
            public string LayoutKind { get; set; } = string.Empty;

            [PixelKind(Px.Logical)]
            public double X { get; set; }
            [PixelKind(Px.Logical)]
            public double Y { get; set; }
            [PixelKind(Px.Logical)]
            public double Width { get; set; }
            [PixelKind(Px.Logical)]
            public double Height { get; set; }

            #endregion
        }

        #endregion

        public NoteLayoutsEntityDao(IDatabaseContext context, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(context, statementLoader, implementation, loggerFactory)
        { }

        #region property

        public static class Column
        {
            #region property

            public static string NoteId { get; } = "NoteId";
            public static string LayoutKind { get; } = "LayoutKind";
            public static string X { get; } = "X";
            public static string Y { get; } = "Y";
            public static string Width { get; } = "Width";
            public static string Height { get; } = "Height";

            #endregion
        }

        #endregion

        #region function

        NoteLayoutsEntityDto ConvertFromData(NoteLayoutData data, IDatabaseCommonStatus databaseCommonStatus)
        {
            var noteLayoutKindTransfer = new EnumTransfer<NoteLayoutKind>();

            var dto = new NoteLayoutsEntityDto() {
                NoteId = data.NoteId,
                LayoutKind = noteLayoutKindTransfer.ToString(data.LayoutKind),
                X = data.X,
                Y = data.Y,
                Width = data.Width,
                Height = data.Height,
            };

            databaseCommonStatus.WriteCommon(dto);

            return dto;
        }

        NoteLayoutData ConvertFromDto(NoteLayoutsEntityDto dto)
        {
            var noteLayoutKindTransfer = new EnumTransfer<NoteLayoutKind>();

            var data = new NoteLayoutData() {
                NoteId = dto.NoteId,
                LayoutKind = noteLayoutKindTransfer.ToEnum(dto.LayoutKind),
                X = dto.X,
                Y = dto.Y,
                Width = dto.Width,
                Height = dto.Height,
            };

            return data;
        }

        public NoteLayoutData? SelectLayout(Guid noteId, NoteLayoutKind layoutKind)
        {
            var noteLayoutKindTransfer = new EnumTransfer<NoteLayoutKind>();

            var statement = LoadStatement();
            var param = new {
                NoteId = noteId,
                LayoutKind = noteLayoutKindTransfer.ToString(layoutKind),
            };
            return Context.QueryFirstOrDefault<NoteLayoutData>(statement, param);
        }
        public bool SelectExistsLayout(Guid noteId, NoteLayoutKind layoutKind)
        {
            var noteLayoutKindTransfer = new EnumTransfer<NoteLayoutKind>();

            var statement = LoadStatement();
            var param = new {
                NoteId = noteId,
                LayoutKind = noteLayoutKindTransfer.ToString(layoutKind),
            };
            return Context.QuerySingle<bool>(statement, param);
        }

        public bool InsertLayout(NoteLayoutData noteLayout, IDatabaseCommonStatus databaseCommonStatus)
        {
            var statement = LoadStatement();
            var param = ConvertFromData(noteLayout, databaseCommonStatus);
            return Context.Execute(statement, param) == 1;
        }

        public bool UpdateLayout(NoteLayoutData noteLayout, IDatabaseCommonStatus databaseCommonStatus)
        {
            var statement = LoadStatement();
            var param = ConvertFromData(noteLayout, databaseCommonStatus);
            return Context.Execute(statement, param) == 1;
        }
        public bool UpdatePickupLayout(NoteLayoutData noteLayout, bool isEnabledLocation, bool isEnabledSize, IDatabaseCommonStatus databaseCommonStatus)
        {
            var noteLayoutKindTransfer = new EnumTransfer<NoteLayoutKind>();

            var statement = LoadStatement();
            var param = databaseCommonStatus.CreateCommonDtoMapping();
            param[Column.NoteId] = noteLayout.NoteId;
            param[Column.LayoutKind] = noteLayoutKindTransfer.ToString(noteLayout.LayoutKind);
            param[Column.X] = noteLayout.X;
            param[Column.Y] = noteLayout.Y;
            param[Column.Width] = noteLayout.Width;
            param[Column.Height] = noteLayout.Height;
            param[nameof(isEnabledLocation)] = isEnabledLocation;
            param[nameof(isEnabledSize)] = isEnabledSize;
            return Context.Execute(statement, param) == 1;
        }

        public int DeleteLayouts(Guid noteId)
        {
            var statement = LoadStatement();
            var parameter = new {
                NoteId = noteId,
            };
            return Context.Execute(statement, parameter);
        }

        #endregion
    }
}
