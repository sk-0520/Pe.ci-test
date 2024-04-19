using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.ViewModels;
using ContentTypeTextNet.Pe.Plugins.Reference.FileFinder.Models.Data;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Plugins.Reference.FileFinder.ViewModels
{
    public class FileFinderSettingViewModel: ViewModelSkeleton
    {
        public FileFinderSettingViewModel(FileFinderSetting setting, ISkeletonImplements skeletonImplements, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(skeletonImplements, dispatcherWrapper, loggerFactory)
        {
            Setting = setting;
        }

        #region property

        internal FileFinderSetting Setting { get; }

        /// <summary>
        /// 隠しファイルを列挙するか。
        /// </summary>
        public bool IncludeHiddenFile
        {
            get => Setting.IncludeHiddenFile;
            set => SetPropertyValue(Setting, value);
        }

        /// <summary>
        /// PATHの通っている実行ファイルを列挙するか。
        /// </summary>
        public bool IncludePath
        {
            get => Setting.IncludePath;
            set => SetPropertyValue(Setting, value);
        }


        /// <summary>
        /// パスからの列挙において列挙する上限数。
        /// </summary>
        /// <remarks>
        /// <para>0 で制限しない。</para>
        /// </remarks>
        public int MaximumPathItem
        {
            get => Setting.MaximumPathItem;
            set => SetPropertyValue(Setting, value);
        }


        /// <summary>
        /// パス検索を有効にする入力文字数(以上)。
        /// </summary>
        public int PathEnabledInputCharCount
        {
            get => Setting.PathEnabledInputCharCount;
            set => SetPropertyValue(Setting, value);
        }

        public ObservableCollection<int> MaximumPathItems { get; } = new ObservableCollection<int>() {
            0,
            10,
            20,
            50,
            100,
        };

        public ObservableCollection<int> PathEnabledInputCharCounts { get; } = new ObservableCollection<int>() {
            0,
            1,
            2,
            3,
        };

        #endregion
    }
}
