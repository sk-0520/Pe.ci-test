using System;
using System.Collections.Generic;
using System.Linq;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.Models.Platform;
using ContentTypeTextNet.Pe.Library.Base;

namespace ContentTypeTextNet.Pe.Main.Models.Logic
{
    public class PowerShellArguments
    {
        #region function

        /// <summary>
        /// PowerShell で使用するパラメータ形式に変換する。
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public KeyValuePair<string, string> Create(KeyValuePair<string, string> input)
        {
            if(string.IsNullOrWhiteSpace(input.Key)) {
                throw new ArgumentException(nameof(input.Key), nameof(input));
            }
            if(input.Key.IndexOf(' ') != -1) {
                throw new ArgumentException(nameof(input.Key), nameof(input));
            }

            static string Escape(string value)
            {
                if(string.IsNullOrEmpty(value)) {
                    return "\"" + value + "\"";
                }

                return CommandLine.Escape(value);
            }

            if(input.Key.StartsWith('-')) {
                return KeyValuePair.Create(input.Key, Escape(input.Value));
            }

            return KeyValuePair.Create("-" + input.Key, Escape(input.Value));
        }

        /// <inheritdoc cref="Create(KeyValuePair{string, string})"/>
        public KeyValuePair<string, string> Create(string key, string value)
        {
            return Create(KeyValuePair.Create(key, value));
        }


        /// <summary>
        /// キーと値の並びでパラメータとして扱えるデータを生成
        /// </summary>
        /// <param name="isExecutable">実行可能状態パラメータを付与するか。</param>
        /// <param name="keyValues"></param>
        /// <returns>戻り値は好きにこねくり回してOK。</returns>
        internal List<string> CreateParameters(bool isExecutable, IEnumerable<KeyValuePair<string, string>> keyValues)
        {
            var parameters = keyValues
                .Select(i => Create(i))
                .Select(i => new[] { i.Key, i.Value }) // ここで配列作らんでもなんかいけないものか
                .SelectMany(i => i)
            ;

            if(isExecutable) {
                var list = new List<string>() {
                    "-NoProfile",
                    "-ExecutionPolicy", "Unrestricted",
                };
                list.AddRange(parameters);
                return list;
            }

            return parameters.ToList();
        }

        /// <summary>
        /// 残りパラメータとして安全な形に加工。
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public string ToRemainingValue(string s)
        {
            return "\"\"\"" + s.Replace("\"", "\"\"") + "\"\"\"";
        }

        internal IEnumerable<string> ConvertOptions() => ConvertOptions(Environment.GetCommandLineArgs().Skip(1));
        internal IEnumerable<string> ConvertOptions(IEnumerable<string> optionsWithoutProgramName)
        {
            return optionsWithoutProgramName.Select(s => ToRemainingValue(s));
        }

        internal IResultSuccess<string> GetPowerShellFromCommandName(EnvironmentExecuteFile environmentExecuteFile)
        {
            var executeFiles = environmentExecuteFile.GetPathExecuteFiles();
            var pwsh = environmentExecuteFile.Get("pwsh", executeFiles);
            var powershell = environmentExecuteFile.Get("powershell", executeFiles);

            if(pwsh == null && powershell == null) {
                return Result.CreateFailure<string>();
            }


            var s = pwsh?.File.FullName ?? powershell!.File.FullName;
            return Result.CreateSuccess(s);
        }

        #endregion
    }
}
