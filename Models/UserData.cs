using System;
using Newtonsoft.Json;

namespace EmbernodeApp.Models
{
    public class UserData
    {
        [JsonProperty(PropertyName = "id")] // Mappt die Eigenschaft "id" auf Cosmos DB
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [JsonProperty(PropertyName = "Id")] // Partition Key mit Großbuchstaben
        public string PartitionKey { get; set; } = Guid.NewGuid().ToString(); // Gleicher Wert wie Id

        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}