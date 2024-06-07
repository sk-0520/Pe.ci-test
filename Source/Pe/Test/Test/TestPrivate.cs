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

    public class PrivateObjectTest
    {
        #region define

        private class Class
        {
            #region variable

#pragma warning disable 169
            private int Field;
#pragma warning restore 169

            #endregion

            #region proeprty

            private int Property { get; set; }

            #endregion

            #region function

            private void Method()
            {
                //nop
            }

            private int Method(int a)
            {
                return a + a;
            }

            #endregion
        }

        #endregion

        #region function

        [Fact]
        public void FieldTest()
        {
            var obj = new Class();
            var test = new PrivateObject(obj);

            Assert.Throws<TestPrivateObjectFieldException>(() => test.SetField("<Field>", null));
            Assert.Throws<TestPrivateObjectFieldException>(() => test.GetField("<Field>"));

            Assert.Equal(0, test.GetField("Field"));
            test.SetField("Field", 123);
            Assert.Equal(123, test.GetField("Field"));
        }

        [Fact]
        public void PropertyTest()
        {
            var obj = new Class();
            var test = new PrivateObject(obj);

            Assert.Throws<TestPrivateObjectPropertyException>(() => test.SetProperty("<Property>", null));
            Assert.Throws<TestPrivateObjectPropertyException>(() => test.GetProperty("<Property>"));

            Assert.Equal(0, test.GetProperty("Property"));
            test.SetProperty("Property", 123);
            Assert.Equal(123, test.GetProperty("Property"));
        }

        [Fact]
        public void MethodTest()
        {
            var obj = new Class();
            var test = new PrivateObject(obj);

            Assert.Null(test.Invoke("Method"));
            Assert.Equal(2, test.Invoke("Method", 1));
            Assert.Equal(2, test.Invoke("Method", 1));

            Assert.Throws<TestPrivateObjectMethodException>(() => test.Invoke("<Method>"));
            Assert.Throws<TestPrivateObjectMethodParametersException>(() => test.Invoke("Method", (short)1));
            Assert.Throws<TestPrivateObjectMethodParametersException>(() => test.Invoke("Method", (long)1));
            Assert.Throws<TestPrivateObjectMethodParametersException>(() => test.Invoke("Method", (float)1));
            Assert.Throws<TestPrivateObjectMethodParametersException>(() => test.Invoke("Method", (double)1));
            Assert.Throws<TestPrivateObjectMethodParametersException>(() => test.Invoke("Method", (decimal)1));
            Assert.Throws<TestPrivateObjectMethodParametersException>(() => test.Invoke("Method", "A"));
        }

        #endregion
    }
}
