using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Library.DependencyInjection;
using Xunit;

namespace ContentTypeTextNet.Pe.Main.Test.Models.Database.Dao.Entity
{
    public class AppProxySettingEntityDaoTest
    {
        #region property

        private Test Test { get; } = Test.Create(TestSetup.Database);

        #endregion

        #region function

        [Fact]
        public void SelectProxyIsEnabledTest()
        {
            var test = Test.BuildDao<AppProxySettingEntityDao>(AccessorKind.Main);

            var actual = test.SelectProxyIsEnabled();
            Assert.False(actual);
        }

        [Fact]
        public void Update_Select_Test()
        {
            var test = Test.BuildDao<AppProxySettingEntityDao>(AccessorKind.Main);

            var expected = new AppProxySettingData() {
                ProxyIsEnabled = true,
                ProxyUrl = "http://localhost/proxy",
                CredentialIsEnabled = true,
                CredentialUser = "user",
                CredentialPassword = "password",
            };

            test.UpdateProxySetting(expected, Test.DiContainer.Build<IDatabaseCommonStatus>());

            var actual = test.SelectProxySetting();

            Assert.Equal(expected.ProxyIsEnabled, actual.ProxyIsEnabled);
            Assert.Equal(expected.ProxyUrl, actual.ProxyUrl);
            Assert.Equal(expected.CredentialIsEnabled, actual.CredentialIsEnabled);
            Assert.Equal(expected.CredentialUser, actual.CredentialUser);
            Assert.Equal(expected.CredentialPassword, actual.CredentialPassword);
        }

        [Fact]
        public void UpdateToggleProxyIsEnabled()
        {
            var test = Test.BuildDao<AppProxySettingEntityDao>(AccessorKind.Main);
            var firstActual = test.SelectProxyIsEnabled();
            test.UpdateToggleProxyIsEnabled(Test.DiContainer.Build<IDatabaseCommonStatus>());
            var secondActual = test.SelectProxyIsEnabled();
            test.UpdateToggleProxyIsEnabled(Test.DiContainer.Build<IDatabaseCommonStatus>());
            var thirdActual = test.SelectProxyIsEnabled();
            Assert.NotEqual(firstActual, secondActual);
            Assert.Equal(firstActual, thirdActual);
        }

        #endregion
    }
}
