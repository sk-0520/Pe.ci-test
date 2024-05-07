using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using Xunit;

namespace ContentTypeTextNet.Pe.Bridge.Test.Models.Data
{
    public class AuthorTest
    {
        #region function

        [Fact]
        public void ConstructorTest()
        {
            var test = new Author("name");
            Assert.Equal("name", test.Name);
            Assert.Empty(test.Contacts);
        }

        [Fact]
        public void ContactsTest()
        {
            var test = new Author("name");
            test.Contacts.Add(new Contact("k1", "v1"));
            test.Contacts.Add(new Contact("k2", "v2"));

            Assert.Equal("name", test.Name);
            Assert.Equal(2, test.Contacts.Count);

            Assert.Equal("k1", test.Contacts[0].ContactKind);
            Assert.Equal("v1", test.Contacts[0].ContactValue);

            Assert.Equal("k2", test.Contacts[1].ContactKind);
            Assert.Equal("v2", test.Contacts[1].ContactValue);
        }

        #endregion
    }
}
