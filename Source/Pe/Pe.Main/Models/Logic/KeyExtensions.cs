using System.Windows.Input;

namespace ContentTypeTextNet.Pe.Main.Models.Logic
{
    public static class KeyExtensions
    {
        #region function

        /// <summary>
        /// 装飾キーか。
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool IsModifierKey(this Key key)
        {
            switch(key) {
                case Key.LeftShift:
                case Key.RightShift:
                case Key.LeftCtrl:
                case Key.RightCtrl:
                case Key.LeftAlt:
                case Key.RightAlt:
                case Key.LWin:
                case Key.RWin:
                    return true;

                default:
                    return false;
            }
        }

        #endregion
    }
}
