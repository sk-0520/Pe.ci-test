#if DEBUG || BETA
#   define CHECK_PROPERTY_NAME
#endif

namespace ContentTypeTextNet.Pe.Library.Property
{
    /// <summary>
    /// 汎用プロパティ設定処理。
    /// </summary>
    public interface IPropertySetter
    {
        #region function

        /// <summary>
        /// 対象オブジェクトのプロパティ値を設定。
        /// </summary>
        /// <param name="owner">対象オブジェクト。</param>
        /// <param name="value">値。</param>
        void Set(object owner, object? value);

        #endregion
    }

    /// <summary>
    /// 型指定プロパティ設定処理。
    /// </summary>
    /// <typeparam name="TOwner">対象型。</typeparam>
    /// <typeparam name="TValue">プロパティ型</typeparam>
    public interface IPropertySetter<in TOwner, in TValue>
    {
        #region function

        /// <summary>
        /// 対象オブジェクトのプロパティ値を設定。
        /// </summary>
        /// <param name="owner">対象オブジェクト。</param>
        /// <param name="value">値。</param>
        void Set(TOwner owner, TValue value);

        #endregion
    }
}
