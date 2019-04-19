using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Main.Model.Data.Dto.Entity
{
    public class NoteLayoutsEntityDto: RowDtoBase
    {
        #region property

        public Guid NoteId { get; set; }
        public string LayoutKind { get; set; }

        public double X { get; set; }
        public double Y { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }

        #endregion
    }
}
