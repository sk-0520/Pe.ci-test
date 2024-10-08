using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using Microsoft.Extensions.Logging;
using ContentTypeTextNet.Pe.Library.Database;
using ContentTypeTextNet.Pe.Library.Base;
using ContentTypeTextNet.Pe.Library.Base.Linq;
using System.Threading;
using System.Reflection;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Setupper
{
    public abstract class SetupperBase
    {
        protected SetupperBase(IIdFactory idFactory, IDatabaseStatementLoader statementLoader, ILoggerFactory loggerFactory)
        {
            IdFactory = idFactory;
            StatementLoader = statementLoader;
            Logger = loggerFactory.CreateLogger(GetType());
        }

        #region property

        protected IIdFactory IdFactory { get; }
        protected IDatabaseStatementLoader StatementLoader { get; }
        protected ILogger Logger { get; }

        private const string TitleMark = "--//";
        private const string TitleCapture = "TITLE";
        private Regex TitleRegex { get; } = new Regex($@"^{TitleMark}\s*(?<{TitleCapture}>.+)", RegexOptions.ExplicitCapture, Timeout.InfiniteTimeSpan);

        public Version Version
        {
            get
            {
                var databaseSetupVersion = GetType().GetCustomAttribute<DatabaseSetupVersionAttribute>();
                Throws.ThrowIfNull(databaseSetupVersion);
                return databaseSetupVersion.Version;
            }
        }

        #endregion

        #region function

        public abstract void ExecuteMainDDL(IDatabaseContext context, IReadOnlySetupDto dto);
        public abstract void ExecuteMainDML(IDatabaseContext context, IReadOnlySetupDto dto);

        public abstract void ExecuteFileDDL(IDatabaseContext context, IReadOnlySetupDto dto);
        public abstract void ExecuteFileDML(IDatabaseContext context, IReadOnlySetupDto dto);

        public abstract void ExecuteTemporaryDDL(IDatabaseContext context, IReadOnlySetupDto dto);
        public abstract void ExecuteTemporaryDML(IDatabaseContext context, IReadOnlySetupDto dto);

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

        private void ExecuteStatementCore(IDatabaseContext context, string statement, IReadOnlyDictionary<string, object> parameters)
        {
            var pairs = SplitMultiStatement(statement);
            foreach(var pair in pairs) {
                Logger.LogInformation("{0}", pair.Key);
                var result = context.Execute(pair.Value, parameters);
                Logger.LogInformation("result: {0}", result);
            }
        }

        protected void ExecuteStatement(IDatabaseContext context, string statement, IReadOnlySetupDto dto)
        {
            var properties = dto.GetType().GetProperties();
            var parameters = new Dictionary<string, object>(properties.Length);

            foreach(var property in properties) {
                var rawValue = property.GetValue(dto);
                parameters.Add(property.Name, rawValue!);
            }

            ExecuteStatementCore(context, statement, parameters);
        }

        protected void ExecuteStatement(IDatabaseContext context, string statement, IReadOnlySetupDto dto, IReadOnlyDictionary<string, object> mergeParameters)
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

            ExecuteStatementCore(context, statement, parameters);
        }

        #endregion
    }
}
