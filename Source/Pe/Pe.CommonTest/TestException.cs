using System;

namespace ContentTypeTextNet.Pe.Test
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
    public class TestPrivateObjectException: TestException
    {
        public TestPrivateObjectException()
        { }

        public TestPrivateObjectException(string message)
            : base(message)
        { }

        public TestPrivateObjectException(string message, Exception inner)
            : base(message, inner)
        { }
    }

    public class TestPrivateObjectFieldException: TestPrivateObjectException
    {
        public TestPrivateObjectFieldException()
        { }

        public TestPrivateObjectFieldException(string message)
            : base(message)
        { }

        public TestPrivateObjectFieldException(string message, Exception inner)
            : base(message, inner)
        { }
    }

    public class TestPrivateObjectPropertyException: TestPrivateObjectException
    {
        public TestPrivateObjectPropertyException()
        { }

        public TestPrivateObjectPropertyException(string message)
            : base(message)
        { }

        public TestPrivateObjectPropertyException(string message, Exception inner)
            : base(message, inner)
        { }
    }

    public class TestPrivateObjectMethodException: TestPrivateObjectException
    {
        public TestPrivateObjectMethodException()
        { }

        public TestPrivateObjectMethodException(string message)
            : base(message)
        { }

        public TestPrivateObjectMethodException(string message, Exception inner)
            : base(message, inner)
        { }
    }

    public class TestPrivateObjectMethodParametersException: TestPrivateObjectException
    {
        public TestPrivateObjectMethodParametersException()
        { }

        public TestPrivateObjectMethodParametersException(string message)
            : base(message)
        { }

        public TestPrivateObjectMethodParametersException(string message, Exception inner)
            : base(message, inner)
        { }
    }

    #endregion
}
