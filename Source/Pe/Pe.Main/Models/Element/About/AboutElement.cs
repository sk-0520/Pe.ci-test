using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ContentTypeTextNet.Pe.Core.Models.Serialization;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Applications.Configuration;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using ContentTypeTextNet.Pe.Main.Models.Platform;
using Microsoft.Extensions.Logging;
using ContentTypeTextNet.Pe.Library.Base;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Base.Linq;
using System.Threading;
using ContentTypeTextNet.Pe.Library.Database;
using ContentTypeTextNet.Pe.Main.Models.Html;
using System.Globalization;

namespace ContentTypeTextNet.Pe.Main.Models.Element.About
{
    public class AboutElement: ElementBase
    {
        public AboutElement(EnvironmentParameters environmentParameters, IClipboardManager clipboardManager, IMainDatabaseBarrier mainDatabaseBarrier, ILargeDatabaseBarrier largeDatabaseBarrier, ITemporaryDatabaseBarrier temporaryDatabaseBarrier, IDatabaseStatementLoader databaseStatementLoader, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            EnvironmentParameters = environmentParameters;
            ApplicationConfiguration = EnvironmentParameters.ApplicationConfiguration;
            ClipboardManager = clipboardManager;
            MainDatabaseBarrier = mainDatabaseBarrier;
            LargeDatabaseBarrier = largeDatabaseBarrier;
            TemporaryDatabaseBarrier = temporaryDatabaseBarrier;
            DatabaseStatementLoader = databaseStatementLoader;
        }

        #region property

        private EnvironmentParameters EnvironmentParameters { get; }
        private ApplicationConfiguration ApplicationConfiguration { get; }
        private IClipboardManager ClipboardManager { get; }

