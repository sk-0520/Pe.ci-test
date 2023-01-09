using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace ContentTypeTextNet.Pe.Main.Models.Telemetry
{
    /// <summary>
    /// 追跡用プロパティ。
    /// <para><see cref="IDictionary{TKey, TValue}"/>となんら変わらないけど宣言を短くする目的。</para>
    /// </summary>
    [SuppressMessage("Naming", "CA1710:識別子は、正しいサフィックスを含んでいなければなりません", Justification = "OK")]
    public class TrackProperties: IDictionary<string, string>, IReadOnlyDictionary<string, string>
    {
        public TrackProperties()
        { }

        public TrackProperties(IEnumerable<KeyValuePair<string, string>> collection)
        { }

        #region property

        public static TrackProperties Empty => new TrackProperties();

        private IDictionary<string, string> Items { get; } = new Dictionary<string, string>();

        #endregion

        #region function

        private void ThrowIfInvalidKey(string key)
        { }
        private void ThrowIfInvalidValue(string value)
        { }

        private void ThrowIfInvalid(string key, string value)
        {
            ThrowIfInvalidKey(key);
            ThrowIfInvalidValue(key);
        }

        private void AddCore(string key, string value)
        {
            ThrowIfInvalid(key, value);
            Items.Add(key, value);
        }

        private void AddOrSet(string key, string value)
        {
            ThrowIfInvalid(key, value);
            Items[key] = value;
        }


        #endregion

        #region IDictionary

        public string this[string key]
        {
            get => Items[key];
            set => AddOrSet(key, value);
        }

        /// <inheritdoc cref="IDictionary{TKey, TValue}.Keys"/>
        public ICollection<string> Keys => Items.Keys;

        /// <inheritdoc cref="IDictionary{TKey, TValue}.Values"/>
        public ICollection<string> Values => Items.Values;

        /// <inheritdoc cref="ICollection.Count"/>
        public int Count => Items.Count;

        public bool IsReadOnly => Items.IsReadOnly;

        public void Add(string key, string value)
        {
            AddCore(key, value);
        }

        public void Add(KeyValuePair<string, string> item)
        {
            AddCore(item.Key, item.Value);
        }

        public void Clear()
        {
            Items.Clear();
        }

        public bool Contains(KeyValuePair<string, string> item)
        {
            return Items.Contains(item);
        }

        public bool ContainsKey(string key)
        {
            return Items.ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<string, string>[] array, int arrayIndex)
        {
            Items.CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        public bool Remove(string key)
        {
            return Items.Remove(key);
        }

        public bool Remove(KeyValuePair<string, string> item)
        {
            return Items.Remove(item);
        }

        public bool TryGetValue(string key, [MaybeNullWhen(false)] out string value)
        {
            var result = Items.TryGetValue(key, out var value2);
            value = value2!;
            return result;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion

        #region IReadOnlyDictionary

        IEnumerable<string> IReadOnlyDictionary<string, string>.Keys => Keys;

        IEnumerable<string> IReadOnlyDictionary<string, string>.Values => Values;

        #endregion
    }
}
