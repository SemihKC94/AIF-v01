using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace SKC.AOMS
{
    public class RemoteConfigLoader : IOperation
    {
        private string _configUrl;

        public RemoteConfigLoader(string configUrl)
        {
            _configUrl = configUrl;
        }

        public async Task Perform()
        {
            Debug.Log($"Loading remote config from: {_configUrl}");
            // Perform network request here
            await Task.Delay(1500); // Simulate network delay
            Debug.Log("Remote config loaded.");
        }
    }
}
