using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Standard.DependencyInjection;
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

        #endregion
    }
}
