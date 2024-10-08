using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Library.Base.Linq;

namespace ContentTypeTextNet.Pe.Main.Models.KeyAction
{
    public abstract class KeyActionJobBase
    {
        protected KeyActionJobBase(KeyActionCommonData commonData, IEnumerable<IReadOnlyKeyMappingData> mappings)
        {
            CommonData = commonData;
            Mappings = mappings.ToList();
        }

        #region property

        public KeyActionCommonData CommonData { get; }
        protected IReadOnlyList<IReadOnlyKeyMappingData> Mappings { get; }

        #endregion

        #region function

        /// <summary>
        /// 装飾キー位置と入力されている装飾キーをチェック。
        /// </summary>
        /// <param name="modifierKey">装飾キー位置。</param>
        /// <param name="state">装飾キー状態。</param>
        /// <returns>入力されているか否かの真偽値。</returns>
        private bool TestModifierKey(ModifierKey modifierKey, in ModifierKeyState state)
        {
            return modifierKey switch {
                ModifierKey.None => !state.Left && !state.Right,
                ModifierKey.Left => state.Left && !state.Right,
                ModifierKey.Right => !state.Left && state.Right,
                ModifierKey.Any => state.Left || state.Right,
                ModifierKey.All => state.Left && state.Right,
                _ => throw new NotImplementedException(),
            };
        }

        /// <summary>
        /// キー入力チェック。
        /// </summary>
        /// <param name="mapping">キー・装飾キー位置</param>
        /// <param name="isDown">押下されているか。</param>
        /// <param name="key">入力キー。</param>
        /// <param name="modifierKeyStatus">入力装飾キー。</param>
        /// <returns></returns>
        protected bool TestMapping(IReadOnlyKeyMappingData mapping, bool isDown, Key key, in ModifierKeyStatus modifierKeyStatus)
        {
            if(mapping.Key != key) {
                return false;
            }
            if(!TestModifierKey(mapping.Shift, modifierKeyStatus.shift)) {
                return false;
            }
            if(!TestModifierKey(mapping.Control, modifierKeyStatus.control)) {
                return false;
            }
            if(!TestModifierKey(mapping.Alt, modifierKeyStatus.alt)) {
                return false;
            }
            if(!TestModifierKey(mapping.Super, modifierKeyStatus.super)) {
                return false;
            }

            return true;
        }

        public abstract void Reset();

        public abstract bool Check(bool isDown, Key key, in ModifierKeyStatus modifierKeyStatus);

        #endregion
    }

    public abstract class KeyActionJobBase<TActionData>: KeyActionJobBase
        where TActionData : KeyActionCommonData
    {
        protected KeyActionJobBase(TActionData actionData, IEnumerable<IReadOnlyKeyMappingData> mappings)
            : base(actionData, mappings)
        {
            ActionData = actionData;
        }

        #region property

        public TActionData ActionData { get; }

        #endregion
    }

    public sealed class KeyActionReplaceJob: KeyActionJobBase<KeyActionReplaceData>
    {
        public KeyActionReplaceJob(KeyActionReplaceData actionData, IReadOnlyKeyMappingData mapping)
            : base(actionData, new[] { mapping })
        {
            if(ActionData.ReplaceKey == System.Windows.Input.Key.None) {
                throw new ArgumentException(null, nameof(actionData) + "." + nameof(actionData.ReplaceKey));
            }
            if(ActionData.ReplaceKey == Mappings[0].Key) {
                throw new ArgumentException(nameof(actionData.ReplaceKey) + " == " + nameof(IReadOnlyKeyMappingData.Key), nameof(mapping));
            }
            if(ActionData.ReplaceKey.IsModifierKey()) {
                var mods = new[] {
                    mapping.Shift,
                    mapping.Alt,
                    mapping.Control,
                    mapping.Super,
                };
                if(mods.Any(i => i != ModifierKey.None)) {
                    throw new ArgumentException("setting: any none");
                }
            } else if(Mapping.Key == Key.None) {
                throw new ArgumentException(null, nameof(mapping) + "." + nameof(mapping.Key));
            }
        }

