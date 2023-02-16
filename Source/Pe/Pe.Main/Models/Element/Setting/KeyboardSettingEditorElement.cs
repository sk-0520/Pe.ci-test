using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Standard.Base.Models;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using ContentTypeTextNet.Pe.Main.Models.Manager.Setting;
using Microsoft.Extensions.Logging;
using ContentTypeTextNet.Pe.Standard.Database;

namespace ContentTypeTextNet.Pe.Main.Models.Element.Setting
{
    public class KeyboardSettingEditorElement: SettingEditorElementBase
    {
        #region define

        private abstract class KeyboardJobSettingBaseComparsion: Comparer<KeyboardJobSettingEditorElementBase>
        {
            #region Comparer

            public override int Compare([AllowNull] KeyboardJobSettingEditorElementBase x, [AllowNull] KeyboardJobSettingEditorElementBase y)
            {
                if(y == null) {
                    return -1;
                }
                if(x == null) {
                    return +1;
                }

                Debug.Assert(x.IsInitialized);
                Debug.Assert(y.IsInitialized);

                if(x.Mappings.Count == 0) {
                    return -1;
                }
                if(y.Mappings.Count == 0) {
                    return +1;
                }

                return x.Mappings[0].Data.Key - y.Mappings[0].Data.Key;
            }

            #endregion
        }

        private class ReplaceComparsion: KeyboardJobSettingBaseComparsion
        { }

        private class DisableComparsion: KeyboardJobSettingBaseComparsion
        { }

        #endregion

