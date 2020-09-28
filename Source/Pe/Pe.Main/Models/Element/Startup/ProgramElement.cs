using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.Startup
{
    public class ProgramElement: ElementBase
    {
        public ProgramElement(FileInfo fileInfo, IReadOnlyList<Regex> autoImportUntargetRegexItems, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            FileInfo = fileInfo;
            AutoImportUntargetRegexItems = autoImportUntargetRegexItems;
            DispatcherWrapper = dispatcherWrapper;
            IconImageLoader = new IconImageLoader(
                new Data.IconData() {
                    Path = FileInfo.FullName
                },
                DispatcherWrapper,
                LoggerFactory
            );
        }

        #region property

        IconBox IconBox { get; } = IconBox.Small;

        public FileInfo FileInfo { get; }

        IDispatcherWrapper DispatcherWrapper { get; }
        public bool IsShortcut => PathUtility.IsShortcut(FileInfo.Name);
        IReadOnlyList<Regex> AutoImportUntargetRegexItems { get; }
        public bool IsImport { get; set; }
        public IconImageLoader IconImageLoader { get; private set; }

        #endregion

        #region function

        bool IsAutoImportTarget(string path)
        {
            return !AutoImportUntargetRegexItems.Any(i => i.IsMatch(path));
        }

        void ApplyShortcutFileIconLoader(string targetPath, string targetIconPath, int targetIconIndex)
        {
            if(string.IsNullOrWhiteSpace(targetPath)) {
                return;
            }

            // ショートカットのリンク先パスと設定アイコンパスが異なれば設定アイコンパスを優先する
            if(!string.IsNullOrWhiteSpace(targetIconPath)) {
                var expandedIconPath = Environment.ExpandEnvironmentVariables(targetIconPath);
                var iconIndex = targetIconIndex;
                // パスが異なるのはもとよりパスが同じでもアイコンインデックス指定があればアイコンを優先
                if(!PathUtility.IsEqual(targetPath, expandedIconPath) || iconIndex != 0) {
                    IconImageLoader.Dispose();
                    IconImageLoader = new IconImageLoader(
                        new Data.IconData() {
                            Path = expandedIconPath,
                            Index = iconIndex,
                        },
                        DispatcherWrapper,
                        LoggerFactory
                    );
                    return;
                }
            }

            // とりま対象リンク先パスを指定
            IconImageLoader.Dispose();
            IconImageLoader = new IconImageLoader(
                new Data.IconData() {
                    Path = targetPath,
                    Index = 0,
                },
                DispatcherWrapper,
                LoggerFactory
            );

            return;
        }

        #endregion

        #region ContextElementBase

        protected override void InitializeImpl()
        {
            if(IsShortcut) {
                try {
                    var path = Environment.ExpandEnvironmentVariables(FileInfo.FullName);
                    using var shortcutFile = new ShortcutFile(path);
                    var targetPath = Environment.ExpandEnvironmentVariables(shortcutFile.TargetPath);
                    IsImport = IsAutoImportTarget(targetPath);
                    ApplyShortcutFileIconLoader(targetPath, shortcutFile.IconPath, shortcutFile.IconIndex);
                    return;
                } catch(Exception ex) {
                    Logger.LogError(ex, "{0}, ショートカット情報読み込み失敗のためショートカットファイルから処理: {1}", ex.Message, FileInfo.FullName);
                }
            }

            IsImport = IsAutoImportTarget(FileInfo.Name);
        }

        #endregion
    }
}
