using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Main.Models.Data;
using System.Diagnostics;

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

        protected bool TryConvert<TValue>(KeyActionOptionAttribute attribute, IReadOnlyDictionary<string, string> map, Func<KeyActionOptionAttribute, string, TValue> func, out TValue result)
        {
            if(map.TryGetValue(attribute.OptionName, out var optionValue)) {
                try {
                    result = func(attribute, optionValue);
                    return true;
                } catch(Exception ex) {
                    Debug.WriteLine(ex);
                }
            }

            result = default!;
            return false;
        }

        #endregion
    }

    public sealed class ReplaceOptionConverter : KeyOptionConverterBase
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

    public sealed class DisableOptionConverter : KeyOptionConverterBase
    {
        #region function

        public bool ToForever(IReadOnlyDictionary<string, string> map)
        {
            var attribute = GetAttribute(KeyActionDisableOption.Forever);
            return Convert(attribute, map, (a, s) => {
                return System.Convert.ToBoolean(s);
            });
        }

        public bool TryGetForever(IReadOnlyDictionary<string, string> map, out bool result)
        {
            var attribute = GetAttribute(KeyActionDisableOption.Forever);
            return TryConvert(attribute, map, (a, s) => {
                return System.Convert.ToBoolean(s);
            }, out result);
        }

        public void SetForever(IDictionary<string, string> map, bool forever)
        {
            var attribute = GetAttribute(KeyActionDisableOption.Forever);
            map[attribute.OptionName] = forever.ToString();
        }

        #endregion
    }

    public class PressedOptionConverter : KeyOptionConverterBase
    {
        #region function

        public bool ToThroughSystem(IReadOnlyDictionary<string, string> map)
        {
            var attribute = GetAttribute(KeyActionPresseOption.ThroughSystem);
            return Convert(attribute, map, (a, s) => {
                return System.Convert.ToBoolean(s);
            });
        }

        public bool TryGetThroughSystem(IReadOnlyDictionary<string, string> map, out bool result)
        {
            var attribute = GetAttribute(KeyActionPresseOption.ThroughSystem);
            return TryConvert(attribute, map, (a, s) => {
                return System.Convert.ToBoolean(s);
            }, out result);
        }

        public void SetThroughSystem(IDictionary<string, string> map, bool conveySystem)
        {
            var attribute = GetAttribute(KeyActionPresseOption.ThroughSystem);
            map[attribute.OptionName] = conveySystem.ToString();
        }

        #endregion
    }

    public sealed class LauncherItemOptionConverter : PressedOptionConverter
    {
        #region function

        public Guid ToLauncherItemId(IReadOnlyDictionary<string, string> map)
        {
            var attribute = GetAttribute(KeyActionLauncherItemOption.LauncherItemId);
            return Convert(attribute, map, (a, s) => {
                return Guid.Parse(s);
            });
        }

        public bool TryGetLauncherItemId(IReadOnlyDictionary<string, string> map, out Guid result)
        {
            var attribute = GetAttribute(KeyActionLauncherItemOption.LauncherItemId);
            return TryConvert(attribute, map, (a, s) => {
                return Guid.Parse(s);
            }, out result);
        }

        public void WriteLauncherItemId(IDictionary<string, string> map, Guid launcherItemId)
        {
            var attribute = GetAttribute(KeyActionLauncherItemOption.LauncherItemId);
            map[attribute.OptionName] = launcherItemId.ToString("D");
        }

        #endregion
    }

}
