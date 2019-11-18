using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Domain;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.KeyAction
{
    public class KeyActionFactory
    {
        public KeyActionFactory(IMainDatabaseBarrier mainDatabaseBarrier, IDatabaseStatementLoader statementLoader, ILoggerFactory loggerFactory)
        {
            MainDatabaseBarrier = mainDatabaseBarrier;
            StatementLoader = statementLoader;
            LoggerFactory = loggerFactory;
            Logger = LoggerFactory.CreateLogger(GetType());
        }

        #region property

        IMainDatabaseBarrier MainDatabaseBarrier { get; }
        IDatabaseStatementLoader StatementLoader { get; }

        ILoggerFactory LoggerFactory { get; }
        ILogger Logger { get; }


        #endregion

        #region function

        IReadOnlyList<KeyActionData> LoadKeyActionData(KeyActionKind keyActionKind)
        {
            IReadOnlyList<KeyActionData> result;

            using(var commander = MainDatabaseBarrier.WaitRead()) {
                var dao = new KeyActionDomainDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
                result = dao.SelectAllKeyActionsFromKind(keyActionKind).ToList();
            }

            return result;
        }

        IReadOnlyList<KeyActionData> LoadKeyActionPressedData()
        {
            IReadOnlyList<KeyActionData> result;

            var noPressedKinds = new[] { KeyActionKind.Replace, KeyActionKind.Disable };

            using(var commander = MainDatabaseBarrier.WaitRead()) {
                var dao = new KeyActionDomainDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
                result = dao.SelectAllKeyActionsIgnoreKinds(noPressedKinds).ToList();
            }

            return result;
        }

        IEnumerable<TJob> CreateJobs<TJob>(IReadOnlyList<KeyActionData> items, Func<Guid, IReadOnlyList<KeyActionData>, TJob> func)
        {
            var groups = items.GroupBy(i => i.KeyActionId);
            foreach(var group in groups) {
                var jobItems = group.ToList();
                TJob result;
                try {
                    result = func(group.Key, jobItems);
                } catch(Exception ex) {
                    Logger.LogError(ex, ex.Message + " {0}", group.Key);
                    continue;
                }
                yield return result;
            }
        }

        KeyMappingData ConvertKeyMapping(KeyActionData data)
        {
            return new KeyMappingData() {
                Key = data.Key,
                Shift = data.Shift,
                Control = data.Contrl,
                Alt = data.Alt,
                Super = data.Super,
            };
        }

        public IEnumerable<KeyActionReplaceJob> CreateReplaceJobs()
        {
            var items = LoadKeyActionData(KeyActionKind.Replace);
            return CreateJobs(items, (id, items) => {
                if(1 < items.Count) {
                    Logger.LogWarning("置き換え処理に不要な設定項目あり: {0}", id);
                }
                var keyConverter = new KeyConverter();
                var item = items[0];
                var data = new KeyActionReplaceData(item.KeyActionId, (Key)keyConverter.ConvertFromInvariantString(item.KeyActionContent));
                var mapping = ConvertKeyMapping(item);

                return new KeyActionReplaceJob(data, mapping);
            });
        }

        public IEnumerable<KeyActionDisableJob> CreateDisableJobs()
        {
            var items = LoadKeyActionData(KeyActionKind.Disable);
            return CreateJobs(items, (id, items) => {
                if(1 < items.Count) {
                    Logger.LogWarning("入力無効処理に不要な設定項目あり: {0}", id);
                }
                var item = items[0];
                var data = new KeyActionDisableData(item.KeyActionId, Convert.ToBoolean(item.KeyActionOption));
                var mapping = ConvertKeyMapping(item);

                return new KeyActionDisableJob(data, mapping);
            });
        }

        KeyActionLauncherItemJob CreateLauncherItemJob(IReadOnlyList<KeyActionData> items)
        {
            var keyActionContentLauncherItemTransfer = new EnumTransfer<KeyActionContentLauncherItem>();

            var item = items[0];
            var data = new KeyActionLauncherItemData(item.KeyActionId, keyActionContentLauncherItemTransfer.ToEnum(item.KeyActionContent), Guid.Parse(item.KeyActionOption));
            var mapping = items.Select(i => ConvertKeyMapping(i));
            return new KeyActionLauncherItemJob(data, mapping);
        }

        public IEnumerable<KeyActionPressedJobBase> CreatePressedJobs()
        {
            var items = LoadKeyActionPressedData();
            return CreateJobs(items, (id, items) => {
                var baseItem = items[0];
                KeyActionPressedJobBase job = baseItem.KeyActionKind switch
                {
                    KeyActionKind.LauncherItem => CreateLauncherItemJob(items),
                    _ => throw new NotImplementedException(),
                };
                return job;
            });
        }

        #endregion
    }
}
