using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Domain;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Library.DependencyInjection;
using Xunit;

namespace ContentTypeTextNet.Pe.Main.Test.Models.Database.Dao.Domain
{
    public class NoteDomainDaoTest
    {
        #region define

        private sealed class ScreenData: IScreen
        {
            public required int BitsPerPixel { get; init; }

            public required Rect DeviceBounds { get; init; }

            public required string DeviceName { get; init; }

            public required bool Primary { get; init; }

            public required Rect DeviceWorkingArea { get; init; }
        }

        #endregion

        #region property

        private Test Test { get; } = Test.Create(TestSetup.Database);

        #endregion

        #region function

        [Fact]
        public void SelectNoteScreens_NoScreen_Test()
        {
            var testDomain = Test.BuildDao<NoteDomainDao>(AccessorKind.Main);
            var testNotes = Test.BuildDao<NotesEntityDao>(AccessorKind.Main);
            var testFonts = Test.BuildDao<FontsEntityDao>(AccessorKind.Main);

            var noteId = NoteId.NewId();
            var fontId = FontId.NewId();

            testFonts.InsertFont(
                fontId,
                new FontData(),
                Test.DiContainer.Build<IDatabaseCommonStatus>()
            );

            testNotes.InsertNewNote(
                new NoteData() {
                    NoteId = noteId,
                    ScreenName = "screen"
                },
                Test.DiContainer.Build<IDatabaseCommonStatus>()
            );

            //TODO: DBの制約上これ複数返ることは絶対ないと思う。
            var actual = testDomain.SelectNoteScreens(noteId);
            Assert.Single(actual);

            var actualItem = actual.Single();
            Assert.Equal("screen", actualItem.ScreenName);
            Assert.Equal(0, actualItem.X);
            Assert.Equal(0, actualItem.Y);
            Assert.Equal(0, actualItem.Width);
            Assert.Equal(0, actualItem.Height);
        }

        [Fact]
        public void SelectNoteScreens_Screen_Test()
        {
            var testDomain = Test.BuildDao<NoteDomainDao>(AccessorKind.Main);
            var testNotes = Test.BuildDao<NotesEntityDao>(AccessorKind.Main);
            var testFonts = Test.BuildDao<FontsEntityDao>(AccessorKind.Main);
            var testScreens = Test.BuildDao<ScreensEntityDao>(AccessorKind.Main);

            var noteId = NoteId.NewId();
            var fontId = FontId.NewId();

            testFonts.InsertFont(
                fontId,
                new FontData(),
                Test.DiContainer.Build<IDatabaseCommonStatus>()
            );

            testScreens.InsertScreen(
                new ScreenData() {
                    DeviceName = "screen",
                    Primary = false,
                    BitsPerPixel = 24,
                    DeviceBounds = new Rect(10, 20, 30, 40),
                    DeviceWorkingArea = new Rect(),
                },
                Test.DiContainer.Build<IDatabaseCommonStatus>()
            );

            testNotes.InsertNewNote(
                new NoteData() {
                    NoteId = noteId,
                    ScreenName = "screen"
                },
                Test.DiContainer.Build<IDatabaseCommonStatus>()
            );

            //TODO: DBの制約上これ複数返ることは絶対ないと思う。
            var actual = testDomain.SelectNoteScreens(noteId);
            Assert.Single(actual);

            var actualItem = actual.Single();
            Assert.Equal("screen", actualItem.ScreenName);
            Assert.Equal(10, actualItem.X);
            Assert.Equal(20, actualItem.Y);
            Assert.Equal(30, actualItem.Width);
            Assert.Equal(40, actualItem.Height);
        }

        #endregion
    }
}
