using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity
{
    internal class LauncherFilesEntityPathDto : DtoBase
    {
        #region property

        Guid LauncherItemId { get; set; }
        public string File { get; set; } = string.Empty;
        public string Option { get; set; } = string.Empty;
        public string WorkDirectory { get; set; } = string.Empty;


        #endregion
    }

    internal class LauncherFilesEntityDto : CommonDtoBase
    {
        #region property

        Guid LauncherItemId { get; set; }

        public string File { get; set; } = string.Empty;
        public string Option { get; set; } = string.Empty;
        public string WorkDirectory { get; set; } = string.Empty;

        public bool IsEnabledCustomEnvVar { get; set; }
        public bool IsEnabledStandardIo { get; set; }
        public string StandardIoEncoding { get; set; } = string.Empty;
        public bool RunAdministrator { get; set; }

        #endregion
    }

    public class LauncherFilesEntityDao : EntityDaoBase
    {
        public LauncherFilesEntityDao(IDatabaseCommander commander, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
         : base(commander, statementLoader, implementation, loggerFactory)
        { }

        #region property

        public static class Column
        {
            #region property

            public static string LauncherItemId { get; } = "LauncherItemId";
            public static string File { get; } = "File";
            public static string Option { get; } = "Option";
            public static string WorkDirectory { get; } = "WorkDirectory";

            public static string IsEnabledCustomEnvVar { get; } = "IsEnabledCustomEnvVar";
            public static string IsEnabledStandardIo { get; } = "IsEnabledStandardIo";
            public static string StandardIoEncoding { get; } = "StandardIoEncoding";
            public static string RunAdministrator { get; } = "RunAdministrator";

            #endregion
        }

        #endregion

        #region function

        LauncherExecutePathData ConvertFromDto(LauncherFilesEntityPathDto dto)
        {
            var data = new LauncherExecutePathData() {
                Path = dto.File ?? string.Empty,
                Option = dto.Option ?? string.Empty,
                WorkDirectoryPath = dto.WorkDirectory ?? string.Empty,
            };

            return data;
        }

        LauncherFileData ConvertFromDto(LauncherFilesEntityDto dto)
        {
            var encodingConverter = new EncodingConverter(LoggerFactory);

            var data = new LauncherFileData() {
                Path = dto.File,
                Option = dto.Option,
                WorkDirectoryPath = dto.WorkDirectory,
                IsEnabledCustomEnvironmentVariable = dto.IsEnabledCustomEnvVar,
                IsEnabledStandardInputOutput = dto.IsEnabledStandardIo,
                StandardInputOutputEncoding = encodingConverter.Parse(dto.StandardIoEncoding),
                RunAdministrator = dto.RunAdministrator,
            };

            return data;
        }

        public LauncherExecutePathData SelectPath(Guid launcherItemId)
        {
            var builder = CreateSelectBuilder();
            builder.AddSelect(Column.File);
            builder.AddSelect(Column.Option);
            builder.AddSelect(Column.WorkDirectory);

            builder.AddValueParameter(Column.LauncherItemId, launcherItemId);

            var dto = SelectSingle<LauncherFilesEntityPathDto>(builder);
            var data = ConvertFromDto(dto);
            return data;
        }

        public LauncherFileData SelectFile(Guid launcherItemId)
        {
            var builder = CreateSelectBuilder();
            builder.AddSelect(Column.File);
            builder.AddSelect(Column.Option);
            builder.AddSelect(Column.WorkDirectory);
            builder.AddSelect(Column.IsEnabledCustomEnvVar);
            builder.AddSelect(Column.IsEnabledStandardIo);
            builder.AddSelect(Column.StandardIoEncoding);
            builder.AddSelect(Column.RunAdministrator);

            builder.AddValueParameter(Column.LauncherItemId, launcherItemId);

            var dto = SelectFirst<LauncherFilesEntityDto>(builder);
            var data = ConvertFromDto(dto);
            return data;
        }

        public bool InsertFile(Guid launcherItemId, LauncherExecutePathData data, IDatabaseCommonStatus commonStatus)
        {
            var encodingConverter = new EncodingConverter(LoggerFactory);

            var statement = LoadStatement();
            var param = commonStatus.CreateCommonDtoMapping();
            param[Column.LauncherItemId] = launcherItemId;
            param[Column.File] = data.Path;
            param[Column.Option] = data.Option;
            param[Column.WorkDirectory] = data.WorkDirectoryPath;
            param[Column.StandardIoEncoding] = encodingConverter.ToString(EncodingConverter.DefaultStandardInputOutputEncoding);

            return Commander.Execute(statement, param) == 1;
        }

        public bool UpdateCustomizeLauncherFile(Guid launcherItemId, ILauncherExecutePathParameter pathParameter, ILauncherExecuteCustomParameter customParameter, IDatabaseCommonStatus commonStatus)
        {
            var encodingConverter = new EncodingConverter(LoggerFactory);

            var builder = CreateUpdateBuilder(commonStatus);
            builder.AddValueParameter(Column.File, pathParameter.Path);
            builder.AddValueParameter(Column.Option, pathParameter.Option);
            builder.AddValueParameter(Column.WorkDirectory, pathParameter.WorkDirectoryPath);
            builder.AddValueParameter(Column.IsEnabledCustomEnvVar, customParameter.IsEnabledCustomEnvironmentVariable);
            builder.AddValueParameter(Column.IsEnabledStandardIo, customParameter.IsEnabledStandardInputOutput);
            builder.AddValueParameter(Column.StandardIoEncoding, encodingConverter.ToString(customParameter.StandardInputOutputEncoding));

            builder.AddValueParameter(Column.RunAdministrator, customParameter.RunAdministrator);
            builder.AddKey(Column.LauncherItemId, launcherItemId);

            return ExecuteUpdate(builder) == 1;
        }

        public bool DeleteFileByLauncherItemId(Guid launcherItemId)
        {
            var builder = CreateDeleteBuilder();
            builder.AddKey(Column.LauncherItemId, launcherItemId);
            return ExecuteDelete(builder) == 1;
        }

        #endregion
    }
}
