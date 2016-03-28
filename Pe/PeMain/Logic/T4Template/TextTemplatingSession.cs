/*
This file is part of Pe.

Pe is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

Pe is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with Pe.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TextTemplating;

namespace ContentTypeTextNet.Pe.PeMain.Logic.T4Template
{
    /// <summary>
    /// セッションの実装.
    /// 単純にDictionaryと、IDを持っているだけのコレクションクラスである.
    /// </summary>
    [Serializable]
    public sealed class TextTemplatingSession: Dictionary<string, Object>, ITextTemplatingSession, ISerializable
    {
        public TextTemplatingSession()
        {
            this.Id = Guid.NewGuid();
        }

        private TextTemplatingSession(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            this.Id = (Guid)info.GetValue("Id", typeof(Guid));
        }

        [SecurityCritical]
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("Id", this.Id);
        }

        public Guid Id
        {
            get;
            private set;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var o = obj as TextTemplatingSession;
            return o != null && o.Equals(this);
        }

        public bool Equals(Guid other)
        {
            return other.Equals(Id);
        }

        public bool Equals(ITextTemplatingSession other)
        {
            return other != null && other.Id == this.Id;
        }
    }
}
