using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Main.Models.Data.ServerApi
{
    public record class ServerApiError
    {
        [JsonPropertyName("message")]
        public string Message { get; init; } = string.Empty;
        [JsonPropertyName("code")]
        public string Code { get; init; } = string.Empty;
        [JsonPropertyName("info")]
        public object Information { get; init; } = new();
    }

    public record class ServerApiResultData<T>
    {
        #region property

        [JsonPropertyName("data")]
        public T? Data { get; init; }

        [JsonPropertyName("error")]
        public ServerApiError? Error { get; init; }

        #endregion
    }
}