        #region property

        public IReadOnlyKeyMappingData Mapping => Mappings[0];

        #endregion

        #region function
        #endregion

        #region KeyActionJobBase

        public override void Reset()
        { }

        public override bool Check(bool isDown, Key key, in ModifierKeyStatus modifierKeyStatus)
        {
            if(!isDown) {
                return false;
            }

            var mapping = Mappings[0];
            var result = TestMapping(mapping, isDown, key, modifierKeyStatus);
            return result;
        }
        #endregion
    }

    public sealed class KeyActionDisableJob: KeyActionJobBase<KeyActionDisableData>
    {
        public KeyActionDisableJob(KeyActionDisableData actionData, IReadOnlyKeyMappingData mapping)
            : base(actionData, new[] { mapping })
        {
            if(mapping.Key == System.Windows.Input.Key.None) {
                var mods = new[] {
                    mapping.Shift,
                    mapping.Alt,
                    mapping.Control,
                    mapping.Super,
                };
                if(mods.All(i => i == ModifierKey.None)) {
                    throw new ArgumentException("setting: all none");
                }
            }
        }

        #region property

        /// <summary>
        /// 最後にチェック対象とした時間。
        /// </summary>
        [DateTimeKind(DateTimeKind.Utc)]
        public DateTime LastCheckTimestamp { get; private set; } = DateTime.MinValue.ToUniversalTime();

        #endregion

        #region function
        #endregion

        #region KeyActionJobBase

        public override void Reset()
        {
            LastCheckTimestamp = DateTime.MinValue.ToUniversalTime();
        }

        public override bool Check(bool isDown, Key key, in ModifierKeyStatus modifierKeyStatus)
        {
            if(!isDown) {
                return false;
            }

            var mapping = Mappings[0];
            var result = TestMapping(mapping, isDown, key, modifierKeyStatus);
            if(result) {
                LastCheckTimestamp = DateTime.UtcNow;
            }
            return result;
        }

        #endregion
    }

    public abstract class KeyActionPressedJobBase: KeyActionJobBase<KeyActionPressedDataBase>
    {
        protected KeyActionPressedJobBase(KeyActionPressedDataBase actionData, IEnumerable<IReadOnlyKeyMappingData> mappings)
            : base(actionData, mappings)
        {
            if(Mappings.Count == 0) {
                throw new ArgumentException(null, nameof(mappings));
            }

            var keyIsNoneOrMods = Mappings.Counting().Where(i => i.Value.Key == Key.None || i.Value.Key.IsModifierKey()).ToList();
            if(keyIsNoneOrMods.Any()) {
                var errors = string.Join(", ", keyIsNoneOrMods.Select(i => $"{nameof(mappings)}[{i.Number}]"));
                throw new ArgumentException("不正なキー設定(キー設定なし、修飾キーのみ): " + errors);
            }

            ThroughSystem = actionData.ThroughSystem;
        }

        #region property

        /// <summary>
        /// 次に調べるインデックス。
        /// </summary>
        private int NextIndex { get; set; }
        /// <summary>
        /// キー設定にヒットしたか。
        /// </summary>
        /// <remarks>
        /// <para><see cref="Check"/>ではあくまで今のキー設定にヒットしたかどうかを確認するのでキー設定全てに該当したかを判定するために使用する。</para>
        /// </remarks>
        public bool IsAllHit { get; private set; }
        /// <summary>
        /// 次キー入力待ちか。
        /// </summary>
        public bool NextWaiting => 0 < NextIndex;

        /// <summary>
        /// OSへキー入力を伝達させるか。
        /// </summary>
        /// <remarks>
        /// <para>基本的には伝達しないが特別な状況でこれを認めたい場合に有効にする。 他の<see cref="KeyActionPressedJobBase"/>が伝達を抑制していても優先される。</para>
        /// <para>Pe の過去機能で ESC 2回押下でツールーバーを隠す処理を再現する場合など、一度目のキー入力は通常操作で使用する場合などが有効にしたい目的。</para>
        /// </remarks>
        public bool ThroughSystem { get; private set; }

