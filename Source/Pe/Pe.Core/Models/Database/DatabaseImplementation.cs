using System;
using System.Collections.Generic;
using System.Text;

namespace ContentTypeTextNet.Pe.Core.Models.Database
{
    /// <summary>
    /// データベース実装依存処理。
    /// </summary>
    public interface IDatabaseImplementation
    {
        #region property

        /// <summary>
        /// DDLのトランザクションが有効か。
        /// </summary>
        bool SupportedTransactionDDL { get; }
        /// <summary>
        /// DMLのトランザクションが有効か。
        /// </summary>
        bool SupportedTransactionDML { get; }

        #endregion

        #region function

        /// <summary>
        /// 指定の型がデータベース実装にて null 判定される値の取得。
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        object? GetNullValue(Type type);
        /// <summary>
        /// 指定の型がデータベース実装にてnull 判定される値の取得。
        /// <para><see cref="GetNullValue(Type)"/>のラッパー。</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T GetNullValue<T>();

        /// <summary>
        /// データベース実装における null 判定実施。
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        bool IsNull(object? value);
        /// <summary>
        /// データベース実装における null 判定実施。
        /// <para><see cref="IsNull(object)"/>のラッパー。</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        bool IsNull<T>(T? value)
            where T : struct
        ;

        /// <summary>
        /// 文実行前に実行する文に対して変換処理を実行。
        /// </summary>
        /// <param name="statement"></param>
        /// <returns></returns>
        string PreFormatStatement(string statement);

        string ToStatementTableName(string tableName);
        string ToStatementColumnName(string columnName);
        string ToStatementParameterName(string parameterName, int index);

        #endregion
    }

    public static class IDatabaseImplementationExtensions
    {
        #region function

        /// <summary>
        /// 対象DBに対して null であれば DB 用の null として、そうでなければ <paramref name="value"/> をそのまま使用する。
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="this"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static TValue ToNullValue<TValue>(this IDatabaseImplementation @this, TValue value)
        {
            if(@this == null) {
                throw new ArgumentNullException(nameof(@this));
            }

            if(@this.IsNull(value)) {
                return @this.GetNullValue<TValue>();
            }

            return value;
        }

        #endregion
    }

    public class DatabaseImplementation : IDatabaseImplementation
    {
        #region function

        protected bool IsNullable<T>()
        {
            var type = typeof(T);
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        #endregion

        #region IDatabaseImplementation

        public virtual bool SupportedTransactionDDL { get; } = false;
        public virtual bool SupportedTransactionDML { get; } = true;


        public virtual object? GetNullValue(Type type)
        {
            return null;
        }

        public T GetNullValue<T>()
        {
#pragma warning disable CS8601 // Null 参照割り当ての可能性があります。
            return (T)GetNullValue(typeof(T));
#pragma warning restore CS8601 // Null 参照割り当ての可能性があります。
        }

        public virtual bool IsNull(object? value)
        {
            return value == null;
        }

        public bool IsNull<T>(T? value)
            where T : struct
        {
            if(!value.HasValue) {
                return true;
            }

            return IsNull(value.Value);
        }

        public virtual string PreFormatStatement(string statement) => statement;

        public virtual string ToStatementTableName(string tableName) => tableName;
        public virtual string ToStatementColumnName(string columnName) => columnName;
        public virtual string ToStatementParameterName(string parameterName, int index) => "@" + parameterName;

        #endregion
    }
}
