using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.CliProxy.System;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace ContentTypeTextNet.Pe.Library.CliProxy.Test.System
{
    public class DirectEnvironmentProxyTest
    {
        #region function

        [Fact]
        public void CommandLineTest()
        {
            var test = new DirectEnvironmentProxy();
            Assert.Equal(Environment.CommandLine, test.CommandLine);
        }

        [Fact]
        public void CurrentDirectoryTest()
        {
            var test = new DirectEnvironmentProxy();
            Assert.Equal(Environment.CurrentDirectory, test.CurrentDirectory);
        }

        [Fact]
        public void Is64BitOperatingSystemTest()
        {
            var test = new DirectEnvironmentProxy();
            Assert.Equal(Environment.Is64BitOperatingSystem, test.Is64BitOperatingSystem);
        }

        [Fact]
        public void Is64BitProcessTest()
        {
            var test = new DirectEnvironmentProxy();
            Assert.Equal(Environment.Is64BitProcess, test.Is64BitProcess);
        }

        [Fact]
        public void MachineNameTest()
        {
            var test = new DirectEnvironmentProxy();
            Assert.Equal(Environment.MachineName, test.MachineName);
        }

        [Fact]
        public void NewLineTest()
        {
            var test = new DirectEnvironmentProxy();
            Assert.Equal(Environment.NewLine, test.NewLine);
        }

        [Fact]
        public void UserNameTest()
        {
            var test = new DirectEnvironmentProxy();
            Assert.Equal(Environment.UserName, test.UserName);
        }


        [Fact]
        public void ExpandEnvironmentVariablesTest()
        {
            var test = new DirectEnvironmentProxy();
            var input = "[%PATH%]";

            var expected = Environment.ExpandEnvironmentVariables(input);
            var actual = test.ExpandEnvironmentVariables(input);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetCommandLineArgsTest()
        {
            var test = new DirectEnvironmentProxy();

            var expected = Environment.GetCommandLineArgs();
            var actual = test.GetCommandLineArgs();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetEnvironmentVariableTest()
        {
            var test = new DirectEnvironmentProxy();
            var input = "PATH";

            var expected = Environment.GetEnvironmentVariable(input);
            var actual = test.GetEnvironmentVariable(input);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetEnvironmentVariable_throw_Test()
        {
            var test = new DirectEnvironmentProxy();
            var input = default(string)!;

            var expected = Assert.Throws<ArgumentNullException>(() => Environment.GetEnvironmentVariable(input));
            var actual = Assert.Throws<ArgumentNullException>(() => test.GetEnvironmentVariable(input));

            Assert.Equal(expected.Message, actual.Message);
        }

        [Fact]
        public void GetEnvironmentVariablesTest()
        {
            var test = new DirectEnvironmentProxy();

            var expected = Environment.GetEnvironmentVariables();
            var actual = test.GetEnvironmentVariables();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SetEnvironmentVariable_throw_arg1_null_Test() {
            var test = new DirectEnvironmentProxy();
            var input = default(string)!;

            var expected = Assert.Throws<ArgumentNullException>(() => Environment.SetEnvironmentVariable(input, null));
            var actual = Assert.Throws<ArgumentNullException>(() => test.SetEnvironmentVariable(input, null));

            Assert.Equal(expected.Message, actual.Message);
        }

        [Fact]
        public void SetEnvironmentVariableTest()
        {
            var test = new DirectEnvironmentProxy();
            const string key = "_PE_KEY";

            Environment.SetEnvironmentVariable(key, "value1");
            Assert.Equal("value1", Environment.GetEnvironmentVariable(key));

            test.SetEnvironmentVariable(key, "value2");
            Assert.Equal("value2", test.GetEnvironmentVariable(key));
        }

        [Theory]
        [InlineData("")]
        [InlineData("\0")]
        [InlineData("k=")]
        [InlineData("k=v")]
        [InlineData("=v")]
        public void SetEnvironmentVariable_throw_arg1_Test(string input)
        {
            var test = new DirectEnvironmentProxy();

            var expected = Assert.Throws<ArgumentException>(() => Environment.SetEnvironmentVariable(input, null));
            var actual = Assert.Throws<ArgumentException>(() => test.SetEnvironmentVariable(input, null));

            Assert.Equal(expected.Message, actual.Message);
            Assert.Equal(expected.ParamName, actual.ParamName);
            Assert.Equal("variable", actual.ParamName);
        }

        [Theory]
        [InlineData(Environment.SpecialFolder.Desktop)]
        [InlineData(Environment.SpecialFolder.UserProfile)]
        [InlineData(Environment.SpecialFolder.MyDocuments)]
        public void GetFolderPathTest(Environment.SpecialFolder input)
        {
            var test = new DirectEnvironmentProxy();

            var expected = Environment.GetFolderPath(input);
            var actual = test.GetFolderPath(input);

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
