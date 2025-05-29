using System;
using System.Collections.Generic;
using UnityEngine;
using SKC.AIF.Storage;
using SKC.AIF.NaughtyAttributes_2._1._4.Core.MetaAttributes;
using SKC.AIF.Helpers;
using SKC.AIF.Interactables;

namespace SKC.AIF.Processors
{
    [SelectionBase]
    public class Transformer : MonoBehaviour
    {
        [SerializeField] TransformerDefinition _definition;

        [Header("Inventories")]
        [SerializeField] Storage.Storage _inputInventory;
        [SerializeField] Storage.Storage _outputInventory;

        [Header("Points")]
        [SerializeField, Tooltip("Point where the transformation happens. You most likely want to place it somewhere hidden from user.")]
        Transform _transformationPoint;
        [SerializeField, ShowIf(nameof(_usePreTransformationQueue))]
        Transform _transformationQueue;

        [Header("Timers")]
        [SerializeField] Timer _transformationTimer;
        [SerializeField] Timer _collectingForTransformationTimer;

        [Header("Configurations")]
        [SerializeField] bool _usePreTransformationQueue;
        [SerializeField, ShowIf(nameof(_usePreTransformationQueue))]
        RowColumnHeight _transformationQueueLayout;

        [Header("Effects")]
        [SerializeField] private ProcessEffect _processorEffect;

        Dictionary<ObjectDefinition, int> _neededResources = new Dictionary<ObjectDefinition, int>();
        List<ObjectItem> _itemsOnTransformationQueue;
        bool _transforming;

        public event Action<ObjectDefinition, int> ItemAdded;
        public event Action ItemProduced;

        public TransformerRuleset Ruleset => _definition.Ruleset;

        void Awake()
        {
            foreach (ObjectDefinitionCountPair rulesetTypeCountPair in _definition.Ruleset.Inputs)
            {
                _neededResources.Add(rulesetTypeCountPair.ItemDefinition, rulesetTypeCountPair.Count);
            }

            if (_usePreTransformationQueue)
            {
                _itemsOnTransformationQueue = new List<ObjectItem>(6);
            }
        }

        void Update()
        {
            if (_transforming)
            {
                _transformationTimer.Tick();
                if(_processorEffect != null)
                    _processorEffect.RunEffect(true);
                if (_transformationTimer.IsCompleted)
                {
                    TryProduceOutput();
                    _transformationTimer.SetZero();
                    _transforming = false;
                    if (_processorEffect != null)
                        _processorEffect.RunEffect(false);
                }
            }

            if (_inputInventory.IsEmpty)
            {
                return;
            }

            // If we still need to pick something and we can pick something, start the timer.
            foreach (ObjectDefinitionCountPair itemDefinitionCountPair in _definition.Ruleset.Inputs)
            {
                if (_neededResources[itemDefinitionCountPair.ItemDefinition] <= 0)
                {
                    continue;
                }

                if (_inputInventory.Contains(itemDefinitionCountPair.ItemDefinition, out ObjectItem item))
                {
                    if (_collectingForTransformationTimer.IsCompleted)
                    {
                        _inputInventory.Remove(item);
                        AddToTransformationQueue(item);
                        _collectingForTransformationTimer.SetZero();
                    }
                    else
                    {
                        _collectingForTransformationTimer.Tick();
                    }
                }
            }
        }

        void AddToTransformationQueue(ObjectItem item)
        {
            TweenHelper.KillAllTweens(item.transform);
            _neededResources[item.Definition] -= 1;

            if (_usePreTransformationQueue)
            {
                _itemsOnTransformationQueue.Add(item);
                Vector3 point = AIFHelper.GetPoint(_itemsOnTransformationQueue.Count, _transformationQueueLayout);
                Transform trans = item.transform;
                trans.SetParent(_transformationQueue);
                TweenHelper.LocalJumpAndRotate(trans, point, Vector3.zero, _definition.JumpHeight, _definition.JumpDuration);
            }
            else
            {
                ItemUtil.JumpToDisappearIntoPool(item, _transformationPoint.position, _definition.JumpHeight, 1, _definition.JumpDuration);
            }

            ItemAdded?.Invoke(item.Definition, _neededResources[item.Definition]);

            // Try to start a transforming process
            foreach (KeyValuePair<ObjectDefinition, int> neededResource in _neededResources)
            {
                if (neededResource.Value > 0)
                {
                    return;
                }
            }
            if (_neededResources[item.Definition] <= 0)
            {
                _transforming = true;

                if (_usePreTransformationQueue)
                {
                    foreach (ObjectItem val in _itemsOnTransformationQueue)
                    {
                        ItemUtil.JumpToDisappearIntoPool(val, _transformationPoint.position, _definition.JumpHeight, 1, _definition.JumpDuration);
                    }
                    _itemsOnTransformationQueue.Clear();
                }
            }
        }

        void TryProduceOutput()
        {
            foreach (var neededResource in _neededResources)
            {
                if (neededResource.Value > 0)
                {
                    return;
                }
            }

            foreach (ObjectDefinitionCountPair output in _definition.Ruleset.Outputs)
            {
                for (int i = 0; i < output.Count; i++)
                {
                    ObjectItem p = output.ItemDefinition.Pool.TakeFromPool();
                    p.transform.position = transform.position;
                    _outputInventory.AddVisible(p);
                }
            }

            foreach (ObjectDefinitionCountPair rulesetTypeCountPair in _definition.Ruleset.Inputs)
            {
                _neededResources[rulesetTypeCountPair.ItemDefinition] = rulesetTypeCountPair.Count;
            }
            ItemProduced?.Invoke();
        }
    }
}
