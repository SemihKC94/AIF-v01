using System;
using UnityEngine;
using SKC.AIF.Interfaces.Interactables;
using SKC.AIF.Interactables;
using Random = UnityEngine.Random;

namespace SKC.AIF.Storage
{
    public class Storage : MonoBehaviour
    {
		[SerializeField] StorageInvisible _storageInvisible;
		[SerializeField] StorageVisible _storageVisible;

		IInteractable _interactor;

		public event Action<ObjectItem, int> ItemAdded;
		public event Action<ObjectItem, int> ItemRemoved;

		public bool Interactable => _interactor.Interactable;
		public bool IsEmpty => _storageInvisible.IsEmpty() && _storageVisible.IsEmpty();
		public bool IsFull => _storageInvisible.IsFull() && _storageVisible.IsFull();
		public bool IsVisibleEmpty => _storageVisible.IsEmpty();
		public bool IsInvisibleEmpty => _storageInvisible.IsEmpty();
		public bool IsVisibleFull => _storageVisible.IsFull();
		public bool IsInvisibleFull => _storageInvisible.IsFull();

		void Awake()
		{
			_interactor = GetComponent<IInteractable>();
			if (_interactor == null)
			{
				_interactor = new AlwaysEnableInteraction();
			}
		}

		public bool Contains(ObjectDefinition definition, out ObjectItem obj)
		{
			return definition.Visible ? _storageVisible.Contains(definition, out obj) : _storageInvisible.Contains(definition, out obj);
		}

		public void SetVisibleCapacity(RowColumnHeight rowColumnHeight)
		{
			_storageVisible.SetCapacity(rowColumnHeight);
		}

		public void SetInvisibleCapacity(int capacity)
		{
			_storageInvisible.SetCapacity(capacity);
		}

		public void Add(ObjectItem obj)
		{
			if (obj.Definition.Visible)
			{
				AddVisible(obj);
			}
			else
			{
				AddInvisible(obj);
			}
		}

		public void AddVisible(ObjectItem obj)
		{
			_storageVisible.Add(obj);
			ItemAdded?.Invoke(obj, _storageVisible.Count);
		}

		public void AddInvisible(ObjectItem obj)
		{
			_storageInvisible.Add(obj);
			ItemAdded?.Invoke(obj, _storageInvisible.Count);
		}

		public bool CanAdd(ObjectDefinition definition)
		{
			bool result = definition.Visible ? !_storageVisible.IsFull() : !_storageInvisible.IsFull();
			return result;
		}

		public void Remove(ObjectItem obj)
		{
			if (obj.Definition.Visible)
			{
				_storageVisible.Remove(obj);
				ItemRemoved?.Invoke(obj, _storageVisible.Count);
			}
			else
			{
				_storageInvisible.Remove(obj);
				ItemRemoved?.Invoke(obj, _storageInvisible.Count);
			}
		}

		public bool TryRemove(ObjectDefinition definition, out ObjectItem obj)
		{
			if (definition.Visible)
			{
				if (_storageVisible.Contains(definition, out obj))
				{
					Remove(obj);
					return true;
				}
			}
			else
			{
				if (_storageVisible.Contains(definition, out obj))
				{
					Remove(obj);
					return true;
				}
			}

			return false;
		}

		public bool TryRemoveLastVisibleItem(out ObjectItem obj)
		{
			obj = null;

			if (_storageVisible.IsEmpty())
			{
				return false;
			}

			if (_storageVisible.TryRemoveLast(out obj))
			{
				ItemRemoved?.Invoke(obj, _storageVisible.Count);
			}
			return true;
		}

		public bool TryRemoveLastInvisibleItem(out ObjectItem obj)
		{
			obj = null;

			if (_storageInvisible.IsEmpty())
			{
				return false;
			}

			if (_storageInvisible.TryRemoveLast(out obj))
			{
				ItemRemoved?.Invoke(obj, _storageInvisible.Count);
			}
			return true;
		}

		public bool TryRemoveRandomVisibleItem(out ObjectItem obj)
		{
			obj = null;

			if (_storageVisible.IsEmpty())
			{
				return false;
			}


			_storageVisible.TryRemoveRandom(out obj);
			return true;
		}

		public bool TryRemoveRandomInvisibleItem(out ObjectItem obj)
		{
			obj = null;

			if (_storageInvisible.IsEmpty())
			{
				return false;
			}

			_storageInvisible.TryRemoveRandom(out obj);
			return true;
		}

		public bool TryRemoveRandomItem(out ObjectItem obj)
		{
			if (_storageInvisible.IsEmpty() && _storageVisible.IsEmpty())
			{
				obj = null;
				return false;
			}

			if (!_storageVisible.IsEmpty() && !_storageInvisible.IsEmpty())
			{
				if (Random.value > 0.5f)
				{
					return _storageInvisible.TryRemoveRandom(out obj);
				}

				return _storageVisible.TryRemoveRandom(out obj);
			}
			else if (!_storageVisible.IsEmpty())
			{
				return _storageVisible.TryRemoveRandom(out obj);
			}
			else if (!_storageInvisible.IsEmpty())
			{
				return _storageInvisible.TryRemoveRandom(out obj);
			}

			obj = null;
			return false;
		}
	}
}
