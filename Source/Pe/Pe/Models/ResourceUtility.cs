using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ContentTypeTextNet.Pe.Main.Models
{
    public static class ResourceUtility
    {
        #region function

        public static Stream OpenSyntaxStreamByName(string syntaxResourceName)
        {
            var resourceValue = Properties.Resources.ResourceManager.GetString(syntaxResourceName);
            if(resourceValue == null) {
                throw new InvalidProgramException($"{nameof(syntaxResourceName)}: {syntaxResourceName}");
            }

            return OpenSyntaxStream(resourceValue);
        }

        public static Stream OpenSyntaxStream(string syntax)
        {
            var binary = Encoding.UTF8.GetBytes(syntax);
            return new MemoryStream(binary);
        }

        #endregion
    }
}
