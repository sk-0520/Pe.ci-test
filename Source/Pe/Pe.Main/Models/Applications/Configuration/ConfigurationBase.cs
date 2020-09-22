using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using ContentTypeTextNet.Pe.Core.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Configuration;

namespace ContentTypeTextNet.Pe.Main.Models.Applications.Configuration
{
    /// <summary>
    /// アプリケーション構成ファイルの読み込み基底処理。
    /// <para>すべてをコンストラクタで処理し、失敗時は例外を投げて先に進ませないようにする。</para>
    /// <para>バッキングフィールド名を信じてえんやこらさっさ。</para>
    /// </summary>
    public abstract class ConfigurationBase
    {
        /// <summary>
        /// 規約として get-only プロパティ を対象とする。
        /// </summary>
        /// <param name="conf"></param>
        protected ConfigurationBase(IConfiguration conf)
        {
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
                .Select(i => (i.field, i.property, config: i.property.GetCustomAttribute<ConfigurationAttribute>()!))
                .Where(i => i.config != null)
            ;

            var nameConverter = new NameConveter();

            foreach(var item in items) {
                var memberKey = item.config.MemberName.Length == 0
                    ? nameConverter.PascalToSnake(item.property.Name)
                    : item.config.MemberName
                ;
                if(memberKey == null) {
                    memberKey = nameConverter.PascalToSnake(item.property.Name);
                }

                Debug.WriteLine("[{2}] {0}:{1} - `{3}' -> `{4}'", item.field.Name, item.property.Name, item.field.FieldType, item.config.MemberName, memberKey);

                if(item.field.FieldType.IsSubclassOf(typeof(ConfigurationBase))) {
                    var childSection = conf.GetSection(memberKey);
                    var result = Activator.CreateInstance(item.field.FieldType, new[] { childSection });
                    item.field.SetValue(this, result);
                } else if(item.field.FieldType == typeof(string)) {
                    var result = conf.GetValue(item.field.FieldType, memberKey);
                    item.field.SetValue(this, result);
                } else {
                    var result = conf.GetValue(item.field.FieldType, memberKey);
                    if(result == null) {
                        var child = conf.GetSection(memberKey);
                        if(child.Value == null) {
                            throw new Exception(child.Path);
                        }
                    } else {
                        item.field.SetValue(this, result);
                    }
                }
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

        #region function

        protected static IReadOnlyList<T> GetList<T>(IConfigurationSection section, string key)
        {
            return section.GetSection(key).Get<T[]>();
        }

        protected static Size GetSize(IConfigurationSection section, string key)
        {
            var size = section.GetSection(key);
            return new Size(size.GetValue<double>("width"), size.GetValue<double>("height"));
        }

        protected static MinMax<T> GetMinMax<T>(IConfigurationSection section, string key)
            where T : IComparable<T>
        {
            var size = section.GetSection(key);
            return new MinMax<T>(size.GetValue<T>("minimum"), size.GetValue<T>("maximum"));
        }
        protected static MinMaxDefault<T> GetMinMaxDefault<T>(IConfigurationSection section, string key)
            where T : IComparable<T>
        {
            var size = section.GetSection(key);
            return new MinMaxDefault<T>(size.GetValue<T>("minimum"), size.GetValue<T>("maximum"), size.GetValue<T>("default"));
        }

        #endregion
    }
}
