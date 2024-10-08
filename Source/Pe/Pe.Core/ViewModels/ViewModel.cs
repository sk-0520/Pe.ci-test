using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Windows.Input;
using System.Xml.Serialization;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Library.Property;
using ContentTypeTextNet.Pe.Library.Base;
using Microsoft.Extensions.Logging;
using Prism.Mvvm;

namespace ContentTypeTextNet.Pe.Core.ViewModels
{
    /// <summary>
    /// 検証無視。
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public sealed class IgnoreValidationAttribute: Attribute
    {
        public IgnoreValidationAttribute()
        { }
    }

    /// <summary>
    /// ViewModel の基底。
    /// </summary>
    public abstract class ViewModelBase: BindableBase, INotifyDataErrorInfo, IDisposer
    {
        /// <summary>
        /// 生成。
        /// </summary>
        /// <param name="cacheProperty">プロパティ情報をキャッシュするか。</param>
        /// <param name="loggerFactory"><inheritdoc cref="ILoggerFactory"/></param>
        protected ViewModelBase(bool cacheProperty, ILoggerFactory loggerFactory)
        {
            LoggerFactory = loggerFactory;
            Logger = loggerFactory.CreateLogger(GetType());
            ErrorsContainer = new ErrorsContainer<string>(OnErrorsChanged);
            WeakDisposing = new WeakEvent<EventArgs>(nameof(Disposing));

            if(cacheProperty) {
                CachedProperty = new ConcurrentDictionary<object, CachedProperty>();
            }
        }

        /// <summary>
        /// プロパティ情報をキャッシュする状態で生成。
        /// </summary>
        /// <param name="loggerFactory"><inheritdoc cref="ILoggerFactory"/></param>
        protected ViewModelBase(ILoggerFactory loggerFactory)
            : this(true, loggerFactory)
        { }


        ~ViewModelBase()
        {
            Dispose(false);
        }

        #region property

        private WeakEvent<EventArgs> WeakDisposing { get; }

        /// <inheritdoc cref="ILoggerFactory"/>
        protected ILoggerFactory LoggerFactory { get; }
        /// <summary>
        /// ロガー。
        /// </summary>
        protected ILogger Logger { get; }

        /// <summary>
        /// プロパティアクセス処理キャッシュ。
        /// </summary>
        private ConcurrentDictionary<object, CachedProperty>? CachedProperty { get; }

        /// <summary>
        /// プロパティ変更時のイベントキャッシュ。
        /// </summary>
        private ConcurrentDictionary<string, PropertyChangedEventArgs> PropertyChangedEventArgsCache { get; } = new ConcurrentDictionary<string, PropertyChangedEventArgs>();

        /// <summary>
        /// このVMは検証非対象か。
        /// </summary>
        protected virtual bool SkipValidation { get; } = false;

        #endregion

        #region function

        /// <summary>
        /// オブジェクトのプロパティに値設定。
        /// </summary>
        /// <typeparam name="TValue">設定値のデータ。</typeparam>
        /// <param name="obj">対象オブジェクト。</param>
        /// <param name="value">設定値。</param>
        /// <param name="targetMemberName">設定対象となる<paramref name="obj"/>のメンバ名。未設定で呼び出しメンバ名。</param>
        /// <param name="notifyPropertyName">値設定後に通知するプロパティ名。未設定で呼び出しメンバ名。</param>
        /// <returns>設定されたか。同一値の場合は設定しない。</returns>
        protected virtual bool SetPropertyValue<TValue>(object obj, TValue value, [CallerMemberName] string targetMemberName = "", [CallerMemberName] string notifyPropertyName = "")
        {
#if DEBUG
            using var _ = ActionDisposerHelper.Create((d, sw) => Logger.LogTrace("PROP TIME: {0}", sw.Elapsed), Stopwatch.StartNew());
#endif
            ThrowIfDisposed();

            PropertyInfo? propertyInfo = null;
            CachedProperty? cachedProperty = null;
            TValue? nowValue;

            if(CachedProperty is null) {
                var type = obj.GetType();
                propertyInfo = type.GetProperty(targetMemberName);
                Debug.Assert(propertyInfo != null);

                nowValue = (TValue?)propertyInfo.GetValue(obj);
            } else {
                cachedProperty = CachedProperty.GetOrAdd(obj, o => new CachedProperty(o));

                nowValue = (TValue?)cachedProperty.Get(targetMemberName);
            }

            if(!EqualityComparer<TValue>.Default.Equals(nowValue, value)) {
                if(CachedProperty is null) {
                    propertyInfo!.SetValue(obj, value);
                } else {
                    cachedProperty!.Set(targetMemberName, value);
                }

                var e = PropertyChangedEventArgsCache.GetOrAdd(notifyPropertyName, s => new PropertyChangedEventArgs(s));
                OnPropertyChanged(e);

                return true;
            }

            return false;
        }

