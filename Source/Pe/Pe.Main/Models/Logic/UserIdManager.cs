using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Models.Platform;
using ContentTypeTextNet.Pe.Library.Base;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Logic
{
    public class UserIdManager
    {
        public UserIdManager(ILoggerFactory loggerFactory)
        {
            LoggerFactory = loggerFactory;
            Logger = LoggerFactory.CreateLogger(GetType());
        }

        #region property

        /// <inheritdoc cref="ILoggerFactory"/>
        private ILoggerFactory LoggerFactory { get; }
        /// <inheritdoc cref="ILogger"/>
        private ILogger Logger { get; }

        #endregion

        #region function

        public bool IsValidUserId(string userId)
        {
            if(string.IsNullOrWhiteSpace(userId)) {
                return false;
            }

            if(userId != userId.Trim()) {
                return false;
            }

            return Regex.IsMatch(userId, @"[a-z0-9]{128}", RegexOptions.ExplicitCapture | RegexOptions.Singleline, Timeout.InfiniteTimeSpan);
        }

        private HashAlgorithm CreateHash()
        {
            return SHA512.Create();
        }

        private string ComputeHash(byte[] buffer, int count)
        {
            byte[] hashValue;
            using(var hash = CreateHash()) {
                hashValue = hash.ComputeHash(buffer, 0, count);
            }
            return BitConverter.ToString(hashValue).Replace("-", string.Empty).ToLowerInvariant();
        }

        public string CreateFromRandom()
        {
            var bufferCount = 20 * 1024;
            using var buffer = new ArrayPoolObject<byte>(bufferCount);
            var rand = new Random();
            rand.NextBytes(buffer.Items);
            return ComputeHash(buffer.Items, bufferCount);
        }

        public string CreateFromEnvironment()
        {
            var buffer = new StringBuilder();

            var pi = new PlatformInformation(LoggerFactory);
            buffer.Append(Environment.OSVersion.VersionString);
            buffer.Append(pi.GetUserName());
            buffer.Append(pi.GetCpuCaption());
            //TODO: メモリを追加する
            var a = Encoding.UTF8.GetBytes(buffer.ToString());
            return ComputeHash(a, a.Length);
        }

        public string SafeGetOrCreateUserId(AppExecuteSettingEntityDao appExecuteSettingEntityDao)
        {
            var setting = appExecuteSettingEntityDao.SelectSettingExecuteSetting();
            var userId = setting.UserId;
            if(!IsValidUserId(userId)) {
                Logger.LogInformation("ユーザーIDが存在しないため環境から生成");
                userId = CreateFromEnvironment();
            }

            if(string.IsNullOrWhiteSpace(userId)) {
                Logger.LogInformation("ユーザーIDがダメっぽいのでダミー文字列の投入");
                userId = ":-(";
            }

            return userId;
        }

        #endregion
    }
}
