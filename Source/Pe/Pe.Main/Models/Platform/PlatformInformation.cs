using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Platform
{
    /// <summary>
    /// システム情報取得。
    /// </summary>
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
                var userName = userPrincipal.DisplayName;
                //#514対応: NULL の可能性あり
                if(!string.IsNullOrEmpty(userName)) {
                    return userName;
                }
            } catch(Exception ex) {
                Logger.LogWarning(ex, ex.Message);
            }

            // Windows ログオンユーザー名
            return Environment.UserName;

        }

        public string? GetCpuCaption()
        {
            var platformInformationCollector = new PlatformInformationCollector();

            var cpu = platformInformationCollector.GetCPU();
            return cpu.FirstOrDefault(i => i.Key == "Caption").Value as string;
        }

        #endregion
    }
}
