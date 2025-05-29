using SKC.AIF.Animate;
using SKC.AIF.Save;
using SKC.AIF.Storage;
using UnityEngine;

namespace SKC.AIF.Processors
{
	[CreateAssetMenu(menuName = nameof(AIF) + "/" + nameof(Processors) + "/" + nameof(Processors) + "/" + nameof(SellerFloatingTextDefinition))]
	public class SellerFloatingTextDefinition : ScriptableObject
	{
		[field: SerializeField] public IntValue IncomeResource { get; private set; }
		[field: SerializeField] public FloatingTextResourceAnimator FloatingTextResourceAnimator { get; private set; }
		[field: SerializeField] public ObjectDefinition[] SellableItemDefinitions { get; private set; }
		[field: SerializeField, Range(0f, 10f)] public float JumpHeight { get; private set; }
		[field: SerializeField, Range(0.01f, 5f)] public float JumpDuration { get; private set; }
	}
}
