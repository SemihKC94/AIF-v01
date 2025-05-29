using SKC.AIF.Helpers;
using UnityEngine;
using UnityEngine.UI;

namespace SKC.AIF.UI
{
    public class AnvilMonitor : MonoBehaviour
    {
		[SerializeField] private Timer _timer;
		[SerializeField] private Animator _anvilAnimator;

		private float mValue = 0.0f;

		void OnEnable()
		{
			_timer.ValueChanged += Timer_ValueChanged;
		}

		void OnDisable()
		{
			_timer.ValueChanged -= Timer_ValueChanged;
		}

		void Timer_ValueChanged(Timer.EventArgs eventArgs)
		{
			mValue = eventArgs.Value / eventArgs.MaxValue;

			if(mValue == 0)
            {
				if (_anvilAnimator.GetBool("IsOn"))
					_anvilAnimator.SetBool("IsOn", false);

				return;
			}

			if (!_anvilAnimator.GetBool("IsOn"))
				_anvilAnimator.SetBool("IsOn", true);
		}
	}
}
