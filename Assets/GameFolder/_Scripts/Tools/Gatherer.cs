using SKC.AIF.Data;
using SKC.AIF.Character;
using ObjectItem = SKC.AIF.Storage.ObjectItem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SKC.AIF.Worker;

namespace SKC.AIF.Tools
{
	public class Gatherer : MonoBehaviour
	{
		[SerializeField] GatheringToolDefinitionLookup gatheringToolDefinitionLookup;
		[SerializeField] Storage.Storage _inventory;
		[SerializeField] Transform _playerHand;
		[SerializeField] HumanoidAnimator _humanoidAnimationManager;

		List<GatherableSource> _gatherableSources = new List<GatherableSource>();
		GatheringTool _activeGatheringTool;
		WaitForSeconds _delayedCollectWait = new WaitForSeconds(0.7f);
		CharacterMover _characterMover;
		WorkerMovement _workerMover;

        private void Start()
        {
			if(GetComponentInParent<CharacterMover>())
				_characterMover = GetComponentInParent<CharacterMover>();
			else
				_workerMover = GetComponentInParent<WorkerMovement>();
		}

        void Update()
		{
			if (!_inventory.Interactable)
			{
				_humanoidAnimationManager.StopInteraction();
				if (_activeGatheringTool)
				{
					_activeGatheringTool.enabled = false;
				}
				return;
			}

			int gatherableSourcesCount = _gatherableSources.Count;
			if (!_activeGatheringTool && gatherableSourcesCount > 0)
			{
				TryInstantiateTool(_gatherableSources[0]);
			}

			if (_activeGatheringTool)
			{
				_activeGatheringTool.enabled = true;
				_humanoidAnimationManager.PlayInteraction(_activeGatheringTool.GatheringToolDefinition.InteractionAnimationId, 1f / _activeGatheringTool.GatheringToolDefinition.UseInterval);
			}
			else
			{
				return;
			}

			bool hasNonDepletedSource = false;
			foreach (GatherableSource source in _activeGatheringTool.GatherableSources)
			{
				if (!source.Depleted)
				{
					hasNonDepletedSource = true;
					break;
				}
			}
			if (!hasNonDepletedSource)
			{
				_humanoidAnimationManager.StopInteraction();
				if (_activeGatheringTool)
				{
					_activeGatheringTool.enabled = false;
				}
			}

			if (gatherableSourcesCount == 0)
			{
				return;
			}
			for (int i = gatherableSourcesCount - 1; i >= 0; i--)
			{
				GatherableSource gatherableSource = _gatherableSources[i];
				_activeGatheringTool.AddGatherable(gatherableSource);
				gatherableSource.GatheredItemInstantiated += GatherableSource_GatheredItemInstantiated;
				_gatherableSources.RemoveAt(i);
			}
		}

		void OnTriggerEnter(Collider other)
		{
			if (other.TryGetComponent(out GatherableSource gatherableSource))
			{
				_gatherableSources.Add(gatherableSource);
			}
		}

        private void OnTriggerStay(Collider other)
        {
			if (other.TryGetComponent(out GatherableSource gatherableSource))
			{
				if(_characterMover != null)
					_characterMover.LookTarget(other.gameObject.transform.position);
				else
					_workerMover.LookTarget(other.gameObject.transform.position);
			}
		}

        void OnTriggerExit(Collider other)
		{
			if (other.TryGetComponent(out GatherableSource gatherableItemSource))
			{
				if (_characterMover != null)
					_characterMover.LookTarget(Vector3.zero);
				else
					_workerMover.LookTarget(Vector3.zero);

				_gatherableSources.Remove(gatherableItemSource);
				if (!_activeGatheringTool)
				{
					return;
				}

				_activeGatheringTool.RemoveGatherable(gatherableItemSource);
				gatherableItemSource.GatheredItemInstantiated -= GatherableSource_GatheredItemInstantiated;
				if (!_activeGatheringTool.HasInteractableGatherable)
				{
					Destroy(_activeGatheringTool.gameObject);
					_activeGatheringTool = null;
					_humanoidAnimationManager.StopInteraction();
				}
			}
		}

		bool TryInstantiateTool(GatherableSource gatherableSource)
		{
			int highestTierIndex = -99999;
			int prefabIndex = -1;
			List<GatheringToolDefinition> availableObjects = gatheringToolDefinitionLookup.AvailableObjects;
			for (int i = 0; i < availableObjects.Count; i++)
			{
				GatheringToolDefinition tool = availableObjects[i];
				if (tool.CanGather(gatherableSource.GatherableDefinition) && tool.Tier > highestTierIndex)
				{
					highestTierIndex = tool.Tier;
					prefabIndex = i;
				}
			}

			if (prefabIndex != -1)
			{
				_activeGatheringTool = Instantiate(availableObjects[prefabIndex].GatheringToolPrefab, _playerHand);
				return true;
			}
			else
			{
				return false;
			}
		}

		void GatherableSource_GatheredItemInstantiated(List<ObjectItem> items)
		{
			StartCoroutine(DelayedAddItem(items));
		}

		IEnumerator DelayedAddItem(List<ObjectItem> items)
		{
			yield return _delayedCollectWait;
			foreach (ObjectItem item in items)
			{
				_inventory.Add(item);
			}
			items.Clear();
		}
	}
}
