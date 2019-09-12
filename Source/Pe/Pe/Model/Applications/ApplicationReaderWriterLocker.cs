using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Core.Model;

namespace ContentTypeTextNet.Pe.Main.Model.Applications
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
