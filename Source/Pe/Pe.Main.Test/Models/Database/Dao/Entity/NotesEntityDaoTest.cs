using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Library.DependencyInjection;
using Xunit;

namespace ContentTypeTextNet.Pe.Main.Test.Models.Database.Dao.Entity
{
    public class NotesEntityDaoTest
    {
        #region property

        private Test Test { get; } = Test.Create(TestSetup.Database);

        #endregion

        #region function

        [Fact]
        public void Insert_Select_Delete_Test()
        {
            var testNotes = Test.BuildDao<NotesEntityDao>(AccessorKind.Main);
            var testFonts = Test.BuildDao<FontsEntityDao>(AccessorKind.Main);
            var testAppNote = Test.BuildDao<AppNoteSettingEntityDao>(AccessorKind.Main);

            var data = new NoteData() {
                NoteId = NoteId.NewId(),
                FontId = FontId.NewId(),
                ScreenName = "ScreenName",
                Title = "Title",
                LayoutKind = NoteLayoutKind.Absolute,
                IsVisible = true,
                ForegroundColor = Colors.Red,
                BackgroundColor = Colors.Lime,
                IsLocked = true,
                IsTopmost = true,
                TextWrap = true,
                ContentKind = NoteContentKind.RichText,
                HiddenMode = NoteHiddenMode.Compact,
                CaptionPosition = NoteCaptionPosition.Left,
            };

            Assert.Null(testNotes.SelectNote(data.NoteId));

            testNotes.InsertNewNote(data, Test.DiContainer.Build<IDatabaseCommonStatus>());
            var actual = testNotes.SelectNote(data.NoteId);
            var app = new {
                FontId = testAppNote.SelectAppNoteSettingFontId(),
                Setting = testAppNote.SelectSettingNoteSetting(),
            };
            Assert.NotNull(actual);

            Assert.Equal(data.NoteId, actual.NoteId);
            Assert.NotEqual(data.FontId, actual.FontId);
            Assert.Equal(app.FontId, actual.FontId);
            Assert.Equal(data.ScreenName, actual.ScreenName);
            Assert.NotEqual(data.Title, actual.Title);
            Assert.Equal(NoteCreateTitleKind.Timestamp, app.Setting.TitleKind);
            Assert.NotEqual(data.LayoutKind, actual.LayoutKind);
            Assert.NotEqual(data.LayoutKind, app.Setting.LayoutKind);
            Assert.Equal(data.IsVisible, actual.IsVisible);
            Assert.NotEqual(data.ForegroundColor, actual.ForegroundColor);
            Assert.Equal(app.Setting.ForegroundColor, actual.ForegroundColor);
            Assert.NotEqual(data.BackgroundColor, actual.BackgroundColor);
            Assert.Equal(app.Setting.BackgroundColor, actual.BackgroundColor);
            Assert.Equal(data.IsLocked, actual.IsLocked);
            Assert.NotEqual(data.IsTopmost, actual.IsTopmost);
            Assert.Equal(app.Setting.IsTopmost, actual.IsTopmost);
            Assert.Equal(data.TextWrap, actual.TextWrap);
            Assert.Equal(data.ContentKind, actual.ContentKind);
            Assert.Equal(data.HiddenMode, actual.HiddenMode);
            Assert.NotEqual(data.CaptionPosition, actual.CaptionPosition);
            Assert.Equal(app.Setting.CaptionPosition, actual.CaptionPosition);

            testNotes.DeleteNote(data.NoteId);
            Assert.Null(testNotes.SelectNote(data.NoteId));
        }

        [Fact]
        public void SelectAllNoteIds_Empty_Test()
        {
            var test = Test.BuildDao<NotesEntityDao>(AccessorKind.Main);

            Assert.Empty(test.SelectAllNoteIds());
        }

        [Fact]
        public void SelectAllNoteIds_Single_Test()
        {
            var test = Test.BuildDao<NotesEntityDao>(AccessorKind.Main);

            var data = new NoteData() {
                NoteId = NoteId.NewId(),
            };
            test.InsertNewNote(data, Test.DiContainer.Build<IDatabaseCommonStatus>());

            Assert.Single(test.SelectAllNoteIds());
            Assert.Contains(data.NoteId, test.SelectAllNoteIds());
        }

        [Fact]
        public void SelectAllNoteIds_Multi_Test()
        {
            var test = Test.BuildDao<NotesEntityDao>(AccessorKind.Main);

            var data = new[]{
                new NoteData() {
                    NoteId = NoteId.NewId(),
                },
                new NoteData() {
                    NoteId = NoteId.NewId(),
                },
                new NoteData() {
                    NoteId = NoteId.NewId(),
                },
            };
            foreach(var item in data) {
                test.InsertNewNote(item, Test.DiContainer.Build<IDatabaseCommonStatus>());
            }
            var actual = test.SelectAllNoteIds().ToArray();
            Assert.Equal(3, actual.Length);
            foreach(var item in data) {
                Assert.Contains(item.NoteId, test.SelectAllNoteIds());
            }
        }

