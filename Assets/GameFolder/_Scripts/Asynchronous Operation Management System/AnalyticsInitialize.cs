using System.Threading.Tasks;
using UnityEngine;

namespace SKC.AOMS
{
    public class AnalyticsInitialize : MonoBehaviour, IOperation
    {
        public string ApiEndpoint;

        public async Task Perform()
        {
            Debug.Log($"Initializing Analytics with endpoint: {ApiEndpoint}");
            await Task.Delay(Random.Range(1000, 3000)); // Simulate initialization write ur initialize code before
            Debug.Log("Analytics Initialized.");
        }
    }
}
