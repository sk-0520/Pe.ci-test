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
    public interface IDiScopeContainerFactory
    {
        /// <summary>
        /// 限定的なDIコンテナを作成。
        /// </summary>
        /// <returns>現在マッピングを複製したDIコンテナ。</returns>
        IScopeDiContainer Scope();
    }

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

    /// <summary>
    ///登録可能コンテナ。
    /// </summary>
    public interface IDiRegisterContainer: IDiContainer
#if ENABLED_PRISM7
        , IContainerRegistry
#endif
    {
        #region Register

        /// <summary>
        /// シンプルなマッピングを追加。
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <typeparam name="TObject"></typeparam>
        IDiRegisterContainer Register<TInterface, TObject>(DiLifecycle lifecycle)
#if !ENABLED_STRUCT
            where TObject : class, TInterface
#endif
        ;

        /// <inheritdoc cref="Register{TInterface, TObject}(DiLifecycle)"/>
        IDiRegisterContainer Register<TInterface, TObject>(string name, DiLifecycle lifecycle)
#if !ENABLED_STRUCT
            where TObject : class, TInterface
#endif
        ;

        /// <summary>
        /// 自分で作る版のマッピング。
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="lifecycle"></param>
        /// <param name="creator"></param>
        IDiRegisterContainer Register<TInterface, TObject>(DiLifecycle lifecycle, DiCreator creator)
#if !ENABLED_STRUCT
            where TObject : class, TInterface
#endif
        ;
        /// <inheritdoc cref="Register{TInterface, TObject}(DiLifecycle, DiCreator)"/>
        IDiRegisterContainer Register<TInterface, TObject>(string name, DiLifecycle lifecycle, DiCreator creator)
#if !ENABLED_STRUCT
            where TObject : class, TInterface
#endif
        ;

        /// <summary>
        /// シングルトンとしてオブジェクトを単純登録。
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        IDiRegisterContainer Register<TInterface, TObject>(TObject value)
#if !ENABLED_STRUCT
            where TObject : class, TInterface
#endif
        ;

        /// <inheritdoc cref="Register{TInterface, TObject}(TObject)"/>
        IDiRegisterContainer Register<TInterface, TObject>(string name, TObject value)
#if !ENABLED_STRUCT
            where TObject : class, TInterface
#endif
        ;

        #endregion

        #region RegisterMember

        /// <summary>
        /// <see cref="IDiContainer.Inject{TObject}(TObject)"/> を行う際に <see cref="InjectAttribute"/> を設定できないプロパティに無理やり設定する。
        /// </summary>
        /// <param name="baseType"></param>
        /// <param name="memberName"></param>
        /// <param name="objectType"></param>
        IDiRegisterContainer RegisterMember(Type baseType, string memberName, Type objectType);
        /// <inheritdoc cref="RegisterMember(Type, string, Type)"/>
        IDiRegisterContainer RegisterMember(Type baseType, string memberName, Type objectType, string name);

        /// <inheritdoc cref="RegisterMember(Type, string, Type)"/>
        IDiRegisterContainer RegisterMember<TBase, TObject>(string memberName);
        /// <inheritdoc cref="RegisterMember(Type, string, Type)"/>
        IDiRegisterContainer RegisterMember<TBase, TObject>(string memberName, string name);

        #endregion

        #region Unregister

        /// <summary>
        /// 登録解除。
        /// </summary>
        /// <param name="interfaceType"></param>
        /// <returns></returns>
        bool Unregister(Type interfaceType);
        /// <inheritdoc cref="Unregister(Type)"/>
        bool Unregister(Type interfaceType, string name);
        /// <inheritdoc cref="Unregister(Type)"/>
        bool Unregister<TInterface>();
        /// <inheritdoc cref="Unregister(Type)"/>
        bool Unregister<TInterface>(string name);

        #endregion

    }
}
