using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace SKC.AOMS
{
    public class AdSDKInitialize : MonoBehaviour, IOperation
    {
        public string SdkKey;

        public async Task Perform()
        {
            Debug.Log($"Initializing Ad SDK with key: {SdkKey}");
            // Simulate initialization, write ur initialize code before
            await Task.Delay(Random.Range(500, 2000)); 
            Debug.Log("Ad SDK Initialized.");
        }
    }
}
