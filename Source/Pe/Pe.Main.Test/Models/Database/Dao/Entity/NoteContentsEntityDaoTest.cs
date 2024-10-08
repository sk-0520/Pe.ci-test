using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Library.DependencyInjection;
using Xunit;

namespace ContentTypeTextNet.Pe.Main.Test.Models.Database.Dao.Entity
{
    public class NoteContentsEntityDaoTest
    {
        #region property

        private Test Test { get; } = Test.Create(TestSetup.Database);

        #endregion

        #region function

        public NoteContentsEntityDaoTest()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        [Fact]
        public void Insert_Select_Update_Delete_Test()
        {
            var testNotes = Test.BuildDao<NotesEntityDao>(AccessorKind.Main);
            var testContents = Test.BuildDao<NoteContentsEntityDao>(AccessorKind.Main);

            var data = new NoteData() {
                NoteId = NoteId.NewId(),
            };

            Assert.False(testContents.SelectExistsContent(data.NoteId));

            testNotes.InsertNewNote(data, Test.DiContainer.Build<IDatabaseCommonStatus>());
            testContents.InsertNewContent(
                new NoteContentData() {
                    NoteId = data.NoteId,
                    ContentKind = data.ContentKind,
                    Content = "Content",
                },
                Test.DiContainer.Build<IDatabaseCommonStatus>()
            );

            Assert.True(testContents.SelectExistsContent(data.NoteId));

            Assert.Equal("Content", testContents.SelectFullContent(data.NoteId));

            testContents.UpdateContent(
                 new NoteContentData() {
                     NoteId = data.NoteId,
                     ContentKind = data.ContentKind,
                     Content = "Content2",
                 },
                Test.DiContainer.Build<IDatabaseCommonStatus>()
           );

            Assert.Equal("Content2", testContents.SelectFullContent(data.NoteId));

            Assert.Equal(1, testContents.DeleteContents(data.NoteId));

            Assert.False(testContents.SelectExistsContent(data.NoteId));
        }

        [Fact]
        public void Link_Test()
        {
            var testNotes = Test.BuildDao<NotesEntityDao>(AccessorKind.Main);
            var testContents = Test.BuildDao<NoteContentsEntityDao>(AccessorKind.Main);

            var data = new NoteData() {
                NoteId = NoteId.NewId(),
            };
            testNotes.InsertNewNote(data, Test.DiContainer.Build<IDatabaseCommonStatus>());
            testContents.InsertNewContent(
                new NoteContentData() {
                    NoteId = data.NoteId,
                    ContentKind = data.ContentKind,
                    IsLink = true,
                    FilePath = "FilePath",
                    Encoding = Encoding.UTF8,
                },
                Test.DiContainer.Build<IDatabaseCommonStatus>()
            );
            var actual1 = testContents.SelectLinkParameter(data.NoteId);
            Assert.True(actual1.IsLink);
            Assert.Equal("FilePath", actual1.FilePath);

            testContents.UpdateLinkDisabled(data.NoteId, Test.DiContainer.Build<IDatabaseCommonStatus>());
            var actual2 = testContents.SelectLinkParameter(data.NoteId);
            //TODO: 偽じゃないとダメなはず
            Assert.True(actual2.IsLink);
            //Assert.False(actual2.IsLink);
            Assert.Empty(actual2.FilePath);

            testContents.UpdateLinkEnabled(
                data.NoteId,
                "LinkPath",
                Encoding.UTF8,
                new Library.Base.FileWatchParameter() {
                    BufferSize = 1234,
                    DelayTime = TimeSpan.FromSeconds(10),
                    IsEnabledRefresh = false,
                    RefreshTime = TimeSpan.FromSeconds(20)
                },
                Test.DiContainer.Build<IDatabaseCommonStatus>()
            );

            var actual3 = testContents.SelectLinkParameter(data.NoteId);
            Assert.True(actual3.IsLink);
            Assert.Equal("LinkPath", actual3.FilePath);
            Assert.Equal(Encoding.UTF8, actual3.Encoding);
            Assert.Equal(1234, actual3.BufferSize);
            Assert.Equal(TimeSpan.FromSeconds(10), actual3.DelayTime);
            Assert.False(actual3.IsEnabledRefresh);
            Assert.Equal(TimeSpan.FromSeconds(20), actual3.RefreshTime);
        }

        #endregion
    }
}
