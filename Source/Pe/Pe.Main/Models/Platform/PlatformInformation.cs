using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Text;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Platform
{
    public class PlatformInformation
    {
        public PlatformInformation(ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger(GetType());
        }

        #region property

        ILogger Logger { get; }

        #endregion

        #region function

        /// <summary>
        /// ユーザー名を取得。
        /// </summary>
        /// <returns></returns>
        public string GetUserName()
        {
            // アカウント情報のユーザー名(取れなくてもいい)
            try {
                var userPrincipal = UserPrincipal.Current;
                return userPrincipal.DisplayName;
            } catch(Exception ex) {
                Logger.LogWarning(ex, ex.Message);
            }

            // Windows ログオンユーザー名
            return Environment.UserName;

        }

        #endregion
    }
}
