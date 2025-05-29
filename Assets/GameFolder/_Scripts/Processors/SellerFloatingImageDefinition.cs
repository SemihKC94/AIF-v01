using UnityEngine;
using SKC.AIF.Storage;
using SKC.AIF.Animate;

namespace SKC.AIF.Processors
{
	[CreateAssetMenu(menuName = nameof(AIF) + "/" + nameof(Processors) + "/" + nameof(Processors) + "/" + nameof(SellerFloatingImageDefinition))]
	public class SellerFloatingImageDefinition : ScriptableObject
	{
		[field: SerializeField] public FloatingImageResourceAnimator FloatingImageResourceAnimator { get; private set; }
		[field: SerializeField] public ObjectDefinition[] SellableItemDefinitions { get; private set; }
		[field: SerializeField, Range(0f, 10f)] public float JumpHeight { get; private set; }
		[field: SerializeField, Range(0.01f, 5f)] public float JumpDuration { get; private set; }
	}
}
