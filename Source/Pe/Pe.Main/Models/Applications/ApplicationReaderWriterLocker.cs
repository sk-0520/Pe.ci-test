using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Standard.Base;

namespace ContentTypeTextNet.Pe.Main.Models.Applications
{
    internal abstract class ApplicationReaderWriterLockerBase: ReaderWriterLocker
    { }

    internal class ApplicationMainReaderWriterLocker: ApplicationReaderWriterLockerBase
    { }

    internal class ApplicationLargeReaderWriterLocker: ApplicationReaderWriterLockerBase
    { }

    internal class ApplicationTemporaryReaderWriterLocker: ApplicationReaderWriterLockerBase
    { }
}
