using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ContentTypeTextNet.Pe.Main.Models
{
    public static class ResourceUtility
    {
        #region function

        public static Stream OpenSyntaxStream(string syntaxResourceName)
        {
            var s = Properties.Resources.ResourceManager.GetString(syntaxResourceName);
            if(s == null) {
                throw new InvalidProgramException($"{nameof(syntaxResourceName)}: {syntaxResourceName}");
            }

            var binary = Encoding.UTF8.GetBytes(s);
            return new MemoryStream(binary);
        }

        #endregion
    }
}
