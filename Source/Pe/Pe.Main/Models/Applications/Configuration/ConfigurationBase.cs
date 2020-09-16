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
        /// <param name="section"></param>
        protected ConfigurationBase(IConfigurationSection section)
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
                .Select(i => (i.field, attribute: i.attribute!, propertyName: i.field.Name.Substring(1, i.field.Name.IndexOf('>') - 1)))
                .Select(i => (i.field, i.attribute, property: properties[i.propertyName]))
            ;

            var nameConverter = new NameConveter();

            foreach(var item in items) {
                var conf = item.property.GetCustomAttribute<ConfigurationAttribute>();
                if(conf == null) {
                    //throw new Exception($"{item}: attr {nameof(ConfigurationAttribute)}");
                }

                var memberKey = conf?.MemberName.Length == 0
                    ? nameConverter.PascalToSnake(item.property.Name)
                    : conf?.MemberName
                ;
                if(memberKey == null) {
                    memberKey = nameConverter.PascalToSnake(item.property.Name);
                }

                Debug.WriteLine("[{2}] {0}:{1} - `{3}' -> `{4}'", item.field.Name, item.property.Name, item.field.FieldType, conf?.MemberName, memberKey);
                if(item.field.FieldType.IsArray) {
                    Debug.Assert(false, "未実装");
                } else if(item.field.FieldType.IsSubclassOf(typeof(ConfigurationBase))) {
                    var childSection = section.GetSection(memberKey);
                    var result = Activator.CreateInstance(item.field.FieldType, new[] { childSection });
                    item.field.SetValue(this, result);
                } else if(item.field.FieldType == typeof(string)) {
                    var result = section.GetValue(item.field.FieldType, memberKey);
                    item.field.SetValue(this, result);
                } else if(typeof(IReadOnlyList<>).IsAssignableFrom(item.field.FieldType)) {
                    var type2 = item.field.FieldType.GenericTypeArguments;
                } else  {
                    var result = section.GetValue(item.field.FieldType, memberKey);
                    if(result == null) {

                    }
                    item.field.SetValue(this, result);
                }
            }
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
