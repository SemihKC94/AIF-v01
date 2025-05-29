using System;
using UnityEngine;
using SKC.AIF.Storage;
using SKC.AIF.Interactables;
using Object = SKC.AIF.Storage.ObjectItem;
using SKC.AIF.Helpers;

namespace SKC.AIF.Storage
{
	[Serializable]
	public class StorageVisible : StorageBase
	{
		[SerializeField] RowColumnHeight _rowColumnHeight;

		public override bool IsFull() => Count >= _rowColumnHeight.GetCapacity();

		public void SetCapacity(RowColumnHeight rowColumnHeight)
		{
			_rowColumnHeight = rowColumnHeight;
		}

		protected override void OnAdding(ObjectItem item)
		{
			Vector3 targetPos = AIFHelper.GetPoint(Count, _rowColumnHeight);
			Transform trans = item.transform;
			TweenHelper.KillAllTweens(trans);
			trans.SetParent(StackingPoint);
			TweenHelper.LocalJumpAndRotate(trans, targetPos, Vector3.zero, 2f, PickUpDuration);
		}

		protected override void OnRemoved(ObjectItem item)
		{
			// Adjust the positions of other items
			int indexOf = Items.IndexOf(item);
			if (Items.Count >= indexOf + 1)
			{
				for (int i = indexOf + 1; i < Items.Count; i++)
				{
					Vector3 targetPoint = AIFHelper.GetPoint(i, _rowColumnHeight);
					TweenHelper.KillAllTweens(Items[i].transform);
					TweenHelper.LocalMove(Items[i].transform, targetPoint, 0.1f);
				}
			}
		}
	}
}
