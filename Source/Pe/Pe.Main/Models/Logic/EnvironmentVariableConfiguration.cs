using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ICSharpCode.AvalonEdit.Document;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Logic
{
    public class EnvironmentVariableConfiguration
    {
        public EnvironmentVariableConfiguration(ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger(GetType());
        }

        #region property

        ILogger Logger { get; }

        #endregion

        #region function

        public IReadOnlyList<LauncherEnvironmentVariableData> GetMergeItems(TextDocument textDocument)
        {
            return TextUtility.ReadLines(textDocument.Text)
                .Where(i => !string.IsNullOrWhiteSpace(i))
                .Select(i => i.Split(new char[] { '=' }, 2, StringSplitOptions.RemoveEmptyEntries).Select(i => i.Trim()).ToArray())
                .Where(i => i.Length == 2)
                .Select(i => new LauncherEnvironmentVariableData() { Name = i[0], Value = i[1] })
                .ToList()
            ;
        }

        public IReadOnlyList<string> GetRemoveItems(TextDocument textDocument)
        {
            return TextUtility.ReadLines(textDocument.Text)
                .Where(i => !string.IsNullOrWhiteSpace(i))
                .Select(i => i.Trim())
                .ToList()
            ;
        }

        public IReadOnlyList<LauncherEnvironmentVariableData> Join(IEnumerable<LauncherEnvironmentVariableData> mergeItems, IEnumerable<string> removeItems)
        {
            var envVarItems = mergeItems.ToList();
            foreach(var item in removeItems) {
                var index = envVarItems.FindIndex(i => i.Name == item);
                if(index != -1) {
                    envVarItems.RemoveAt(index);
                }
                envVarItems.Add(new LauncherEnvironmentVariableData() {
                    Name = item
                });
            }
            return envVarItems;
        }

        public TextDocument CreateMergeDocument(IEnumerable<LauncherEnvironmentVariableData> items)
        {
            var mergeItems = items
                .Where(i => !i.IsRemove)
                .Select(i => $"{i.Name}={i.Value}")
            ;

            return new TextDocument(string.Join(Environment.NewLine, mergeItems));
        }

        public TextDocument CreateRemoveDocument(IEnumerable<LauncherEnvironmentVariableData> items)
        {
            var removeItems = items
                .Where(i => i.IsRemove)
                .Select(i => i.Name)
            ;

            return new TextDocument(string.Join(Environment.NewLine, removeItems));
        }

        #endregion
    }
}
