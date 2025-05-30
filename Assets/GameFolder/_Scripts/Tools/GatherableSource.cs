using System;
using System.Collections.Generic;
using UnityEngine;
using SKC.AIF.Storage;
using SKC.AIF.Helpers;
using ObjectItem = SKC.AIF.Storage.ObjectItem;
using Random = UnityEngine.Random;

namespace SKC.AIF.Tools
{
	public class GatherableSource : MonoBehaviour
	{
		[field: SerializeField, Tooltip("Defines a lot of essential properties.")]
		public GatherableDefinition GatherableDefinition { get; private set; }

		[SerializeField, Tooltip("Feedbacks will be played on this transform component.")]
		Transform _visualTransform;

		List<ObjectItem> _instantiatedItems = new List<ObjectItem>();
		int _currentHitPoint;

		public bool Depleted { get; private set; }

		public event Action<List<ObjectItem>> GatheredItemInstantiated;

		void OnDestroy()
		{
			foreach (ObjectItem instantiatedItems in _instantiatedItems)
			{
				Destroy(instantiatedItems);
			}
			_instantiatedItems.Clear();
		}

		public void TakeHit(int amount)
		{
			if (_currentHitPoint == GatherableDefinition.MaxHitPoint)
			{
				return;
			}

			int newHitPoint = Mathf.Clamp(_currentHitPoint + amount, 0, GatherableDefinition.MaxHitPoint);
			bool isItemSpawned = GatherableDefinition.Gather(_currentHitPoint, newHitPoint, out GatherableReward gatherableReward);
			if (isItemSpawned)
			{
				for (int i = 0; i < gatherableReward.Amount; i++)
				{
					Vector3 randomPoint = Random.insideUnitCircle;
					randomPoint.z = randomPoint.y;
					randomPoint.y = 0f;
					randomPoint = randomPoint.normalized * GatherableDefinition.ItemSpawnRadius;
					Vector3 position = transform.position;
					ObjectItem item = gatherableReward.ItemPool.TakeFromPool();
					item.transform.position = position;
					_instantiatedItems.Add(item);
					TweenHelper.Jump(item.transform, position + randomPoint, 2f, 1, 0.6f);
				}
				GatheredItemInstantiated?.Invoke(_instantiatedItems);

				if (GatherableDefinition.UseFeedbacks)
				{
					GatherableDefinition.ItemSpawnedFeedback.Play(_visualTransform);
				}
			}
			else
			{
				if (GatherableDefinition.UseFeedbacks)
				{
					GatherableDefinition.TakeHitFeedback.Play(_visualTransform);
				}
			}
			_currentHitPoint = newHitPoint;
			if (_currentHitPoint == GatherableDefinition.MaxHitPoint)
			{
				Depleted = true;
				TweenHelper.DisappearSlowly(_visualTransform, GatherableDefinition.DisappearingDuration, OnDisappeared);
			}
		}

		void OnDisappeared()
		{
			_visualTransform.gameObject.SetActive(false);
			TweenHelper.DelayedCall(GatherableDefinition.IdleDuration, OnRespawning);
		}

		void OnRespawning()
		{
			TweenHelper.ShowSlowly(_visualTransform, Vector3.one, GatherableDefinition.ReappearDuration, OnRespawned);
		}

		void OnRespawned()
		{
			_currentHitPoint = 0;
			Depleted = false;
		}
	}
}
