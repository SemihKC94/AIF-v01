using SKC.AIF.NaughtyAttributes_2._1._4.Core.DrawerAttributes_SpecialCase;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SKC.AIF.Customers
{
    [CreateAssetMenu(menuName = nameof(AIF) + "/" + nameof(Customers) + "/" + nameof(CustomerManager))]
    public class CustomerManager : ScriptableObject
    {
        [SerializeField] Customer _customerMoverPrefab;

        [NonSerialized] readonly List<Customer> _activeEmployees = new List<Customer>();

        public void AssignCustomers(CustomerPath[] customerPaths, CustomerBase _customerBase,Vector3 instantiatingPoint)
        {
            var employee = Instantiate(_customerMoverPrefab, instantiatingPoint, Quaternion.identity);
            employee.Initialize(customerPaths,_customerBase);
            _activeEmployees.Add(employee);
        }

        [Button]
        public void PauseAll()
        {
            foreach (Customer activeEmployee in _activeEmployees)
            {
                activeEmployee.Pause();
            }
        }

        [Button]
        public void ResumeAll()
        {
            foreach (Customer activeEmployee in _activeEmployees)
            {
                activeEmployee.Resume();
            }
        }
    }
}