using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Library.Base;

namespace ContentTypeTextNet.Pe.Main.Models.Applications
{
    internal abstract class ApplicationReadWriteLockHelperBase: ReadWriteLockHelper
    { }

    internal class ApplicationMainReadWriteLockHelper: ApplicationReadWriteLockHelperBase
    { }

    internal class ApplicationLargeReadWriteLockHelper: ApplicationReadWriteLockHelperBase
    { }

    internal class ApplicationTemporaryReadWriteLockHelper: ApplicationReadWriteLockHelperBase
    { }
}
