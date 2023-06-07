using System;
using System.Collections.Generic;
using System.Reflection;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Plugin;

namespace ContentTypeTextNet.Pe.Main.Models.Data
{
    /// <summary>
    /// DB共通カラム状態設定処理。
    /// </summary>
    public interface IDatabaseCommonStatus
    {
        #region function

        /// <summary>
        /// 作成ステータスの書き込み。
        /// </summary>
        /// <param name="dto">書き込み対象。</param>
        void WriteCreateTo(IWritableCreateDto dto);
        /// <summary>
        /// 更新ステータスの書き込み。
        /// </summary>
        /// <param name="dto">書き込み対象。</param>
        void WriteUpdateTo(IWritableUpdateDto dto);
        /// <summary>
        /// 作成・更新ステータスの書き込み。
        /// </summary>
        /// <param name="dto">書き込み対象。</param>
        void WriteCommonTo(IWritableCommonDto dto);
        /// <summary>
        /// 作成・更新ステータス設定済み辞書の生成。
        /// </summary>
        /// <returns>生成辞書。</returns>
        IDictionary<string, object> CreateCommonDtoMapping();

        #endregion
    }

    internal class DatabaseCommonStatus: IDatabaseCommonStatus
    {
        #region define

        class CommonDtoImpl: CommonDtoBase
        { }

        #endregion

        #region property

        public string? Account { get; set; }
        public string? ProgramName { get; set; }
        public Version? ProgramVersion { get; set; }

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

        public static DatabaseCommonStatus CreatePluginAccount(IPluginIdentifiers pluginIdentifiers, IPluginVersions pluginVersions)
        {
            return new DatabaseCommonStatus() {
                Account = Environment.UserName,
                ProgramName = pluginIdentifiers.PluginName,
                ProgramVersion = pluginVersions.PluginVersion,
            };
        }
        public static DatabaseCommonStatus CreatePluginAccount(IPluginInformation pluginInformation) => CreatePluginAccount(pluginInformation.PluginIdentifiers, pluginInformation.PluginVersions);

        void WriteCreateCore(IWritableCreateDto dto, [DateTimeKind(DateTimeKind.Utc)] DateTime timestamp)
        {
            dto.CreatedAccount = Account;
            dto.CreatedTimestamp = timestamp;
            dto.CreatedProgramName = ProgramName;
            dto.CreatedProgramVersion = ProgramVersion;
        }

        void WriteUpdateCore(IWritableUpdateDto dto, [DateTimeKind(DateTimeKind.Utc)] DateTime timestamp)
        {
            dto.UpdatedAccount = Account;
            dto.UpdatedTimestamp = timestamp;
            dto.UpdatedProgramName = ProgramName;
            dto.UpdatedProgramVersion = ProgramVersion;
        }


        #endregion

        #region IDatabaseCommonStatus

        public void WriteCreateTo(IWritableCreateDto dto)
        {
            WriteCreateCore(dto, DateTime.UtcNow);
        }

        public void WriteUpdateTo(IWritableUpdateDto dto)
        {
            WriteUpdateCore(dto, DateTime.UtcNow);
        }

        public void WriteCommonTo(IWritableCommonDto dto)
        {
            var timestamp = DateTime.UtcNow;

            WriteCreateCore(dto, timestamp);
            WriteUpdateCore(dto, timestamp);
        }

        public IDictionary<string, object> CreateCommonDtoMapping()
        {
            var result = new Dictionary<string, object>();

            var commonDto = new CommonDtoImpl();
            WriteCommonTo(commonDto);
            foreach(var propertyInfo in commonDto.GetType().GetProperties()) {
                var value = propertyInfo.GetValue(commonDto);
                result.Add(propertyInfo.Name, value!); // null は来んでしょ
            }

            return result;
        }

        #endregion
    }
}
