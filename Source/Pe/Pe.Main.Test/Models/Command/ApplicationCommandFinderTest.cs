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
            var members = EnumUtility.GetMembers<ApplicationCommand>();
            foreach(var member in members) {
                var memberType = enumType.GetField(member.ToString());
                Assert.IsNotNull(memberType);
                Assert.IsNotNull(memberType!.GetCustomAttribute<System.ComponentModel.DescriptionAttribute>(), member.ToString());
            }
        }


        #endregion
    }

    class ApplicationCommandFinderTest
    {
    }
}
