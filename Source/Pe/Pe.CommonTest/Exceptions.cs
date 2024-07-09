using System;

namespace ContentTypeTextNet.Pe.CommonTest
{
    [Serializable]
    public class TestException: System.Exception
    {
        public TestException()
        { }
        public TestException(string message)
            : base(message)
        { }
        public TestException(string message, Exception inner)
            : base(message, inner)
        { }
    }

    #region PrivateObject

    [Serializable]
    public class PrivateObjectException: TestException
    {
        public PrivateObjectException()
        { }

        public PrivateObjectException(string message)
            : base(message)
        { }

        public PrivateObjectException(string message, Exception inner)
            : base(message, inner)
        { }
    }

    public class PrivateObjectFieldException: PrivateObjectException
    {
        public PrivateObjectFieldException()
        { }

        public PrivateObjectFieldException(string message)
            : base(message)
        { }

        public PrivateObjectFieldException(string message, Exception inner)
            : base(message, inner)
        { }
    }

    public class PrivateObjectPropertyException: PrivateObjectException
    {
        public PrivateObjectPropertyException()
        { }

        public PrivateObjectPropertyException(string message)
            : base(message)
        { }

        public PrivateObjectPropertyException(string message, Exception inner)
            : base(message, inner)
        { }
    }

    public class PrivateObjectMethodException: PrivateObjectException
    {
        public PrivateObjectMethodException()
        { }

        public PrivateObjectMethodException(string message)
            : base(message)
        { }

        public PrivateObjectMethodException(string message, Exception inner)
            : base(message, inner)
        { }
    }

    public class PrivateObjectMethodParametersException: PrivateObjectException
    {
        public PrivateObjectMethodParametersException()
        { }

        public PrivateObjectMethodParametersException(string message)
            : base(message)
        { }

        public PrivateObjectMethodParametersException(string message, Exception inner)
            : base(message, inner)
        { }
    }

    #endregion
}
