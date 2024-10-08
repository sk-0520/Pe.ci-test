using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.Models.Platform;
using Microsoft.Extensions.Configuration;
using ContentTypeTextNet.Pe.Library.Base;
using ContentTypeTextNet.Pe.Library.Base.Linq;

namespace ContentTypeTextNet.Pe.Main.Models.Applications.Configuration
{
    internal readonly struct ConfigurationSetting
    {
        public ConfigurationSetting(FieldInfo field, PropertyInfo property, ConfigurationAttribute configuration)
        {
            Field = field;
            Property = property;
            Configuration = configuration;
        }

        #region property

        public FieldInfo Field { get; }
        public PropertyInfo Property { get; }
        public ConfigurationAttribute Configuration { get; }

        #endregion
    }

    /// <summary>
    /// アプリケーション構成ファイルの読み込み基底処理。
    /// </summary>
    /// <remarks>
    /// <para>すべてをコンストラクタで処理し、失敗時は例外を投げて先に進ませないようにする。</para>
    /// <para>バッキングフィールド名を信じてえんやこらさっさ。</para>
    /// </remarks>
    public abstract class ConfigurationBase
    {
        /// <summary>
        /// 規約として get-only プロパティ を対象とする。
        /// </summary>
        /// <param name="conf"></param>
        protected ConfigurationBase(IConfiguration conf)
        {
            Configuration = conf;

            var type = GetType();
            var properties = type.GetProperties()
                .ToDictionary(i => i.Name, i => i)
            ;
            var items =
                type.GetMembers(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.SetField)
                .Where(i => i.MemberType == MemberTypes.Field)
                .Cast<FieldInfo>()
                .Select(i => (field: i, attribute: i.GetCustomAttribute<CompilerGeneratedAttribute>()))
                .Where(i => i.attribute != null)
                .Select(i => (i.field, propertyName: i.field.Name.Substring(1, i.field.Name.IndexOf('>') - 1)))
                .Where(i => properties.ContainsKey(i.propertyName))
                .Select(i => (i.field, property: properties[i.propertyName]))
                .Select(i => new ConfigurationSetting(i.field, i.property, i.property.GetCustomAttribute<ConfigurationAttribute>()!))
                .Where(i => i.Configuration != null)
            ;

            var nameConverter = new NameConverter();

            foreach(var item in items) {
                var memberKey = item.Configuration.MemberName.Length == 0
                    ? nameConverter.PascalToSnake(item.Property.Name)
                    : item.Configuration.MemberName
                ;
                if(memberKey == null) {
                    memberKey = nameConverter.PascalToSnake(item.Property.Name);
                }

                if(string.IsNullOrEmpty(item.Configuration.RootConvertMethodName)) {
                    MethodInfo? method = null;
                    if(!string.IsNullOrEmpty(item.Configuration.NestConvertMethodName)) {
                        method = GetMethod(type, item.Configuration.NestConvertMethodName);
                    }
                    var result = GetValue(this, conf, memberKey, item.Field.FieldType, method);
                    item.Field.SetValue(this, result);
                } else {
                    var method = GetMethod(type, item.Configuration.RootConvertMethodName);
                    Debug.Assert(method != null);
                    var result = GetCustomValue(this, conf, memberKey, item.Field.FieldType, method);
                    item.Field.SetValue(this, result);
                }

                Debug.WriteLine("[{2}] {0}:{1} - `{3}' -> `{4}'", item.Field.Name, item.Property.Name, item.Field.FieldType, item.Configuration.MemberName, memberKey);
            }

#if DEBUG
            var checkHasConfiguration = false;
            foreach(var debugProperty in type.GetProperties().Where(i => i.GetCustomAttribute<ConfigurationAttribute>() != null)) {
                if(debugProperty.GetValue(this) == null) {
                    checkHasConfiguration = true;
                    Debug.WriteLine("default -> {0}", debugProperty);
                }
            }

            if(checkHasConfiguration) {
                throw new Exception();
            }
#endif
        }

        protected IConfiguration Configuration { get; }

        #region function

        private MethodInfo? GetMethod(Type type, string methodName)
        {
            var method = type.GetMethod(methodName, BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.InvokeMethod);
            if(method == null) {
                method = typeof(ConfigurationBase).GetMethod(methodName, BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.InvokeMethod);
                if(method == null) {
                    throw new Exception($"method not found: {methodName}");
                }
            }

            return method;
        }

