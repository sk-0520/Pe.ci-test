using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace ContentTypeTextNet.Pe.Core.Models
{
    public class ObjectDumpItem
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

    class DummyInfo : MemberInfo
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

        static IReadOnlyList<ObjectDumpItem> EmptyChildren { get; } = new ObjectDumpItem[0];

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

        bool CanGetValue(MemberInfo memberInfo, bool ignoreAutoMember)
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

        object? GetMemberValue(object target, MemberInfo memberInfo)
        {
            switch(memberInfo.MemberType) {
                case MemberTypes.Field:
                    return ((FieldInfo)memberInfo).GetValue(target);

                case MemberTypes.Property:
                    return ((PropertyInfo)memberInfo).GetValue(target);

                default:
                    throw new NotImplementedException();
            }
        }

        int GetNextNest(int nest) => nest < 0 ? -1 : nest - 1;

        IReadOnlyList<ObjectDumpItem> DumpDictionary(IDictionary dictionary, int nest, bool ignoreAutoMember)
        {
            var result = new List<ObjectDumpItem>(dictionary.Count);
            var dictionaryType = dictionary.GetType();
            foreach(var baseKey in dictionary.Keys) {
                if(baseKey == null) {
                    continue;
                }

                var key = baseKey;
                var target = dictionary[key];
                if(target == null) {
                    var item = new ObjectDumpItem(new DummyInfo(key.ToString()!, dictionaryType, typeof(object)), null, EmptyChildren);
                    result.Add(item);
                } else {
                    var targetType = target.GetType();
                    if(IgnoreNestedMembers.Contains(targetType)) {
                        var item = new ObjectDumpItem(new DummyInfo(key.ToString()!, dictionaryType, targetType), target, EmptyChildren);
                        result.Add(item);
                    } else {
                        var item = new ObjectDumpItem(new DummyInfo(key.ToString()!, dictionaryType, targetType), key, DumpCore(target, GetNextNest(nest), ignoreAutoMember));
                        result.Add(item);
                    }
                }
            }

            return result;
        }

        private IReadOnlyList<ObjectDumpItem> DumpArray(Array array, int nest, bool ignoreAutoMember)
        {
            var result = new List<ObjectDumpItem>(array.Length);
            var arrayType = array.GetType();
            for(var i = 0; i < array.Length; i++) {
                var s = "[" + i.ToString() + "]";
                var target = array.GetValue(i);

                if(target == null) {
                    var item = new ObjectDumpItem(new DummyInfo(s, arrayType, typeof(object)), null, EmptyChildren);
                    result.Add(item);
                } else {
                    var targetType = target.GetType();
                    if(IgnoreNestedMembers.Contains(targetType)) {
                        var item = new ObjectDumpItem(new DummyInfo(s, arrayType, targetType), target, EmptyChildren);
                        result.Add(item);
                    } else {
                        var item = new ObjectDumpItem(new DummyInfo(s, arrayType, targetType), target, DumpCore(target, GetNextNest(nest), ignoreAutoMember));
                        result.Add(item);
                    }
                }
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
                    var s = "[" + i.ToString() + "]";
                    var target = enumerator.Current;

                    if(target == null) {
                        var item = new ObjectDumpItem(new DummyInfo(s, arrayType, typeof(object)), null, EmptyChildren);
                        result.Add(item);
                    } else {
                        var targetType = target.GetType();
                        if(IgnoreNestedMembers.Contains(targetType)) {
                            var item = new ObjectDumpItem(new DummyInfo(s, arrayType, targetType), target, EmptyChildren);
                            result.Add(item);
                        } else {
                            var item = new ObjectDumpItem(new DummyInfo(s, arrayType, targetType), target, DumpCore(target, GetNextNest(nest), ignoreAutoMember));
                            result.Add(item);
                        }
                    }

                } finally {
                    i++;
                }

            }

            return result;

        }


        IReadOnlyList<ObjectDumpItem> DumpFileSystemInfo(FileSystemInfo fileSystemInfo, int nest, bool ignoreAutoMember)
        {
            var result = new List<ObjectDumpItem>() {
                new ObjectDumpItem(new DummyInfo(string.Empty, typeof(FileSystemInfo), fileSystemInfo.GetType()), fileSystemInfo.ToString(), EmptyChildren),
            };

            return result;
        }

        IReadOnlyList<ObjectDumpItem> DumpUri(Uri uri, int nest, bool ignoreAutoMember)
        {
            var result = new List<ObjectDumpItem>() {
                new ObjectDumpItem(new DummyInfo(string.Empty, typeof(Uri), uri.GetType()), uri.ToString(), EmptyChildren),
            };

            return result;
        }

        IReadOnlyList<ObjectDumpItem> DumpCore(object target, int nest, bool ignoreAutoMember)
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
                if(member is IEnumerable seq) {
                    continue;
                }

                var memberValue = GetMemberValue(target, member);
                var children = EmptyChildren;
                if(memberValue != null && !IgnoreNestedMembers.Contains(memberValue.GetType())) {
                    if((memberValue is IEnumerable)) {
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
                throw new ArgumentException(nameof(nest));
            }

            return DumpCore(target, nest, ignoreAutoMember);
        }

        public IReadOnlyList<ObjectDumpItem> Dump(object target, int nest)
        {
            if(target == null) {
                throw new ArgumentNullException(nameof(target));
            }
            if(nest == 0) {
                throw new ArgumentException(nameof(nest));
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
            using(var writer = new StreamWriter(stream, Encoding.UTF8, 1024 * 4, true)) {
                WriteDump(dumpItems, writer);
            }
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
            sb.Append(":");
            sb.AppendLine();

            using(var stream = new MemoryStream()) {
                objectDumper.WriteDump(dumpItems, stream);
                var dumpValue = Encoding.UTF8.GetString(stream.GetBuffer(), 0, (int)stream.Length);
                sb.Append(dumpValue);
            }

            return sb.ToString();
        }

        #endregion
    }
}
