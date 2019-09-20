using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Library.Shared.Library.Model
{
    public static class UriUtility
    {
        #region function

        public static string CombinePath(IEnumerable<string> hierarchies)
        {
            return hierarchies.Aggregate((left, right) => left.TrimEnd('/') + "/" + right.TrimStart('/')); ;
        }

        public static Uri CombineUri(string baseUri, IEnumerable<string> hierarchies)
        {
            var list = new List<string>() {
                baseUri,
            };
            list.AddRange(hierarchies);
            //hierarchy
            var originUri = CombinePath(list);

            return new Uri(originUri);
        }

        public static Uri CombineUri(string baseUri, params string[] hierarchies) => CombineUri(baseUri, hierarchies.AsEnumerable<string>());

        public static Uri CombineUri(Uri baseUri, IEnumerable<string> hierarchies) => CombineUri(baseUri.OriginalString, hierarchies);
        public static Uri CombineUri(Uri baseUri, params string[] hierarchies) => CombineUri(baseUri, hierarchies.AsEnumerable<string>());
        #endregion
    }
}
