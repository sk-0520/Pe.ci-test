using System;
using System.Reflection;
using System.Security.Cryptography;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.Models.Command;
using Xunit;

namespace ContentTypeTextNet.Pe.Main.Test.Models.Command
{
    public class ApplicationCommandTest
    {
        #region function

        [Fact]
        public void EnumCheck()
        {
            var enumType = typeof(ApplicationCommand);
            var members = Enum.GetValues<ApplicationCommand>();
            foreach(var member in members) {
                var memberType = enumType.GetField(member.ToString());
                Assert.NotNull(memberType);
                Assert.NotNull(memberType!.GetCustomAttributes<CommandDescriptionAttribute>());
            }
        }


        #endregion
    }

    public class ApplicationCommandParameterFactoryTest
    {
        #region property

        //private Test Test = Test.Create();

        #endregion

        #region function

        [Fact]
        public void CreateParameterTest()
        {
            var applicationConfiguration = Test.GetApplicationConfiguration(this);

            var test = new ApplicationCommandParameterFactory(applicationConfiguration.Command, new CurrentDispatcherWrapper());
            var actual = test.CreateParameter(ApplicationCommand.Help, a => {
                Assert.Fail();
            });
            Assert.Equal("help", actual.Header);
        }

        #endregion
    }

    class ApplicationCommandFinderTest
    {
    }
}
