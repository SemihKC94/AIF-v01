using UnityEngine;

namespace SKC.AIF.Storage
{
	public class StorageRegister : MonoBehaviour
	{
		[SerializeField] Storage _inventory;

		void OnEnable()
		{
			_inventory.ItemAdded += Inventory_ItemAdded;
			_inventory.ItemRemoved += Inventory_ItemRemoved;
		}

		void OnDisable()
		{
			_inventory.ItemAdded -= Inventory_ItemAdded;
			_inventory.ItemRemoved -= Inventory_ItemRemoved;
		}

		void Inventory_ItemRemoved(ObjectItem p, int arg2)
		{
			if (p.Definition.Variable)
			{
				p.Definition.Variable.RuntimeValue -= 1;
			}
		}

		void Inventory_ItemAdded(ObjectItem p, int arg2)
		{
			if (p.Definition.Variable)
			{
				p.Definition.Variable.RuntimeValue += 1;
			}
		}
	}
}
