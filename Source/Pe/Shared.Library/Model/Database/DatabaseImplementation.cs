using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Library.Shared.Library.Model.Database
{
    public interface IDatabaseImplementation
    {
        #region function

        object GetNullValue(Type type);
        T GetNullValue<T>();

        bool IsNull(object value);

        bool IsNull<T>(T? value)
            where T : struct
        ;

        /// <summary>
        /// SQL 実行前に実行する SQL に対して変換処理を実行。
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        string PreFormatSql(string sql);

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

        #region IDatabaseNullChecker

        public virtual object GetNullValue(Type type)
        {
            return null;
        }

        public T GetNullValue<T>()
        {
            return (T)GetNullValue(typeof(T));
        }

        public virtual bool IsNull(object value)
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

        public virtual string PreFormatSql(string sql) => sql;


        #endregion
    }

}
