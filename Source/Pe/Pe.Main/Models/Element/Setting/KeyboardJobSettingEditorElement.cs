using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Models.KeyAction;
using Microsoft.Extensions.Logging;
using ContentTypeTextNet.Pe.Library.Database;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Base.Linq;
using System.Threading;

namespace ContentTypeTextNet.Pe.Main.Models.Element.Setting
{
    public abstract class KeyboardJobSettingEditorElementBase: ElementBase, IKeyActionId
    {
        protected KeyboardJobSettingEditorElementBase(KeyActionData keyActionData, bool isNewJob, IMainDatabaseBarrier mainDatabaseBarrier, IDatabaseStatementLoader databaseStatementLoader, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            ActionData = keyActionData;

            IsNewJob = isNewJob;

            MainDatabaseBarrier = mainDatabaseBarrier;
            DatabaseStatementLoader = databaseStatementLoader;
        }

        #region property

        protected KeyActionData ActionData { get; }

        protected IMainDatabaseBarrier MainDatabaseBarrier { get; }
        protected IDatabaseStatementLoader DatabaseStatementLoader { get; }

        public bool IsNewJob { get; }

        public KeyActionKind Kind => ActionData.KeyActionKind;
        public string Comment
        {
            get => ActionData.Comment;
            set => ActionData.Comment = value;
        }
        public string Content
        {
            get => ActionData.KeyActionContent;
            set => ActionData.KeyActionContent = value;
        }

        public Dictionary<string, string> Options { get; } = new Dictionary<string, string>();
        public ObservableCollection<KeyMappingData> Mappings { get; } = new ObservableCollection<KeyMappingData>();

        #endregion

        #region IKeyActionId
        public KeyActionId KeyActionId => ActionData.KeyActionId;
        #endregion

        #region ElementBase

        protected override Task InitializeCoreAsync(CancellationToken cancellationToken)
        {
            using(var context = MainDatabaseBarrier.WaitRead()) {
                var keyOptionsEntityDao = new KeyOptionsEntityDao(context, DatabaseStatementLoader, context.Implementation, LoggerFactory);
                var keyMappingsEntityDao = new KeyMappingsEntityDao(context, DatabaseStatementLoader, context.Implementation, LoggerFactory);

                var options = keyOptionsEntityDao.SelectOptions(KeyActionId);
                var mappings = keyMappingsEntityDao.SelectMappings(KeyActionId);

                foreach(var pair in options) {
                    Options.Add(pair.Key, pair.Value);
                }

                if(mappings.Any()) {
                    Mappings.AddRange(mappings);
                }

                if(Mappings.Count == 0) {
                    if(!IsNewJob) {
                        Logger.LogWarning("マッピングデータが存在しないため補正: {0}", KeyActionId);
                    }
                    Mappings.Add(new KeyMappingData());
                }
            }

            return Task.CompletedTask;
        }


        public void Save(IDatabaseContext context, IDatabaseImplementation implementation, IDatabaseCommonStatus commonStatus)
        {
            var keyActionsEntityDao = new KeyActionsEntityDao(context, DatabaseStatementLoader, implementation, LoggerFactory);
            var keyOptionsEntityDao = new KeyOptionsEntityDao(context, DatabaseStatementLoader, implementation, LoggerFactory);
            var keyMappingsEntityDao = new KeyMappingsEntityDao(context, DatabaseStatementLoader, implementation, LoggerFactory);

            if(IsNewJob) {
                keyActionsEntityDao.InsertKeyAction(ActionData, commonStatus);
            } else {
                keyActionsEntityDao.UpdateKeyAction(ActionData, commonStatus);
                keyOptionsEntityDao.DeleteByKeyActionId(ActionData.KeyActionId);
                keyMappingsEntityDao.DeleteByKeyActionId(ActionData.KeyActionId);
            }

            foreach(var pair in Options) {
                keyOptionsEntityDao.InsertOption(ActionData.KeyActionId, pair.Key, pair.Value, commonStatus);
            }

            var keyMappingFactory = new KeyMappingFactory();
            foreach(var mapping in Mappings.Counting()) {
                var seq = keyMappingFactory.MappingStep * mapping.Number;
                keyMappingsEntityDao.InsertMapping(ActionData.KeyActionId, mapping.Value, seq, commonStatus);
            }

        }

