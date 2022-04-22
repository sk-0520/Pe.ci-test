using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models.Data;

namespace ContentTypeTextNet.Pe.Main.Models.Data.ServerApi
{
    public record class PluginInformationItemData
    {
        #region property

        [JsonPropertyName("user_id")]
        public Guid UserId { get; init; }
        [JsonPropertyName("plugin_name")]
        public string PluginName { get; init; } = string.Empty;
        [JsonPropertyName("display_name")]
        public string DisplayName { get; init; } = string.Empty;
        [JsonPropertyName("state")]
        public string State { get; init; } = string.Empty;
        [JsonPropertyName("description")]
        public string Description { get; init; } = string.Empty;
        [JsonPropertyName("check_url")]
        public string CheckUrl { get; init; } = string.Empty;
        [JsonPropertyName("project_url")]
        public string ProjectUrl { get; init; } = string.Empty;

        #endregion
    }

    public record class PluginInformationResultData
    {
        #region property

        [JsonPropertyName("plugins")]
        public Dictionary<Guid, PluginInformationItemData> Plugins { get; init; } = new();

        #endregion
    }
}
