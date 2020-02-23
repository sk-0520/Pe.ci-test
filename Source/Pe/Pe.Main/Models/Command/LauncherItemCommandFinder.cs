using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Models.Element.Command;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherItem;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Command
{
    public class LauncherItemCommandFinder : DisposerBase, ICommandFinder
    {
        public LauncherItemCommandFinder(IMainDatabaseBarrier mainDatabaseBarrier, IDatabaseStatementLoader statementLoader, IOrderManager orderManager, INotifyManager notifyManager, ILoggerFactory loggerFactory)
        {
            LoggerFactory = loggerFactory;
            Logger = LoggerFactory.CreateLogger(GetType());

            MainDatabaseBarrier = mainDatabaseBarrier;
            StatementLoader = statementLoader;
            OrderManager = orderManager;
            NotifyManager = notifyManager;

            NotifyManager.LauncherItemChanged += NotifyManager_LauncherItemChanged;
            NotifyManager.LauncherItemRegistered += NotifyManager_LauncherItemRegistered;
        }

        #region property

        ILoggerFactory LoggerFactory { get; }
        ILogger Logger { get; }

        IMainDatabaseBarrier MainDatabaseBarrier { get; }
        IDatabaseStatementLoader StatementLoader { get; }
        IOrderManager OrderManager { get; }
        INotifyManager NotifyManager { get; }
        internal bool FindTag { get; set; }
        internal IconBox IconBox { get; set; }

        IList<LauncherItemElement> LauncherItemElements { get; } = new List<LauncherItemElement>();
        IDictionary<Guid, LauncherItemElement> LauncherItemElementMap { get; } = new Dictionary<Guid, LauncherItemElement>();
        IDictionary<Guid, IReadOnlyCollection<string>> LauncherTags { get; } = new Dictionary<Guid, IReadOnlyCollection<string>>();


        #endregion

        #region function

        public void ClearIcon()
        {
            foreach(var element in LauncherItemElements) {
                element.Icon.IconImageLoaderPack.IconItems[IconBox].ClearCache();
            }
        }

        #endregion

        #region ICommandFinder

        public void Refresh()
        {
            IReadOnlyList<Guid> ids;
            using(var commander = MainDatabaseBarrier.WaitRead()) {
                var launcherItemsEntityDao = new LauncherItemsEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
                ids = launcherItemsEntityDao.SelectAllLauncherItemIds().ToList();
            }

            var launcherItemElements = ids
                .Select(i => OrderManager.GetOrCreateLauncherItemElement(i))
                .Where(i => i.IsEnabledCommandLauncher)
                .ToList();
            ;
            LauncherItemElements.SetRange(launcherItemElements);
            LauncherItemElementMap.Clear();
            foreach(var LauncherItemElement in LauncherItemElements) {
                LauncherItemElementMap.Add(LauncherItemElement.LauncherItemId, LauncherItemElement);
            }

            if(FindTag) {
                var tagItems = new Dictionary<Guid, IReadOnlyCollection<string>>(ids.Count);
                using(var commander = MainDatabaseBarrier.WaitRead()) {
                    var launcherTagsEntityDao = new LauncherTagsEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
                    foreach(var id in ids) {
                        var tags = launcherTagsEntityDao.SelectUniqueTags(id).ToHashSet();
                        if(tags.Count != 0) {
                            tagItems.Add(id, tags);
                        }
                    }
                }
                LauncherTags.Clear();
                foreach(var pair in tagItems) {
                    LauncherTags.Add(pair.Key, pair.Value);
                }
            } else {
                LauncherTags.Clear();
            }
        }

        public IEnumerable<ICommandItem> ListupCommandItems(string inputValue, Regex inputRegex, IHitValuesCreator hitValuesCreator, CancellationToken cancellationToken)
        {
            if(string.IsNullOrWhiteSpace(inputValue)) {
                var items = LauncherItemElements
                    .Select(i => new LauncherCommandItemElement(i, LoggerFactory))
                ;
                foreach(var item in items) {
                    yield return item;
                }
                yield break;
            }

            foreach(var element in LauncherItemElements) {
                cancellationToken.ThrowIfCancellationRequested();

                var nameMatches = hitValuesCreator.GetMatches(inputRegex, element.Name);
                if(nameMatches.Any()) {
                    Logger.LogTrace("ランチャー: 名前一致, {0}, {1}", element.Name, element.LauncherItemId);
                    var result = new LauncherCommandItemElement(element, LoggerFactory) {
                        EditableKind = CommandItemKind.LauncherItemName,
                    };
                    result.Initialize();
                    var ranges = hitValuesCreator.ConvertRanges(nameMatches);
                    var hitValue = hitValuesCreator.ConvertHitValues(element.Name, ranges);
                    result.EditableHeaderValues.SetRange(hitValue);
                    yield return result;
                    continue;
                }

                var codeMatches = hitValuesCreator.GetMatches(inputRegex, element.Code);
                if(codeMatches.Any()) {
                    Logger.LogTrace("ランチャー: コード一致, {0}, {1}", element.Code, element.LauncherItemId);
                    var result = new LauncherCommandItemElement(element, LoggerFactory) {
                        EditableKind = CommandItemKind.LauncherItemCode,
                    };
                    result.Initialize();

                    var ranges = hitValuesCreator.ConvertRanges(codeMatches);
                    var hitValue = hitValuesCreator.ConvertHitValues(element.Code, ranges);
                    result.EditableDescriptionValues.SetRange(hitValue);
                    yield return result;
                    continue;
                }

                if(FindTag) {
                    if(LauncherTags.TryGetValue(element.LauncherItemId, out var tags)) {
                        foreach(var tag in tags) {
                            var tagMatches = hitValuesCreator.GetMatches(inputRegex, tag);
                            if(tagMatches.Any()) {
                                Logger.LogTrace("ランチャー: タグ, {0}, {1}", tag, element.LauncherItemId);
                                var result = new LauncherCommandItemElement(element, LoggerFactory) {
                                    EditableKind = CommandItemKind.LauncherItemTag,
                                };
                                result.Initialize();

                                var ranges = hitValuesCreator.ConvertRanges(tagMatches);
                                var hitValue = hitValuesCreator.ConvertHitValues(tag, ranges);
                                result.EditableDescriptionValues.SetRange(hitValue);

                                yield return result;
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region DisposerBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                NotifyManager.LauncherItemChanged -= NotifyManager_LauncherItemChanged;
                NotifyManager.LauncherItemRegistered -= NotifyManager_LauncherItemRegistered;
            }

            base.Dispose(disposing);
        }

        #endregion

        private void NotifyManager_LauncherItemChanged(object? sender, LauncherItemChangedEventArgs e)
        {
            var element = LauncherItemElements.FirstOrDefault(i => i.LauncherItemId == e.LauncherItemId);
            if(element != null) {
                element.Icon.IconImageLoaderPack.IconItems[IconBox].ClearCache();
                if(element.IsEnabledCommandLauncher) {
                    Logger.LogInformation("コマンドランチャーから既存ランチャーアイテムの除外: {0}", element.LauncherItemId);
                    LauncherItemElements.Remove(element);
                }
            }
        }

        private void NotifyManager_LauncherItemRegistered(object? sender, LauncherItemRegisteredEventArgs e)
        {
            var element = OrderManager.GetOrCreateLauncherItemElement(e.LauncherItemId);
            if(element.IsEnabledCommandLauncher) {
                Logger.LogInformation("コマンドランチャーへ新規ランチャーアイテムの追加: {0}", element.LauncherItemId);
                LauncherItemElements.Add(element);
            }
        }
    }
}
