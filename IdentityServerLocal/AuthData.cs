using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace IdentityServerLocal
{
    public class AuthData
    {
        [JsonProperty("apiResources")]
        public List<ResourceData> ApiResources { get; set; }
        [JsonProperty("clients")]
        public List<ClientData> Clients { get; set; }
    }

    public class ClientData
    {
        [JsonProperty("clientId")]
        public string ClientId { get; set; }
        [JsonProperty("allowedGrantType")]
        public string AllowedGrantType { get; set; }
        [JsonProperty("clientSecrets")]
        public List<string> ClientSecrets { get; set; }
        [JsonProperty("allowedScopes")]
        public List<string> AllowedScopes { get; set; }
        [JsonProperty("claims")]
        public List<ResourceData> Claims { get; set; }
    }

    public class ResourceData
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("value")]
        public string Value { get; set; }
    }
}
