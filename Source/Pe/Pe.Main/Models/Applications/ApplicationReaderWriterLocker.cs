using ContentTypeTextNet.Pe.Core.Models;

namespace ContentTypeTextNet.Pe.Main.Models.Applications
{
    public abstract class ApplicationReaderWriterLockerBase: ReaderWriterLocker
    { }

    public class ApplicationMainReaderWriterLocker: ApplicationReaderWriterLockerBase
    { }

    public class ApplicationLargeReaderWriterLocker: ApplicationReaderWriterLockerBase
    { }

    public class ApplicationTemporaryReaderWriterLocker: ApplicationReaderWriterLockerBase
    { }
}
