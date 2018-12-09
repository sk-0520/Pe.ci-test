using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Main.Model.Data.Dto
{
    public interface IReadOnlyCreateDto : IReadOnlyDto
    {
        #region property

        DateTime CreatedTimestamp { get; }
        string CreatedAccount { get; }

        #endregion
    }
    public interface IWritableCreateDto : IReadOnlyDto
    {
        #region property

        DateTime CreatedTimestamp { get; set; }
        string CreatedAccount { get; set; }

        #endregion
    }

    public interface IReadOnlyUpdateDto : IReadOnlyDto
    {
        #region property

        DateTime UpdatedTimestamp { get; set; }
        string UpdatedAccount { get; set; }
        long UpdatedCount { get; set; }

        #endregion
    }
    public interface IWritableUpdateDto : IReadOnlyDto
    {
        #region property

        DateTime UpdatedTimestamp { get; set; }
        string UpdatedAccount { get; set; }
        long UpdatedCount { get; set; }

        #endregion
    }

    public interface IReadOnlyCommonDto : IReadOnlyCreateDto, IReadOnlyUpdateDto
    { }

    public interface IWritableCommonDto : IWritableCreateDto, IWritableUpdateDto
    { }

    public abstract class CommonDtoBase : DtoBase, IReadOnlyCommonDto, IWritableCommonDto
    {
        #region IWritableCreateDto

        public DateTime CreatedTimestamp { get; set; }
        public string CreatedAccount { get; set; }

        #endregion

        #region IWritableUpdateDto

        public DateTime UpdatedTimestamp { get; set; }
        public string UpdatedAccount { get; set; }
        public long UpdatedCount { get; set; }

        #endregion
    }
}
