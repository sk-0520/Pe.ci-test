using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.Setting
{
    public abstract class KeyboardJobSettingEditorElementBase : ElementBase, IKeyActionId
    {
        public KeyboardJobSettingEditorElementBase(KeyActionData keyActionData, IMainDatabaseBarrier mainDatabaseBarrier, IDatabaseStatementLoader statementLoader, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            KeyActionId = keyActionData.KeyActionId;
            Kind = keyActionData.KeyActionKind;
            Content = keyActionData.KeyActionContent;
            Comment = keyActionData.Comment;

            MainDatabaseBarrier = mainDatabaseBarrier;
            StatementLoader = statementLoader;
        }

        #region property

        protected IMainDatabaseBarrier MainDatabaseBarrier { get; }
        protected IDatabaseStatementLoader StatementLoader { get; }

        public KeyActionKind Kind { get; }
        public string Comment { get; set; }
        public string Content { get; set; }

        public IDictionary<string, string> Options { get; } = new Dictionary<string, string>();
        public ObservableCollection<WrapModel<KeyMappingData>> Mappings { get; } = new ObservableCollection<WrapModel<KeyMappingData>>();
        #endregion

        #region IKeyActionId
        public Guid KeyActionId { get; }
        #endregion

        #region ElementBase

        protected override void InitializeImpl()
        {
            using(var commander = MainDatabaseBarrier.WaitRead()) {
                var keyOptionsEntityDao = new KeyOptionsEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
                var keyMappingsEntityDao = new KeyMappingsEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);

                var options = keyOptionsEntityDao.SelectOptions(KeyActionId);
                var mappings = keyMappingsEntityDao.SelectMappings(KeyActionId);

                foreach(var pair in options) {
                    Options.Add(pair.Key, pair.Value);
                }

                Mappings.AddRange(mappings.Select(i => WrapModel.Create(i, LoggerFactory)));
                if(Mappings.Count == 0) {
                    Logger.LogWarning("マッピングデータが存在しないため補正: {0}", KeyActionId);
                    Mappings.Add(new WrapModel<KeyMappingData>(new KeyMappingData(), LoggerFactory));
                }
            }
        }

        #endregion

    }

    public sealed class KeyboardReplaceJobSettingEditorElement : KeyboardJobSettingEditorElementBase
    {
        public KeyboardReplaceJobSettingEditorElement(KeyActionData keyActionData, IMainDatabaseBarrier mainDatabaseBarrier, IDatabaseStatementLoader statementLoader, ILoggerFactory loggerFactory)
            : base(keyActionData, mainDatabaseBarrier, statementLoader, loggerFactory)
        {
            if(keyActionData.KeyActionKind != KeyActionKind.Replace) {
                throw new ArgumentException(nameof(keyActionData));
            }
        }
    }

    public sealed class KeyboardDisableJobSettingEditorElement : KeyboardJobSettingEditorElementBase
    {
        public KeyboardDisableJobSettingEditorElement(KeyActionData keyActionData, IMainDatabaseBarrier mainDatabaseBarrier, IDatabaseStatementLoader statementLoader, ILoggerFactory loggerFactory)
            : base(keyActionData, mainDatabaseBarrier, statementLoader, loggerFactory)
        {
            if(keyActionData.KeyActionKind != KeyActionKind.Disable) {
                throw new ArgumentException(nameof(keyActionData));
            }
        }
    }

    public sealed class KeyboardPressedJobSettingEditorElement : KeyboardJobSettingEditorElementBase
    {
        public KeyboardPressedJobSettingEditorElement(KeyActionData keyActionData, IMainDatabaseBarrier mainDatabaseBarrier, IDatabaseStatementLoader statementLoader, ILoggerFactory loggerFactory)
            : base(keyActionData, mainDatabaseBarrier, statementLoader, loggerFactory)
        {
            if(keyActionData.KeyActionKind == KeyActionKind.Replace || keyActionData.KeyActionKind == KeyActionKind.Disable) {
                throw new ArgumentException(nameof(keyActionData));
            }
        }
    }

}
