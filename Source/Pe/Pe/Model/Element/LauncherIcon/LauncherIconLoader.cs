using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using ContentTypeTextNet.Pe.Library.Shared.Embedded.Model;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model.Database;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Applications;
using ContentTypeTextNet.Pe.Main.Model.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Model.Launcher;

namespace ContentTypeTextNet.Pe.Main.Model.Element.LauncherIcon
{
    public enum IconLoadState
    {
        None,
        Loading,
        Loaded,
        Error,
    }

    public class LauncherIconLoader : BindModelBase
    {
        #region variable

        IconLoadState _iconLoadState;

        #endregion

        public LauncherIconLoader(Guid launcherItemId, IconScale iconScale, IMainDatabaseBarrier mainDatabaseBarrier, IFileDatabaseBarrier fileDatabaseBarrier, IDatabaseStatementLoader statementLoader, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            LauncherItemId = launcherItemId;
            IconScale = iconScale;
            MainDatabaseBarrier = mainDatabaseBarrier;
            FileDatabaseBarrier = fileDatabaseBarrier;
            StatementLoader = statementLoader;
        }

        #region property

        public Guid LauncherItemId { get; }
        public IconScale IconScale { get; }
        IMainDatabaseBarrier MainDatabaseBarrier { get; }
        IFileDatabaseBarrier FileDatabaseBarrier { get; }
        IDatabaseStatementLoader StatementLoader { get; }

        public IconLoadState IconLoadState
        {
            get => this._iconLoadState;
            set => SetProperty(ref this._iconLoadState, value);
        }

        #endregion

        #region function

        BitmapSource ToImage(byte[] binary)
        {
            throw new NotImplementedException();
        }

        (bool exists, BitmapSource image) LoadExistsImage()
        {
            byte[] imageBinary;
            using(var commander = FileDatabaseBarrier.WaitRead()) {
                var dao = new LauncherItemIconsDao(commander, StatementLoader, Logger.Factory);
                imageBinary = dao.SelectImageBinary(LauncherItemId, IconScale);
            }

            if(imageBinary != null) {
                var image = ToImage(imageBinary);
            }

            return (false, null);
        }

        LauncherIconData GetIconData()
        {
            using(var commander = MainDatabaseBarrier.WaitRead()) {
                var dao = new LauncherItemsDao(commander, StatementLoader, Logger.Factory);
                return dao.SelectIcon(LauncherItemId);
            }
        }

        BitmapSource GetIconImageCore(LauncherItemKind kind, IconData iconData)
        {
            var path = TextUtility.SafeTrim(iconData.Path);
            var expandedPath = Environment.ExpandEnvironmentVariables(path);
            if(!File.Exists(expandedPath)) {
                return null;
            }

            var iconLoader = new IconLoader(Logger.Factory);
            var iconImage = iconLoader.Load(expandedPath, IconScale, iconData.Index);

            return iconImage;
        }

        BitmapSource GetIconImage(LauncherIconData launcherIconData)
        {
            var iconImage = GetIconImageCore(launcherIconData.Kind, launcherIconData.Icon);
            if(iconImage != null) {
                return iconImage;
            }

            var commandImage = GetIconImageCore(launcherIconData.Kind, launcherIconData.Command);
            if(commandImage != null) {
                return commandImage;
            }

            // 標準のやつ。。。 xaml でやるべき？
            return null;
        }

        void SaveIconImage(BitmapSource iconImage)
        {

        }

        Task<BitmapSource> MakeImageAsync()
        {
            // アイコンパス取得
            var launcherIconData = GetIconData();

            // アイコン取得
            var iconImage = GetIconImage(launcherIconData);

            // データ書き込み(失敗してもアイコンが取得できてるならOK)
            SaveIconImage(iconImage);

            return Task.FromResult(iconImage);
        }

        public async Task<BitmapSource> LoadAsync()
        {
            IconLoadState = IconLoadState.Loading;
            var existisResult = LoadExistsImage();
            if(existisResult.exists) {
                IconLoadState = IconLoadState.Loaded;
                return existisResult.image;
            }

            try {
                var image = await MakeImageAsync().ConfigureAwait(false);
                IconLoadState = IconLoadState.Loaded;
                return image;
            } catch(Exception ex) {
                Logger.Error(ex);
                IconLoadState = IconLoadState.Error;
                throw;
            }
        }

        #endregion
    }
}
