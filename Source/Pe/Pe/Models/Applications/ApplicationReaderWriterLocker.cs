using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Core.Models;

namespace ContentTypeTextNet.Pe.Main.Models.Applications
{
    public abstract class ApplicationReaderWriterLockerBase : ReaderWriterLocker
    { }

    public class ApplicationMainReaderWriterLocker : ApplicationReaderWriterLockerBase
    { }

    public class ApplicationFileReaderWriterLocker : ApplicationReaderWriterLockerBase
    { }

    public class ApplicationTemporaryReaderWriterLocker : ApplicationReaderWriterLockerBase
    { }
}
