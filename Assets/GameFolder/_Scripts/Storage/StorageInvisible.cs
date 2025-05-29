using System;
using UnityEngine;
using SKC.AIF.Helpers;

namespace SKC.AIF.Storage
{
	[Serializable]
	public class StorageInvisible : StorageBase
	{
		[SerializeField] int _capacity;

		public override bool IsFull() => Count >= _capacity;

		public void SetCapacity(int capacity)
		{
			_capacity = capacity;
		}

		protected override void OnAdding(ObjectItem item)
		{
			Transform trans = item.transform;
			TweenHelper.KillAllTweens(trans);
			trans.SetParent(StackingPoint);
			item.JumpAndDisappear(Vector3.zero, 2f, PickUpDuration);
		}
	}
}
