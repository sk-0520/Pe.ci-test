/*
This file is part of SharedLibrary.

SharedLibrary is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

SharedLibrary is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with SharedLibrary.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ContentTypeTextNet.Library.SharedLibrary.Logic
{
    /// <summary>
    /// <para>https://codeblitz.wordpress.com/2010/08/25/resourcedictionary-use-with-care/</para>
    /// </summary>
    public class CachedResourceDictionary: ResourceDictionary
    {
        private static Dictionary<Uri, WeakReference> _cache;

        static CachedResourceDictionary()
        {
            _cache = new Dictionary<Uri, WeakReference>();
        }

        private Uri _source;

        public new Uri Source
        {
            get { return _source; }
            set
            {
                _source = value;
                if(!_cache.ContainsKey(_source)) {
                    AddToCache();
                } else {
                    WeakReference weakReference = _cache[_source];
                    if(weakReference != null && weakReference.IsAlive) {
                        MergedDictionaries.Add((ResourceDictionary)weakReference.Target);
                    } else {
                        AddToCache();
                    }
                }

            }
        }

        private void AddToCache()
        {
            base.Source = _source;
            if(_cache.ContainsKey(_source)) {
                _cache.Remove(_source);
            }
            _cache.Add(_source, new WeakReference(this, false));
        }
    }
}
