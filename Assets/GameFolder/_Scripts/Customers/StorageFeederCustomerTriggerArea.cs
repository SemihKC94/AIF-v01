using SKC.AIF.Helpers;
using SKC.AIF.Storage;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SKC.AIF.Customers
{
    public class StorageFeederCustomerTriggerArea : MonoBehaviour
    {
        [SerializeField] Storage.Storage _inventory;
        [SerializeField] Timer _timer;

        Dictionary<Storage.Storage, Coroutine> _coroutineDictionary = new Dictionary<Storage.Storage, Coroutine>();
        private Customer _customer;
        
        void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Storage.Storage inventoryManager) && other.gameObject.CompareTag("Customer"))
            {
                _coroutineDictionary.Add(inventoryManager, StartCoroutine(Co_Feed(inventoryManager)));
                _customer = other.gameObject.GetComponent<Customer>();
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out Storage.Storage inventoryManager) && other.gameObject.CompareTag("Customer"))
            {
                StopCoroutine(_coroutineDictionary[inventoryManager]);
                _coroutineDictionary.Remove(inventoryManager);
                _customer = null;
            }
        }

        IEnumerator Co_Feed(Storage.Storage inventory)
        {
            while (true)
            {
                if (!inventory.Interactable || _inventory.IsEmpty)
                {
                    yield return null;
                    continue;
                }

                if (_timer.IsCompleted)
                {
                    if (!inventory.IsVisibleFull && _inventory.TryRemoveLastVisibleItem(out ObjectItem visibleItem))
                    {
                        inventory.Add(visibleItem);
                        if(_customer != null) _customer.UpdateVisual();
                        
                        _timer.SetZero();
                    }
                    else if (!inventory.IsInvisibleFull && _inventory.TryRemoveLastInvisibleItem(out ObjectItem invisibleItem))
                    {
                        inventory.Add(invisibleItem);
                        _timer.SetZero();
                    }
                }
                else
                {
                    _timer.Tick();
                }

                yield return null;
            }
        }
    }
}