using System;


namespace ContentTypeTextNet.Pe.Core.Models.Serialization
{
    /// <summary>
    /// シリアライズ・デシリアライズ処理。
    /// </summary>
    public static class SerializeUtility
    {
        #region property

        public static Func<SerializerBase> SerializerCreator { get; set; } = () => new BinaryDataContractSerializer();

        #endregion

        #region function

        public static TResult Clone<TResult>(object value)
        {
            if(!(value is TResult)) {
                throw new ArgumentException($"cast error: {nameof(value)} is not ${typeof(TResult).FullName}");
            }

            var serializer = SerializerCreator();
            return serializer.Clone<TResult>(value);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "HAA0601:Value type to reference type conversion causing boxing allocation")]
        public static TResult Clone<TResult>(TResult value)
            where TResult : new()
        {
            if(!(value is TResult)) {
                throw new ArgumentException($"cast error: {nameof(value)} is not ${typeof(TResult).FullName}");
            }

            var serializer = SerializerCreator();
            return serializer.Clone(value);
        }

        #endregion
    }
}
