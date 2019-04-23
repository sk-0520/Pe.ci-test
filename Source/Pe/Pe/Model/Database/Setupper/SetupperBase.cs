using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Embedded.Model;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model.Database;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Data.Dto;

namespace ContentTypeTextNet.Pe.Main.Model.Database.Setupper
{
    public abstract class SetupperBase
    {
        public SetupperBase(IDatabaseStatementLoader statementLoader, ILoggerFactory loggerFactory)
        {
            StatementLoader = statementLoader;
            Logger = loggerFactory.CreateTartget(GetType());
        }

        #region property

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

        protected IEnumerable<KeyValuePair<string, string>> SplitMultiSql(string statement)
        {
            var linePairs = TextUtility.ReadLines(statement)
                .Select((s, i) => new { Line = s, Index = i })
                .ToList()
            ;
            var titleMap = linePairs
                .Where(i => i.Line.StartsWith(TitleMark, StringComparison.Ordinal))
                .Select(i => new {
                    i.Index,
                    Match = TitleRegex.Match(i.Line),
                })
                .Where(i => i.Match.Success)
                .ToDictionary(
                    k => k.Index,
                    v => v.Match.Groups[TitleCapture].Value
                )
            ;
            var indexItems = titleMap.Keys
                .Concat(new[] { linePairs.Last().Index })
                .ToList()
            ;
            var indexLengthMap = new Dictionary<int, int>(titleMap.Count);
            for(var i = 0; i < indexItems.Count - 1; i++) {
                var length = indexItems[i+1] - indexItems[i];
                indexLengthMap.Add(indexItems[i], length);
            }
            Debug.Assert(titleMap.Count == indexLengthMap.Count);

            foreach(var pair in indexLengthMap) {
                var blockLines = linePairs.GetRange(pair.Key, pair.Value);
                var buffer = new StringBuilder(blockLines.Sum(b => b.Line.Length + 2));
                foreach(var line in blockLines) {
                    buffer.AppendLine(line.Line);
                }
                yield return new KeyValuePair<string, string>(titleMap[pair.Key], buffer.ToString());
            }
        }

        protected void ExecuteSql(IDatabaseCommander commander, string statement, IReadOnlySetupDto dto)
        {
            var pairs = SplitMultiSql(statement);
            foreach(var pair in pairs) {
                Logger.Information(pair.Key);
                var result = commander.Execute(pair.Value, dto);
                Logger.Information($"result: {result}");
            }
        }


        #endregion
    }
}
