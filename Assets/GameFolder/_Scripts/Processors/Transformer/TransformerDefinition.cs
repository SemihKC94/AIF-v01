using UnityEngine;

namespace SKC.AIF.Processors
{
	[CreateAssetMenu(menuName = nameof(AIF) + "/" + nameof(Processors) + "/" + ("Transformers") + "/" + nameof(TransformerDefinition))]
	public class TransformerDefinition : ScriptableObject
	{
		[field: SerializeField] public TransformerRuleset Ruleset { get; private set; }
		[field: SerializeField, Range(0.01f, 2f)] public float JumpHeight { get; private set; }
		[field: SerializeField, Range(0.01f, 2f)] public float JumpDuration { get; private set; }
	}
}
