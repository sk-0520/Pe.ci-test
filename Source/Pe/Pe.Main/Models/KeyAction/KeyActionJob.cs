using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Logic;

namespace ContentTypeTextNet.Pe.Main.Models.KeyAction
{
    public abstract class KeyActionJobBase
    {
        public KeyActionJobBase(IReadOnlyKeyActionCommonData commonData, IEnumerable<IReadOnlyKeyMappingData> mappings)
        {
            CommonData = commonData;
            Mappings = mappings.ToList();
        }

        #region property

        public IReadOnlyKeyActionCommonData CommonData { get; }
        protected IReadOnlyList<IReadOnlyKeyMappingData> Mappings { get; }

        #endregion

        #region function

        bool TestModifierKey(ModifierKey modifierKey, in ModifierKeyState state)
        {
            return modifierKey switch
            {
                ModifierKey.None => !state.Left && !state.Right,
                ModifierKey.Left => state.Left && !state.Right,
                ModifierKey.Right => !state.Left && state.Right,
                ModifierKey.Any => state.Left || state.Right,
                ModifierKey.All => state.Left && state.Right,
                _ => throw new NotImplementedException(),
            };
        }

        protected bool TestMapping(IReadOnlyKeyMappingData mapping, bool isDown, Key key, in ModifierKeyStatus modifierKeyStatus)
        {
            if(mapping.Key != key) {
                return false;
            }
            if(!TestModifierKey(mapping.Shift, modifierKeyStatus.shift)) {
                return false;
            }
            if(!TestModifierKey(mapping.Control, modifierKeyStatus.contrl)) {
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

    public abstract class KeyActionJobBase<TActionData> : KeyActionJobBase
        where TActionData : IReadOnlyKeyActionCommonData
    {
        public KeyActionJobBase(TActionData actionData, IEnumerable<IReadOnlyKeyMappingData> mappings)
            : base(actionData, mappings)
        {
            ActionData = actionData;
        }

        #region property

        public TActionData ActionData { get; }

        #endregion
    }

    public sealed class KeyActionReplaceJob : KeyActionJobBase<IReadOnlyKeyActionReplaceData>
    {
        public KeyActionReplaceJob(IReadOnlyKeyActionReplaceData actionData, IReadOnlyKeyMappingData mapping)
            : base(actionData, new[] { mapping })
        {
            if(ActionData.ReplaceKey == System.Windows.Input.Key.None) {
                throw new ArgumentException(nameof(actionData) + "." + nameof(actionData.ReplaceKey));
            }
            if(ActionData.ReplaceKey == Mappings[0].Key) {
                throw new ArgumentException(nameof(actionData.ReplaceKey) + " == " + nameof(IReadOnlyKeyMappingData.Key));
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
            }else if(Mapping.Key == Key.None) {
                throw new ArgumentException(nameof(mapping) + "." + nameof(mapping.Key));
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

    public sealed class KeyActionDisableJob : KeyActionJobBase<IReadOnlyKeyActionDisableData>
    {
        public KeyActionDisableJob(IReadOnlyKeyActionDisableData actionData, IReadOnlyKeyMappingData mapping)
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
        [Timestamp(DateTimeKind.Utc)]
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

    public class KeyActionPressedJobBase<TActionData> : KeyActionJobBase<TActionData>
        where TActionData : IReadOnlyKeyActionPressedData
    {
        public KeyActionPressedJobBase(TActionData actionData, IEnumerable<IReadOnlyKeyMappingData> mappings)
            : base(actionData, mappings)
        {
            if(Mappings.Count == 0) {
                throw new ArgumentException(nameof(mappings));
            }

            var keyIsNoneOrMods = Mappings.Counting().Where(i => i.Value.Key == Key.None || i.Value.Key.IsModifierKey()).ToList();
            if(keyIsNoneOrMods.Any()) {
                var errors = string.Join(", ", keyIsNoneOrMods.Select(i => $"{nameof(mappings)}[{i.Number}]"));
                throw new ArgumentException("不正なキー設定(キー設定なし、修飾キーのみ): " + errors);
            }
        }

        #region property

        /// <summary>
        /// 次に調べるインデックス。
        /// </summary>
        int NextIndex { get; set; }
        /// <summary>
        /// キー設定にヒットしたか。
        /// <para><see cref="Check"/>ではあくまで今のキー設定にヒットしたかどうかを確認するのでキー設定全てに該当したかを判定するために使用する。</para>
        /// </summary>
        public bool IsAllHit { get; private set; }
        /// <summary>
        /// 次キー入力待ちか。
        /// <para></para>
        /// </summary>
        public bool NextWaiting => 0 < NextIndex;

        #endregion

        #region function

        #endregion

        #region KeyActionJobBase

        public override void Reset()
        {
            NextIndex = 0;
            IsAllHit = false;
        }

        /// <summary>
        /// <para>有効であるかどうかは<see cref="IsAllHit"/>を確認すること。</para>
        /// </summary>
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

    public sealed class KeyActionPressJob : KeyActionPressedJobBase<IReadOnlyKeyActionPressedData>
    {
        public KeyActionPressJob(IReadOnlyKeyActionPressedData actionData, IEnumerable<IReadOnlyKeyMappingData> mappings)
            : base(actionData, mappings)
        { }
    }

}