        /// <summary>
        /// プロパティ検証。
        /// </summary>
        /// <param name="value"></param>
        /// <param name="propertyName"></param>
        protected void ValidateProperty(object? value, [CallerMemberName] string propertyName = "")
        {
            ThrowIfDisposed();

            var context = new ValidationContext(this) {
                MemberName = propertyName
            };
            var validationErrors = new List<ValidationResult>();
            if(!Validator.TryValidateProperty(value, context, validationErrors)) {
                var errors = validationErrors.Select(error => error.ErrorMessage ?? string.Empty);
                ErrorsContainer.SetErrors(propertyName, errors);
            } else {
                ErrorsContainer.ClearErrors(propertyName);
            }
        }

        /// <summary>
        /// 子を含む全ての検証要素を取得。
        /// </summary>
        /// <returns></returns>
        private (IReadOnlyCollection<PropertyInfo> properties, IReadOnlyCollection<ViewModelBase> childViewModels) GetValidationItems()
        {
            ThrowIfDisposed();

            if(SkipValidation) {
                return (Array.Empty<PropertyInfo>(), Array.Empty<ViewModelBase>());
            }

            var type = GetType();
            var properties = type.GetProperties()
                .Select(i => (property: i, attribute: i.GetCustomAttribute<IgnoreValidationAttribute>()))
                .Where(i => i.attribute == null)
                .Select(i => i.property)
                .ToList()
            ;
            var targetProperties = properties
                .Select(i => (property: i, attributes: i.GetCustomAttributes<ValidationAttribute>()))
                .Where(i => i.attributes.Any())
                .Select(i => i.property)
                .ToList()
            ;

            var childProperties = properties.Except(targetProperties);
            var childViewModels = new List<ViewModelBase>();
            foreach(var property in childProperties) {
                var rawValue = property.GetValue(this);
                switch(rawValue) {
                    case ViewModelBase viewModel:
                        childViewModels.Add(viewModel);
                        break;

                    case IEnumerable enumerable:
                        foreach(var element in enumerable.OfType<ViewModelBase>()) {
                            childViewModels.Add(element);
                        }
                        break;

                    default:
                        break;
                }
            }

            return (targetProperties, childViewModels);
        }

        /// <summary>
        /// 子を含む全てのプロパティ検証。
        /// </summary>
        private void ValidateAllProperty()
        {
            ThrowIfDisposed();

            var validationItems = GetValidationItems();

            foreach(var property in validationItems.properties) {
                var rawValue = property.GetValue(this);
                ValidateProperty(rawValue!, property.Name);
            }

            foreach(var childViewModel in validationItems.childViewModels) {
                childViewModel.ValidateAllProperty();
            }
        }

        /// <summary>
        /// ビジネスロジックの検証。
        /// </summary>
        /// <remarks>
        /// <para>継承先でこいつを最初に呼び出すこと。</para>
        /// </remarks>
        protected virtual void ValidateDomain()
        {
            ThrowIfDisposed();
        }

        /// <summary>
        /// 子を含む全てのビジネスロジックの検証。
        /// </summary>
        private void ValidateAllDomain()
        {
            ThrowIfDisposed();

            var v = GetValidationItems();
            ValidateDomain();
            foreach(var childViewModel in v.childViewModels) {
                childViewModel.ValidateAllDomain();
            }
        }

        private bool HasChildrenErrors()
        {
            ThrowIfDisposed();

            var v = GetValidationItems();
            var result = v.childViewModels.Any(i => i.HasErrors || i.HasChildrenErrors());
            return result;
        }

        private void ClearAllErrors()
        {
            ThrowIfDisposed();

            ErrorsContainer.ClearErrors();
            var v = GetValidationItems();
            foreach(var property in v.properties) {
                ClearError(property.Name);
            }
            foreach(var viewModels in v.childViewModels) {
                viewModels.ClearAllErrors();
            }
        }

        public bool Validate()
        {
            ThrowIfDisposed();

            if(HasErrors || HasChildrenErrors()) {
                return false;
            }

            ValidateAllProperty();

            if(HasErrors || HasChildrenErrors()) {
                return false;
            }

            ClearAllErrors();
            ValidateAllDomain();

            return !HasErrors && !HasChildrenErrors();
        }

        protected void ClearError([CallerMemberName] string propertyName = "")
        {
            ThrowIfDisposed();

            ErrorsContainer.ClearErrors(propertyName);
        }

