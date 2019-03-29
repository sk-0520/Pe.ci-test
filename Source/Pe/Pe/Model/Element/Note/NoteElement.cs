using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;

namespace ContentTypeTextNet.Pe.Main.Model.Element.Note
{
    public class NoteElement : ElementBase
    {
        public NoteElement(Guid noteId, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            NoteId = noteId;
        }

        #region property

        public Guid NoteId { get; }

        #endregion

        #region function

        void LoadNote()
        {

        }

        #endregion

        #region ElementBase

        protected override void InitializeImpl()
        {
            LoadNote();
        }

        #endregion
    }
}
