using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Windows.Input;
using System.Xml.Serialization;
using ContentTypeTextNet.Pe.Core.Models;
using Microsoft.Extensions.Logging;
using Prism.Mvvm;

namespace ContentTypeTextNet.Pe.Core.ViewModels
{
    [System.AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public sealed class IgnoreValidationAttribute : Attribute
    {
        // This is a positional argument
        public IgnoreValidationAttribute()
        { }
    }

    public abstract class ViewModelBase : BindableBase, INotifyDataErrorInfo, IDisposable, IDisposer
    {
        public ViewModelBase(ILoggerFactory loggerFactory)
        {
            LoggerFactory = loggerFactory;
            Logger = loggerFactory.CreateLogger(GetType());
            ErrorsContainer = new ErrorsContainer<string>(OnErrorsChanged);
        }

        ~ViewModelBase()
        {
            Dispose(false);
        }

        #region property

        protected ILoggerFactory LoggerFactory { get; }
        protected ILogger Logger { get; }
        //IDictionary<string, ICommand> CommandCache { get; } = new Dictionary<string, ICommand>();
        protected IEnumerable<ICommand> Commands => CommandStore.Commands;
        CommandStore CommandStore { get; } = new CommandStore();

        #endregion

        #region function

        protected virtual bool SetPropertyValue<TValue>(object obj, TValue value, [CallerMemberName] string targetMemberName = "", [CallerMemberName] string notifyPropertyName = "")
        {
            ThrowIfDisposed();

            var type = obj.GetType();
            var propertyInfo = type.GetProperty(targetMemberName);

#pragma warning disable CS8601 // Null 参照割り当ての可能性があります。
#pragma warning disable CS8602 // null 参照の可能性があるものの逆参照です。
            var nowValue = (TValue)propertyInfo.GetValue(obj);
#pragma warning restore CS8602 // null 参照の可能性があるものの逆参照です。
#pragma warning restore CS8601 // Null 参照割り当ての可能性があります。

            if(!IComparable<TValue>.Equals(nowValue, value)) {
                propertyInfo.SetValue(obj, value);
                OnPropertyChanged(new PropertyChangedEventArgs(notifyPropertyName));

                return true;
            }

            return false;
        }

        protected TCommand GetOrCreateCommand<TCommand>(Func<TCommand> creator, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
            where TCommand : ICommand
        {
            ThrowIfDisposed();

            //var sb = new StringBuilder();
            //sb.Append(GetType().FullName);
            //sb.Append(':');
            //sb.Append(callerMemberName);
            //sb.Append(':');
            //sb.Append(callerFilePath.GetHashCode());
            //sb.Append(':');
            //sb.Append(callerLineNumber);

            //var key = sb.ToString();

            //if(CommandCache.TryGetValue(key, out var cahceCommand)) {
            //    return (TCommand)cahceCommand;
            //}

            //var command = creator();
            //CommandCache.Add(key, command);

            //return command;
            return CommandStore.GetOrCreate(creator, callerMemberName, callerFilePath, callerLineNumber);
        }

        /// <summary>
        /// プロパティ検証。
        /// </summary>
        /// <param name="value"></param>
        /// <param name="propertyName"></param>
        protected void ValidateProperty(object? value, [CallerMemberName] string propertyName = "")
        {
            ThrowIfDisposed();

            var context = new ValidationContext(this) { MemberName = propertyName };
            var validationErrors = new List<ValidationResult>();
            if(!Validator.TryValidateProperty(value, context, validationErrors)) {
                var errors = validationErrors.Select(error => error.ErrorMessage);
                ErrorsContainer.SetErrors(propertyName, errors);
            } else {
                ErrorsContainer.ClearErrors(propertyName);
            }
        }

        private (IReadOnlyCollection<PropertyInfo> properties, IReadOnlyCollection<ViewModelBase> childViewModels) GetValidationItems()
        {
            ThrowIfDisposed();

            var type = GetType();
            //var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            var properties = type.GetProperties()
                .Select(i => new { Property = i, Attribute = i.GetCustomAttribute<IgnoreValidationAttribute>() })
                .Where(i => i.Attribute == null)
                .Select(i => i.Property)
                .ToList()
            ;
            var targetProperties = properties
                .Select(i => new { Property = i, Attributes = i.GetCustomAttributes<ValidationAttribute>() })
                .Where(i => i.Attributes.Any())
                .Select(i => i.Property)
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
        /// 全プロパティ検証。
        /// </summary>
        private void ValidateAllProperty()
        {
            ThrowIfDisposed();

            var v = GetValidationItems();
            //var type = GetType();
            //var properties = type.GetProperties();
            //var targetProperties = properties
            //    .Select(i => new { Property = i, Attributes = i.GetCustomAttributes<ValidationAttribute>() })
            //    .Where(i => i.Attributes != null)
            //    .Select(i => i.Property)
            //    .ToList()
            //;
            foreach(var property in v.properties) {
                var rawValue = property.GetValue(this);
                ValidateProperty(rawValue!, property.Name);
            }

            //var childProperties = properties.Except(targetProperties);
            //foreach(var property in childProperties) {
            //    var rawValue = property.GetValue(this);
            //    switch(rawValue) {
            //        case ViewModelBase viewModel:
            //            viewModel.ValidateAllProperty();
            //            break;

            //        case IEnumerable enumerable:
            //            foreach(var element in enumerable.OfType<ViewModelBase>()) {
            //                element.ValidateAllProperty();
            //            }
            //            break;

            //        default:
            //            break;
            //    }
            //}
            foreach(var childViewModel in v.childViewModels) {
                childViewModel.ValidateAllProperty();
            }
        }

        /// <summary>
        /// ビジネスロジックの検証。
        /// </summary>
        protected virtual void ValidateDomain()
        {
            ThrowIfDisposed();
        }

        private void ValidateAllDomain()
        {
            ThrowIfDisposed();

            var v = GetValidationItems();
            ValidateDomain();
            foreach(var childViewModel in v.childViewModels) {
                childViewModel.ValidateAllDomain();
            }
        }

        bool HasChildrenErros()
        {
            ThrowIfDisposed();

            var v = GetValidationItems();
            var result = v.childViewModels.Any(i => i.HasErrors || i.HasChildrenErros());
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

            if(HasErrors || HasChildrenErros()) {
                return false;
            }

            ValidateAllProperty();

            if(HasErrors || HasChildrenErros()) {
                return false;
            }

            ClearAllErrors();
            ValidateAllDomain();

            return !HasErrors && !HasChildrenErros();
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

        #endregion

        #region INotifyDataErrorInfo

        private ErrorsContainer<string> ErrorsContainer { get; }

        protected void OnErrorsChanged([CallerMemberName] string propertyName = "")
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        public System.Collections.IEnumerable GetErrors(string propertyName)
        {
            return ErrorsContainer.GetErrors(propertyName);
        }

        public bool HasErrors => ErrorsContainer.HasErrors;

        #endregion

        #region IDisposable

        protected void ThrowIfDisposed([CallerMemberName] string _callerMemberName = "")
        {
            if(IsDisposed) {
                throw new ObjectDisposedException(_callerMemberName);
            }
        }

        /// <summary>
        /// <see cref="IDisposable.Dispose"/>時に呼び出されるイベント。
        /// <para>呼び出し時点では<see cref="IsDisposed"/>は偽のまま。</para>
        /// </summary>
        [field: NonSerialized]
        public event EventHandler? Disposing;

        /// <summary>
        /// <see cref="IDisposable.Dispose"/>されたか。
        /// </summary>
        [IgnoreDataMember, XmlIgnore]
        public bool IsDisposed { get; private set; }

        /// <summary>
        /// <see cref="IDisposable.Dispose"/>の内部処理。
        /// <para>継承先クラスでは本メソッドを呼び出す必要がある。</para>
        /// </summary>
        /// <param name="disposing">CLRの管理下か。</param>
        protected virtual void Dispose(bool disposing)
        {
            if(IsDisposed) {
                return;
            }

            if(Disposing != null) {
                Disposing(this, EventArgs.Empty);
            }

            if(disposing) {
                GC.SuppressFinalize(this);
            }

            IsDisposed = true;
        }

        /// <summary>
        /// 解放。
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        #endregion
    }

    public abstract class SingleModelViewModelBase<TModel> : ViewModelBase
        where TModel : INotifyPropertyChanged
    {
        public SingleModelViewModelBase(TModel model, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            Model = model;

            AttachModelEvents();
        }


        #region property

        protected TModel Model { get; private set; }

        #endregion

        #region function

        protected bool SetModelValue<T>(T value, [CallerMemberName] string targetMemberName = "", [CallerMemberName] string notifyPropertyName = "")
        {
            return SetPropertyValue(Model, value, targetMemberName, notifyPropertyName);
        }

        protected virtual void AttachModelEventsImpl()
        {
            ThrowIfDisposed();

        }

        protected void AttachModelEvents()
        {
            if(Model != null) {
                ThrowIfDisposed();

                AttachModelEventsImpl();
            }
        }

        protected virtual void DetachModelEventsImpl()
        { }

        protected void DetachModelEvents()
        {
            if(Model != null) {
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

    public class SimpleDataViewModel<TData> : ViewModelBase
    {
        #region variable

        TData _data;

        #endregion

        public SimpleDataViewModel(TData data, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            this._data = data;
        }

        #region function

        public TData Data
        {
            get => this._data;
            set => SetProperty(ref this._data, value);
        }

        #endregion

        #region ViewModelBase

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        #endregion
    }

    public static class SimpleDataViewModel
    {
        #region function

        public static SimpleDataViewModel<TData> Create<TData>(TData data, ILoggerFactory loggerFactory) => new SimpleDataViewModel<TData>(data, loggerFactory);

        #endregion
    }
}