        private IMainDatabaseBarrier MainDatabaseBarrier { get; }
        private ILargeDatabaseBarrier LargeDatabaseBarrier { get; }
        private ITemporaryDatabaseBarrier TemporaryDatabaseBarrier { get; }
        private IDatabaseStatementLoader DatabaseStatementLoader { get; }

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
                    Uri = ApplicationConfiguration.General.AuthorWebSiteUri.ToString(),
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
        public void OpenRepositoryUri()
        {
            OpenUri(ApplicationConfiguration.General.ProjectRepositoryUri);
        }
        public void OpenWebsiteUri()
        {
            OpenUri(ApplicationConfiguration.General.ProjectWebSiteUri);
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

        public void OutputHtmlSetting(string outputPath)
        {
            var settingExporter = new SettingExporter(
                MainDatabaseBarrier,
                LargeDatabaseBarrier,
                TemporaryDatabaseBarrier,
                DatabaseStatementLoader,
                LoggerFactory
            );

            var groups = settingExporter.GetGroups();
            var items = settingExporter.GetLauncherItems();
            var notes = settingExporter.GetNotes();

            var html = new HtmlDocument();

            // ヘッダー
            var headerElement = html.Factory.CreateTree(
                "header",
                html.Factory.CreateTree(
                    "h1",
                    html.CreateTextNode($"Pe: {BuildStatus.BuildType} {BuildStatus.Version} - {BuildStatus.Revision}")
                )
            );
            html.Body.AppendChild(headerElement);

            // グループ
            var groupElement = html.Factory.CreateTree(
                "section",
                [
                    html.Factory.CreateTree(
                        "h2",
                        html.CreateTextNode("group")
                    ),
                    html.Factory.CreateTree(
                        "ul",
                        groups.Select(a => {
                            return html.Factory.CreateTree(
                                "li",
                                [
                                    html.Factory.CreateTree(
                                        "strong",
                                        html.CreateTextNode(a.GroupName)
                                    ),
                                    html.CreateTextNode(" "),
                                    html.Factory.CreateTree(
                                        "code",
                                        html.CreateTextNode(a.GroupId.ToString())
                                    ),
                                    html.Factory.CreateTree(
                                        "ul",
                                        a.Items.Select(b => {
                                            return html.Factory.CreateTree(
                                                "li",
                                                [
                                                    html.Factory.CreateTree(
                                                        "em",
                                                        html.CreateTextNode(b.LauncherItemName)
                                                    ),
                                                    html.CreateTextNode(" "),
                                                    html.Factory.CreateTree(
                                                        "code",
                                                        html.CreateTextNode(b.LauncherItemId.ToString())
                                                    ),
                                                ]
                                            );
                                        })
                                    )
                                ]
                            );
                        })
                    ),
                ]
            );
            html.Body.AppendChild(groupElement);

            // ランチャーアイテム
            var launcherItemElement = html.Factory.CreateTree(
                "section",
                [
                    html.Factory.CreateTree(
                        "h2",
                        html.CreateTextNode("items")
                    ),
                    html.Factory.CreateTree(
                        "ul",
                        items.Select(a => {
                            return html.Factory.CreateTree(
                                "li",
                                [
                                    html.Factory.CreateTree(
                                        "strong",
                                        html.CreateTextNode(a.LauncherItemName)
                                    ),
                                    html.CreateTextNode(" "),
                                    html.Factory.CreateTree(
                                        "code",
                                        html.CreateTextNode(a.LauncherItemId.ToString())
                                    ),
                                    html.Factory.CreateTree(
                                        "dl",
                                        [
                                            // 種別
                                            html.Factory.CreateTree(
                                                "dt",
                                                html.CreateTextNode("種別") // TODO: i18n
                                            ),
                                            html.Factory.CreateTree(
                                                "dd",
                                                html.CreateTextNode(a.LauncherItemKind.ToString())
                                            ),
                                            // アイコン
                                            html.Factory.CreateTree(
                                                "dt",
                                                html.CreateTextNode("アイコン(インデックス)") // TODO: i18n
                                            ),
                                            html.Factory.CreateTree(
                                                "dd",
                                                string.IsNullOrEmpty(a.IconPath)
                                                    ? [html.CreateTextNode("なし")]
                                                    : [
                                                        html.Factory.CreateTree(
                                                            "code",
                                                            html.CreateTextNode(a.IconPath)
                                                        ),
                                                        html.Factory.CreateTree(
                                                            "em",
                                                            [
                                                                html.CreateTextNode("("),
                                                                html.CreateTextNode(a.IconIndex.ToString(CultureInfo.InvariantCulture)),
                                                                html.CreateTextNode(")"),
                                                            ]
                                                        ),
                                                    ]
                                            ),
                                            // ファイルパス
                                            html.Factory.CreateTree(
                                                "dt",
                                                html.CreateTextNode("ファイルパス") // TODO: i18n
                                            ),
                                            html.Factory.CreateTree(
                                                "dd",
                                                html.Factory.CreateTree(
                                                    "code",
                                                    html.CreateTextNode(a.FilePath)
                                                )
                                            ),
                                            // オプション
                                            html.Factory.CreateTree(
                                                "dt",
                                                html.CreateTextNode("オプション") // TODO: i18n
                                            ),
                                            html.Factory.CreateTree(
                                                "dd",
                                                string.IsNullOrEmpty(a.FileOption)
                                                    ? html.CreateTextNode("なし")
                                                    : html.Factory.CreateTree(
                                                        "code",
                                                        html.CreateTextNode(a.FileOption)
                                                    )
                                            ),
                                            // 作業ディレクトリ
                                            html.Factory.CreateTree(
                                                "dt",
                                                html.CreateTextNode("作業ディレクトリ") // TODO: i18n
                                            ),
                                            html.Factory.CreateTree(
                                                "dd",
                                                string.IsNullOrEmpty(a.FileWorkDirectory)
                                                    ? html.CreateTextNode("なし")
                                                    : html.Factory.CreateTree(
                                                        "code",
                                                        html.CreateTextNode(a.FileWorkDirectory)
                                                    )
                                            ),
                                            // コメント
                                            html.Factory.CreateTree(
                                                "dt",
                                                html.CreateTextNode("コメント") // TODO: i18n
                                            ),
                                            html.Factory.CreateTree(
                                                "dd",
                                                string.IsNullOrEmpty(a.Comment)
                                                    ? html.CreateTextNode("なし")
                                                    : html.Factory.CreateTree(
                                                        "pre",
                                                        html.CreateTextNode(a.Comment)
                                                    )
                                            ),
                                        ]
                                    )
                                ]
                            );
                        })
                    ),
                ]
            );
            html.Body.AppendChild(launcherItemElement);

            // ノート
            var noteElement = html.Factory.CreateTree(
                "section",
                [
                    html.Factory.CreateTree(
                        "h2",
                        html.CreateTextNode("note")
                    ),
                    html.Factory.CreateTree(
                        "ul",
                        notes.Select(a => {
                            return html.Factory.CreateTree(
                                "li",
                                [
                                    html.Factory.CreateTree(
                                        "strong",
                                        html.CreateTextNode(a.Title)
                                    ),
                                    html.CreateTextNode(" "),
                                    html.Factory.CreateTree(
                                        "code",
                                        html.CreateTextNode(a.NoteId.ToString())
                                    ),
                                    html.Factory.CreateTree(
                                        "dl",
                                        [
                                            // スクリーン
                                            html.Factory.CreateTree(
                                                "dt",
                                                html.CreateTextNode("ディスプレイ") // TODO: i18n
                                            ),
                                            html.Factory.CreateTree(
                                                "dd",
                                                html.CreateTextNode(a.ScreenName)
                                            ),
                                            // 前景色/背景色
                                            html.Factory.CreateTree(
                                                "dt",
                                                html.CreateTextNode("前景色/背景色") // TODO: i18n
                                            ),
                                            html.Factory.CreateTree(
                                                "dd",
                                                [
                                                    html.Factory.CreateTree(
                                                        "code",
                                                        html.CreateTextNode(a.ForegroundColor)
                                                    ),
                                                    html.CreateTextNode("/"),
                                                    html.Factory.CreateTree(
                                                        "code",
                                                        html.CreateTextNode(a.BackgroundColor)
                                                    )
                                                ]
                                            ),
                                            // 固定
                                            html.Factory.CreateTree(
                                                "dt",
                                                html.CreateTextNode("固定") // TODO: i18n
                                            ),
                                            html.Factory.CreateTree(
                                                "dd",
                                                html.CreateTextNode(a.IsLocked.ToString())
                                            ),
                                            // 最前面
                                            html.Factory.CreateTree(
                                                "dt",
                                                html.CreateTextNode("最前面") // TODO: i18n
                                            ),
                                            html.Factory.CreateTree(
                                                "dd",
                                                html.CreateTextNode(a.IsTopmost.ToString())
                                            ),
                                            // 最小化
                                            html.Factory.CreateTree(
                                                "dt",
                                                html.CreateTextNode("最小化") // TODO: i18n
                                            ),
                                            html.Factory.CreateTree(
                                                "dd",
                                                html.CreateTextNode(a.IsCompact.ToString())
                                            ),
                                            // 種別
                                            html.Factory.CreateTree(
                                                "dt",
                                                html.CreateTextNode("種別") // TODO: i18n
                                            ),
                                            html.Factory.CreateTree(
                                                "dd",
                                                html.CreateTextNode(a.ContentKind.ToString())
                                            ),
                                            // 自動的に隠す
                                            html.Factory.CreateTree(
                                                "dt",
                                                html.CreateTextNode("自動的に隠す方法") // TODO: i18n
                                            ),
                                            html.Factory.CreateTree(
                                                "dd",
                                                html.CreateTextNode(a.HiddenMode.ToString())
                                            ),
                                            // タイトルバー位置
                                            html.Factory.CreateTree(
                                                "dt",
                                                html.CreateTextNode("タイトルバー位置") // TODO: i18n
                                            ),
                                            html.Factory.CreateTree(
                                                "dd",
                                                html.CreateTextNode(a.CaptionPosition.ToString())
                                            ),
                                            // エンコーディング
                                            html.Factory.CreateTree(
                                                "dt",
                                                html.CreateTextNode("エンコーディング") // TODO: i18n
                                            ),
                                            html.Factory.CreateTree(
                                                "dd",
                                                html.CreateTextNode(a.Encoding.ToString() ?? a.Encoding.EncodingName)
                                            ),
                                            // 内容
                                            html.Factory.CreateTree(
                                                "dt",
                                                html.CreateTextNode("内容") // TODO: i18n
                                            ),
                                            html.Factory.CreateTree(
                                                "dd",
                                                html.Factory.CreateTree(
                                                    "pre",
                                                    html.CreateTextNode(a.Content)
                                                )
                                            ),
                                        ]
                                    ),
                                ]
                            );
                        })
                    )
                ]
            );
            html.Body.AppendChild(noteElement);

            using var writer = File.CreateText(outputPath);
            html.Write(writer, new HtmlNodeOutputOptions() { Optimization = true });
        }

        #endregion

        #region ElementBase

        protected override Task InitializeCoreAsync(CancellationToken cancellationToken)
        {
            var components = LoadComponents();

            ComponentsImpl.AddRange(GetApplicationItems());
            ComponentsImpl.AddRange(ToItems(AboutComponentKind.Library, components.Library));
            ComponentsImpl.AddRange(ToItems(AboutComponentKind.Resource, components.Resource));

            return Task.CompletedTask;
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
