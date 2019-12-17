using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Data.Dto;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Setupper
{
    public abstract class SetupperBase
    {
        public SetupperBase(IIdFactory idFactory, IDatabaseStatementLoader statementLoader, ILoggerFactory loggerFactory)
        {
            IdFactory = idFactory;
            StatementLoader = statementLoader;
            Logger = loggerFactory.CreateLogger(GetType());
        }

        #region property

        protected IIdFactory IdFactory { get; }
        protected IDatabaseStatementLoader StatementLoader { get; }
        protected ILogger Logger { get; }

        public abstract Version Version { get; }

        const string TitleMark = "--//";
        const string TitleCapture = "TITLE";
        Regex TitleRegex { get; } = new Regex($@"^{TitleMark}\s*(?<{TitleCapture}>.+)", RegexOptions.ExplicitCapture);

        #endregion

        #region function

        public abstract void ExecuteMainDDL(IDatabaseCommander commander, IReadOnlySetupDto dto);
        public abstract void ExecuteMainDML(IDatabaseCommander commander, IReadOnlySetupDto dto);

        public abstract void ExecuteFileDDL(IDatabaseCommander commander, IReadOnlySetupDto dto);
        public abstract void ExecuteFileDML(IDatabaseCommander commander, IReadOnlySetupDto dto);

        public abstract void ExecuteTemporaryDDL(IDatabaseCommander commander, IReadOnlySetupDto dto);
        public abstract void ExecuteTemporaryDML(IDatabaseCommander commander, IReadOnlySetupDto dto);

        protected IEnumerable<KeyValuePair<string, string>> SplitMultiStatement(string statement)
        {
            var linePairs = TextUtility.ReadLines(statement)
                .Counting()
                .ToList()
            ;
            var titleMap = linePairs
                .Where(i => i.Value.StartsWith(TitleMark, StringComparison.Ordinal))
                .Select(i => new {
                    i.Number,
                    Match = TitleRegex.Match(i.Value),
                })
                .Where(i => i.Match.Success)
                .ToDictionary(
                    k => k.Number,
                    v => v.Match.Groups[TitleCapture].Value
                )
            ;
            var indexItems = titleMap.Keys
                .Concat(new[] { linePairs.Last().Number })
                .ToList()
            ;
            var indexLengthMap = new Dictionary<int, int>(titleMap.Count);
            for(var i = 0; i < indexItems.Count - 1; i++) {
                var length = indexItems[i + 1] - indexItems[i];
                indexLengthMap.Add(indexItems[i], length);
            }
            Debug.Assert(titleMap.Count == indexLengthMap.Count);

            foreach(var pair in indexLengthMap) {
                var blockLines = linePairs.GetRange(pair.Key, pair.Value);
                var buffer = new StringBuilder(blockLines.Sum(b => b.Value.Length + 2));
                foreach(var line in blockLines) {
                    buffer.AppendLine(line.Value);
                }
                yield return new KeyValuePair<string, string>(titleMap[pair.Key], buffer.ToString());
            }
        }

        void ExecuteStatementCore(IDatabaseCommander commander, string statement, IReadOnlyDictionary<string, object> parameters)
        {
            var pairs = SplitMultiStatement(statement);
            foreach(var pair in pairs) {
                Logger.LogInformation(pair.Key);
                var result = commander.Execute(pair.Value, parameters);
                Logger.LogInformation("result: {0}", result);
            }
        }

        protected void ExecuteStatement(IDatabaseCommander commander, string statement, IReadOnlySetupDto dto)
        {
            var properties = dto.GetType().GetProperties();
            var parameters = new Dictionary<string, object>(properties.Length);

            foreach(var property in properties) {
                var rawValue = property.GetValue(dto);
                parameters.Add(property.Name, rawValue!);
            }

            ExecuteStatementCore(commander, statement, parameters);
        }

        protected void ExecuteStatement(IDatabaseCommander commander, string statement, IReadOnlySetupDto dto, IReadOnlyDictionary<string, object> mergeParameters)
        {
            var properties = dto.GetType().GetProperties();
            var parameters = new Dictionary<string, object>(properties.Length + mergeParameters.Count);

            foreach(var property in properties) {
                var rawValue = property.GetValue(dto);
                parameters.Add(property.Name, rawValue!);
            }
            foreach(var pair in mergeParameters) {
                parameters[pair.Key] = pair.Value;
            }

            ExecuteStatementCore(commander, statement, parameters);
        }


        #endregion
    }
}