        public KeyboardSettingEditorElement(ISettingNotifyManager settingNotifyManager, IClipboardManager clipboardManager, IMainDatabaseBarrier mainDatabaseBarrier, ILargeDatabaseBarrier largeDatabaseBarrier, ITemporaryDatabaseBarrier temporaryDatabaseBarrier, IDatabaseStatementLoader databaseStatementLoader, IIdFactory idFactory, IImageLoader imageLoader, IMediaConverter mediaConverter, IPolicy policy, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(settingNotifyManager, clipboardManager, mainDatabaseBarrier, largeDatabaseBarrier, temporaryDatabaseBarrier, databaseStatementLoader, idFactory, imageLoader, mediaConverter, policy, dispatcherWrapper, loggerFactory)
        { }

        #region property

        public ObservableCollection<KeyboardReplaceJobSettingEditorElement> ReplaceJobEditors { get; } = new ObservableCollection<KeyboardReplaceJobSettingEditorElement>();
        public ObservableCollection<KeyboardDisableJobSettingEditorElement> DisableJobEditors { get; } = new ObservableCollection<KeyboardDisableJobSettingEditorElement>();
        public ObservableCollection<KeyboardPressedJobSettingEditorElement> PressedJobEditors { get; } = new ObservableCollection<KeyboardPressedJobSettingEditorElement>();
        /// <summary>
        /// 削除したアイテム。
        /// <para>最後にdelete流してあげる用。</para>
        /// </summary>
        private IList<KeyboardJobSettingEditorElementBase> RemovedJobEditors { get; } = new List<KeyboardJobSettingEditorElementBase>();

        #endregion

        #region function

        private void StockRemoveItemIfSavedJob(KeyboardJobSettingEditorElementBase editor)
        {
            if(!editor.IsNewJob) {
                RemovedJobEditors.Add(editor);
            }
        }


        public void AddReplaceJob()
        {
            var keyActionData = new KeyActionData() {
                KeyActionId = IdFactory.CreateKeyActionId(),
                KeyActionKind = KeyActionKind.Replace,
            };

            var editor = new KeyboardReplaceJobSettingEditorElement(keyActionData, true, MainDatabaseBarrier, DatabaseStatementLoader, LoggerFactory);
            editor.Initialize();
            ReplaceJobEditors.Add(editor);
        }

        public void RemoveReplaceJob(KeyActionId keyActionId)
        {
            var editor = ReplaceJobEditors.First(i => i.KeyActionId == keyActionId);
            ReplaceJobEditors.Remove(editor);
            StockRemoveItemIfSavedJob(editor);
        }

        public void AddDisableJob()
        {
            var keyActionData = new KeyActionData() {
                KeyActionId = IdFactory.CreateKeyActionId(),
                KeyActionKind = KeyActionKind.Disable,
            };

            var editor = new KeyboardDisableJobSettingEditorElement(keyActionData, true, MainDatabaseBarrier, DatabaseStatementLoader, LoggerFactory);
            editor.Initialize();

            DisableJobEditors.Add(editor);
        }

        public void RemoveDisableJob(KeyActionId keyActionId)
        {
            var editor = DisableJobEditors.First(i => i.KeyActionId == keyActionId);
            DisableJobEditors.Remove(editor);
            StockRemoveItemIfSavedJob(editor);
        }

        public void AddPressedJob(KeyActionKind kind)
        {
            if(kind == KeyActionKind.Replace || kind == KeyActionKind.Disable) {
                throw new ArgumentException(nameof(kind));
            }

            var keyActionData = new KeyActionData() {
                KeyActionId = IdFactory.CreateKeyActionId(),
                KeyActionKind = kind,
            };

            var editor = new KeyboardPressedJobSettingEditorElement(keyActionData, true, MainDatabaseBarrier, DatabaseStatementLoader, LoggerFactory);
            editor.Initialize();

            PressedJobEditors.Add(editor);
        }

        public void RemovePressedJob(KeyActionId keyActionId)
        {
            var editor = PressedJobEditors.First(i => i.KeyActionId == keyActionId);
            PressedJobEditors.Remove(editor);
            StockRemoveItemIfSavedJob(editor);
        }

        #endregion

        #region SettingEditorElementBase

        protected override void LoadImpl()
        {
            IReadOnlyList<KeyActionData> replaceKeyActions;
            IReadOnlyList<KeyActionData> disableKeyActions;
            IReadOnlyList<KeyActionData> pressedKeyActions;
            using(var context = MainDatabaseBarrier.WaitRead()) {
                var keyActionsEntityDao = new KeyActionsEntityDao(context, DatabaseStatementLoader, context.Implementation, LoggerFactory);
                replaceKeyActions = keyActionsEntityDao.SelectAllKeyActionsFromKind(KeyActionKind.Replace).ToList();
                disableKeyActions = keyActionsEntityDao.SelectAllKeyActionsFromKind(KeyActionKind.Disable).ToList();
                pressedKeyActions = keyActionsEntityDao.SelectAllKeyActionsIgnoreKinds(new[] { KeyActionKind.Replace, KeyActionKind.Disable }).ToList();
            }

            var replaceJobEditor = replaceKeyActions.Select(i => new KeyboardReplaceJobSettingEditorElement(i, false, MainDatabaseBarrier, DatabaseStatementLoader, LoggerFactory)).ToArray();
            var disableJobEditor = disableKeyActions.Select(i => new KeyboardDisableJobSettingEditorElement(i, false, MainDatabaseBarrier, DatabaseStatementLoader, LoggerFactory)).ToArray();
            var pressedJobEditor = pressedKeyActions.Select(i => new KeyboardPressedJobSettingEditorElement(i, false, MainDatabaseBarrier, DatabaseStatementLoader, LoggerFactory)).ToArray();

            var editors = replaceJobEditor
                .Cast<KeyboardJobSettingEditorElementBase>()
                .Concat(disableJobEditor)
                .Concat(pressedJobEditor)
            ;
            foreach(var editor in editors) {
                editor.Initialize();
            }

            Array.Sort(replaceJobEditor, new ReplaceComparsion());
            Array.Sort(disableJobEditor, new DisableComparsion());

            ReplaceJobEditors.SetRange(replaceJobEditor);
            DisableJobEditors.SetRange(disableJobEditor);
            PressedJobEditors.SetRange(pressedJobEditor);
        }


        protected override void SaveImpl(IDatabaseContextsPack contextsPack)
        {
            var jobs = ReplaceJobEditors
                .Cast<KeyboardJobSettingEditorElementBase>()
                .Concat(DisableJobEditors)
                .Concat(PressedJobEditors)
            ;
            foreach(var job in jobs) {
                job.Save(contextsPack.Main.Context, contextsPack.Main.Implementation, contextsPack.CommonStatus);
            }
            foreach(var job in RemovedJobEditors) {
                job.Remove(contextsPack.Main.Context, contextsPack.Main.Implementation);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(disposing) {
                    foreach(var item in ReplaceJobEditors) {
                        item.Dispose();
                    }
                    ReplaceJobEditors.Clear();

                    foreach(var item in DisableJobEditors) {
                        item.Dispose();
                    }
                    DisableJobEditors.Clear();

                    foreach(var item in PressedJobEditors) {
                        item.Dispose();
                    }
                    PressedJobEditors.Clear();
                }
            }
            base.Dispose(disposing);
        }

        #endregion
    }

}
