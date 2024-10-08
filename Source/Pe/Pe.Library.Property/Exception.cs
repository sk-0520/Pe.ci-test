using System;
using System.Collections.Generic;
using System.Text;

namespace ContentTypeTextNet.Pe.Library.Property
{
    [Serializable]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class PropertyException: Exception
    {
        public PropertyException(string message) : base(message) { }
        public PropertyException(string message, Exception inner) : base(message, inner) { }

        [Obsolete]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Info Code Smell", "S1133:Deprecated code should be removed")]
        protected PropertyException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class PropertyCanNotAccessException: PropertyException
    {
        public PropertyCanNotAccessException(Type ownerType, string propertyName)
            : base($"{ownerType.FullName}.{propertyName}")
        { }
    }

    [Serializable]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S3925:\"ISerializable\" should be implemented correctly", Justification = "<保留中>")]
    public sealed class PropertyCanNotReadException: PropertyCanNotAccessException
    {
        public PropertyCanNotReadException(Type ownerType, string propertyName)
            : base(ownerType, propertyName)
        { }
    }

    [Serializable]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S3925:\"ISerializable\" should be implemented correctly", Justification = "<保留中>")]
    public sealed class PropertyCanNotWriteException: PropertyCanNotAccessException
    {
        public PropertyCanNotWriteException(Type ownerType, string propertyName)
            : base(ownerType, propertyName)
        { }
    }

    [Serializable]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S3925:\"ISerializable\" should be implemented correctly", Justification = "<保留中>")]
    public sealed class PropertyNotFoundException: PropertyException
    {
        public PropertyNotFoundException(string propertyName)
            : base(propertyName)
        { }
    }

}
