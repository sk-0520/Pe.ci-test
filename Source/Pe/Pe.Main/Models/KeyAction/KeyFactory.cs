using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Library.Base;
using ContentTypeTextNet.Pe.Library.Database;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.KeyAction
{
    public class KeyActionFactory
    {
        public KeyActionFactory(IMainDatabaseBarrier mainDatabaseBarrier, IDatabaseStatementLoader databaseStatementLoader, ILoggerFactory loggerFactory)
        {
            MainDatabaseBarrier = mainDatabaseBarrier;
            DatabaseStatementLoader = databaseStatementLoader;
            LoggerFactory = loggerFactory;
            Logger = LoggerFactory.CreateLogger(GetType());
        }

        #region property

        private IMainDatabaseBarrier MainDatabaseBarrier { get; }
        private IDatabaseStatementLoader DatabaseStatementLoader { get; }

        /// <inheritdoc cref="ILoggerFactory"/>
        private ILoggerFactory LoggerFactory { get; }
        /// <inheritdoc cref="ILogger"/>
        private ILogger Logger { get; }

        #endregion

        #region function

        private KeyItem CreateKeyItem(KeyActionData keyAction, KeyOptionsEntityDao keyOptionsEntityDao, KeyMappingsEntityDao keyMappingsEntityDao)
        {
            var options = keyOptionsEntityDao.SelectOptions(keyAction.KeyActionId);
            var mappings = keyMappingsEntityDao.SelectMappings(keyAction.KeyActionId);

            var result = new KeyItem(
                keyAction,
                options.ToDictionary(i => i.Key, i => i.Value),
                mappings.ToList()
            );

            return result;
        }

        private IReadOnlyList<KeyItem> LoadKeyItems(KeyActionKind keyActionKind)
        {
            var result = new List<KeyItem>();

            using(var context = MainDatabaseBarrier.WaitRead()) {
                var keyActionsEntityDao = new KeyActionsEntityDao(context, DatabaseStatementLoader, context.Implementation, LoggerFactory);
                var keyOptionsEntityDao = new KeyOptionsEntityDao(context, DatabaseStatementLoader, context.Implementation, LoggerFactory);
                var keyMappingsEntityDao = new KeyMappingsEntityDao(context, DatabaseStatementLoader, context.Implementation, LoggerFactory);

                foreach(var keyAction in keyActionsEntityDao.SelectAllKeyActionsFromKind(keyActionKind)) {
                    var keyItem = CreateKeyItem(keyAction, keyOptionsEntityDao, keyMappingsEntityDao);
                    result.Add(keyItem);
                }
            }

            return result;
        }

        private IReadOnlyList<KeyItem> LoadKeyActionPressedData()
        {
            var result = new List<KeyItem>();

            var noPressedKinds = new[] { KeyActionKind.Replace, KeyActionKind.Disable };

            using(var context = MainDatabaseBarrier.WaitRead()) {
                var keyActionsEntityDao = new KeyActionsEntityDao(context, DatabaseStatementLoader, context.Implementation, LoggerFactory);
                var keyOptionsEntityDao = new KeyOptionsEntityDao(context, DatabaseStatementLoader, context.Implementation, LoggerFactory);
                var keyMappingsEntityDao = new KeyMappingsEntityDao(context, DatabaseStatementLoader, context.Implementation, LoggerFactory);
                foreach(var keyAction in keyActionsEntityDao.SelectAllKeyActionsIgnoreKinds(noPressedKinds)) {
                    var keyItem = CreateKeyItem(keyAction, keyOptionsEntityDao, keyMappingsEntityDao);
                    result.Add(keyItem);
                }
            }

            return result;
        }

        private IEnumerable<TJob> CreateJobs<TJob>(IReadOnlyList<KeyItem> items, Func<KeyActionId, KeyItem, TJob> func)
        {
            foreach(var item in items) {
                TJob result;
                try {
                    result = func(item.Action.KeyActionId, item);
                } catch(Exception ex) {
                    Logger.LogError(ex, "{Message} {KeyActionId}", ex.Message, item.Action.KeyActionId);
                    continue;
                }
                yield return result;
            }
        }

        //KeyMappingData ConvertKeyMapping(KeyActionData data)
        //{
        //    return new KeyMappingData() {
        //        Key = data.Key,
        //        Shift = data.Shift,
        //        Control = data.Contrl,
        //        Alt = data.Alt,
        //        Super = data.Super,
        //    };
        //}

        public IEnumerable<KeyActionReplaceJob> CreateReplaceJobs()
        {
            var items = LoadKeyItems(KeyActionKind.Replace);
            return CreateJobs(items, (id, item) => {
                //var replaceOptionConverter = new ReplaceOptionConverter();
                var keyConverter = new KeyConverter();
                var data = new KeyActionReplaceData(
                    item.Action.KeyActionId,
                    (Key)keyConverter.ConvertFromInvariantString(item.Action.KeyActionContent)!
                );

                return new KeyActionReplaceJob(data, item.Mappings.First());
            });
        }

        public IEnumerable<KeyActionDisableJob> CreateDisableJobs()
        {
            var items = LoadKeyItems(KeyActionKind.Disable);
            return CreateJobs(items, (id, item) => {
                var disableOptionConverter = new DisableOptionConverter();
                var data = new KeyActionDisableData(
                    item.Action.KeyActionId,
                    disableOptionConverter.ToForever(item.Options)
                );

                return new KeyActionDisableJob(data, item.Mappings.First());
            });
        }

        private KeyActionCommandJob CreateCommandJob(KeyItem item)
        {
            var pressedOptionConverter = new PressedOptionConverter();

            var data = new KeyActionCommandData(item.Action.KeyActionId);

            data.ThroughSystem = pressedOptionConverter.ToThroughSystem(item.Options);

            return new KeyActionCommandJob(data, item.Mappings);
        }

        private KeyActionLauncherItemJob CreateLauncherItemJob(KeyItem item)
        {
            var launcherItemContentConverter = new LauncherItemContentConverter();
            var launcherItemOptionConverter = new LauncherItemOptionConverter();

            var data = new KeyActionLauncherItemData(
                item.Action.KeyActionId,
                launcherItemContentConverter.ToKeyActionContentLauncherItem(item.Action.KeyActionContent),
                launcherItemOptionConverter.ToLauncherItemId(item.Options)
            );

            data.ThroughSystem = launcherItemOptionConverter.ToThroughSystem(item.Options);

            return new KeyActionLauncherItemJob(data, item.Mappings);
        }

        private KeyActionLauncherToolbarJob CreateLauncherToolbarJob(KeyItem item)
        {
            var launcherToolbarContentConverter = new LauncherToolbarContentConverter();
            var pressedOptionConverter = new PressedOptionConverter();

            var data = new KeyActionLauncherToolbarData(
                item.Action.KeyActionId,
                launcherToolbarContentConverter.ToKeyActionContentLauncherToolbar(item.Action.KeyActionContent)
            );

            data.ThroughSystem = pressedOptionConverter.ToThroughSystem(item.Options);

            return new KeyActionLauncherToolbarJob(data, item.Mappings);
        }

        private KeyActionNoteJob CreateNoteJob(KeyItem item)
        {
            var noteContentConverter = new NoteContentConverter();
            var pressedOptionConverter = new PressedOptionConverter();

            var data = new KeyActionNoteData(
                item.Action.KeyActionId,
                noteContentConverter.ToKeyActionContentNote(item.Action.KeyActionContent)
            );

            data.ThroughSystem = pressedOptionConverter.ToThroughSystem(item.Options);

            return new KeyActionNoteJob(data, item.Mappings);
        }

        public IEnumerable<KeyActionPressedJobBase> CreatePressedJobs()
        {
            var items = LoadKeyActionPressedData();
            return CreateJobs(items, (id, item) => {
                KeyActionPressedJobBase job = item.Action.KeyActionKind switch {
                    KeyActionKind.Command => CreateCommandJob(item),
                    KeyActionKind.LauncherItem => CreateLauncherItemJob(item),
                    KeyActionKind.LauncherToolbar => CreateLauncherToolbarJob(item),
                    KeyActionKind.Note => CreateNoteJob(item),
                    _ => throw new NotImplementedException(),
                };
                return job;
            });
        }

        #endregion
    }

    public class KeyMappingFactory
    {
        #region property

        public int MappingStep { get; } = 10;

        #endregion

        #region function

        public string ToString(ICultureService cultureService, ModifierKeys key, ModifierKey modifierKey)
        {
            if(modifierKey == ModifierKey.None) {
                return string.Empty;
            }

            if(modifierKey == ModifierKey.Any) {
                return cultureService.GetString(key, ResourceNameKind.Normal, true);
            }

            return TextUtility.ReplaceFromDictionary(
                Properties.Resources.String_Hook_Keyboard_ModifierFormat,
                new Dictionary<string, string>() {
                    ["KEY"] = cultureService.GetString(key, ResourceNameKind.Normal, true),
                    ["MOD"] = cultureService.GetString(modifierKey, ResourceNameKind.Normal, true)
                }
            );
        }

        public string ToString(ICultureService cultureService, IReadOnlyKeyMappingData data, string join)
        {
            var key = cultureService.GetString(data.Key, ResourceNameKind.Normal, true);

            var mods = new (ModifierKeys key, ModifierKey modifier)[] {
                (key: ModifierKeys.Control, modifier: data.Control),
                (key: ModifierKeys.Shift, modifier: data.Shift),
                (key: ModifierKeys.Alt, modifier: data.Alt),
                (key: ModifierKeys.Windows, modifier: data.Super),
            };

            var sb = new StringBuilder();

            foreach(var mod in mods) {
                if(mod.modifier != ModifierKey.None) {
                    sb.Append(ToString(cultureService, mod.key, mod.modifier));
                    sb.Append(join);
                }
            }

            sb.Append(key);

            return sb.ToString();
        }

        #endregion
    }
}