        [Fact]
        public void UpdateScreenTest()
        {
            var test = Test.BuildDao<NotesEntityDao>(AccessorKind.Main);

            var data = new NoteData() {
                NoteId = NoteId.NewId(),
                ScreenName = "ScreenName1",
            };
            test.InsertNewNote(data, Test.DiContainer.Build<IDatabaseCommonStatus>());
            Assert.Equal("ScreenName1", test.SelectNote(data.NoteId)!.ScreenName);

            test.UpdateScreen(data.NoteId, "ScreenName2", Test.DiContainer.Build<IDatabaseCommonStatus>());
            Assert.Equal("ScreenName2", test.SelectNote(data.NoteId)!.ScreenName);
        }

        [Fact]
        public void UpdateCompactTest()
        {
            var test = Test.BuildDao<NotesEntityDao>(AccessorKind.Main);

            var data = new NoteData() {
                NoteId = NoteId.NewId(),
            };
            test.InsertNewNote(data, Test.DiContainer.Build<IDatabaseCommonStatus>());
            Assert.False(test.SelectNote(data.NoteId)!.IsCompact);

            test.UpdateCompact(data.NoteId, true, Test.DiContainer.Build<IDatabaseCommonStatus>());
            Assert.True(test.SelectNote(data.NoteId)!.IsCompact);
        }

        [Fact]
        public void UpdateTopmostTest()
        {
            var test = Test.BuildDao<NotesEntityDao>(AccessorKind.Main);

            var data = new NoteData() {
                NoteId = NoteId.NewId(),
            };
            test.InsertNewNote(data, Test.DiContainer.Build<IDatabaseCommonStatus>());
            Assert.False(test.SelectNote(data.NoteId)!.IsTopmost);

            test.UpdateTopmost(data.NoteId, true, Test.DiContainer.Build<IDatabaseCommonStatus>());
            Assert.True(test.SelectNote(data.NoteId)!.IsTopmost);
        }

        [Fact]
        public void UpdateLockTest()
        {
            var test = Test.BuildDao<NotesEntityDao>(AccessorKind.Main);

            var data = new NoteData() {
                NoteId = NoteId.NewId(),
            };
            test.InsertNewNote(data, Test.DiContainer.Build<IDatabaseCommonStatus>());
            Assert.False(test.SelectNote(data.NoteId)!.IsLocked);

            test.UpdateLock(data.NoteId, true, Test.DiContainer.Build<IDatabaseCommonStatus>());
            Assert.True(test.SelectNote(data.NoteId)!.IsLocked);
        }

        [Fact]
        public void UpdateTextWrapTest()
        {
            var test = Test.BuildDao<NotesEntityDao>(AccessorKind.Main);

            var data = new NoteData() {
                NoteId = NoteId.NewId(),
            };
            test.InsertNewNote(data, Test.DiContainer.Build<IDatabaseCommonStatus>());
            Assert.False(test.SelectNote(data.NoteId)!.TextWrap);

            test.UpdateTextWrap(data.NoteId, true, Test.DiContainer.Build<IDatabaseCommonStatus>());
            Assert.True(test.SelectNote(data.NoteId)!.TextWrap);
        }

        [Fact]
        public void UpdateTitleTest()
        {
            var test = Test.BuildDao<NotesEntityDao>(AccessorKind.Main);

            var data = new NoteData() {
                NoteId = NoteId.NewId(),
            };
            test.InsertNewNote(data, Test.DiContainer.Build<IDatabaseCommonStatus>());
            Assert.Matches(@"(\d{4})/(\d{2})/(\d{2}) (\d{2}):(\d{2}):(\d{2})", test.SelectNote(data.NoteId)!.Title);

            test.UpdateTitle(data.NoteId, "Title2", Test.DiContainer.Build<IDatabaseCommonStatus>());
            Assert.Equal("Title2", test.SelectNote(data.NoteId)!.Title);
        }

        [Fact]
        public void UpdateFontIdTest()
        {
            var testNotes = Test.BuildDao<NotesEntityDao>(AccessorKind.Main);
            var testFonts = Test.BuildDao<FontsEntityDao>(AccessorKind.Main);
            var testAppNote = Test.BuildDao<AppNoteSettingEntityDao>(AccessorKind.Main);

            var data = new NoteData() {
                NoteId = NoteId.NewId(),
            };
            testNotes.InsertNewNote(data, Test.DiContainer.Build<IDatabaseCommonStatus>());
            Assert.Equal(testAppNote.SelectAppNoteSettingFontId(), testNotes.SelectNote(data.NoteId)!.FontId);

            var fontId = FontId.NewId();
            var font = new FontData();
            testFonts.InsertFont(fontId, font, Test.DiContainer.Build<IDatabaseCommonStatus>());

            testNotes.UpdateFontId(data.NoteId, fontId, Test.DiContainer.Build<IDatabaseCommonStatus>());
            Assert.Equal(fontId, testNotes.SelectNote(data.NoteId)!.FontId);
        }

