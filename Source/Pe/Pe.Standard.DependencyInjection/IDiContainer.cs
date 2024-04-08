//#define ENABLED_PRISM7

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

#if ENABLED_PRISM7
using Prism.Ioc;
#endif

namespace ContentTypeTextNet.Pe.Standard.DependencyInjection
{
    /// <summary>
    /// 取得可能コンテナ。
    /// </summary>
    public interface IDiContainer: IDiScopeContainerFactory
#if ENABLED_PRISM7
        , IContainerProvider
#endif
    {
        #region get

        /// <summary>
        /// マッピングから実体を取得。
        /// <para>必ずしも依存が解決されるわけではない。</para>
        /// </summary>
        /// <param name="interfaceType"></param>
        /// <returns>実体そのまま</returns>
        object Get(Type interfaceType);
        /// <inheritdoc cref="Get(Type)"/>
        object Get(Type interfaceType, string name);
        /// <inheritdoc cref="Get(Type)"/>
        TInterface Get<TInterface>();
        /// <inheritdoc cref="Get(Type)"/>
        TInterface Get<TInterface>(string name);

        #endregion

        #region new

        /// <summary>
        /// コンストラクタインジェクション。
        /// <para>依存を解決する。</para>
        /// </summary>
        /// <param name="type"></param>
        /// <param name="manualParameters">依存関係以外のパラメータ。前方から型に一致するものが使用される。</param>
        /// <returns></returns>
        /// <remarks>null をパラメータとして使用する場合は型情報が死ぬので <see cref="DiDefaultParameter"/> を使用すること。</remarks>
        object New(Type type, IReadOnlyList<object> manualParameters);
        /// <inheritdoc cref="New(Type, IReadOnlyList{object})"/>
        object New(Type type, string name, IReadOnlyList<object> manualParameters);

        /// <inheritdoc cref="New(Type, IReadOnlyList{object})"/>
        object New(Type type);
        /// <inheritdoc cref="New(Type, IReadOnlyList{object})"/>
        object New(Type type, string name);

        /// <inheritdoc cref="New(Type, IReadOnlyList{object})"/>
        TObject New<TObject>(IReadOnlyList<object> manualParameters)
#if !ENABLED_STRUCT
            where TObject : class
#endif
        ;
        /// <inheritdoc cref="New(Type, IReadOnlyList{object})"/>
        TObject New<TObject>(string name, IReadOnlyList<object> manualParameters)
#if !ENABLED_STRUCT
            where TObject : class
#endif
        ;

        /// <inheritdoc cref="New(Type, IReadOnlyList{object})"/>
        TObject New<TObject>()
#if !ENABLED_STRUCT
            where TObject : class
#endif
        ;
        /// <inheritdoc cref="New(Type, IReadOnlyList{object})"/>
        TObject New<TObject>(string name)
#if !ENABLED_STRUCT
            where TObject : class
#endif
        ;

        /// <summary>
        /// 指定メソッドを実行する。
        /// <para>基本的に <see cref="IDiContainerExtensions.Call{TResult}(IDiContainer, object, string, object, object[])"/> を使用すればよろし。</para>
        /// </summary>
        /// <param name="name"></param>
        /// <param name="instance">対象インスタンス。</param>
        /// <param name="methodInfo">メソッド。</param>
        /// <param name="manualParameters"></param>
        /// <returns></returns>
        object? CallMethod(string name, object instance, MethodInfo methodInfo, IReadOnlyList<object> manualParameters);

        #endregion

        #region inject

        /// <summary>
        /// プロパティインジェクション。
        /// <para><see cref="InjectAttribute"/> を補完する。</para>
        /// </summary>
        /// <typeparam name="TObject">生成済みオブジェクト</typeparam>
        /// <param name="target">クラスインスタンス。</param>
        void Inject<TObject>(TObject target)
            where TObject : class
        ;
#if ENABLED_STRUCT
        void Inject<TObject>(ref TObject target)
            where TObject : struct
        ;
#endif

        #endregion
    }
}
