using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Logic;

namespace ContentTypeTextNet.Pe.Main.Models.KeyAction
{
    public abstract class KeyActionJobBase
    {
        public KeyActionJobBase(IReadOnlyKeyActionCommonData commonData, IEnumerable<IReadOnlyKeyMappingItemData> mappings)
        {
            CommonData = commonData;
            Mappings = mappings.ToList();
        }

        #region property

        protected IReadOnlyKeyActionCommonData CommonData { get; }
        protected IReadOnlyList<IReadOnlyKeyMappingItemData> Mappings { get; }

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

        protected bool TestMapping(IReadOnlyKeyMappingItemData mapping, bool isDown, Key key, in ModifierKeyStatus modifierKeyStatus)
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

        #endregion
    }

    public abstract class KeyActionJobBase<TActionData> : KeyActionJobBase
        where TActionData : IReadOnlyKeyActionCommonData
    {
        public KeyActionJobBase(TActionData actionData, IEnumerable<IReadOnlyKeyMappingItemData> mappings)
            : base(actionData, mappings)
        {
            ActionData = actionData;
        }

        #region property

        protected TActionData ActionData { get; }

        #endregion
    }

    public sealed class KeyActionReplaceJob : KeyActionJobBase<IReadOnlyKeyActionReplaceData>
    {
        public KeyActionReplaceJob(IReadOnlyKeyActionReplaceData actionData, IReadOnlyKeyMappingItemData mapping)
            : base(actionData, new[] { mapping })
        {
            if(ActionData.ReplaceKey == System.Windows.Input.Key.None) {
                throw new ArgumentException(nameof(actionData) + "." + nameof(actionData.ReplaceKey));
            }
        }

        #region property
        #endregion

        #region function
        #endregion
    }

    public sealed class KeyActionDisableJob : KeyActionJobBase<IReadOnlyKeyActionDisableData>
    {
        public KeyActionDisableJob(IReadOnlyKeyActionDisableData actionData, IReadOnlyKeyMappingItemData mapping)
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
        #endregion

        #region function

        public bool Check(bool isDown, Key key, in ModifierKeyStatus modifierKeyStatus)
        {
            var mapping = Mappings[0];
            return TestMapping(mapping, isDown, key, modifierKeyStatus);
        }

        #endregion
    }

}
