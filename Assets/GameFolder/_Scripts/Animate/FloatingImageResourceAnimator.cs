using SKC.AIF.Save;
using SKC.AIF.Helpers;
using UnityEngine;
using SKC.AIF.Pool;

namespace SKC.AIF.Animate
{
	/// <summary>
	/// Creates animated resource entities and moves them on the screen.
	/// </summary>
	[CreateAssetMenu(menuName = nameof(AIF) + "/" + nameof(Animate) + "/" + nameof(FloatingImageResourceAnimator))]
	public class FloatingImageResourceAnimator : ScriptableObject
	{
		[SerializeField, Tooltip("GameObject pool that will spawn on the screen when resource amount changed.")]
		AnimatedResourceEntityPool animatedResourceEntityPool;

		[SerializeField, Tooltip("Actual resource amount we have. This will be changed when resource modified.")]
		IntValue _resource;

		[SerializeField, Range(0f, 5f), Tooltip("Total duration for the animation effect in seconds.")]
		float _visualizationDuration;

		public ResourceTargetImage ResourceTargetImage { get; set; }

		public void Play(Vector3 screenSpacePosition, int resourceAmount)
		{
			AnimatedResourceEntity pooledImage = animatedResourceEntityPool.TakeFromPool();
			pooledImage.Initialize(_resource, resourceAmount, ResourceTargetImage, animatedResourceEntityPool, screenSpacePosition);
			Transform trans = pooledImage.transform;
			AIFHelper.MoveUI(trans, trans.position + (Vector3)Random.insideUnitCircle * 100f, _visualizationDuration, pooledImage.OnMoveSequenceEnded);
		}
	}
}
