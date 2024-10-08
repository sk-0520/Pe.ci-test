#if DEBUG || BETA
#   define CHECK_PROPERTY_NAME
#endif

namespace ContentTypeTextNet.Pe.Library.Property
{
    /// <summary>
    /// 汎用プロパティ取得処理。
    /// </summary>
    public interface IPropertyGetter
    {
        #region function

        /// <summary>
        /// 対象オブジェクトからプロパティの値取得。
        /// </summary>
        /// <param name="owner">対象オブジェクト。</param>
        /// <returns>値。</returns>
        object? Get(object owner);

        #endregion
    }

    /// <summary>
    /// 型指定プロパティ値取得処理。
    /// </summary>
    /// <typeparam name="TOwner">対象型。</typeparam>
    /// <typeparam name="TValue">プロパティ型</typeparam>
    public interface IPropertyGetter<in TOwner, out TValue>
    {
        #region function

        /// <summary>
        /// 対象オブジェクトからプロパティの値取得。
        /// </summary>
        /// <param name="owner">対象オブジェクト。</param>
        /// <returns>値。</returns>
        TValue Get(TOwner owner);

        #endregion
    }
}
