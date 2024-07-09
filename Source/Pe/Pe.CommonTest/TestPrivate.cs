using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit;

namespace ContentTypeTextNet.Pe.Test
{
    public class PrivateObject
    {
        public PrivateObject(object instance)
        {
            Instance = instance;

            Type = Instance.GetType();
            Fields = Type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).ToDictionary(k => k.Name, v => v);
            Properties = Type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).ToDictionary(k => k.Name, v => v);
            Methods = Type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .GroupBy(a => a.Name)
                .ToDictionary(k => k.Key, v => (IReadOnlyList<MethodInfo>)v.ToList())
            ;
        }

        #region property

        private object Instance { get; }
        public Type Type { get; }

        public IReadOnlyDictionary<string, FieldInfo> Fields { get; }
        public IReadOnlyDictionary<string, PropertyInfo> Properties { get; }
        public IReadOnlyDictionary<string, IReadOnlyList<MethodInfo>> Methods { get; }

        #endregion

        #region function

        public object? GetField(string name)
        {
            if(Fields.TryGetValue(name, out var member)) {
                return member.GetValue(Instance);
            }
            throw new TestPrivateObjectFieldException(name);
        }

        public void SetField(string name, object? value)
        {
            if(Fields.TryGetValue(name, out var member)) {
                member.SetValue(Instance, value);
                return;
            }
            throw new TestPrivateObjectFieldException(name);
        }

        public object? GetProperty(string name)
        {
            if(Properties.TryGetValue(name, out var member)) {
                return member.GetValue(Instance);
            }
            throw new TestPrivateObjectPropertyException(name);
        }

        public void SetProperty(string name, object? value)
        {
            if(Properties.TryGetValue(name, out var member)) {
                member.SetValue(Instance, value);
                return;
            }
            throw new TestPrivateObjectPropertyException(name);
        }

        public object? Invoke(string name, params object[] arguments)
        {
            if(Methods.TryGetValue(name, out var members)) {
                var parameters = arguments.Select(a => a.GetType()).ToArray();
                foreach(var member in members) {
                    var memberParams = member
                        .GetParameters()
                        .Select(a => a.ParameterType)
                    ;
                    if(parameters.SequenceEqual(memberParams)) {
                        var result = member.Invoke(Instance, arguments);
                        return result;
                    }
                }

                throw new TestPrivateObjectMethodParametersException(name);
            }

            throw new TestPrivateObjectMethodException(name);
        }

        #endregion
    }
}
