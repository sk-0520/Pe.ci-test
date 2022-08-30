using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Applications.Configuration;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using ContentTypeTextNet.Pe.Main.Models.Platform;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.About
{
    public class AboutElement: ElementBase
    {
        public AboutElement(EnvironmentParameters environmentParameters, IClipboardManager clipboardManager, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            EnvironmentParameters = environmentParameters;
            ApplicationConfiguration = EnvironmentParameters.ApplicationConfiguration;
            ClipboardManager = clipboardManager;
        }

        #region property

        private EnvironmentParameters EnvironmentParameters { get; }
        private ApplicationConfiguration ApplicationConfiguration { get; }
        private IClipboardManager ClipboardManager { get; }

        private List<AboutComponentItem> ComponentsImpl { get; } = new List<AboutComponentItem>();
        public IReadOnlyList<AboutComponentItem> Components => ComponentsImpl;

        #endregion

        #region function

        private AboutComponentsData LoadComponents()
        {
            var serializer = new JsonDataSerializer();
            using(var stream = EnvironmentParameters.ComponentsFile.OpenRead()) {
                return serializer.Load<AboutComponentsData>(stream);
            }
        }

        private IEnumerable<AboutComponentItem> ToItems(AboutComponentKind kind, IEnumerable<AboutComponentData> items)
        {
            return items
                .Counting()
                .Select(i => new AboutComponentItem(kind, i.Value, i.Number))
            ;
        }

        private IEnumerable<AboutComponentItem> GetApplicationItems()
        {
            var versionConverter = new VersionConverter();

            var data = new[] {
                new AboutComponentData() {
                    Name = BuildStatus.Name,
                    Uri = ApplicationConfiguration.General.ProjectWebSiteUri.ToString(),
                    License = new AboutLicenseData() {
                        Name = ApplicationConfiguration.General.LicenseName,
                        Uri = ApplicationConfiguration.General.LicenseUri.ToString(),
                    },
                    Comment = string.Join(
                        Environment.NewLine,
                        new [] {
                            $"{BuildStatus.BuildType}: {versionConverter.ConvertNormalVersion(BuildStatus.Version)} - {BuildStatus.Revision}",
                            $"CLR: {Environment.Version}",
                        }
                    )
                },
            };

            return ToItems(AboutComponentKind.Application, data);
        }

        public void OpenUri(Uri uri)
        {
            var systemExecutor = new SystemExecutor();
            try {
                systemExecutor.OpenUri(uri);
            } catch(Exception ex) {
                Logger.LogWarning(ex, ex.Message);
            }
        }

        public void OpenForumUri()
        {
            OpenUri(ApplicationConfiguration.General.ProjectForumUri);
        }
        public void OpenProjectUri()
        {
            OpenUri(ApplicationConfiguration.General.ProjectRepositoryUri);
        }

        public void CopyShortInformation()
        {
            var infoCollector = new ApplicationInformationCollector(EnvironmentParameters);
            var s = infoCollector.GetShortInformation();
            ClipboardManager.CopyText(s, ClipboardNotify.User);
        }
        public void CopyLongInformation()
        {
            var infoCollector = new ApplicationInformationCollector(EnvironmentParameters);
            var s = infoCollector.GetLongInformation();
            ClipboardManager.CopyText(s, ClipboardNotify.User);
        }

        private void OpenDirectory(DirectoryInfo directory)
        {
            try {
                var systemExecutor = new SystemExecutor();
                systemExecutor.ExecuteFile(directory.FullName);
            } catch(Exception ex) {
                Logger.LogWarning(ex, ex.Message);
            }
        }

        public void OpenApplicationDirectory()
        {
            OpenDirectory(EnvironmentParameters.RootDirectory);
        }
        public void OpenUserDirectory()
        {
            OpenDirectory(EnvironmentParameters.UserRoamingDirectory);
        }
        public void OpenMachineDirectory()
        {
            OpenDirectory(EnvironmentParameters.MachineDirectory);
        }
        public void OpenTemporaryDirectory()
        {
            OpenDirectory(EnvironmentParameters.TemporaryDirectory);
        }

        #endregion

        #region ElementBase

        protected override void InitializeImpl()
        {
            var components = LoadComponents();

            ComponentsImpl.AddRange(GetApplicationItems());
            ComponentsImpl.AddRange(ToItems(AboutComponentKind.Library, components.Library));
            ComponentsImpl.AddRange(ToItems(AboutComponentKind.Resource, components.Resource));
        }

        public bool CheckCreateUninstallBatch(string uninstallBatchFilePath, UninstallTarget uninstallTargets)
        {
            if(uninstallTargets == UninstallTarget.None) {
                Logger.LogInformation("アンインストール対象未選択");
                return false;
            }

            if(string.IsNullOrWhiteSpace(uninstallBatchFilePath)) {
                Logger.LogInformation("アンインストールバッチファイルパス未設定");
                return false;
            }

            var path = Environment.ExpandEnvironmentVariables(uninstallBatchFilePath);
            if(Directory.Exists(path)) {
                Logger.LogInformation("アンインストールバッチファイルパスはディレクトリして存在する: {0}", path);
                return false;
            }

            return true;
        }

        public void CreateUninstallBatch(string uninstallBatchFilePath, UninstallTarget uninstallTargets)
        {
            if(!CheckCreateUninstallBatch(uninstallBatchFilePath, uninstallTargets)) {
                throw new InvalidOperationException();
            }

            IOUtility.MakeFileParentDirectory(uninstallBatchFilePath);
            using(var stream = new FileStream(uninstallBatchFilePath, FileMode.Create, FileAccess.ReadWrite, FileShare.Read)) {
                using var writer = new StreamWriter(stream, Encoding.UTF8);
                writer.WriteLine("echo OFF");
                writer.WriteLine("chcp 65001");
                writer.WriteLine();

                var deleteItems = new[] {
                    new { Target = UninstallTarget.User, Directory = EnvironmentParameters.UserRoamingDirectory },
                    new { Target = UninstallTarget.Machine, Directory = EnvironmentParameters.MachineDirectory },
                    new { Target = UninstallTarget.Temporary, Directory = EnvironmentParameters.TemporaryDirectory },
                    new { Target = UninstallTarget.Application, Directory = EnvironmentParameters.RootDirectory },
                };

                foreach(var deleteItem in deleteItems.Where(i => uninstallTargets.HasFlag(i.Target))) {
                    writer.WriteLine("echo [{0}]", deleteItem.Target);
                    writer.WriteLine("rmdir /S /Q {0}", CommandLine.Escape(deleteItem.Directory.FullName));
                    writer.WriteLine();
                }

                if(uninstallTargets.HasFlag(UninstallTarget.Application)) {
                    var startupRegister = new StartupRegister(LoggerFactory);
                    if(startupRegister.Exists()) {
                        var startupFilePath = startupRegister.GetStartupFilePath();
                        writer.WriteLine("echo [{0}]", "STARTUP");
                        writer.WriteLine("del {0}", CommandLine.Escape(startupFilePath));
                        writer.WriteLine();
                    }
                }

                if(uninstallTargets.HasFlag(UninstallTarget.Batch)) {
                    writer.WriteLine("echo [{0}]", UninstallTarget.Batch);
                    writer.WriteLine("echo {0}", "バッチファイル削除エラーは無視してください");
                    writer.WriteLine("del /F \"%~f0\"");
                    writer.WriteLine();
                }
            }
        }

        #endregion
    }
}
