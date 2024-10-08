using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Library.DependencyInjection;
using ContentTypeTextNet.Pe.Library.Database;
using Xunit;
using ContentTypeTextNet.Pe.Main.Models.Data;
using System.Text.RegularExpressions;
using ContentTypeTextNet.Pe.Bridge.Models.Data;

namespace ContentTypeTextNet.Pe.Main.Test.Models.Database.Dao.Entity
{
    public class LauncherGroupsEntityDaoTest
    {
        #region property

        private Test Test { get; } = Test.Create(TestSetup.Database);

        #endregion

        #region function

        [Fact]
        public void SelectMaxSequence_Empty_Test()
        {
            var mainDatabaseAccessor = Test.DiContainer.Build<IMainDatabaseAccessor>();
            var test = Test.DiContainer.Build<LauncherGroupsEntityDao>(mainDatabaseAccessor, mainDatabaseAccessor.DatabaseFactory.CreateImplementation());

            mainDatabaseAccessor.Delete("delete from LauncherGroups");

            Assert.Equal(0, test.SelectMaxSequence());
        }

        [Fact]
        public void SelectMaxSequence_Init_Test()
        {
            var test = Test.BuildDao<LauncherGroupsEntityDao>(AccessorKind.Main);

            Assert.Equal(10, test.SelectMaxSequence());
        }

        [Fact]
        public void Insert_Select_Update_Delete_Test()
        {
            var test = Test.BuildDao<LauncherGroupsEntityDao>(AccessorKind.Main);

            var data = new {
                A = new LauncherGroupData() {
                    LauncherGroupId = LauncherGroupId.NewId(),
                    Name = "Name1",
                },
                B = new LauncherGroupData() {
                    LauncherGroupId = LauncherGroupId.NewId(),
                    Name = "Name2",
                },
            };
            test.InsertNewGroup(data.A, Test.DiContainer.Build<IDatabaseCommonStatus>());
            test.InsertNewGroup(data.B, Test.DiContainer.Build<IDatabaseCommonStatus>());

            var actualIds = test.SelectAllLauncherGroupIds();
            Assert.Contains(actualIds, a => a == data.A.LauncherGroupId);
            Assert.Contains(actualIds, a => a == data.B.LauncherGroupId);

            var actualNames = test.SelectAllLauncherGroupNames();
            Assert.Contains(actualNames, a => a == data.A.Name);
            Assert.Contains(actualNames, a => a == data.B.Name);

            var actualA = test.SelectLauncherGroup(data.A.LauncherGroupId);
            Assert.Equal("Name1", actualA.Name);

            var actualB = test.SelectLauncherGroup(data.B.LauncherGroupId);
            Assert.Equal("Name2", actualB.Name);

            var dataUpdateA = new LauncherGroupData() {
                LauncherGroupId = data.A.LauncherGroupId,
                Name = "Name1+",
            };
            test.UpdateGroup(dataUpdateA, Test.DiContainer.Build<IDatabaseCommonStatus>());
            var actualA2 = test.SelectLauncherGroup(data.A.LauncherGroupId);
            Assert.Equal("Name1+", actualA2.Name);

            test.DeleteGroup(data.A.LauncherGroupId);
            Assert.Throws<InvalidOperationException>(() => test.SelectLauncherGroup(data.A.LauncherGroupId));
        }

        #endregion
    }
}
