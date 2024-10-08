using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Library.DependencyInjection;
using Xunit;

namespace ContentTypeTextNet.Pe.Main.Test.Models.Database.Dao.Entity
{
    public class AppNoteSettingEntityDaoTest
    {
        #region property

        private Test Test { get; } = Test.Create(TestSetup.Database);

        #endregion

        #region function

        [Fact]
        public void SelectAppNoteSettingFontIdTest()
        {
            var test = Test.BuildDao<AppNoteSettingEntityDao>(AccessorKind.Main);
            var actual = test.SelectAppNoteSettingFontId();
            Assert.NotEqual(FontId.Empty, actual);
        }

        [Fact]
        public void Update_Select_Test()
        {
            var test = Test.BuildDao<AppNoteSettingEntityDao>(AccessorKind.Main);

            var expected = new SettingAppNoteSettingData() {
                FontId = test.SelectAppNoteSettingFontId(),
                BackgroundColor = Colors.Red,
                ForegroundColor = Colors.Green,
                CaptionPosition = NoteCaptionPosition.Right,
                IsTopmost = true,
                LayoutKind = NoteLayoutKind.Absolute,
                TitleKind = NoteCreateTitleKind.Count,
            };

            test.UpdateSettingNoteSetting(expected, Test.DiContainer.New<IDatabaseCommonStatus>());

            var actual = test.SelectSettingNoteSetting();

            Assert.Equal(expected.FontId, actual.FontId);
            Assert.Equal(expected.BackgroundColor, actual.BackgroundColor);
            Assert.Equal(expected.ForegroundColor, actual.ForegroundColor);
            Assert.Equal(expected.CaptionPosition, actual.CaptionPosition);
            Assert.Equal(expected.IsTopmost, actual.IsTopmost);
            Assert.Equal(expected.LayoutKind, actual.LayoutKind);
            Assert.Equal(expected.TitleKind, actual.TitleKind);
        }


        [Fact]
        public void Select_Latest_Test()
        {
            var mainDatabaseAccessor = Test.DiContainer.New<IMainDatabaseAccessor>();
            var test = Test.DiContainer.Build<AppNoteSettingEntityDao>(mainDatabaseAccessor, mainDatabaseAccessor.DatabaseFactory.CreateImplementation());
            var entityDaoHelper = new EntityDaoHelper(mainDatabaseAccessor, mainDatabaseAccessor.DatabaseFactory.CreateImplementation());
            entityDaoHelper.CloneRecord(
                test.TableName,
                new Dictionary<string, string>() {
                    ["Generation"] = "-10",
                    ["TitleKind"] = "'count'"
                },
                new Dictionary<string, string>() {
                    ["Generation"] = "1",
                }
            );

            entityDaoHelper.CloneRecord(
                test.TableName,
                new Dictionary<string, string>() {
                    ["Generation"] = "20",
                    ["TitleKind"] = "'count'"
                },
                new Dictionary<string, string>() {
                    ["Generation"] = "1",
                }
            );

            var actual = test.SelectSettingNoteSetting();
            Assert.Equal(NoteCreateTitleKind.Count, actual.TitleKind);
        }

        #endregion
    }
}
