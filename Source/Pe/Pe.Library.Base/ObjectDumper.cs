using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace ContentTypeTextNet.Pe.Library.Base
{
    public struct ObjectDumpItem
    {
        public ObjectDumpItem(MemberInfo memberInfo, object? value, IReadOnlyList<ObjectDumpItem> children)
        {
            MemberInfo = memberInfo;
            Value = value;
            Children = children;
        }

        #region property

        public MemberInfo MemberInfo { get; }
        public object? Value { get; }

        public IReadOnlyList<ObjectDumpItem> Children { get; }

        #endregion
    }

    internal class DummyInfo: MemberInfo
    {
        public DummyInfo(string name, Type declaringType, Type reflectedType)
        {
            Name = name;
            DeclaringType = declaringType;
            ReflectedType = reflectedType;
        }

        public override MemberTypes MemberType => MemberTypes.Field | MemberTypes.Property;

        public override string Name { get; }

        public override Type DeclaringType { get; }

        public override Type ReflectedType { get; }

        public override object[] GetCustomAttributes(bool inherit)
        {
            throw new NotImplementedException();
        }

        public override object[] GetCustomAttributes(Type attributeType, bool inherit)
        {
            throw new NotImplementedException();
        }

        public override bool IsDefined(Type attributeType, bool inherit)
        {
            throw new NotImplementedException();
        }

        #region object

        public override string ToString() => $"{ReflectedType} {Name}";

        #endregion
    }

    public class ObjectDumper
    {
        #region property

        private static IReadOnlyList<ObjectDumpItem> EmptyChildren { get; } = Array.Empty<ObjectDumpItem>();

        public ISet<Type> IgnoreNestedMembers { get; } = new HashSet<Type>() {
            typeof(bool),
            typeof(char),
            typeof(byte),
            typeof(short),
            typeof(ushort),
            typeof(int),
            typeof(uint),
            typeof(long),
            typeof(ulong),
            typeof(float),
            typeof(double),
            typeof(decimal),
            typeof(string),
            typeof(DateTime),
            typeof(DateTimeOffset),
            typeof(TimeSpan),
            typeof(Version),
            typeof(Guid),
        };

        #endregion

        #region function

        private bool CanGetValue(MemberInfo memberInfo, bool ignoreAutoMember)
        {
            if(ignoreAutoMember) {
                if(memberInfo.GetCustomAttribute<CompilerGeneratedAttribute>() != null) {
                    return false;
                }
            }

            if(memberInfo.MemberType == MemberTypes.Field) {
                return true;
            }

            if(memberInfo.MemberType == MemberTypes.Property) {
                var propertInfo = (PropertyInfo)memberInfo;
                // インデクサはキツイ
                return propertInfo.GetIndexParameters().Length == 0;
            }

            return false;
        }

        private object? GetMemberValue(object target, MemberInfo memberInfo)
        {
            return memberInfo.MemberType switch {
                MemberTypes.Field => ((FieldInfo)memberInfo).GetValue(target),
                MemberTypes.Property => ((PropertyInfo)memberInfo).GetValue(target),
                _ => throw new NotImplementedException(),
            };
        }

        private int GetNextNest(int nest) => nest < 0 ? -1 : nest - 1;

        private ObjectDumpItem DumpEnumerableItem(string key, object? current, Type collectiontype, int nest, bool ignoreAutoMember)
        {
            var target = current;

            if(target == null) {
                return new ObjectDumpItem(new DummyInfo(key, collectiontype, typeof(object)), null, EmptyChildren);
            }

            var targetType = target.GetType();
            if(IgnoreNestedMembers.Contains(targetType)) {
                return new ObjectDumpItem(new DummyInfo(key, collectiontype, targetType), target, EmptyChildren);
            }
            return new ObjectDumpItem(new DummyInfo(key, collectiontype, targetType), target, DumpCore(target, GetNextNest(nest), ignoreAutoMember));
        }

        private IReadOnlyList<ObjectDumpItem> DumpDictionary(IDictionary dictionary, int nest, bool ignoreAutoMember)
        {
            var result = new List<ObjectDumpItem>(dictionary.Count);
            var dictionaryType = dictionary.GetType();
            foreach(var baseKey in dictionary.Keys) {
                if(baseKey == null) {
                    continue;
                }

                var key = baseKey;
                var current = dictionary[key];
                var item = DumpEnumerableItem(key.ToString()!, current, dictionaryType, nest, ignoreAutoMember);
                result.Add(item);
            }

            return result;
        }

        private IReadOnlyList<ObjectDumpItem> DumpArray(Array array, int nest, bool ignoreAutoMember)
        {
            var result = new List<ObjectDumpItem>(array.Length);
            var arrayType = array.GetType();
            for(var i = 0; i < array.Length; i++) {
                var key = "[" + i.ToString(CultureInfo.InvariantCulture) + "]";
                var current = array.GetValue(i);

                var item = DumpEnumerableItem(key, current, arrayType, nest, ignoreAutoMember);
                result.Add(item);
            }

            return result;

        }

        private IReadOnlyList<ObjectDumpItem> DumpEnumerable(IEnumerable enumerable, int nest, bool ignoreAutoMember)
        {
            var result = new List<ObjectDumpItem>();
            var arrayType = enumerable.GetType();
            var enumerator = enumerable.GetEnumerator();
            int i = 0;
            while(enumerator.MoveNext()) {
                try {
                    var key = "[" + i.ToString(CultureInfo.InvariantCulture) + "]";
                    var current = enumerator.Current;

                    var item = DumpEnumerableItem(key, current, arrayType, nest, ignoreAutoMember);
                    result.Add(item);
                } finally {
                    i++;
                }

            }

            return result;

        }

        private IReadOnlyList<ObjectDumpItem> DumpFileSystemInfo(FileSystemInfo fileSystemInfo, int nest, bool ignoreAutoMember)
        {
            var result = new List<ObjectDumpItem>() {
                new ObjectDumpItem(new DummyInfo(string.Empty, typeof(FileSystemInfo), fileSystemInfo.GetType()), fileSystemInfo.ToString(), EmptyChildren),
            };

            return result;
        }

        private IReadOnlyList<ObjectDumpItem> DumpUri(Uri uri, int nest, bool ignoreAutoMember)
        {
            var result = new List<ObjectDumpItem>() {
                new ObjectDumpItem(new DummyInfo(string.Empty, typeof(Uri), uri.GetType()), uri.ToString(), EmptyChildren),
            };

            return result;
        }

        private IReadOnlyList<ObjectDumpItem> DumpCore(object target, int nest, bool ignoreAutoMember)
        {
            if(nest == 0) {
                return EmptyChildren;
            }

            switch(target) {
                case string _:
                    break;

                case IDictionary dic:
                    return DumpDictionary(dic, nest, ignoreAutoMember);

                case Array array:
                    return DumpArray(array, nest, ignoreAutoMember);

                case IEnumerable enumerable:
                    return DumpEnumerable(enumerable, nest, ignoreAutoMember);

                case FileSystemInfo fsi:
                    return DumpFileSystemInfo(fsi, nest, ignoreAutoMember);

                case Uri uri:
                    return DumpUri(uri, nest, ignoreAutoMember);

                default:
                    break;
            }

            var members = target.GetType().GetMembers(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.GetProperty);
            var result = new List<ObjectDumpItem>(members.Length);
            foreach(var member in members) {
                if(!CanGetValue(member, ignoreAutoMember)) {
                    continue;
                }
                if(member is IEnumerable) {
                    continue;
                }

                var memberValue = GetMemberValue(target, member);
                var children = EmptyChildren;
                if(memberValue != null && !IgnoreNestedMembers.Contains(memberValue.GetType())) {
                    if(memberValue is IEnumerable) {
                        children = DumpCore(memberValue, GetNextNest(nest), ignoreAutoMember);
                    }
                }

                var item = new ObjectDumpItem(member, memberValue, children);
                result.Add(item);
            }

            return result;
        }

        public IReadOnlyList<ObjectDumpItem> Dump(object target, int nest, bool ignoreAutoMember)
        {
            if(target == null) {
                throw new ArgumentNullException(nameof(target));
            }
            if(nest == 0) {
                throw new ArgumentException(null, nameof(nest));
            }

            return DumpCore(target, nest, ignoreAutoMember);
        }

        public IReadOnlyList<ObjectDumpItem> Dump(object target, int nest)
        {
            if(target == null) {
                throw new ArgumentNullException(nameof(target));
            }
            if(nest == 0) {
                throw new ArgumentException(null, nameof(nest));
            }

            return DumpCore(target, nest, true);
        }

        public IReadOnlyList<ObjectDumpItem> Dump(object target)
        {
            if(target == null) {
                throw new ArgumentNullException(nameof(target));
            }

            return DumpCore(target, -1, true);
        }

        protected virtual string GetIndent(int nest)
        {
            return new string('-', nest) + "> ";
        }

        protected virtual string GetMemberValue(ObjectDumpItem dumpItem)
        {
            return $"{dumpItem.MemberInfo.Name}: {dumpItem.Value} [{dumpItem.MemberInfo.DeclaringType}]";
        }

        public void WriteDump(IEnumerable<ObjectDumpItem> dumpItems, TextWriter writer)
        {
            void Write(ObjectDumpItem dumpItem, int nest)
            {
                writer.Write(GetIndent(nest));
                writer.WriteLine(GetMemberValue(dumpItem));

                foreach(var childItem in dumpItem.Children) {
                    Write(childItem, nest + 1);
                }
            }

            //writer.WriteLine($"[{target.GetType().Name}]:");
            foreach(var item in dumpItems) {
                Write(item, 1);
            }
        }

        public void WriteDump(IEnumerable<ObjectDumpItem> dumpItems, Stream stream)
        {
            using var writer = new StreamWriter(stream, Encoding.UTF8, 1024 * 4, true);
            WriteDump(dumpItems, writer);
        }

        public static string GetDumpString(object? target)
        {
            if(target == null) {
                return $"[{nameof(target)}]: null";
            }

            var objectDumper = new ObjectDumper();
            var dumpItems = objectDumper.Dump(target);

            var sb = new StringBuilder(dumpItems.Count * 256); // 特に意味はない
            sb.Append('[');
            sb.Append(target.GetType().Name);
            sb.Append(':');
            sb.AppendLine();

            using(var stream = new MemoryReleaseStream()) {
                objectDumper.WriteDump(dumpItems, stream);
                var dumpValue = Encoding.UTF8.GetString(stream.GetBuffer(), 0, (int)stream.Length);
                sb.Append(dumpValue);
            }

            sb.Append(']');

            return sb.ToString();
        }

        #endregion
    }
}
