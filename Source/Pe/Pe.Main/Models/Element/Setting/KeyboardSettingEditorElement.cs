using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using ContentTypeTextNet.Pe.Main.Models.Manager.Setting;
using Microsoft.Extensions.Logging;
using ContentTypeTextNet.Pe.Library.Database;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Base.Linq;
using System.Threading;

namespace ContentTypeTextNet.Pe.Main.Models.Element.Setting
{
    public class KeyboardSettingEditorElement: SettingEditorElementBase
    {
        #region define

        private abstract class KeyboardJobSettingBaseComparer: Comparer<KeyboardJobSettingEditorElementBase>
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

                return x.Mappings[0].Key - y.Mappings[0].Key;
            }

            #endregion
        }

        private sealed class ReplaceComparer: KeyboardJobSettingBaseComparer
        { }

        private sealed class DisableComparer: KeyboardJobSettingBaseComparer
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
        /// </summary>
        /// <remarks>
        /// <para>最後に delete 流してあげる用。</para>
        /// </remarks>
        private IList<KeyboardJobSettingEditorElementBase> RemovedJobEditors { get; } = new List<KeyboardJobSettingEditorElementBase>();

        #endregion

        #region function

        private void StockRemoveItemIfSavedJob(KeyboardJobSettingEditorElementBase editor)
        {
            if(!editor.IsNewJob) {
                RemovedJobEditors.Add(editor);
            }
        }


        public async Task AddReplaceJobAsync(CancellationToken cancellationToken)
        {
            var keyActionData = new KeyActionData() {
                KeyActionId = IdFactory.CreateKeyActionId(),
                KeyActionKind = KeyActionKind.Replace,
            };

            var editor = new KeyboardReplaceJobSettingEditorElement(keyActionData, true, MainDatabaseBarrier, DatabaseStatementLoader, LoggerFactory);
            await editor.InitializeAsync(cancellationToken);
            ReplaceJobEditors.Add(editor);
        }

        public void RemoveReplaceJob(KeyActionId keyActionId)
        {
            var editor = ReplaceJobEditors.First(i => i.KeyActionId == keyActionId);
            ReplaceJobEditors.Remove(editor);
            StockRemoveItemIfSavedJob(editor);
        }

        public async Task AddDisableJobAsync(CancellationToken cancellationToken)
        {
            var keyActionData = new KeyActionData() {
                KeyActionId = IdFactory.CreateKeyActionId(),
                KeyActionKind = KeyActionKind.Disable,
            };

            var editor = new KeyboardDisableJobSettingEditorElement(keyActionData, true, MainDatabaseBarrier, DatabaseStatementLoader, LoggerFactory);
            await editor.InitializeAsync(cancellationToken);

            DisableJobEditors.Add(editor);
        }

        public void RemoveDisableJob(KeyActionId keyActionId)
        {
            var editor = DisableJobEditors.First(i => i.KeyActionId == keyActionId);
            DisableJobEditors.Remove(editor);
            StockRemoveItemIfSavedJob(editor);
        }

        public async Task AddPressedJobAsync(KeyActionKind kind, CancellationToken cancellationToken)
        {
            if(kind == KeyActionKind.Replace || kind == KeyActionKind.Disable) {
                throw new ArgumentException(null, nameof(kind));
            }

            var keyActionData = new KeyActionData() {
                KeyActionId = IdFactory.CreateKeyActionId(),
                KeyActionKind = kind,
            };

            var editor = new KeyboardPressedJobSettingEditorElement(keyActionData, true, MainDatabaseBarrier, DatabaseStatementLoader, LoggerFactory);
            await editor.InitializeAsync(cancellationToken);

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

        protected override async Task LoadCoreAsync(CancellationToken cancellationToken)
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
                await editor.InitializeAsync(cancellationToken);
            }

            Array.Sort(replaceJobEditor, new ReplaceComparer());
            Array.Sort(disableJobEditor, new DisableComparer());

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
