using System;
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
            var items = type
                .GetMembers(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.SetField)
                .Where(i => i.MemberType == MemberTypes.Field)
                .Cast<FieldInfo>()
                .Select(i => (field: i, attribute: i.GetCustomAttribute<CompilerGeneratedAttribute>()))
                .Where(i => i.attribute != null)
                .Select(i => (i.field, i.attribute!, proeprtyName: i.field.Name.Substring(1, i.field.Name.IndexOf('>') - 1)))
            ;

            var nameConverter = new NameConveter();

            foreach(var item in items) {
                string propertyName = item.field.Name.Substring(1, item.field.Name.IndexOf('>') - 1);
                var conf = item.field.GetCustomAttribute<ConfigurationAttribute>();
                if(conf == null) {
                    //throw new Exception($"{item}: attr {nameof(ConfigurationAttribute)}");
                }

                var confMemberName = conf?.MemberName.Length == 0
                    ? nameConverter.PascalToSnake(propertyName)
                    : conf?.MemberName
                ;
                if(confMemberName == null) {
                    confMemberName = nameConverter.PascalToSnake(propertyName);
                }

                Debug.WriteLine("[{2}] {0}:{1} - `{3}' -> `{4}'", item.field.Name, propertyName, item.field.FieldType, conf?.MemberName, confMemberName);
                if(item.field.FieldType.IsArray) {
                    Debug.Assert(false, "未実装");
                } else if(item.field.FieldType.IsSubclassOf(typeof(ConfigurationBase))) {
                    var childSection = section.GetSection(confMemberName);
                    var result = Activator.CreateInstance(item.field.FieldType, new[] { childSection });
                    item.field.SetValue(this, result);
                } else {
                    Debug.WriteLine(item.field.FieldType);
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
