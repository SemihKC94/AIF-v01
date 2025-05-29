using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SKC.AIF.Storage
{
    public abstract class StorageBase
    {
        [SerializeField] protected Transform StackingPoint;
        [SerializeField] protected float PickUpDuration = 0.3f;

        protected List<ObjectItem> Items = new List<ObjectItem>();

        public int Count => Items.Count;

        public bool IsEmpty()
        {
            return Items.Count == 0;
        }

        public bool TryRemoveRandom(out ObjectItem item)
        {
            if (Items.Count > 0)
            {
                int rnd = Random.Range(0, Items.Count);
                item = Items[rnd];
                Remove(item);
                return true;
            }

            item = null;
            return false;
        }

        public bool TryRemoveLast(out ObjectItem item)
        {
            if (Items.Count > 0)
            {
                item = Items[Items.Count - 1];
                Remove(item);
                return true;
            }

            item = null;
            return false;
        }

        public bool Contains(ObjectDefinition definition, out ObjectItem result)
        {
            for (int i = Items.Count - 1; i >= 0; i--)
            {
                ObjectItem it = Items[i];
                if (it.Definition == definition)
                {
                    result = it;
                    return true;
                }
            }

            result = null;
            return false;
        }

        public void Add(ObjectItem p)
        {
            OnAdding(p);
            Items.Add(p);
        }

        public abstract bool IsFull();

        protected abstract void OnAdding(ObjectItem item);
        protected virtual void OnRemoved(ObjectItem item)
        {
        }

        public void Remove(ObjectItem item)
        {
            Items.Remove(item);
            item.transform.SetParent(null);
            item.gameObject.SetActive(true);
            OnRemoved(item);
        }
    }
}
