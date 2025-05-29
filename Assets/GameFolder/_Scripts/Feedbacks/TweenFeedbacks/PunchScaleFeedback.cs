using SKC.AIF.Helpers;
using UnityEngine;

namespace SKC.AIF.Feedbacks
{
	[CreateAssetMenu(menuName = nameof(AIF) + "/" + nameof(Feedbacks) + "/" + nameof(PunchScaleFeedback))]
	public class PunchScaleFeedback : TweenFeedback
	{
		[SerializeField] Vector3 _targetScale;

		protected override void OnTweening(Transform trans)
		{
			TweenHelper.CompleteAll(trans);
			TweenHelper.PunchScale(trans, _targetScale, Duration);
		}
	}
}
