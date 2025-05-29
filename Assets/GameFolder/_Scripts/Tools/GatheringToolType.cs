using UnityEngine;

namespace SKC.AIF.Tools
{
	[CreateAssetMenu(menuName = nameof(AIF) + "/" + nameof(Tools) + "/" + nameof(GatheringToolType))]
	public class GatheringToolType : ScriptableObject
	{
		[field: SerializeField, Tooltip("Watering Can, Axe, Pickaxe, etc.")]
		public string TypeName { get; private set; }

		[SerializeField, Tooltip("This id will be used in animator controller to specify which animation to play.")]
		int _interactionAnimationId;

		public int InteractionAnimationId => _interactionAnimationId;
	}
}