        protected void SetError(string errorMessage, [CallerMemberName] string propertyName = "")
        {
            ThrowIfDisposed();

            ErrorsContainer.SetErrors(propertyName, new[] { errorMessage });
        }
        protected void SetErrors(IEnumerable<string> errorMessage, [CallerMemberName] string propertyName = "")
        {
            ThrowIfDisposed();

            ErrorsContainer.SetErrors(propertyName, errorMessage);
        }
        protected void AddError(string message, [CallerMemberName] string propertyName = "")
        {
            ThrowIfDisposed();

            var errors = ErrorsContainer.GetErrors(propertyName).ToList();
            if(!errors.Contains(message)) {
                errors.Add(message);
                ErrorsContainer.SetErrors(propertyName, errors);
            }
        }
        protected void AddErrors(IEnumerable<string> messages, [CallerMemberName] string propertyName = "")
        {
            ThrowIfDisposed();

            var errors = ErrorsContainer.GetErrors(propertyName).ToList();
            foreach(var message in messages) {
                if(!errors.Contains(message)) {
                    errors.Add(message);
                    ErrorsContainer.SetErrors(propertyName, errors);
                }
            }
        }

        protected void ThrowIfDisposed()
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
        }

        #endregion

        #region INotifyDataErrorInfo

        private ErrorsContainer<string> ErrorsContainer { get; }

        protected void OnErrorsChanged([CallerMemberName] string propertyName = "")
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        public System.Collections.IEnumerable GetErrors(string? propertyName)
        {
            return ErrorsContainer.GetErrors(propertyName);
        }

        public bool HasErrors => ErrorsContainer.HasErrors;

        #endregion

        #region IDisposable

        /// <summary>
        /// <see cref="IDisposable.Dispose"/>時に呼び出されるイベント。
        /// </summary>
        /// <remarks>
        /// <para>呼び出し時点では<see cref="IsDisposed"/>は偽のまま。</para>
        /// </remarks>
        public event EventHandler<EventArgs>? Disposing
        {
            add => WeakDisposing.Add(value);
            remove => WeakDisposing.Remove(value);
        }
        /// <summary>
        /// <see cref="IDisposable.Dispose"/>されたか。
        /// </summary>
        [IgnoreDataMember, XmlIgnore]
        public bool IsDisposed { get; private set; }

        /// <summary>
        /// <see cref="IDisposable.Dispose"/>の内部処理。
        /// </summary>
        /// <remarks>
        /// <para>継承先クラスでは本メソッドを呼び出す必要がある。</para>
        /// </remarks>
        /// <param name="disposing">CLRの管理下か。</param>
        protected virtual void Dispose(bool disposing)
        {
            if(IsDisposed) {
                return;
            }

            WeakDisposing.Raise(this, EventArgs.Empty);

            if(CachedProperty is not null) {
                CachedProperty.Clear();
            }

            ErrorsContainer.ClearErrors();
            PropertyChangedEventArgsCache.Clear();

            IsDisposed = true;
        }


        /// <inheritdoc cref="IDisposable.Dispose" />
        [SuppressMessage("Usage", "CA1816:Dispose メソッドは、SuppressFinalize を呼び出す必要があります")]
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }

    /// <summary>
    /// model と対になる ViewModel の基底クラス。
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public abstract class SingleModelViewModelBase<TModel>: ViewModelBase
        where TModel : INotifyPropertyChanged
    {
        protected SingleModelViewModelBase(TModel model, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            Model = model;

            AttachModelEvents();
        }


        #region property

        /// <summary>
        /// 取り込んだモデル。
        /// </summary>
        /// <remarks>
        /// <para><see cref="Dispose(bool)"/>後は <see langword="null" /> が入るので注意ね。</para>
        /// </remarks>
        protected TModel Model { get; private set; }

        #endregion

        #region function

        /// <summary>
        /// <see cref="Model"/>のプロパティに対して値設定。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="targetMemberName"></param>
        /// <param name="notifyPropertyName"></param>
        /// <returns></returns>
        protected bool SetModelValue<T>(T value, [CallerMemberName] string targetMemberName = "", [CallerMemberName] string notifyPropertyName = "")
        {
            return SetPropertyValue(Model, value, targetMemberName, notifyPropertyName);
        }

        /// <summary>
        /// モデルを取り込んだ際に一度だけ呼び出される処理。
        /// </summary>
        /// <remarks>
        /// <para>継承クラスでは一番最初に呼び出すこと。</para>
        /// </remarks>
        protected virtual void AttachModelEventsImpl()
        {
            ThrowIfDisposed();
        }

        protected void AttachModelEvents()
        {
            if(Model is not null) {
                ThrowIfDisposed();

                AttachModelEventsImpl();
            }
        }

        /// <summary>
        /// モデルとサヨナラするとき(<see cref="Dispose(bool)"/>とか)するときに一度だけ呼び出される。
        /// </summary>
        /// <remarks>
        /// <para>継承クラスでは一番最初に呼び出すこと。</para>
        /// </remarks>
        protected virtual void DetachModelEventsImpl()
        { }

        protected void DetachModelEvents()
        {
            if(Model is not null) {
                DetachModelEventsImpl();
            }
        }

        #endregion

        #region ViewModelBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                DetachModelEvents();
            }
            base.Dispose(disposing);
            Model = default(TModel)!;
        }

        #endregion
    }
}
