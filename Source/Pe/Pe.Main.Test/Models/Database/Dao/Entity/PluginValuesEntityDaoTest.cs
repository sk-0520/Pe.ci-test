using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Standard.DependencyInjection;
using ContentTypeTextNet.Pe.Standard.Database;
using Xunit;

namespace ContentTypeTextNet.Pe.Main.Test.Models.Database.Dao.Entity
{
    public class PluginValuesEntityDaoTest
    {
        #region property

        private Test Test { get; } = Test.Create(TestSetup.Database);

        #endregion

        #region function

        [Fact]
        public void DeletePluginValuesByPluginIdTest()
        {
            var largeDatabaseAccessor = Test.DiContainer.New<ILargeDatabaseAccessor>();
            var test = Test.DiContainer.Build<PluginValuesEntityDao>(largeDatabaseAccessor, largeDatabaseAccessor.DatabaseFactory.CreateImplementation());

            var pluginId = PluginId.NewId();

            Assert.Equal(0, test.DeletePluginValuesByPluginId(pluginId));

            var sql = @"
                insert into
                    PluginValues
                    (
                        PluginId,
                        PluginSettingKey,
                        Sequence,
                        Data,
                        CreatedTimestamp, CreatedAccount, CreatedProgramName, CreatedProgramVersion
                    )
                    values
                    (
                        @PluginId,
                        @PluginSettingKey,
                        @Sequence,
                        @Data,
                        @CreatedTimestamp, @CreatedAccount, @CreatedProgramName, @CreatedProgramVersion
                    )
            ";

            largeDatabaseAccessor.Insert(
                sql,
                new {
                    PluginId = pluginId,
                    PluginSettingKey = string.Empty,
                    Sequence = 0,
                    Data = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 },
                    CreatedTimestamp = DateTime.UtcNow,
                    CreatedAccount = "CreatedAccount",
                    CreatedProgramName = "CreatedProgramName",
                    CreatedProgramVersion = new Version(1, 2, 3)
                }
            );

            largeDatabaseAccessor.Insert(
                sql,
                new {
                    PluginId = pluginId,
                    PluginSettingKey = "key",
                    Sequence = 0,
                    Data = new byte[] { 10, 20, 30, 40, 50, 60, 70, 80 },
                    CreatedTimestamp = DateTime.UtcNow,
                    CreatedAccount = "CreatedAccount",
                    CreatedProgramName = "CreatedProgramName",
                    CreatedProgramVersion = new Version(1, 2, 3)
                }
            );

            Assert.Equal(2, test.DeletePluginValuesByPluginId(pluginId));

        }

        #endregion
    }
}
