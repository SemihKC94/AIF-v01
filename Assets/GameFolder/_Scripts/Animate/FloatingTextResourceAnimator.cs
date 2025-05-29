using UnityEngine;
using TMPro;
using DG.Tweening;
using SKC.AIF.Pool;

namespace SKC.AIF.Animate
{
	[CreateAssetMenu(menuName = nameof(AIF) + "/" + nameof(Animate) + "/" + nameof(FloatingTextResourceAnimator))]
	public class FloatingTextResourceAnimator : ScriptableObject
	{
		[field: SerializeField] public TextPool TextPool { get; private set; }
		[field: SerializeField] public Vector2 TextFloatingRandomRange { get; private set; }
		[field: SerializeField, Range(0f, 10f)] public float FloatingDuration { get; private set; }


		public void Play(Transform feedbackStartTransform, Transform cameraTransform, int increaseAmount)
		{
			TextMeshPro txt = TextPool.TakeFromPool();
			txt.text = increaseAmount.ToString();
			txt.transform.position = feedbackStartTransform.position + Vector3.up;
			txt.transform.LookAt(cameraTransform.position + cameraTransform.forward * 500f);
			float rndX = Random.Range(-TextFloatingRandomRange.x, TextFloatingRandomRange.x);
			txt.transform.DOMove(txt.transform.position + Vector3.up * TextFloatingRandomRange.y + feedbackStartTransform.right * rndX, FloatingDuration)
				.SetRecyclable()
				.OnComplete(() => TextPool.PutBackToPool(txt));
		}
	}
}
