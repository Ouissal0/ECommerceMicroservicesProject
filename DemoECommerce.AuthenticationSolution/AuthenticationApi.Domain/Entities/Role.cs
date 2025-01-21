using System.Text.Json.Serialization;

namespace AuthenticationApi.Domain.Entities
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Role
    {
        CLIENT,
        VENDEUR
       
    }
}
