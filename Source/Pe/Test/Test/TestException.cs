namespace ContentTypeTextNet.Pe.Test
{
    /// <summary>
    /// 単体テストで何かしら変になった時に投げられる例外。
    /// <para>テストの対象処理自体で投げられることはない。</para>
    /// </summary>
    public class TestException: System.Exception
    {
        public TestException() { }
        public TestException(string message) : base(message) { }
        public TestException(string message, System.Exception inner) : base(message, inner) { }
        protected TestException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
