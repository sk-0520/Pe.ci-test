using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;

namespace ContentTypeTextNet.Pe.Main.Model.Logic
{
    public interface IIdFactory
    {
        #region function

        Guid CreateLauncherItemId();
        Guid CreateCredentId();
        Guid CreateLauncherToolbarId();
        Guid CreateFontId();
        Guid CreateLauncherGroupId();
        Guid CreateNoteId();
        Guid CreateNoteFileId();

        #endregion
    }

    public sealed class IdFactory : IIdFactory
    {
        public IdFactory(ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateTartget(GetType());
        }

        #region property

        ILogger Logger { get; }

        #endregion

        #region IIdFactory

        public Guid CreateLauncherItemId() => Guid.NewGuid();
        public Guid CreateCredentId() => Guid.NewGuid();
        public Guid CreateLauncherToolbarId() => Guid.NewGuid();
        public Guid CreateFontId() => Guid.NewGuid();
        public Guid CreateLauncherGroupId() => Guid.NewGuid();
        public Guid CreateNoteId() => Guid.NewGuid();
        public Guid CreateNoteFileId() => Guid.NewGuid();

        #endregion
    }
}
