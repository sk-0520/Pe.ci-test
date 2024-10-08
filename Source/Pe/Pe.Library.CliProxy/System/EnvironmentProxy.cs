using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace ContentTypeTextNet.Pe.Library.CliProxy.System
{
    public interface IEnvironmentProxy
    {
        #region property

        /// <inheritdoc cref="Environment.CommandLine"/>
        string CommandLine { get; }

        /// <inheritdoc cref="Environment.CurrentDirectory"/>
        string CurrentDirectory { get; }

        /// <inheritdoc cref="Environment.Is64BitOperatingSystem"/>
        bool Is64BitOperatingSystem { get; }
        /// <inheritdoc cref="Environment.Is64BitProcess"/>
        bool Is64BitProcess { get; }

        /// <inheritdoc cref="Environment.MachineName"/>
        string MachineName { get; }
        /// <inheritdoc cref="Environment.NewLine"/>
        string NewLine { get; }

        /// <inheritdoc cref="Environment.UserName"/>
        string UserName { get; }

        #endregion

        #region function

        /// <inheritdoc cref="Environment.ExpandEnvironmentVariables(string)"/>
        string ExpandEnvironmentVariables(string name);

        /// <inheritdoc cref="Environment.GetCommandLineArgs()"/>
        string[] GetCommandLineArgs();

        /// <inheritdoc cref="Environment.GetEnvironmentVariable(string)"/>
        string? GetEnvironmentVariable(string variable);

        /// <inheritdoc cref="Environment.GetEnvironmentVariables()"/>
        IDictionary GetEnvironmentVariables();

        /// <inheritdoc cref="Environment.SetEnvironmentVariable(string, string?)"/>
        void SetEnvironmentVariable(string variable, string? value);

        /// <inheritdoc cref="Environment.SetEnvironmentVariable(Environment.SpecialFolder)"/>
        string GetFolderPath(Environment.SpecialFolder folder);

        #endregion
    }

    public class DirectEnvironmentProxy: IEnvironmentProxy
    {
        #region IEnvironmentProxy

        #region property

        public string CommandLine => Environment.CommandLine;
        public string CurrentDirectory => Environment.CurrentDirectory;

        public bool Is64BitOperatingSystem => Environment.Is64BitOperatingSystem;
        public bool Is64BitProcess => Environment.Is64BitProcess;

        public string MachineName => Environment.MachineName;
        public string NewLine => Environment.NewLine;

        public string UserName => Environment.UserName;

        #endregion

        #region function

        public string ExpandEnvironmentVariables(string name)
        {
            return Environment.ExpandEnvironmentVariables(name);
        }

        public string[] GetCommandLineArgs()
        {
            return Environment.GetCommandLineArgs();
        }

        public string? GetEnvironmentVariable(string variable)
        {
            return Environment.GetEnvironmentVariable(variable);
        }

        public IDictionary GetEnvironmentVariables()
        {
            return Environment.GetEnvironmentVariables()
                .Cast<DictionaryEntry>()
                .ToDictionary(k => (string)k.Key, v => v.Value as string ?? string.Empty)
            ;
        }

        public void SetEnvironmentVariable(string variable, string? value)
        {
            Environment.SetEnvironmentVariable(variable, value);
        }

        public string GetFolderPath(Environment.SpecialFolder folder)
        {
            return Environment.GetFolderPath(folder);
        }

        #endregion

        #endregion
    }
}