        private static object? GetValue(object methodParent, IConfiguration conf, string memberKey, Type valueType, MethodInfo? methodInfo)
        {
            if(valueType.IsSubclassOf(typeof(ConfigurationBase))) {
                var childSection = conf.GetSection(memberKey);
                var subclassResult = Activator.CreateInstance(valueType, new[] { childSection });
                return subclassResult;
            }

            if(valueType == typeof(string)) {
                if(string.IsNullOrEmpty(memberKey)) {
                    var getResult = conf.Get(typeof(string));
                    return getResult;
                }
                var stringResult = conf.GetValue(valueType, memberKey);
                return stringResult;
            }

            var result = conf.GetValue(valueType, memberKey);
            if(result == null) {
                var childSection = conf.GetSection(memberKey);
                if(childSection.Value == null) {
                    if(valueType.IsGenericType) {
                        // ReadOnlyList<T>, IReadOnlyDictionary<string|Enum,T> のみサポートする
                        var genArgs = valueType.GetGenericArguments();
                        Debug.Assert(genArgs != null);
                        var genIndex = valueType.Name.IndexOf('`');
                        var genName = valueType.Name.Substring(0, genIndex);
                        switch(genName) {
                            case "IReadOnlyList": {
                                    var rawChildren = childSection.GetChildren().ToList();
                                    var array = Array.CreateInstance(genArgs[0], rawChildren.Count);
                                    foreach(var child in rawChildren.Counting()) {
                                        var childValue = GetValue(methodParent, child.Value, string.Empty, genArgs[0], methodInfo);
                                        array.SetValue(childValue, child.Number);
                                    }
                                    return array;
                                }

                            case "IReadOnlyDictionary": {
                                    var rawChildren = childSection.GetChildren().ToList();
                                    var dictionaryType = typeof(Dictionary<,>).MakeGenericType(genArgs);
                                    var dictionary = (IDictionary)Activator.CreateInstance(dictionaryType)!;
                                    foreach(var raw in rawChildren) {
                                        var value = GetValue(methodParent, raw, string.Empty, genArgs[1], methodInfo);

                                        if(genArgs[0] == typeof(string)) {
                                            dictionary.Add(raw.Key, value);
                                        } else if(genArgs[0].IsEnum) {
                                            var key = Enum.Parse(genArgs[0], raw.Key, true);
                                            dictionary.Add(key, value);
                                        } else {
                                            break;
                                        }
                                    }
                                    return dictionary;
                                }

                            default:
                                break;
                        }
                    }
                }
                if(methodInfo != null) {
                    var customResult = GetCustomValue(methodParent, conf, memberKey, valueType, methodInfo);
                    return customResult;
                }
                throw new Exception($"{childSection.Path}: {valueType}");
            } else {
                return result;
            }
        }

        private static object? GetCustomValue(object methodParent, IConfiguration conf, string memberKey, Type fieldType, MethodInfo methodInfo)
        {
            Debug.WriteLine(fieldType);
            Debug.WriteLine(methodInfo);
            if(methodInfo.IsGenericMethod) {
                Debug.Assert(fieldType.IsGenericType);
                var genArgs = fieldType.GetGenericArguments();
                var genMethod = methodInfo.MakeGenericMethod(genArgs);

                //Debug.WriteLine("@ {0}", genMethod);
                return genMethod.Invoke(methodParent, new object[] { conf, memberKey });
            } else {
                var result = methodInfo.Invoke(methodParent, new object[] { conf, memberKey });
                return result;
            }
        }

        protected static Size ConvertSize(IConfigurationSection section, string key)
        {
            var size = section.GetSection(key);
            return new Size(size.GetValue<double>("width"), size.GetValue<double>("height"));
        }

        protected static MinMax<T> ConvertMinMax<T>(IConfigurationSection section, string key)
            where T : notnull, IComparable<T>
        {
            var size = section.GetSection(key);
            if(size is null) {
                throw new KeyNotFoundException(key);
            }
            return MinMax.Create(size.GetValue<T>("minimum")!, size.GetValue<T>("maximum")!);
        }
        protected static MinMaxDefault<T> ConvertMinMaxDefault<T>(IConfigurationSection section, string key)
            where T : notnull, IComparable<T>
        {
            var size = section.GetSection(key);
            if(size is null) {
                throw new KeyNotFoundException(key);
            }
            return MinMaxDefault.Create(size.GetValue<T>("minimum")!, size.GetValue<T>("maximum")!, size.GetValue<T>("default")!);
        }

        protected static ClassAndText ConvertClassAndText(IConfigurationSection section, string key)
        {
            return new ClassAndText(section.GetValue<string>("class")!, section.GetValue<string>("text")!);
        }

        #endregion
    }

}
