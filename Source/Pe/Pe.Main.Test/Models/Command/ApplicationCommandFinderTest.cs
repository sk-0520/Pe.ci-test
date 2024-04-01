using System;
using System.Reflection;
using ContentTypeTextNet.Pe.Main.Models.Command;
using Xunit;

namespace ContentTypeTextNet.Pe.Main.Test.Models.Command
{
    public class ApplicationCommandTest
    {
        #region property

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

    class ApplicationCommandFinderTest
    {
    }
}