        public void Remove(IDatabaseContext context, IDatabaseImplementation implementation)
        {
            var keyActionsEntityDao = new KeyActionsEntityDao(context, DatabaseStatementLoader, implementation, LoggerFactory);
            var keyOptionsEntityDao = new KeyOptionsEntityDao(context, DatabaseStatementLoader, implementation, LoggerFactory);
            var keyMappingsEntityDao = new KeyMappingsEntityDao(context, DatabaseStatementLoader, implementation, LoggerFactory);

            keyMappingsEntityDao.DeleteByKeyActionId(KeyActionId);
            keyOptionsEntityDao.DeleteByKeyActionId(KeyActionId);
            keyActionsEntityDao.DeleteKeyAction(KeyActionId);
        }

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(disposing) {
                    Mappings.Clear();
                }
            }

            base.Dispose(disposing);
        }

        #endregion

    }

    public sealed class KeyboardReplaceJobSettingEditorElement: KeyboardJobSettingEditorElementBase
    {
        public KeyboardReplaceJobSettingEditorElement(KeyActionData keyActionData, bool isNewJob, IMainDatabaseBarrier mainDatabaseBarrier, IDatabaseStatementLoader databaseStatementLoader, ILoggerFactory loggerFactory)
            : base(keyActionData, isNewJob, mainDatabaseBarrier, databaseStatementLoader, loggerFactory)
        {
            if(keyActionData.KeyActionKind != KeyActionKind.Replace) {
                throw new ArgumentException(null, nameof(keyActionData));
            }
        }
    }

    public sealed class KeyboardDisableJobSettingEditorElement: KeyboardJobSettingEditorElementBase
    {
        public KeyboardDisableJobSettingEditorElement(KeyActionData keyActionData, bool isNewJob, IMainDatabaseBarrier mainDatabaseBarrier, IDatabaseStatementLoader databaseStatementLoader, ILoggerFactory loggerFactory)
            : base(keyActionData, isNewJob, mainDatabaseBarrier, databaseStatementLoader, loggerFactory)
        {
            if(keyActionData.KeyActionKind != KeyActionKind.Disable) {
                throw new ArgumentException(null, nameof(keyActionData));
            }
        }

        #region KeyboardJobSettingEditorElementBase

        protected override async Task InitializeCoreAsync(CancellationToken cancellationToken)
        {
            await base.InitializeCoreAsync(cancellationToken);

            var doc = new DisableOptionConverter();
            if(!doc.TryGetForever(Options, out _)) {
                doc.SetForever(Options, false);
            }
        }

        #endregion
    }

    public sealed class KeyboardPressedJobSettingEditorElement: KeyboardJobSettingEditorElementBase
    {
        public KeyboardPressedJobSettingEditorElement(KeyActionData keyActionData, bool isNewJob, IMainDatabaseBarrier mainDatabaseBarrier, IDatabaseStatementLoader databaseStatementLoader, ILoggerFactory loggerFactory)
            : base(keyActionData, isNewJob, mainDatabaseBarrier, databaseStatementLoader, loggerFactory)
        {
            if(keyActionData.KeyActionKind == KeyActionKind.Replace || keyActionData.KeyActionKind == KeyActionKind.Disable) {
                throw new ArgumentException(null, nameof(keyActionData));
            }
        }

        #region function

        public void AddMapping()
        {
            var mapping = new KeyMappingData();
            Mappings.Add(mapping);
        }

        public void RemoveMappingAt(int index)
        {
            Mappings.RemoveAt(index);
        }

        public void MoveMapping(int oldIndex, int newIndex)
        {
            Mappings.Move(oldIndex, newIndex);
        }

        #endregion

        #region KeyboardJobSettingEditorElementBase

        protected override async Task InitializeCoreAsync(CancellationToken cancellationToken)
        {
            await base.InitializeCoreAsync(cancellationToken);

            var poc = new PressedOptionConverter();
            if(!poc.TryGetThroughSystem(Options, out _)) {
                poc.SetThroughSystem(Options, false);
            }
        }

        #endregion
    }

}
