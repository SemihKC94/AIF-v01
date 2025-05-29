using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

// Asynchronous Operation Management System
namespace SKC.AOMS
{
    public class OperationManager : MonoBehaviour
    {
        public List<MonoBehaviourOperationWrapper> MonoBehaviourOperations = new List<MonoBehaviourOperationWrapper>(); // For Inspector-assigned MonoBehaviour-based operations
        private List<IOperation> _operations = new List<IOperation>();

        [System.Serializable]
        public class MonoBehaviourOperationWrapper
        {
            public MonoBehaviour OperationComponent; // Must implement IOperation
        }

        private async void Start()
        {
            // Populate the internal list of operations
            foreach (var wrapper in MonoBehaviourOperations)
            {
                if (wrapper.OperationComponent is IOperation operation)
                {
                    _operations.Add(operation);
                }
                else if (wrapper.OperationComponent != null)
                {
                    Debug.LogError($"Component {wrapper.OperationComponent.name} does not implement IOperation!");
                }
            }

            await PerformAllOperations();
            Debug.Log("Performed all operations");
        }

        public void AddOperation(IOperation operation)
        {
            _operations.Add(operation);
        }

        public async Task PerformAllOperations()
        {
            List<Task> performTasks = new List<Task>();
            foreach (var operation in _operations)
            {
                performTasks.Add(operation.Perform());
            }

            try
            {
                await Task.WhenAll(performTasks);
            }
            catch (AggregateException ex)
            {
                foreach (var innerException in ex.InnerExceptions)
                {
                    Debug.LogError($"An operation encountered an error: {innerException.Message}");
                }
            }
        }
    }
}
