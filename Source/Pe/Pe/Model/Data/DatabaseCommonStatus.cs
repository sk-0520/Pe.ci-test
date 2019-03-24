using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;
using ContentTypeTextNet.Pe.Main.Model.Data.Dto;

namespace ContentTypeTextNet.Pe.Main.Model.Data
{
    public interface IDatabaseCommonStatus
    {
        #region function

        void WriteCreate(IWritableCreateDto dto);
        void WriteUpdate(IWritableUpdateDto dto);
        void WriteCommon(IWritableCommonDto dto);
        IDictionary<string, object> CreateCommonDtoMapping();

        #endregion
    }

    public class DatabaseCommonStatus: IDatabaseCommonStatus
    {
        #region define

        class CommonDtoImpl: CommonDtoBase
        { }

        #endregion

        #region property

        public string Account { get; set; }
        public string ProgramName { get; set; }
        public Version ProgramVersion { get; set; }

        #endregion

        #region function

        public static DatabaseCommonStatus CreateCurrentAccount()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var assemblyName = assembly.GetName();

            return new DatabaseCommonStatus() {
                Account = Environment.UserName,
                ProgramName = assemblyName.Name,
                ProgramVersion = assemblyName.Version,
            };
        }

        void WriteCreateCore(IWritableCreateDto dto, [Timestamp(DateTimeKind.Utc)] DateTime timestamp)
        {
            dto.CreatedAccount = Account;
            dto.CreatedTimestamp = timestamp;
            dto.CreatedProgramName = ProgramName;
            dto.CreatedProgramVersion = ProgramVersion;
        }

        void WriteUpdateCore(IWritableUpdateDto dto, [Timestamp(DateTimeKind.Utc)] DateTime timestamp)
        {
            dto.UpdatedAccount = Account;
            dto.UpdatedTimestamp = timestamp;
            dto.UpdatedProgramName = ProgramName;
            dto.UpdatedProgramVersion = ProgramVersion;
        }


        #endregion

        #region IDatabaseCommonStatus

        public void WriteCreate(IWritableCreateDto dto)
        {
            WriteCreateCore(dto, DateTime.UtcNow);
        }

        public void WriteUpdate(IWritableUpdateDto dto)
        {
            WriteUpdateCore(dto, DateTime.UtcNow);
        }

        public void WriteCommon(IWritableCommonDto dto)
        {
            var timestamp = DateTime.UtcNow;

            WriteCreateCore(dto, timestamp);
            WriteUpdateCore(dto, timestamp);
        }

        public IDictionary<string, object> CreateCommonDtoMapping()
        {
            var result = new Dictionary<string, object>();

            var commonDto = new CommonDtoImpl();
            WriteCommon(commonDto);
            foreach(var propertyInfo in commonDto.GetType().GetProperties()) {
                var value = propertyInfo.GetValue(commonDto);
                result.Add(propertyInfo.Name, value);
            }

            return result;
        }

        #endregion
    }
}
