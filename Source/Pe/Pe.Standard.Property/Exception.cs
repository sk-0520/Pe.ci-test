using System;
using System.Collections.Generic;
using System.Text;

namespace ContentTypeTextNet.Pe.Standard.Property
{
    [Serializable]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class PropertyException: Exception
    {
        public PropertyException(string message) : base(message) { }
        public PropertyException(string message, Exception inner) : base(message, inner) { }
        protected PropertyException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public abstract class PropertyCanNotAccessExceptionBase: PropertyException
    {
        public PropertyCanNotAccessExceptionBase(Type ownerType, string propertyName)
            : base($"{ownerType.FullName}.{propertyName}")
        { }
    }

    [Serializable]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public sealed class PropertyCanNotReadException: PropertyCanNotAccessExceptionBase
    {
        public PropertyCanNotReadException(Type ownerType, string propertyName)
            : base(ownerType, propertyName)
        { }
    }

    [Serializable]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public sealed class PropertyCanNotWriteException: PropertyCanNotAccessExceptionBase
    {
        public PropertyCanNotWriteException(Type ownerType, string propertyName)
            : base(ownerType, propertyName)
        { }
    }

    [Serializable]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public sealed class PropertyNotFoundException: PropertyException
    {
        public PropertyNotFoundException(string propertyName)
            : base(propertyName)
        { }
    }

}
