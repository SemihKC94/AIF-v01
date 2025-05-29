using System.Collections.Generic;
using UnityEngine;
using SKC.AIF.UI;
using SKC.AIF.Storage;

namespace SKC.AIF.Processors
{
	public class TransformerMonitor : MonoBehaviour
	{
		[SerializeField] Transformer _transformer;
		[SerializeField] MonitorEntrySpriteText[] _inputs;
		[SerializeField] MonitorEntrySpriteText[] _outputs;

		Dictionary<ObjectDefinition, MonitorEntrySpriteText> _itemMonitors;

		void Awake()
		{
			_itemMonitors = new Dictionary<ObjectDefinition, MonitorEntrySpriteText>();

			if (_inputs.Length < _transformer.Ruleset.Inputs.Length)
			{
				Debug.LogError($"Not enough text fields to monitor! Add new text fields so monitor can show them", gameObject);
			}
			else if (_inputs.Length > _transformer.Ruleset.Inputs.Length)
			{
				Debug.LogError($"Too much text fields to monitor! Remove the excessive text fields", gameObject);
			}

			for (int i = 0; i < _transformer.Ruleset.Inputs.Length; i++)
			{
				ObjectDefinitionCountPair itemDefinitionCountPair = _transformer.Ruleset.Inputs[i];
				_itemMonitors.Add(itemDefinitionCountPair.ItemDefinition, _inputs[i]);
				_inputs[i].Initialize(itemDefinitionCountPair.ItemDefinition.Sprite, itemDefinitionCountPair.Count.ToString());
			}

			for (int i = 0; i < _outputs.Length; i++)
			{
				MonitorEntrySpriteText output = _outputs[i];
				output.Initialize(_transformer.Ruleset.Outputs[i].ItemDefinition.Sprite, _transformer.Ruleset.Outputs[i].Count.ToString());
			}
		}

		void OnEnable()
		{
			_transformer.ItemAdded += Transformer_ItemAdded;
			_transformer.ItemProduced += Transformer_ItemProduced;
		}

		void OnDisable()
		{
			_transformer.ItemAdded -= Transformer_ItemAdded;
			_transformer.ItemProduced -= Transformer_ItemProduced;
		}

		void OnValidate()
		{
			_transformer ??= GetComponentInParent<Transformer>();
		}

		void Transformer_ItemProduced()
		{
			for (int i = 0; i < _transformer.Ruleset.Inputs.Length; i++)
			{
				ObjectDefinitionCountPair pair = _transformer.Ruleset.Inputs[i];
				_itemMonitors[pair.ItemDefinition].SetText(pair.Count.ToString());
			}
		}

		void Transformer_ItemAdded(ObjectDefinition arg1, int arg2)
		{
			_itemMonitors[arg1].SetText(arg2.ToString());
		}
	}
}
