using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.CommonTest;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Library.Database;
using ContentTypeTextNet.Pe.Library.DependencyInjection;
using Xunit;

namespace ContentTypeTextNet.Pe.Main.Test.Models.Database.Dao.Entity
{
    public class NoteLayoutsEntityDaoTest
    {
        #region property

        private Test Test { get; } = Test.Create(TestSetup.Database);

        #endregion

        #region function

        [Fact]
        public void Insert_Select_Update_Delete_Test()
        {
            var testNotes = Test.BuildDao<NotesEntityDao>(AccessorKind.Main);
            var testLayouts = Test.BuildDao<NoteLayoutsEntityDao>(AccessorKind.Main);

            var data = new NoteData() {
                NoteId = NoteId.NewId(),
            };

            Assert.False(testLayouts.SelectExistsLayout(data.NoteId, NoteLayoutKind.Relative));

            testNotes.InsertNewNote(data, Test.DiContainer.Build<IDatabaseCommonStatus>());
            Assert.Equal(NoteLayoutKind.Relative, testNotes.SelectNote(data.NoteId)!.LayoutKind); // このテストとは関係ないけど心の安心のため
            testLayouts.InsertLayout(
                new NoteLayoutData() {
                    NoteId = data.NoteId,
                    LayoutKind = NoteLayoutKind.Relative,
                    X = 10,
                    Y = 20,
                    Width = 30,
                    Height = 40,
                },
                Test.DiContainer.Build<IDatabaseCommonStatus>()
            );

            Assert.True(testLayouts.SelectExistsLayout(data.NoteId, NoteLayoutKind.Relative));
            Assert.False(testLayouts.SelectExistsLayout(data.NoteId, NoteLayoutKind.Absolute));

            var actual1 = testLayouts.SelectLayout(data.NoteId, NoteLayoutKind.Relative);
            Assert.NotNull(actual1);
            Assert.Null(testLayouts.SelectLayout(data.NoteId, NoteLayoutKind.Absolute));
            Assert.Equal(data.NoteId, actual1.NoteId);
            Assert.Equal(NoteLayoutKind.Relative, actual1.LayoutKind);
            Assert.Equal(10, actual1.X);
            Assert.Equal(20, actual1.Y);
            Assert.Equal(30, actual1.Width);
            Assert.Equal(40, actual1.Height);

            Assert.Throws<DatabaseManipulationException>(() => {
                testLayouts.UpdateLayout(
                    new NoteLayoutData() {
                        NoteId = data.NoteId,
                        LayoutKind = NoteLayoutKind.Absolute,
                        X = 100,
                        Y = 200,
                        Width = 300,
                        Height = 400,
                    },
                    Test.DiContainer.Build<IDatabaseCommonStatus>()
                );
            });

            testLayouts.UpdateLayout(
                new NoteLayoutData() {
                    NoteId = data.NoteId,
                    LayoutKind = NoteLayoutKind.Relative,
                    X = 100,
                    Y = 200,
                    Width = 300,
                    Height = 400,
                },
                Test.DiContainer.Build<IDatabaseCommonStatus>()
            );
            var actual2 = testLayouts.SelectLayout(data.NoteId, NoteLayoutKind.Relative);
            Assert.NotNull(actual2);
            Assert.Equal(data.NoteId, actual2.NoteId);
            Assert.Equal(NoteLayoutKind.Relative, actual2.LayoutKind);
            Assert.Equal(100, actual2.X);
            Assert.Equal(200, actual2.Y);
            Assert.Equal(300, actual2.Width);
            Assert.Equal(400, actual2.Height);

            Assert.Equal(1, testLayouts.DeleteLayouts(data.NoteId));
            Assert.False(testLayouts.SelectExistsLayout(data.NoteId, NoteLayoutKind.Relative));
            Assert.Null(testLayouts.SelectLayout(data.NoteId, NoteLayoutKind.Relative));
        }

        public static TheoryData<NoteLayoutData, NoteLayoutData, bool, bool> UpdatePickupLayoutData => new() {
            {
                new NoteLayoutData() {
                    X = 10,
                    Y = 20,
                    Width = 30,
                    Height = 40,
                },
                new NoteLayoutData() {
                    X = 100,
                    Y = 200,
                    Width = 300,
                    Height = 400,
                },
                false,
                false
            },
            {
                new NoteLayoutData() {
                    X = 100,
                    Y = 200,
                    Width = 30,
                    Height = 40,
                },
                new NoteLayoutData() {
                    X = 100,
                    Y = 200,
                    Width = 300,
                    Height = 400,
                },
                true,
                false
            },
            {
                new NoteLayoutData() {
                    X = 10,
                    Y = 20,
                    Width = 300,
                    Height = 400,
                },
                new NoteLayoutData() {
                    X = 100,
                    Y = 200,
                    Width = 300,
                    Height = 400,
                },
                false,
                true
            },
            {
                new NoteLayoutData() {
                    X = 100,
                    Y = 200,
                    Width = 300,
                    Height = 400,
                },
                new NoteLayoutData() {
                    X = 100,
                    Y = 200,
                    Width = 300,
                    Height = 400,
                },
                true,
                true
            },
        };

        [Theory]
        [MemberData(nameof(UpdatePickupLayoutData))]
        public void UpdatePickupLayoutTest(NoteLayoutData expected, NoteLayoutData edit, bool isEnabledLocation, bool isEnabledSize)
        {
            var nodeId = NoteId.NewId();
            var testNotes = Test.BuildDao<NotesEntityDao>(AccessorKind.Main);
            var testLayouts = Test.BuildDao<NoteLayoutsEntityDao>(AccessorKind.Main);

            var data = new NoteData() {
                NoteId = nodeId,
            };
            testNotes.InsertNewNote(data, Test.DiContainer.Build<IDatabaseCommonStatus>());
            testLayouts.InsertLayout(
                new NoteLayoutData() {
                    NoteId = nodeId,
                    LayoutKind = NoteLayoutKind.Relative,
                    X = 10,
                    Y = 20,
                    Width = 30,
                    Height = 40,
                },
                Test.DiContainer.Build<IDatabaseCommonStatus>()
            );

            testLayouts.UpdatePickupLayout(
                new NoteLayoutData() {
                    NoteId = nodeId,
                    LayoutKind = NoteLayoutKind.Relative,
                    X = edit.X,
                    Y = edit.Y,
                    Width = edit.Width,
                    Height = edit.Height,
                },
                isEnabledLocation,
                isEnabledSize,
                Test.DiContainer.Build<IDatabaseCommonStatus>()
            );
            var actual = testLayouts.SelectLayout(nodeId, NoteLayoutKind.Relative)!;
            Assert.NotNull(actual);
            Assert.Equal(data.NoteId, actual.NoteId);
            Assert.Equal(NoteLayoutKind.Relative, actual.LayoutKind);
            Assert.Equal(expected.X, actual.X);
            Assert.Equal(expected.Y, actual.Y);
            Assert.Equal(expected.Width, actual.Width);
            Assert.Equal(expected.Height, actual.Height);
        }

        #endregion
    }
}
