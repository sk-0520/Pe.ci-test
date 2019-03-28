using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Library.Shared.Library.Model
{
    public class ObjectDumpItem
    {
        public ObjectDumpItem(MemberInfo memberInfo, object value, IReadOnlyList<ObjectDumpItem> children)
        {
            MemberInfo = memberInfo;
            Value = value;
            Children = children;
        }

        #region property

        public MemberInfo MemberInfo { get; }
        public object Value { get; }

        public IReadOnlyList<ObjectDumpItem> Children { get; }

        #endregion
    }


    public class ObjectDumper
    {
        #region property

        static IReadOnlyList<ObjectDumpItem> EmptyChildren { get; } = new ObjectDumpItem[0];

        public ISet<Type> IgnoreNestedMembers { get; } = new HashSet<Type>() {
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

        object GetMemberValue(object target, MemberInfo memberInfo)
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

        IReadOnlyList<ObjectDumpItem> DumpCore(object target, int nest, bool ignoreAutoMember)
        {
            if(nest == 0) {
                return EmptyChildren;
            }

            var members = target.GetType().GetMembers(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.GetProperty);
            var result = new List<ObjectDumpItem>(members.Length);
            foreach(var member in members) {
                if(!CanGetValue(member, ignoreAutoMember)) {
                    continue;
                }

                var memberValue = GetMemberValue(target, member);
                var children = EmptyChildren;
                if(memberValue != null && !IgnoreNestedMembers.Contains(memberValue.GetType())) {
                    children = DumpCore(memberValue, nest < 0 ? -1 : nest - 1, ignoreAutoMember);
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
            return new string('-', nest) + ">";
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

                if(dumpItem.Children.Count != 0) {
                    foreach(var childItem in dumpItem.Children) {
                        Write(childItem, nest + 1);
                    }
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


        public static string GetDumpString(object target)
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
