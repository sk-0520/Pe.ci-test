namespace ContentTypeTextNet.Pe.Test
{
    [System.Serializable]
    public class TestException: System.Exception
    {
        public TestException() { }
        public TestException(string message) : base(message) { }
        public TestException(string message, System.Exception inner) : base(message, inner) { }
    }
}