        [Fact]
        public void UpdateForegroundColorTest()
        {
            var test = Test.BuildDao<NotesEntityDao>(AccessorKind.Main);

            var data = new NoteData() {
                NoteId = NoteId.NewId(),
            };
            test.InsertNewNote(data, Test.DiContainer.Build<IDatabaseCommonStatus>());

            test.UpdateForegroundColor(data.NoteId, Colors.Red, Test.DiContainer.Build<IDatabaseCommonStatus>());
            Assert.Equal(Colors.Red, test.SelectNote(data.NoteId)!.ForegroundColor);
        }


        [Fact]
        public void UpdateBackgroundColorTest()
        {
            var test = Test.BuildDao<NotesEntityDao>(AccessorKind.Main);

            var data = new NoteData() {
                NoteId = NoteId.NewId(),
            };
            test.InsertNewNote(data, Test.DiContainer.Build<IDatabaseCommonStatus>());

            test.UpdateBackgroundColor(data.NoteId, Colors.Lime, Test.DiContainer.Build<IDatabaseCommonStatus>());
            Assert.Equal(Colors.Lime, test.SelectNote(data.NoteId)!.BackgroundColor);
        }

        [Fact]
        public void UpdateCaptionPositionTest()
        {
            var test = Test.BuildDao<NotesEntityDao>(AccessorKind.Main);

            var data = new NoteData() {
                NoteId = NoteId.NewId(),
            };
            test.InsertNewNote(data, Test.DiContainer.Build<IDatabaseCommonStatus>());

            test.UpdateCaptionPosition(data.NoteId, NoteCaptionPosition.Right, Test.DiContainer.Build<IDatabaseCommonStatus>());
            Assert.Equal(NoteCaptionPosition.Right, test.SelectNote(data.NoteId)!.CaptionPosition);
        }

        [Fact]
        public void UpdateContentKindTest()
        {
            var test = Test.BuildDao<NotesEntityDao>(AccessorKind.Main);

            var data = new NoteData() {
                NoteId = NoteId.NewId(),
            };
            test.InsertNewNote(data, Test.DiContainer.Build<IDatabaseCommonStatus>());

            test.UpdateContentKind(data.NoteId, NoteContentKind.RichText, Test.DiContainer.Build<IDatabaseCommonStatus>());
            Assert.Equal(NoteContentKind.RichText, test.SelectNote(data.NoteId)!.ContentKind);
        }

        [Fact]
        public void UpdateLayoutKindTest()
        {
            var test = Test.BuildDao<NotesEntityDao>(AccessorKind.Main);

            var data = new NoteData() {
                NoteId = NoteId.NewId(),
            };
            test.InsertNewNote(data, Test.DiContainer.Build<IDatabaseCommonStatus>());

            test.UpdateLayoutKind(data.NoteId, NoteLayoutKind.Absolute, Test.DiContainer.Build<IDatabaseCommonStatus>());
            Assert.Equal(NoteLayoutKind.Absolute, test.SelectNote(data.NoteId)!.LayoutKind);
        }

        [Fact]
        public void UpdateVisibleTest()
        {
            var test = Test.BuildDao<NotesEntityDao>(AccessorKind.Main);

            var data = new NoteData() {
                NoteId = NoteId.NewId(),
                IsVisible = true,
            };
            test.InsertNewNote(data, Test.DiContainer.Build<IDatabaseCommonStatus>());
            Assert.True(test.SelectNote(data.NoteId)!.IsVisible);

            test.UpdateVisible(data.NoteId, false, Test.DiContainer.Build<IDatabaseCommonStatus>());
            Assert.False(test.SelectNote(data.NoteId)!.IsVisible);
        }

        [Fact]
        public void UpdateHiddenModeTest()
        {
            var test = Test.BuildDao<NotesEntityDao>(AccessorKind.Main);

            var data = new NoteData() {
                NoteId = NoteId.NewId(),
                HiddenMode = NoteHiddenMode.Blind,
            };
            test.InsertNewNote(data, Test.DiContainer.Build<IDatabaseCommonStatus>());
            Assert.Equal(NoteHiddenMode.Blind, test.SelectNote(data.NoteId)!.HiddenMode);

            test.UpdateHiddenMode(data.NoteId, NoteHiddenMode.Compact, Test.DiContainer.Build<IDatabaseCommonStatus>());
            Assert.Equal(NoteHiddenMode.Compact, test.SelectNote(data.NoteId)!.HiddenMode);
        }
        #endregion
    }
}
