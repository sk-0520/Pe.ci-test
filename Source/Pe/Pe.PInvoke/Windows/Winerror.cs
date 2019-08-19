using System;

namespace ContentTypeTextNet.Pe.PInvoke.Windows
{
    public enum ComResult: int
    {
        S_OK = unchecked((int)0x00000000),
        S_FALSE = unchecked((int)0x00000001),
        E_NOINTERFACE = unchecked((int)0x80004002),
        INET_E_DEFAULT_ACTION = unchecked((int)0x800C0011),
    }
}
