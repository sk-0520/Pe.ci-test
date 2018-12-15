using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;

namespace ContentTypeTextNet.Pe.Main.Model
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
