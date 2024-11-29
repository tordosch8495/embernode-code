using Microsoft.Azure.Cosmos;
using System.Threading.Tasks;
using EmbernodeApp.Models;

namespace EmbernodeApp.Services
{
    public class CosmosDbService
    {
        private readonly Container _container;

        public CosmosDbService(CosmosClient cosmosClient, string databaseName, string containerName)
        {
            _container = cosmosClient.GetContainer(databaseName, containerName);
        }

        public async Task AddUserDataAsync(UserData userData)
        {
            if (string.IsNullOrEmpty(userData.Id))
            {
                userData.Id = Guid.NewGuid().ToString(); // ID generieren, falls nicht vorhanden
            }
            // Setze Partition Key auf den gleichen Wert wie Id
            userData.PartitionKey = userData.Id;

            Console.WriteLine($"Speichere Daten: ID={userData.Id}, Name={userData.Name}, Email={userData.Email}");

            try
            {
                await _container.CreateItemAsync(userData, new PartitionKey(userData.Id)); // Speichern
                Console.WriteLine("Daten erfolgreich in Cosmos DB gespeichert.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler beim Speichern in Cosmos DB: {ex.Message}");
                throw; // Fehler erneut auslösen, um Debugging zu erleichtern
            }
        }
    }
}