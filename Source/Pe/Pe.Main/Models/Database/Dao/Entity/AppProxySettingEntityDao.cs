using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Library.Database;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity
{
    public class AppProxySettingEntityDao: EntityDaoBase
    {
        #region define

        private sealed class SettingAppProxySettingDto: CommonDtoBase
        {
            #region property

            public bool ProxyIsEnabled { get; init; }
            public string ProxyUrl { get; set; } = string.Empty;
            public bool CredentialIsEnabled { get; init; }
            public string CredentialUser { get; set; } = string.Empty;
            public byte[] CredentialPassword { get; set; } = Array.Empty<byte>();

            #endregion
        }

        #endregion

        public AppProxySettingEntityDao(IDatabaseContext context, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(context, statementLoader, implementation, loggerFactory)
        { }

        #region property

        private static class Column
        {
            #region property

            public static string ProxyIsEnabled => nameof(ProxyIsEnabled);
            public static string ProxyUrl => nameof(ProxyUrl);
            public static string CredentialIsEnabled => nameof(CredentialIsEnabled);
            public static string CredentialUser => nameof(CredentialUser);
            public static string CredentialPassword => nameof(CredentialPassword);

            #endregion
        }

        #endregion

        #region function

        private SettingAppProxySettingDto ConvertFromData(AppProxySettingData data, IDatabaseCommonStatus commonStatus)
        {
            var dto = new SettingAppProxySettingDto() {
                ProxyIsEnabled = data.ProxyIsEnabled,
                ProxyUrl = data.ProxyUrl,
                CredentialIsEnabled = data.CredentialIsEnabled,
                CredentialUser = data.CredentialUser,
                CredentialPassword = EncryptByCurrentUser(data.CredentialPassword),
            };
            commonStatus.WriteCommonTo(dto);

            return dto;
        }

        public AppProxySettingData SelectProxySetting()
        {
            var statement = LoadStatement();
            var dto = Context.QuerySingle<SettingAppProxySettingDto>(statement);

            return new AppProxySettingData() {
                ProxyIsEnabled = dto.ProxyIsEnabled,
                ProxyUrl = dto.ProxyUrl,
                CredentialIsEnabled = dto.CredentialIsEnabled,
                CredentialUser = dto.CredentialUser,
                CredentialPassword = DecryptByCurrentUser(dto.CredentialPassword),
            };
        }

        public bool SelectProxyIsEnabled()
        {
            var statement = LoadStatement();
            return Context.QuerySingle<bool>(statement);
        }

        public void UpdateProxySetting(AppProxySettingData data, IDatabaseCommonStatus commonStatus)
        {
            var statement = LoadStatement();
            var parameter = ConvertFromData(data, commonStatus);
            Context.UpdateByKey(statement, parameter);
        }

        public void UpdateToggleProxyIsEnabled(IDatabaseCommonStatus commonStatus)
        {
            // トグルって嫌いなんよなぁ
            var statement = LoadStatement();
            var parameter = commonStatus.CreateCommonDtoMapping();
            Context.UpdateByKey(statement, parameter);
        }
        #endregion
    }
}
