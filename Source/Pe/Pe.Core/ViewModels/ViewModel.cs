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
            var context = new ValidationContext(this) { MemberName = propertyName };
            var validationErrors = new List<ValidationResult>();
            if(!Validator.TryValidateProperty(value, context, validationErrors)) {
                var errors = validationErrors.Select(error => error.ErrorMessage);
                ErrorsContainer.SetErrors(propertyName, errors);
            } else {
                ErrorsContainer.ClearErrors(propertyName);
            }
        }

        /// <summary>
        /// 全プロパティ検証。
        /// </summary>
        protected void ValidateAllProperty()
        {
            var type = GetType();
            var properties = type.GetProperties();
            var targetProperties = properties
                .Select(i => new { Property = i, Attributes = i.GetCustomAttributes<ValidationAttribute>() })
                .Where(i => i.Attributes != null)
                .Select(i => i.Property)
                .ToList()
            ;
            foreach(var property in targetProperties) {
                var rawValue = property.GetValue(this);
                ValidateProperty(rawValue!, property.Name);
            }

            var childProperties = properties.Except(targetProperties);
            foreach(var property in childProperties) {
                var rawValue = property.GetValue(this);
                switch(rawValue) {
                    case ViewModelBase viewModel:
                        viewModel.ValidateAllProperty();
                        break;

                    case IEnumerable enumerable:
                        foreach(var element in enumerable.OfType<ViewModelBase>()) {
                            element.ValidateAllProperty();
                        }
                        break;

                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// ビジネスロジックの検証。
        /// </summary>
        protected virtual void ValidateDomain()
        { }

        protected bool Validate()
        {
            if(HasErrors) {
                return false;
            }

            ValidateAllProperty();

            if(HasErrors) {
                return false;
            }

            ValidateDomain();

            return !HasErrors;
        }

        protected void ClearError([CallerMemberName] string propertyName = "")
        {
            ErrorsContainer.ClearErrors(propertyName);
        }

        protected void AddError(string errorMessage, [CallerMemberName] string propertyName = "")
        {
            ErrorsContainer.SetErrors(propertyName, new[] { errorMessage });
        }
        protected void AddErrors(IEnumerable<string> errorMessage, [CallerMemberName] string propertyName = "")
        {
            ErrorsContainer.SetErrors(propertyName, errorMessage);
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
        { }

        protected void AttachModelEvents()
        {
            if(Model != null) {
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

        #region property

        public TData Data
        {
            get => this._data;
            set => SetProperty(ref this._data, value);
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
