using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models.Data;

namespace ContentTypeTextNet.Pe.PluginBase.Models.Data
{
    public class Contact : IContact
    {
        public Contact(string kind, string value)
        {
            ContactKind = kind;
            ContactValue = value;
        }

        #region IContact

        public string ContactKind { get; }

        public string ContactValue { get; }

        #endregion
    }
}
