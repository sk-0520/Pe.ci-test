using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Core.Models;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Dao
{
    public interface IReadOnlyCreateDto : IReadOnlyDto
    {
        #region property

        [Timestamp(DateTimeKind.Utc)]
        DateTime CreatedTimestamp { get; }
        string? CreatedAccount { get; }
        string? CreatedProgramName { get; }
        Version? CreatedProgramVersion { get; }

        #endregion
    }
    public interface IWritableCreateDto : IReadOnlyDto
    {
        #region property

        [Timestamp(DateTimeKind.Utc)]
        DateTime CreatedTimestamp { get; set; }
        string? CreatedAccount { get; set; }
        string? CreatedProgramName { get; set; }
        Version? CreatedProgramVersion { get; set; }

        #endregion
    }

    public interface IReadOnlyUpdateDto : IReadOnlyDto
    {
        #region property

        [Timestamp(DateTimeKind.Utc)]
        DateTime UpdatedTimestamp { get; }
        string? UpdatedAccount { get; }
        long UpdatedCount { get; }
        string? UpdatedProgramName { get; }
        Version? UpdatedProgramVersion { get; }

        #endregion
    }
    public interface IWritableUpdateDto : IReadOnlyDto
    {
        #region property

        [Timestamp(DateTimeKind.Utc)]
        DateTime UpdatedTimestamp { get; set; }
        string? UpdatedAccount { get; set; }
        string? UpdatedProgramName { get; set; }
        Version? UpdatedProgramVersion { get; set; }
        long UpdatedCount { get; set; }

        #endregion
    }

    public abstract class CreateDtoBase : IReadOnlyCreateDto, IWritableCreateDto
    {
        #region IReadOnlyCreateDto

        [Timestamp(DateTimeKind.Utc)]
        public DateTime CreatedTimestamp { get; set; }
        public string? CreatedAccount { get; set; }
        public string? CreatedProgramName { get; set; }
        public Version? CreatedProgramVersion { get; set; }

        #endregion
    }
    public abstract class UpdateDtoBase : IReadOnlyUpdateDto, IWritableUpdateDto
    {
        #region IReadOnlyUpdateDto

        [Timestamp(DateTimeKind.Utc)]
        public DateTime UpdatedTimestamp { get; set; }
        public string? UpdatedAccount { get; set; }
        public string? UpdatedProgramName { get; set; }
        public Version? UpdatedProgramVersion { get; set; }
        public long UpdatedCount { get; set; }

        #endregion
    }
    public interface IReadOnlyCommonDto : IReadOnlyCreateDto, IReadOnlyUpdateDto
    { }

    public interface IWritableCommonDto : IWritableCreateDto, IWritableUpdateDto
    { }

    public abstract class CommonDtoBase : DtoBase, IReadOnlyCommonDto, IWritableCommonDto
    {
        #region IWritableCreateDto

        [Timestamp(DateTimeKind.Utc)]
        public DateTime CreatedTimestamp { get; set; }
        public string? CreatedAccount { get; set; }
        public string? CreatedProgramName { get; set; }
        public Version? CreatedProgramVersion { get; set; }

        #endregion

        #region IWritableUpdateDto

        [Timestamp(DateTimeKind.Utc)]
        public DateTime UpdatedTimestamp { get; set; }
        public string? UpdatedAccount { get; set; }
        public string? UpdatedProgramName { get; set; }
        public Version? UpdatedProgramVersion { get; set; }
        public long UpdatedCount { get; set; }

        #endregion
    }

    public interface IReadOnlyRowDtoBase : IReadOnlyCommonDto
    { }

    /// <summary>
    /// ドメイン系で使用。
    /// </summary>
    public abstract class RowDtoBase : CommonDtoBase, IReadOnlyRowDtoBase
    { }
}
