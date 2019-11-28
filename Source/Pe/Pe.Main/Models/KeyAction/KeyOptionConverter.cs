using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Main.Models.Data;

namespace ContentTypeTextNet.Pe.Main.Models.KeyAction
{
    public abstract class KeyOptionConverterBase
    {
        #region function

        protected KeyActionOptionAttribute GetAttribute<TEnum>(TEnum member)
            where TEnum : struct, Enum
        {
            var fieldInfo = typeof(TEnum).GetField(member.ToString());
            if(fieldInfo == null) {
                throw new InvalidOperationException(member.ToString());
            }
            var attribute = fieldInfo.GetCustomAttribute<KeyActionOptionAttribute>();
            if(attribute == null) {
                throw new InvalidOperationException(member.ToString());
            }

            return attribute;
        }

        protected TValue Convert<TValue>(KeyActionOptionAttribute attribute, IReadOnlyDictionary<string, string> map, Func<KeyActionOptionAttribute, string, TValue> func)
        {
            if(map.TryGetValue(attribute.OptionName, out var optionValue)) {
                return func(attribute, optionValue);
            }

            throw new ArgumentException(nameof(map) + "[" + attribute.OptionName + "]");
        }

        #endregion
    }

    public class ReplaceOptionConverter : KeyOptionConverterBase
    {
        #region function

        //public Key ToKey(IReadOnlyDictionary<string, string> map)
        //{
        //    var attribute = GetAttribute(KeyActionReplaceOption.ReplaceKey);
        //    return Convert(attribute, map, (a, s) => {
        //        var keyConverter = new KeyConverter();
        //        var key = (Key)keyConverter.ConvertFromInvariantString(s);
        //        return key;
        //    });
        //}

        #endregion
    }

    public class DisableOptionConverter : KeyOptionConverterBase
    {
        #region function

        public bool ToForever(IReadOnlyDictionary<string, string> map)
        {
            var attribute = GetAttribute(KeyActionDisableOption.Forever);
            return Convert(attribute, map, (a, s) => {
                return System.Convert.ToBoolean(s);
            });
        }

        #endregion
    }

    public abstract class PressedOptionConverter: KeyOptionConverterBase
    {
        #region function

        public bool ToConveySystem(IReadOnlyDictionary<string, string> map)
        {
            var attribute = GetAttribute(KeyActionPresseOption.ConveySystem);
            return Convert(attribute, map, (a, s) => {
                return System.Convert.ToBoolean(s);
            });
        }

        #endregion
    }

    public class LauncherItemOptionConverter : PressedOptionConverter
    {
        #region function

        public Guid ToLauncherItemId(IReadOnlyDictionary<string, string> map)
        {
            var attribute = GetAttribute(KeyActionLauncherItemOption.LauncherItemId);
            return Convert(attribute, map, (a, s) => {
                return Guid.Parse(s);
            });
        }

        #endregion
    }

}
