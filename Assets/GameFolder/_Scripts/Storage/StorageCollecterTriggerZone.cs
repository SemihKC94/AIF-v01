using SKC.AIF.Helpers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SKC.AIF.Storage.Trigger
{
	[SelectionBase]
	public class StorageCollecterTriggerZone : MonoBehaviour
	{
		[SerializeField] ObjectDefinitionCountPair[] _itemsToCollect;
		[SerializeField] Timer _collectingIntervalTimer;
		[SerializeField] Storage _outputInventory;

		Dictionary<Storage, Coroutine> _coroutineDictionary = new Dictionary<Storage, Coroutine>();
		ObjectDefinitionCountPair[] _currentItemsToCollect;

		void Awake()
		{
			_currentItemsToCollect = new ObjectDefinitionCountPair[_itemsToCollect.Length];
			for (int i = 0; i < _currentItemsToCollect.Length; i++)
			{
				_currentItemsToCollect[i].ItemDefinition = _itemsToCollect[i].ItemDefinition;
				_currentItemsToCollect[i].Count = _itemsToCollect[i].Count;
			}
		}

		void OnTriggerEnter(Collider other)
		{
			if (other.TryGetComponent(out Storage inventory))
			{
				_coroutineDictionary.Add(inventory, StartCoroutine(Co_Collect(inventory)));
			}
		}

		void OnTriggerExit(Collider other)
		{
			if (other.TryGetComponent(out Storage inventory))
			{
				StopCoroutine(_coroutineDictionary[inventory]);
				_coroutineDictionary.Remove(inventory);
				_collectingIntervalTimer.SetZero();
			}
		}

		IEnumerator Co_Collect(Storage inventory)
		{
			while (true)
			{
				if (_currentItemsToCollect.Length == 0 || !inventory.Interactable || _outputInventory.IsVisibleFull)
				{
					yield return null;
					continue;
				}

				if (_collectingIntervalTimer.IsCompleted)
				{
					for (int i = 0; i < _currentItemsToCollect.Length; i++)
					{
						if ((_itemsToCollect[i].Count > 0 && _currentItemsToCollect[i].Count <= 0) || !inventory.Contains(_currentItemsToCollect[i].ItemDefinition, out ObjectItem item))
						{
							continue;
						}

						_currentItemsToCollect[i].Count--;
						inventory.Remove(item);
						_outputInventory.Add(item);
						_collectingIntervalTimer.SetZero();

						bool shouldRefresh = true;
						foreach (ObjectDefinitionCountPair currentItem in _currentItemsToCollect)
						{
							if (currentItem.Count > 0)
							{
								shouldRefresh = false;
							}
						}

						if (shouldRefresh)
						{
							for (int index = 0; index < _currentItemsToCollect.Length; index++)
							{
								_currentItemsToCollect[index].Count = _itemsToCollect[index].Count;
							}
						}
						break;
					}
				}
				else
				{
					_collectingIntervalTimer.Tick();
				}

				yield return null;
			}
		}
	}
}
