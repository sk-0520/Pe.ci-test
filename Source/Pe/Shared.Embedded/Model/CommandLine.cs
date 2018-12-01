using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ContentTypeTextNet.Pe.Library.Shared.Embedded.Model
{
    internal class CommandLineKey
    {
        #region define

        public const char EmptyShortKey = '\0';

        #endregion

        public CommandLineKey(char shortKey, string longKey, bool hasValue, string description)
        {
            ShortKey = shortKey;
            LongKey = TextUtility.SafeTrim(longKey);
            HasValue = hasValue;
            Description = description;
        }

        #region property

        public char ShortKey { get; }
        public string LongKey { get; }
        public bool HasValue { get; }
        public string Description { get; }

        public bool IsEnabledShortKey => ShortKey != EmptyShortKey;
        public bool IsEnabledLongKey => !string.IsNullOrEmpty(LongKey);

        #endregion
    }

    internal interface ICommandLineValue
    {
        #region property

        IReadOnlyList<string> Items { get; }
        string First { get; }

        #endregion
    }

    internal class CommandLineValue : ICommandLineValue
    {
        #region ICommandLineValue

        public List<string> Items { get; } = new List<string>();
        IReadOnlyList<string> ICommandLineValue.Items => Items;

        public string First => Items.First();

        #endregion

        #region function

        public void Add(string value)
        {
            Items.Add(value);
        }

        #endregion
    }

    /// <summary>
    /// <para>/key value</para>
    /// <para>/key=value</para>
    /// <para>-key value</para>
    /// <para>-key=value</para>
    /// <para>--key value</para>
    /// <para>--key=value</para>
    /// <para>/switch</para>
    /// <para>-switch</para>
    /// <para>--switch</para>
    /// </summary>
    internal class CommandLine
    {
        /// <summary>
        /// <para><see cref="Environment.GetCommandLineArgs()"/>からコマンドライン分解。</para>
        /// </summary>
        public CommandLine()
            : this(Environment.GetCommandLineArgs(), true)
        { }

        /// <summary>
        /// 引数からコマンドライン分解。
        /// </summary>
        /// <param name="arguments">コマンドライン引数。</param>
        /// <param name="firstIsProgram"><see cref="arguments"/>の先頭はプログラム名か。<para>Main関数だと含まれてて、<see cref="Environment.GetCommandLineArgs()"/>だと含まれてない的な。</para></param>
        public CommandLine(IEnumerable<string> arguments, bool firstIsProgram = false)
        {
            if(firstIsProgram) {
                ProgramName = arguments.FirstOrDefault() ?? string.Empty;
                Arguments = arguments.Skip(1).ToList();
            } else {
                ProgramName = string.Empty;
                Arguments = arguments.ToList();
            }
        }

        #region property

        /// <summary>
        /// プログラム名。
        /// <para>nullが入ることはない。</para>
        /// </summary>
        public string ProgramName { get; }
        /// <summary>
        /// プログラム名を含まないコマンドライン引数。
        /// </summary>
        public IReadOnlyList<string> Arguments { get; }

        public bool IsParsed { get; private set; }

        private List<CommandLineKey> KeyItems { get; } = new List<CommandLineKey>();
        public IReadOnlyList<CommandLineKey> Keys => KeyItems;

        private Dictionary<CommandLineKey, ICommandLineValue> ValueItems { get; } = new Dictionary<CommandLineKey, ICommandLineValue>();
        public IReadOnlyDictionary<CommandLineKey, ICommandLineValue> Values => ValueItems;

        private HashSet<CommandLineKey> SwitchItems { get; } = new HashSet<CommandLineKey>();
        public IReadOnlyCollection<CommandLineKey> Switch => SwitchItems;

        private List<string> UnknownItems { get; } = new List<string>();
        public IReadOnlyList<string> Unknowns => UnknownItems;

        public Exception Exception { get; private set; }

        #endregion

        #region function

        private CommandLineKey AddCore(CommandLineKey key)
        {
            KeyItems.Add(key);
            return key;
        }

        public CommandLineKey Add(CommandLineKey key)
        {
            if(key == null) {
                throw new ArgumentNullException(nameof(key));
            }

            if(!key.IsEnabledShortKey && !key.IsEnabledLongKey) {
                throw new ArgumentException($"{nameof(key.ShortKey)} and {nameof(key.LongKey)} is empty");
            }

            if(key.IsEnabledLongKey && key.LongKey.Length == 1) {
                throw new ArgumentException(nameof(key.LongKey));
            }

            if(KeyItems.Where(k => k.IsEnabledShortKey).Any(k => k.ShortKey == key.ShortKey)) {
                throw new ArgumentException($"exists {nameof(key.ShortKey)}: {key.ShortKey}");
            }

            if(KeyItems.Where(k => k.IsEnabledLongKey).Any(k => k.LongKey == key.LongKey)) {
                throw new ArgumentException($"exists {nameof(key.LongKey)}: {key.LongKey}");
            }

            return AddCore(key);
        }

        public CommandLineKey Add(char shortKey = CommandLineKey.EmptyShortKey, string longKey = "", bool hasValue = false, string description = "")
        {
            var value = new CommandLineKey(shortKey, longKey, hasValue, description);
            return Add(value);
        }

        string StripDoubleQuotes(string s)
        {
            if(s.Length > "\"\"".Length && s[0] == '"' && s[s.Length - 1] == '"') {
                return s.Substring(1, s.Length - 1 - 1);
            }

            return s;
        }

        private CommandLineKey GetCommandLineKey(string key)
        {
            Debug.Assert(key.Length != 0);

            if(key.Length == 1) {
                return KeyItems.Find(k => k.IsEnabledShortKey && k.ShortKey == key[0]);
            } else if(1 < key.Length) {
                return KeyItems.Find(k => k.IsEnabledLongKey && k.LongKey == key);
            }

            Debug.Assert(false);
            throw new NotImplementedException();
        }

        void SetValue(CommandLineKey key, string value)
        {
            if(ValueItems.TryGetValue(key, out var val)) {
                ((CommandLineValue)val).Add(value);
            } else {
                var commandLineValue = new CommandLineValue();
                commandLineValue.Add(value);
                ValueItems.Add(key, commandLineValue);
            }
        }

        void SetSwitch(CommandLineKey value)
        {
            SwitchItems.Add(value);
        }

        void SetUnknown(string value)
        {
            UnknownItems.Add(value);
        }

        private bool ExecuteCore()
        {
            try {
                var map = new[] {
                    new { Key = "--", IsLong = true },
                    new { Key = "-", IsLong = false },
                    new { Key = "/",IsLong = false },
                };

                for(var i = 0; i < Arguments.Count; i++) {
                    var argument = Arguments[i];
                    var arg = StripDoubleQuotes(argument);
                    if(string.IsNullOrWhiteSpace(arg)) {
                        continue;
                    }

                    foreach(var pair in map) {
                        if(arg.StartsWith(pair.Key)) {
                            var separatorIndex = arg.IndexOf('=');
                            if(separatorIndex == -1) {
                                var key = GetCommandLineKey(arg.Substring(pair.Key.Length));
                                if(key == null) {
                                    SetUnknown(arg);
                                    continue;
                                }
                                if(key.HasValue) {
                                    if(i < Arguments.Count - 1) {
                                        SetValue(key, Arguments[i + 1]);
                                        i += 1;
                                        continue;
                                    } else {
                                        SetValue(key, string.Empty);
                                        continue;
                                    }
                                } else {
                                    SetSwitch(key);
                                    continue;
                                }
                            } else {
                                var key = GetCommandLineKey(arg.Substring(pair.Key.Length, separatorIndex - pair.Key.Length));
                                if(key == null) {
                                    SetUnknown(arg);
                                    continue;
                                }
                                if(key.HasValue) {
                                    var val = arg.Substring(separatorIndex + 1);
                                    SetValue(key, StripDoubleQuotes(val));
                                    continue;
                                } else {
                                    SetSwitch(key);
                                    continue;
                                }
                            }
                        }
                    }
                }
                return true;
            } catch(Exception ex) {
                Exception = ex;
                return false;
            }
        }

        public bool Execute()
        {
            if(IsParsed) {
                throw new InvalidOperationException();
            }

            var result = ExecuteCore();
            IsParsed = true;

            return result;
        }

        public CommandLineKey GetKey(char shortKey)
        {
            return KeyItems
                .Concat(SwitchItems)
                .Where(k => k.IsEnabledShortKey)
                .FirstOrDefault(k => k.ShortKey == shortKey)
            ;
        }

        public CommandLineKey GetKey(string longKey)
        {
            return KeyItems
                .Concat(SwitchItems)
                .Where(k => k.IsEnabledLongKey)
                .FirstOrDefault(k => k.LongKey == longKey)
            ;
        }

        #endregion
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    internal class CommandLineAttribute : Attribute
    {
        public CommandLineAttribute(char shortKey = CommandLineKey.EmptyShortKey, string longKey = "", string description = "", bool hasValue = true)
        {
            ShortKey = shortKey;
            LongKey = longKey;
            Description = description;
            HasValue = hasValue;
        }

        #region property

        public char ShortKey { get; }
        public string LongKey { get; }
        public string Description { get; }
        public bool HasValue { get; }

        #endregion
    }

    internal class CommandLineConverter<TData>
    {
        public CommandLineConverter(CommandLine commandLine, TData data)
        {
            CommandLine = commandLine;
            Data = data;
        }

        #region property

        public CommandLine CommandLine { get; }
        public TData Data { get; }
        public Exception Exception { get; private set; }

        #endregion

        #region function

        IDictionary<PropertyInfo, CommandLineAttribute> GetPropertyAttributeMapping(Type type)
        {
            var properties = type.GetProperties();

            var map = new Dictionary<PropertyInfo, CommandLineAttribute>(properties.Length);
            foreach(var property in properties) {
                var attributes = property.GetCustomAttributes(typeof(CommandLineAttribute), true);
                if(attributes != null && attributes.Any()) {
                    map.Add(property, attributes.OfType<CommandLineAttribute>().First());
                }
            }

            return map;
        }

        IDictionary<PropertyInfo, CommandLineKey> SetPropertyKeyMapping(CommandLine commandLine, IDictionary<PropertyInfo, CommandLineAttribute> propertyAttributeMap)
        {
            var map = new Dictionary<PropertyInfo, CommandLineKey>();
            foreach(var pair in propertyAttributeMap) {
                var attribute = pair.Value;
                var key = commandLine.Add(attribute.ShortKey, attribute.LongKey, attribute.HasValue, attribute.Description);
                map.Add(pair.Key, key);
            }

            return map;
        }

        object ConvertValue(Type type, string rawValue)
        {
            if(type == typeof(float)) {
                return float.Parse(rawValue);
            }
            if(type == typeof(double)) {
                return double.Parse(rawValue);
            }

            return Convert.ChangeType(rawValue, type);
        }

        object GetTrueSwitch(Type type)
        {
            if(type == typeof(bool)) {
                return true;
            }

            if(type == typeof(char)) {
                return 'Y';
            }

            if(type == typeof(string)) {
                return true.ToString();
            }

            var numTypes = new[] {
                typeof(sbyte),
                typeof(byte),
                typeof(short),
                typeof(ushort),
                typeof(int),
                typeof(uint),
                typeof(long),
                typeof(ulong),
                typeof(float),
                typeof(double),
            };

            if(numTypes.Any(t => t == type)) {
                return 1;
            }

            if(type == typeof(decimal)) {
                return 1m;
            }

            throw new NotImplementedException();
        }

        public bool Mapping()
        {
            var type = Data.GetType();
            var attributeMap = GetPropertyAttributeMapping(type);

            var keyMap = SetPropertyKeyMapping(CommandLine, attributeMap);
            try {
                if(CommandLine.Execute()) {
                    foreach(var pair in keyMap) {
                        if(pair.Value.HasValue) {
                            // 値取得
                            if(CommandLine.Values.TryGetValue(pair.Value, out var value)) {
                                var convertedValue = ConvertValue(pair.Key.PropertyType, value.First);
                                pair.Key.SetValue(Data, convertedValue);
                            }
                        } else {
                            // スイッチ
                            if(CommandLine.Switch.Contains(pair.Value)) {
                                var switchValue = GetTrueSwitch(pair.Key.PropertyType);
                                pair.Key.SetValue(Data, switchValue);
                            }
                        }
                    }
                }

                return true;
            } catch(Exception ex) {
                Exception = ex;
                return false;
            }
        }

        #endregion
    }
}
