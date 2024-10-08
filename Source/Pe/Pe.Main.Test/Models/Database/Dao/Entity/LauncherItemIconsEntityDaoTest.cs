using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Library.DependencyInjection;
using Xunit;

namespace ContentTypeTextNet.Pe.Main.Test.Models.Database.Dao.Entity
{
    public class LauncherItemIconsEntityDaoTest
    {
        #region property

        private Test Test { get; } = Test.Create(TestSetup.Database);

        #endregion

        #region function

        [Theory]
        [InlineData(80 * 1024 * 1)]
        [InlineData(80 * 1024 * 2)]
        [InlineData(80 * 1024 * 3)]
        public void Insert_Select_Test(int size)
        {
            var test = Test.BuildDao<LauncherItemIconsEntityDao>(AccessorKind.Large);
            var id = LauncherItemId.NewId();
            var iconBoxItems = Enum.GetValues<IconBox>();
            var pointXItems = new[] { 1, 1.25, 1.5, 2 };
            foreach(var iconBox in iconBoxItems) {
                foreach(var pointX in pointXItems) {
                    var iconScale = new IconScale(iconBox, new System.Windows.Point(pointX, pointX));
                    var binary = new byte[(int)(size * pointX)];
                    Random.Shared.NextBytes(binary);
                    test.InsertImageBinary(id, iconScale, binary, DateTime.UtcNow, Test.DiContainer.Build<IDatabaseCommonStatus>());
                    var actual = test.SelectImageBinary(id, iconScale);
                    using var stream = new MemoryStream();
                    stream.Write(actual);
                    Assert.Equal(binary, stream.ToArray());
                }
            }

            foreach(var iconBox in iconBoxItems) {
                foreach(var pointX in pointXItems) {
                    var iconScale = new IconScale(iconBox, new System.Windows.Point(pointX, pointX));
                    test.SelectImageBinary(id, iconScale);
                }
            }

            Assert.True(iconBoxItems.Length * pointXItems.Length <= test.DeleteAllSizeImageBinary(id));
        }

        [Fact]
        public void SelectImageBinary_Empty_Test()
        {
            var test = Test.BuildDao<LauncherItemIconsEntityDao>(AccessorKind.Large);
            var actual = test.SelectImageBinary(LauncherItemId.Empty, new IconScale(IconBox.Small, new Point(1,1)));
            Assert.Empty(actual);
        }

        [Fact]
        public void SelectLauncherItemIconKeyStatus_SelectLauncherItemIconAllStatus_Delete_Test()
        {
            var test = Test.BuildDao<LauncherItemIconsEntityDao>(AccessorKind.Large);
            var id = LauncherItemId.NewId();

            test.InsertImageBinary(
                id,
                new IconScale(IconBox.Normal, new Point(1, 1)),
                new byte[] { 1, 2, 3 },
                new DateTime(2000, 1, 2, 4, 5, 6, 7, DateTimeKind.Utc),
                Test.DiContainer.Build<IDatabaseCommonStatus>()
            );

            test.InsertImageBinary(
                id,
                new IconScale(IconBox.Normal, new Point(2, 2)),
                new byte[] { 11, 12, 13 },
                new DateTime(2001, 1, 2, 4, 5, 6, 7, DateTimeKind.Utc),
                Test.DiContainer.Build<IDatabaseCommonStatus>()
            );

            test.InsertImageBinary(
                id,
                new IconScale(IconBox.Large, new Point(1, 1)),
                new byte[] { 21, 22, 23 },
                new DateTime(2002, 1, 2, 4, 5, 6, 7, DateTimeKind.Utc),
                Test.DiContainer.Build<IDatabaseCommonStatus>()
            );

            var actual1 = test.SelectLauncherItemIconKeyStatus(id, new IconScale(IconBox.Normal, new Point(1, 1)));
            Assert.NotNull(actual1);
            Assert.Equal(new DateTime(2000, 1, 2, 4, 5, 6, 7, DateTimeKind.Utc), actual1.LastUpdatedTimestamp);

            var actual2 = test.SelectLauncherItemIconKeyStatus(id, new IconScale(IconBox.Normal, new Point(2, 2)));
            Assert.NotNull(actual2);
            Assert.Equal(new DateTime(2001, 1, 2, 4, 5, 6, 7, DateTimeKind.Utc), actual2.LastUpdatedTimestamp);

            var actual3 = test.SelectLauncherItemIconKeyStatus(id, new IconScale(IconBox.Large, new Point(1, 1)));
            Assert.NotNull(actual3);
            Assert.Equal(new DateTime(2002, 1, 2, 4, 5, 6, 7, DateTimeKind.Utc), actual3.LastUpdatedTimestamp);

            var actual4 = test.SelectLauncherItemIconKeyStatus(id, new IconScale(IconBox.Small, new Point(1, 1)));
            Assert.Null(actual4);

            var actual5 = test.SelectLauncherItemIconKeyStatus(LauncherItemId.Empty, new IconScale(IconBox.Small, new Point(1, 1)));
            Assert.Null(actual5);

            var actual6 = test.SelectLauncherItemIconAllStatus(id).ToArray();
            Assert.Equal(3, actual6.Length);
            Assert.Contains(new DateTime(2000, 1, 2, 4, 5, 6, 7, DateTimeKind.Utc), actual6.Select(a => a.LastUpdatedTimestamp));
            Assert.Contains(new DateTime(2001, 1, 2, 4, 5, 6, 7, DateTimeKind.Utc), actual6.Select(a => a.LastUpdatedTimestamp));
            Assert.Contains(new DateTime(2002, 1, 2, 4, 5, 6, 7, DateTimeKind.Utc), actual6.Select(a => a.LastUpdatedTimestamp));

            Assert.True(test.DeleteImageBinary(id, new IconScale(IconBox.Normal, new Point(1, 1))));
            Assert.False(test.DeleteImageBinary(id, new IconScale(IconBox.Normal, new Point(1, 1))));
            Assert.False(test.DeleteImageBinary(LauncherItemId.Empty, new IconScale(IconBox.Normal, new Point(1, 1))));
            var actual7 = test.SelectLauncherItemIconAllStatus(id).ToArray();
            Assert.Equal(2, actual7.Length);

            Assert.Equal(2, test.DeleteAllSizeImageBinary(id));
            var actual8 = test.SelectLauncherItemIconAllStatus(id).ToArray();
            Assert.Empty(actual8);
        }

        #endregion
    }
}
