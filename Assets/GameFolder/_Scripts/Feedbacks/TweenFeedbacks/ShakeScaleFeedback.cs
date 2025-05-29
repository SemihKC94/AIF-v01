using SKC.AIF.Helpers;
using UnityEngine;

namespace SKC.AIF.Feedbacks
{
	[CreateAssetMenu(menuName = nameof(AIF) + "/" + nameof(Feedbacks) + "/" + nameof(ShakeScaleFeedback))]
	public class ShakeScaleFeedback : TweenFeedback
	{
		[SerializeField] Vector3 _targetScale;

		protected override void OnTweening(Transform trans)
		{
			TweenHelper.ShakeScale(trans, _targetScale, Duration);
		}
	}
}
