using System;
using System.Collections.Generic;
using UnityEngine;
using SKC.AIF.Storage;
using SKC.AIF.Helpers;

namespace SKC.AIF.Processors
{
	[SelectionBase]
	public class SellerFloatingImage : MonoBehaviour
	{
		[SerializeField] Storage.Storage _inventory;
		[SerializeField] SellerFloatingImageDefinition _definition;
		[SerializeField] Timer _timer;
		[SerializeField] Transform _sellingPoint;

		Camera _camera;

		void Awake()
		{
			_camera = Camera.main;
		}

		void Update()
		{
			if (_inventory.IsEmpty)
			{
				return;
			}
			// If inventory is empty do nothing. If it's not, then if it has an item that we can sell, increase the timer and sell it.
			foreach (ObjectDefinition sellable in _definition.SellableItemDefinitions)
			{
				if (!_inventory.Contains(sellable, out ObjectItem result))
				{
					continue;
				}


				if (_timer.IsCompleted)
				{
					_inventory.Remove(result);
					TweenHelper.KillAllTweens(result.transform);
					TweenHelper.Jump(result.transform, _sellingPoint.position, _definition.JumpHeight, 1, _definition.JumpDuration, () => Sell(result));
					_timer.SetZero();
				}
				else
				{
					_timer.Tick();
				}
				return;
			}
		}

		void Sell(ObjectItem item)
		{
			item.ReleaseToPool();
			Vector3 point = _camera.WorldToScreenPoint(item.transform.position);
			_definition.FloatingImageResourceAnimator.Play(point, item.Definition.SellValue);
		}
	}
}
