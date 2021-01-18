using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.Models.Command;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ContentTypeTextNet.Pe.Main.Test.Models.Command
{
    [TestClass]
    public class ApplicationCommandTest
    {
        #region property

        [TestMethod]
        public void EnumCheck()
        {
            var enumType = typeof(ApplicationCommand);
            var members = Enum.GetValues<ApplicationCommand>();
            foreach(var member in members) {
                var memberType = enumType.GetField(member.ToString());
                Assert.IsNotNull(memberType);
                Assert.IsNotNull(memberType!.GetCustomAttributes<CommandDescriptionAttribute>(), member.ToString());
            }
        }


        #endregion
    }

    class ApplicationCommandFinderTest
    {
    }
}
