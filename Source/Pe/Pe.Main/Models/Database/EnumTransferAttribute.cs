using System;

namespace ContentTypeTextNet.Pe.Main.Models.Database
{
    /// <summary>
    /// <see langword="enum" /> の属性設定にて <see cref="EnumTransfer{TEnum}"/> を制御する。
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class EnumTransferAttribute: Attribute
    {
        public EnumTransferAttribute(string value)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(value);

            Value = value;
        }

        #region property

        public string Value { get; }

        #endregion
    }
}
