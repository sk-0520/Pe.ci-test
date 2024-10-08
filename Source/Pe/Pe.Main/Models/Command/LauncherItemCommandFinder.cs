using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Models.Element.Command;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherItem;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using ContentTypeTextNet.Pe.Main.Models.Plugin;
using Microsoft.Extensions.Logging;
using ContentTypeTextNet.Pe.Library.Database;
using ContentTypeTextNet.Pe.Library.Base;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Library.Base.Linq;

namespace ContentTypeTextNet.Pe.Main.Models.Command
{
    public class LauncherItemCommandFinder: DisposerBase, ICommandFinder
    {
        public LauncherItemCommandFinder(IMainDatabaseBarrier mainDatabaseBarrier, IDatabaseStatementLoader databaseStatementLoader, IOrderManager orderManager, INotifyManager notifyManager, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
        {
            LoggerFactory = loggerFactory;
            Logger = LoggerFactory.CreateLogger(GetType());

            MainDatabaseBarrier = mainDatabaseBarrier;
            DatabaseStatementLoader = databaseStatementLoader;
            OrderManager = orderManager;
            NotifyManager = notifyManager;
            DispatcherWrapper = dispatcherWrapper;
        }

        #region property

        /// <inheritdoc cref="ILoggerFactory"/>
        private ILoggerFactory LoggerFactory { get; }
        /// <inheritdoc cref="ILogger"/>
        private ILogger Logger { get; }

        private IMainDatabaseBarrier MainDatabaseBarrier { get; }
        private IDatabaseStatementLoader DatabaseStatementLoader { get; }
        private IOrderManager OrderManager { get; }
        private INotifyManager NotifyManager { get; }
        private IDispatcherWrapper DispatcherWrapper { get; }

        internal IconBox IconBox { get; set; }

        private IList<LauncherItemElement> LauncherItemElements { get; } = new List<LauncherItemElement>();
        private IDictionary<LauncherItemId, LauncherItemElement> LauncherItemElementMap { get; } = new Dictionary<LauncherItemId, LauncherItemElement>();
        private IDictionary<LauncherItemId, IReadOnlyCollection<string>> LauncherTags { get; } = new Dictionary<LauncherItemId, IReadOnlyCollection<string>>();


        #endregion

        #region function

        public void ClearIcon()
        {
            //foreach(var element in LauncherItemElements) {
            //    element.Icon!.IconImageLoaderPack.ClearCache();
            //}
        }

        private async Task<ICommandItem?> GetHitItemAsync(CommandItemKind kind, LauncherItemElement element, string targetValue, string targetLogName, string input, Regex inputRegex, IHitValuesCreator hitValuesCreator, CancellationToken cancellationToken)
        {
            var nameMatches = hitValuesCreator.GetMatches(targetValue, inputRegex);
            if(nameMatches.Any()) {
                Logger.LogTrace("ランチャー: {0}, {1}, {2}", targetLogName, targetValue, element.LauncherItemId);
                var result = new LauncherCommandItemElement(element, DispatcherWrapper, LoggerFactory) {
                    EditableKind = kind,
                };
                await result.InitializeAsync(cancellationToken);
                var ranges = hitValuesCreator.ConvertRanges(nameMatches);
                var hitValue = hitValuesCreator.ConvertHitValues(targetValue, ranges);
                if(kind == CommandItemKind.LauncherItemName) {
                    result.EditableHeaderValues.SetRange(hitValue);
                    result.EditableScore = hitValuesCreator.CalcScore(targetValue, result.EditableHeaderValues);
                } else {
                    result.EditableDescriptionValues.SetRange(hitValue);
                    result.EditableScore = hitValuesCreator.CalcScore(targetValue, result.EditableDescriptionValues);
                }
                return result;
            }

            return null;
        }

        private void AddItem(LauncherItemElement launcherItemElement)
        {
            LauncherItemElements.Add(launcherItemElement);
            LauncherItemElementMap.Add(launcherItemElement.LauncherItemId, launcherItemElement);
            LoadTag(launcherItemElement.LauncherItemId);
        }

        private void LoadTag(LauncherItemId launcherItemId)
        {
            // タグ情報再構築
            Logger.LogTrace("タグ情報再構築");
            var tags = MainDatabaseBarrier.ReadData(c => {
                var launcherTagsEntityDao = new LauncherTagsEntityDao(c, DatabaseStatementLoader, c.Implementation, LoggerFactory);
                return launcherTagsEntityDao.SelectUniqueTags(launcherItemId).ToHashSet();
            });
            LauncherTags.Remove(launcherItemId);
            if(tags.Any()) {
                LauncherTags.Add(launcherItemId, tags);
            }
        }

        #endregion

        #region ICommandFinder

        public bool IsInitialized { get; private set; }

        public void Initialize()
        {
            if(IsInitialized) {
                throw new InvalidOperationException(nameof(IsInitialized));
            }

            NotifyManager.LauncherItemChanged += NotifyManager_LauncherItemChanged;
            NotifyManager.LauncherItemRegistered += NotifyManager_LauncherItemRegistered;

            IsInitialized = true;
        }

        public void Refresh(IPluginContext pluginContext)
        {
            Debug.Assert(pluginContext.GetType() == typeof(NullPluginContext));

            if(!IsInitialized) {
                throw new InvalidOperationException(nameof(IsInitialized));
            }

            IReadOnlyList<LauncherItemId> ids;
            using(var context = MainDatabaseBarrier.WaitRead()) {
                var launcherItemsEntityDao = new LauncherItemsEntityDao(context, DatabaseStatementLoader, context.Implementation, LoggerFactory);
                ids = launcherItemsEntityDao.SelectAllLauncherItemIds().ToList();
            }

            var launcherItemElements = ids
                .Select(i => OrderManager.GetOrCreateLauncherItemElement(i))
                .Where(i => i.IsEnabledCommandLauncher)
                .Where(a => a.Kind != LauncherItemKind.Separator)
                .ToList()
            ;
            LauncherItemElements.SetRange(launcherItemElements);
            LauncherItemElementMap.Clear();
            foreach(var LauncherItemElement in LauncherItemElements) {
                LauncherItemElementMap.Add(LauncherItemElement.LauncherItemId, LauncherItemElement);
            }

            var tagItems = new Dictionary<LauncherItemId, IReadOnlyCollection<string>>(ids.Count);
            using(var context = MainDatabaseBarrier.WaitRead()) {
                var launcherTagsEntityDao = new LauncherTagsEntityDao(context, DatabaseStatementLoader, context.Implementation, LoggerFactory);
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
        }

        public async IAsyncEnumerable<ICommandItem> EnumerateCommandItemsAsync(string inputValue, Regex inputRegex, IHitValuesCreator hitValuesCreator, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            if(!IsInitialized) {
                throw new InvalidOperationException(nameof(IsInitialized));
            }

            if(string.IsNullOrWhiteSpace(inputValue)) {
                var items = LauncherItemElements
                    .Select(i => new LauncherCommandItemElement(i, DispatcherWrapper, LoggerFactory))
                ;
                foreach(var item in items) {
                    yield return item;
                }
                yield break;
            }

            foreach(var element in LauncherItemElements) {
                cancellationToken.ThrowIfCancellationRequested();
                var nameItem = await GetHitItemAsync(CommandItemKind.LauncherItemName, element, element.Name, "名前一致", inputValue, inputRegex, hitValuesCreator, cancellationToken);
                if(nameItem != null) {
                    yield return nameItem;
                    continue;
                }

                if(LauncherTags.TryGetValue(element.LauncherItemId, out var tags)) {
                    foreach(var tag in tags) {
                        var tagItem = await GetHitItemAsync(CommandItemKind.LauncherItemTag, element, tag, "タグ", inputValue, inputRegex, hitValuesCreator, cancellationToken);
                        if(tagItem != null) {
                            yield return tagItem;
                            continue;
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
            Debug.Assert(IsInitialized);

            var element = LauncherItemElements.FirstOrDefault(i => i.LauncherItemId == e.LauncherItemId);
            if(element != null) {
                if(!element.IsEnabledCommandLauncher) {
                    Logger.LogInformation("コマンドランチャーから既存ランチャーアイテムの除外: {0}", element.LauncherItemId);
                    LauncherItemElements.Remove(element);
                    LauncherItemElementMap.Remove(element.LauncherItemId);
                    LauncherTags.Remove(element.LauncherItemId);
                } else {
                    LoadTag(e.LauncherItemId);
                }
            } else {
                // 該当アイテムの投入
                var newElement = OrderManager.GetOrCreateLauncherItemElement(e.LauncherItemId);
                AddItem(newElement);
            }
        }

        private void NotifyManager_LauncherItemRegistered(object? sender, LauncherItemRegisteredEventArgs e)
        {
            Debug.Assert(IsInitialized);

            var element = OrderManager.GetOrCreateLauncherItemElement(e.LauncherItemId);
            if(element.IsEnabledCommandLauncher) {
                Logger.LogInformation("コマンドランチャーへ新規ランチャーアイテムの追加: {0}", element.LauncherItemId);
                AddItem(element);
            }
        }
    }
}
