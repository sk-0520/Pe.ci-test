using ContentTypeTextNet.Pe.Core.Models;

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
