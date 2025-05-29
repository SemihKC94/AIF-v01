using SKC.AIF.Helpers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SKC.AIF.Storage
{
    public class StorageFeederTriggerArea : MonoBehaviour
    {
        [SerializeField] Storage _inventory;
        [SerializeField] Timer _timer;

        Dictionary<Storage, Coroutine> _coroutineDictionary = new Dictionary<Storage, Coroutine>();

        void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Storage inventoryManager) && (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Worker")))
            {
                _coroutineDictionary.Add(inventoryManager, StartCoroutine(Co_Feed(inventoryManager)));
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out Storage inventoryManager) && (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Worker")))
            {
                StopCoroutine(_coroutineDictionary[inventoryManager]);
                _coroutineDictionary.Remove(inventoryManager);
            }
        }

        IEnumerator Co_Feed(Storage inventory)
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
