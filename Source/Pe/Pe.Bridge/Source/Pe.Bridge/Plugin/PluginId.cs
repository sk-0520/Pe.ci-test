using System;
using System.Collections.Generic;
using System.Text;

namespace ContentTypeTextNet.Pe.Bridge.Plugin
{
    public readonly ref struct PluginId
    {
        public PluginId(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        #region property

        /// <summary>
        /// プラグインの識別ID。
        /// <para>重複してるとバグる。</para>
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// プラグインを人が見て判断するための名前。
        /// <para><see cref="Id"/>と異なり重複してもいいけどなるべく重複しない方針。</para>
        /// <para>ローカライズは考えなくていい。</para>
        /// </summary>
        public string Name { get; }

        #endregion
    }
}