        #endregion

        #region function

        public IEnumerable<IReadOnlyKeyMappingData> GetCurrentMappings()
        {
            return Mappings.Take(NextIndex);
        }

        #endregion

        #region KeyActionJobBase

        public override void Reset()
        {
            NextIndex = 0;
            IsAllHit = false;
        }

        /// <summary>
        /// </summary>
        /// <remarks>
        /// <para>有効であるかどうかは<see cref="IsAllHit"/>を確認すること。</para>
        /// </remarks>
        /// <returns></returns>
        public override bool Check(bool isDown, Key key, in ModifierKeyStatus modifierKeyStatus)
        {
            if(!isDown) {
                return false;
            }

            // 修飾キーのみは無視する
            if(key.IsModifierKey()) {
                return false;
            }

            var mapping = Mappings[NextIndex];
            var result = TestMapping(mapping, isDown, key, modifierKeyStatus);
            if(!result) {
                Reset();
                return false;
            }
            if(NextIndex + 1 == Mappings.Count) {
                Reset();
                IsAllHit = true;
            } else {
                NextIndex += 1;
            }

            return true;
        }

        #endregion
    }

    public abstract class KeyActionPressedJobBase<TActionData>: KeyActionPressedJobBase
        where TActionData : KeyActionPressedDataBase
    {
        protected KeyActionPressedJobBase(TActionData actionData, IEnumerable<IReadOnlyKeyMappingData> mappings)
            : base(actionData, mappings)
        {
            PressedData = actionData;

            if(Mappings.Count == 0) {
                throw new ArgumentException(null, nameof(mappings));
            }

            var keyIsNoneOrMods = Mappings.Counting().Where(i => i.Value.Key == Key.None || i.Value.Key.IsModifierKey()).ToList();
            if(keyIsNoneOrMods.Any()) {
                var errors = string.Join(", ", keyIsNoneOrMods.Select(i => $"{nameof(mappings)}[{i.Number}]"));
                throw new ArgumentException("不正なキー設定(キー設定なし、修飾キーのみ): " + errors);
            }
        }

        #region property

        public TActionData PressedData { get; }

        #endregion
    }

    public sealed class KeyActionPressJob: KeyActionPressedJobBase<KeyActionPressedDataBase>
    {
        public KeyActionPressJob(KeyActionPressedDataBase actionData, IEnumerable<IReadOnlyKeyMappingData> mappings)
            : base(actionData, mappings)
        { }
    }

    public sealed class KeyActionCommandJob: KeyActionPressedJobBase<KeyActionCommandData>
    {
        public KeyActionCommandJob(KeyActionCommandData actionData, IEnumerable<IReadOnlyKeyMappingData> mappings)
            : base(actionData, mappings)
        {
        }

        #region property
        #endregion
    }

    public sealed class KeyActionLauncherItemJob: KeyActionPressedJobBase<KeyActionLauncherItemData>
    {
        public KeyActionLauncherItemJob(KeyActionLauncherItemData actionData, IEnumerable<IReadOnlyKeyMappingData> mappings)
            : base(actionData, mappings)
        { }

        #region property
        #endregion
    }

    public sealed class KeyActionLauncherToolbarJob: KeyActionPressedJobBase<KeyActionLauncherToolbarData>
    {
        public KeyActionLauncherToolbarJob(KeyActionLauncherToolbarData actionData, IEnumerable<IReadOnlyKeyMappingData> mappings)
            : base(actionData, mappings)
        { }

        #region property
        #endregion
    }

    public sealed class KeyActionNoteJob: KeyActionPressedJobBase<KeyActionNoteData>
    {
        public KeyActionNoteJob(KeyActionNoteData actionData, IEnumerable<IReadOnlyKeyMappingData> mappings)
            : base(actionData, mappings)
        { }
    }
}
