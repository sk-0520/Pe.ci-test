//#define ENABLED_PRISM7

using System;

#if ENABLED_PRISM7
using Prism.Ioc;
#endif

namespace ContentTypeTextNet.Pe.Library.DependencyInjection
{
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
        /// <see cref="IDiContainer.Inject{TObject}(TObject)"/> を行う際に <see cref="DiInjectionAttribute"/> を設定できないプロパティに無理やり設定する。
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
