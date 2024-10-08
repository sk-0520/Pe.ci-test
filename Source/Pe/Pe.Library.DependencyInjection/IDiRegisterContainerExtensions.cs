namespace ContentTypeTextNet.Pe.Library.DependencyInjection
{
    public static class IDiRegisterContainerExtensions
    {
        #region function

        public static IDiRegisterContainer Register<TObject>(this IDiRegisterContainer container, DiLifecycle lifecycle)
#if !ENABLED_STRUCT
            where TObject : class
#endif
        {
            return container.Register<TObject, TObject>(lifecycle);
        }

        public static IDiRegisterContainer Register<TObject>(this IDiRegisterContainer container, string name, DiLifecycle lifecycle)
#if !ENABLED_STRUCT
            where TObject : class
#endif
        {
            return container.Register<TObject, TObject>(name, lifecycle);
        }

        public static IDiRegisterContainer Register<TObject>(this IDiRegisterContainer container, TObject value)
#if !ENABLED_STRUCT
            where TObject : class
#endif
        {
            return container.Register<TObject, TObject>(value);
        }

        public static IDiRegisterContainer Register<TObject>(this IDiRegisterContainer container, string name, TObject value)
#if !ENABLED_STRUCT
            where TObject : class
#endif
        {
            return container.Register<TObject, TObject>(name, value);
        }

        #endregion
    }
}
