using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model.Database;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Applications;
using ContentTypeTextNet.Pe.Main.Model.Database.Dao.Entity;

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

        public LauncherIconLoader(Guid launcherItemId, IconScale iconScale, IFileDatabaseBarrier fileDatabaseBarrier, IDatabaseStatementLoader statementLoader, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            LauncherItemId = launcherItemId;
            IconScale = iconScale;
            FileDatabaseBarrier = fileDatabaseBarrier;
            StatementLoader = statementLoader;
        }

        #region property

        public Guid LauncherItemId { get; }
        public IconScale IconScale { get; }
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

        Task<BitmapSource> MakeImageAsync()
        {
            throw new NotImplementedException();
            // アイコンパス取得
            // アイコン取得
            // データ書き込み(失敗してもアイコンが取得できてるならOK)
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
