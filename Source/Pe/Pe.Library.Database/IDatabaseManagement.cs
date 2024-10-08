using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Text;
using System.Xml.Linq;

namespace ContentTypeTextNet.Pe.Library.Database
{
    /// <summary>
    /// データベース情報。
    /// </summary>
    public record class DatabaseInformationItem
    {
        public DatabaseInformationItem(string name)
        {
            Name = name;
        }

        #region property

        /// <summary>
        /// データベース名。
        /// </summary>
        public string Name { get; }

        #endregion
    }

    /// <summary>
    /// スキーマ情報。
    /// </summary>
    public record class DatabaseSchemaItem
    {
        public DatabaseSchemaItem(DatabaseInformationItem database, string name, bool isDefault)
        {
            Database = database;
            Name = name;
            IsDefault = isDefault;
        }

        #region property

        /// <summary>
        /// データベース情報。
        /// </summary>
        public DatabaseInformationItem Database { get; }
        /// <summary>
        /// スキーマ名。
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// デフォルトスキーマか。
        /// </summary>
        public bool IsDefault { get; }

        #endregion
    }

    /// <summary>
    /// リソース種別。
    /// </summary>
    [Flags]
    public enum DatabaseResourceKinds
    {
        /// <summary>
        /// 表。
        /// </summary>
        Table = 0b_0000_0001,
        /// <summary>
        /// ビュー。
        /// </summary>
        View = 0b_0000_0010,
        /// <summary>
        /// 実体ビュー。
        /// </summary>
        MaterializedView = 0b_0000_0100,
        /// <summary>
        /// インデックス。
        /// </summary>
        Index = 0b_0000_1000,

        /// <summary>
        /// 全部。
        /// </summary>
        All = Table | View | MaterializedView | Index,
    }

    /// <summary>
    /// データベース内リソース情報。
    /// </summary>
    public record class DatabaseResourceItem
    {
        public DatabaseResourceItem(DatabaseSchemaItem schema, string name, DatabaseResourceKinds kind, string source)
        {
            Schema = schema;
            Name = name;
            Kind = kind;
            Source = source;
        }

        #region property

        /// <summary>
        /// スキーマ。
        /// </summary>
        public DatabaseSchemaItem Schema { get; }
        /// <summary>
        /// リソース名。
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// 種別。
        /// </summary>
        /// <remarks>複数値は持たない。</remarks>
        public DatabaseResourceKinds Kind { get; }
        /// <summary>
        /// ソース。
        /// </summary>
        public string Source { get; }

        #endregion
    }

    /// <summary>
    /// カラム型。
    /// </summary>
    public record class DatabaseColumnType
    {
        public DatabaseColumnType(string rawType, int precision, int scale, Type cliType)
        {
            RawType = rawType;
            CliType = cliType;
            Precision = precision;
            Scale = scale;
        }

        #region property

        /// <summary>
        /// データベース上の型。
        /// </summary>
        public string RawType { get; }
        /// <summary>
        /// CLI上の型。
        /// </summary>
        public virtual Type CliType { get; }
        /// <summary>
        /// 精度: 整数部。
        /// </summary>
        public virtual int Precision { get; }
        /// <summary>
        /// 精度: 小数部。
        /// </summary>
        public virtual int Scale { get; }

        #endregion
    }

    /// <summary>
    /// カラム情報。
    /// </summary>
    public record class DatabaseColumnItem
    {
        public DatabaseColumnItem(DatabaseResourceItem tableResource, string name, int position, bool isPrimary, bool isNullable, string defaultValue, DatabaseColumnType type)
        {
            TableResource = tableResource;
            Name = name;
            Position = position;
            IsPrimary = isPrimary;
            IsNullable = isNullable;
            DefaultValue = defaultValue;
            Type = type;
        }

        #region property

        /// <summary>
        /// テーブルリソース。
        /// </summary>
        public DatabaseResourceItem TableResource { get; }
        /// <summary>
        /// カラム名。
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// カラム位置。
        /// </summary>
        public int Position { get; }

        /// <summary>
        /// 主キーか。
        /// </summary>
        public bool IsPrimary { get; }
        /// <summary>
        /// NULL許容型か。
        /// </summary>
        public bool IsNullable { get; }
        /// <summary>
        /// デフォルト値。
        /// </summary>
        /// <remarks>生値。</remarks>
        public string DefaultValue { get; }
        /// <summary>
        /// 型。
        /// </summary>
        public DatabaseColumnType Type { get; }

        #endregion
    }

    /// <summary>
    /// データベース実装依存処理のうち実際の問い合わせを伴う処理。
    /// </summary>
    public interface IDatabaseManagement
    {
        #region function

        /// <summary>
        /// DB 内のデータベース一覧を取得。
        /// </summary>
        /// <returns></returns>
        ISet<DatabaseInformationItem> GetDatabaseItems();

        /// <summary>
        /// データベースのスキーマを取得。
        /// </summary>
        /// <param name="informationItem"></param>
        /// <returns></returns>
        ISet<DatabaseSchemaItem> GetSchemaItems(DatabaseInformationItem informationItem);

        /// <summary>
        /// スキーマのリソースを取得。
        /// </summary>
        /// <param name="schemaItem"></param>
        /// <param name="kinds"></param>
        /// <returns></returns>
        ISet<DatabaseResourceItem> GetResourceItems(DatabaseSchemaItem schemaItem, DatabaseResourceKinds kinds);

        /// <summary>
        /// テーブルリソースからカラム取得。
        /// </summary>
        /// <param name="tableResource"></param>
        /// <returns></returns>
        IList<DatabaseColumnItem> GetColumns(DatabaseResourceItem tableResource);

        #endregion
    }
}
