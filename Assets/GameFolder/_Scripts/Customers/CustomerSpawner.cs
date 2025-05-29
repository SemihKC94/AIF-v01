using System.Collections;
using UnityEngine;
using SKC.AIF.Helpers;

namespace SKC.AIF.Customers
{
    public class CustomerSpawner : MonoBehaviour
    {
        [SerializeField] CustomerManager _customerManager;
        [SerializeField] CustomerBase _customerBase;
        [SerializeField] Transform _instantiatingPoint;
        [SerializeField] CustomerPath[]  _customerSwordShelfPaths;
        [SerializeField] private int _activeCustomerCount = 2;
        [SerializeField] private float _spawnDelay = 2f;
        
        // Privates 
        private int _currentActiveCustomer = 0;
        private bool _isInitialized = false;

        public void SpawnerInitialized(bool on)
        {
            _isInitialized = on;
        }

        public void InitializeCustomer()
        {
            if (_currentActiveCustomer >= _activeCustomerCount)
            {
                StopCoroutine(SpawnCustomer());
                return;
            }
            
            StartCoroutine(SpawnCustomer());
        }

        public void DeInitializeCustomer(Customer doneCustomer)
        {
            _currentActiveCustomer--;
            Destroy(doneCustomer.gameObject);

            InitializeCustomer();
        }

        private IEnumerator SpawnCustomer()
        {
            _currentActiveCustomer++;
            yield return AIFHelper.GetWait(_spawnDelay);
            
            Assign();
        }
        private void Assign()
        {
            _customerManager.AssignCustomers(_customerSwordShelfPaths, _customerBase,_instantiatingPoint.position);
        }

        private void OnDrawGizmosSelected()
        {
            foreach (var path in _customerSwordShelfPaths)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(path._loadPoint, 1f);
                
            }
        }
    }
}