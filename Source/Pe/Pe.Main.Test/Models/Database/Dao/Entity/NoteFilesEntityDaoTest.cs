using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Library.DependencyInjection;
using Xunit;

namespace ContentTypeTextNet.Pe.Main.Test.Models.Database.Dao.Entity
{
    public class NoteFilesEntityDaoTest
    {
        #region property

        private Test Test { get; } = Test.Create(TestSetup.Database);

        #endregion

        #region function

        [Fact]
        public void Insert_Select_Delete_Test()
        {
            var testNotes = Test.BuildDao<NotesEntityDao>(AccessorKind.Main);
            var testFiles = Test.BuildDao<NoteFilesEntityDao>(AccessorKind.Main);

            var data = new NoteData() {
                NoteId = NoteId.NewId(),
            };

            Assert.Empty(testFiles.SelectNoteFiles(data.NoteId));

            testNotes.InsertNewNote(data, Test.DiContainer.Build<IDatabaseCommonStatus>());

            var files = new[] {
                new NoteFileData() {
                    NoteId = data.NoteId,
                    NoteFileId = NoteFileId.NewId(),
                    NoteFilePath = "Path1",
                    Sequence = 1,
                },
                new NoteFileData() {
                    NoteId = data.NoteId,
                    NoteFileId = NoteFileId.NewId(),
                    NoteFilePath = "Path2",
                    Sequence = 2,
                },
            };
            foreach(var file in files) {
                testFiles.InsertNoteFiles(file, Test.DiContainer.Build<IDatabaseCommonStatus>());
            }

            var actual = testFiles.SelectNoteFiles(data.NoteId).ToArray();
            Assert.Equal(2, actual.Length);
            Assert.Equal("Path1", actual[0].NoteFilePath);
            Assert.Equal("Path2", actual[1].NoteFilePath);

            Assert.Equal(files[0].NoteFileId, testFiles.SelectNoteFileExistsFilePath(data.NoteId, "Path1"));
            Assert.Equal(files[1].NoteFileId, testFiles.SelectNoteFileExistsFilePath(data.NoteId, "Path2"));
            Assert.Null(testFiles.SelectNoteFileExistsFilePath(data.NoteId, "Path0"));
            Assert.Null(testFiles.SelectNoteFileExistsFilePath(data.NoteId, "Path3"));

            testFiles.DeleteNoteFilesById(data.NoteId, files[0].NoteFileId);
            Assert.Single(testFiles.SelectNoteFiles(data.NoteId));
            Assert.Equal("Path2", testFiles.SelectNoteFiles(data.NoteId).First().NoteFilePath);

            testFiles.DeleteNoteFilesById(data.NoteId, files[1].NoteFileId);
            Assert.Empty(testFiles.SelectNoteFiles(data.NoteId));
        }

        [Fact]
        public void Sequence_Test()
        {
            var testNotes = Test.BuildDao<NotesEntityDao>(AccessorKind.Main);
            var testFiles = Test.BuildDao<NoteFilesEntityDao>(AccessorKind.Main);

            var data = new NoteData() {
                NoteId = NoteId.NewId(),
            };

            Assert.Empty(testFiles.SelectNoteFiles(data.NoteId));

            testNotes.InsertNewNote(data, Test.DiContainer.Build<IDatabaseCommonStatus>());

            var files = new[] {
                new NoteFileData() {
                    NoteId = data.NoteId,
                    NoteFileId = NoteFileId.NewId(),
                    NoteFilePath = "Path1",
                    Sequence = 9,
                },
                new NoteFileData() {
                    NoteId = data.NoteId,
                    NoteFileId = NoteFileId.NewId(),
                    NoteFilePath = "Path2",
                    Sequence = 10,
                },
                new NoteFileData() {
                    NoteId = data.NoteId,
                    NoteFileId = NoteFileId.NewId(),
                    NoteFilePath = "Path3",
                    Sequence = 11,
                },
                new NoteFileData() {
                    NoteId = data.NoteId,
                    NoteFileId = NoteFileId.NewId(),
                    NoteFilePath = "Path4",
                    Sequence = 100,
                },
                new NoteFileData() {
                    NoteId = data.NoteId,
                    NoteFileId = NoteFileId.NewId(),
                    NoteFilePath = "Path5",
                    Sequence = 900,
                },
            };
            foreach(var file in files) {
                testFiles.InsertNoteFiles(file, Test.DiContainer.Build<IDatabaseCommonStatus>());
            }

            var actual1 = testFiles.SelectNoteFiles(data.NoteId).ToArray();
            Assert.Equal(5, actual1.Length);
            Assert.Equal([9, 10, 11, 100, 900], actual1.Select(a => a.Sequence));

            Assert.Equal(901, testFiles.SelectNextSequenceNoteFiles(data.NoteId));
        }

        #endregion
    }
}
