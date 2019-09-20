using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models;

namespace ContentTypeTextNet.Pe.Main.Models.Data.Dto.Entity
{
    public class NoteLayoutsEntityDto: CommonDtoBase
    {
        #region property

        public Guid NoteId { get; set; }
        public string? LayoutKind { get; set; }

        [PixelKind(Px.Logical)]
        public double X { get; set; }
        [PixelKind(Px.Logical)]
        public double Y { get; set; }
        [PixelKind(Px.Logical)]
        public double Width { get; set; }
        [PixelKind(Px.Logical)]
        public double Height { get; set; }

        #endregion
    }
}
