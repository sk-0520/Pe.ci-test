using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ContentTypeTextNet.Pe.Main.Models.Data;

namespace ContentTypeTextNet.Pe.Main.Models.KeyAction
{
    public abstract class KeyActionExecutorBase
    {
        public KeyActionExecutorBase(IReadOnlyKeyActionCommonData commonData, IEnumerable<IReadOnlyKeyMappingItemData> mappings)
        {
            CommonData = commonData;
            Mappings = mappings.ToList();
        }

        #region property

        protected IReadOnlyKeyActionCommonData CommonData { get; }
        protected IReadOnlyList<IReadOnlyKeyMappingItemData> Mappings { get; }

        #endregion

        #region function
        #endregion
    }

    public abstract class KeyActionExecutorBase<TActionData> : KeyActionExecutorBase
        where TActionData : IReadOnlyKeyActionCommonData
    {
        public KeyActionExecutorBase(TActionData actionData, IEnumerable<IReadOnlyKeyMappingItemData> mappings)
            : base(actionData, mappings)
        {
            ActionData = actionData;
        }

        #region property

        protected TActionData ActionData { get; }

        #endregion
    }

    public sealed class KeyActionReplaceExecutor : KeyActionExecutorBase<IReadOnlyKeyActionReplaceData>
    {
        public KeyActionReplaceExecutor(IReadOnlyKeyActionReplaceData actionData, IReadOnlyKeyMappingItemData mapping)
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

    public sealed class KeyActionDisableExecutor : KeyActionExecutorBase<IReadOnlyKeyActionDisableData>
    {
        public KeyActionDisableExecutor(IReadOnlyKeyActionDisableData actionData, IReadOnlyKeyMappingItemData mapping)
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
        #endregion
    }

}
