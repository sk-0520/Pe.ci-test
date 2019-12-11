using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using ContentTypeTextNet.Pe.Main.Models.Platform;
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

        ILoggerFactory LoggerFactory { get; }
        ILogger Logger { get; }

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

            return Regex.IsMatch(userId, @"[a-z0-9]{128}", RegexOptions.ExplicitCapture | RegexOptions.Singleline);
        }

        HashAlgorithm CreateHash()
        {
            return SHA512.Create();
        }

        string ComputeHash(byte[] buffer)
        {
            byte[] hashValue;
            using(var hash = CreateHash()) {
                hashValue = hash.ComputeHash(buffer);
            }
            return BitConverter.ToString(hashValue).Replace("-", string.Empty).ToLowerInvariant();
        }

        public string CreateFromRandom()
        {
            var buffer = new byte[256 * 1024];
            var rand = new Random();
            rand.NextBytes(buffer);
            return ComputeHash(buffer);
        }

        public string CreateFromEnvironment()
        {
            var pi = new PlatformInformation(LoggerFactory);
            var userName = pi.GetUserName();
            //TODO: CPUとメモリを追加する
            var a = Encoding.UTF8.GetBytes(userName);
            return ComputeHash(a);
        }

        #endregion
    }
}
