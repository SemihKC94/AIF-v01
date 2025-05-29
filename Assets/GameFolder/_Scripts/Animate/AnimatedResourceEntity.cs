using SKC.AIF.Animate;
using SKC.AIF.Pool;
using SKC.AIF.Save;
using UnityEngine;

namespace SKC.AIF.Animate
{
	/// <summary>
	/// Spawned gameObject that will move on the screen. E.g. after selling an item, money icon appears on the screen
	/// that goes to the money UI.
	/// </summary>
	public class AnimatedResourceEntity : MonoBehaviour
	{
		IntValue _intVariable;
		AnimatedResourceEntityPool _animatedResourceEntityPool;
		ResourceTargetImage _resourceTargetImage;
		int _resourceAmount;

		public void Initialize(IntValue var, int resourceAmount, ResourceTargetImage targetImage, AnimatedResourceEntityPool entityPool, Vector3 startingPosition)
		{
			_intVariable = var;
			_resourceAmount = resourceAmount;
			_animatedResourceEntityPool = entityPool;
			_resourceTargetImage = targetImage;

			Transform trans = transform;
			trans.SetParent(targetImage.transform);
			trans.position = startingPosition;
			trans.localScale = Vector3.zero;
		}

		public void OnMoveSequenceEnded()
		{
			_intVariable.RuntimeValue += _resourceAmount;
			transform.localScale = Vector3.one;
			_animatedResourceEntityPool.PutBackToPool(this);
			_resourceTargetImage.PlayFeedback();
		}
	}
}
