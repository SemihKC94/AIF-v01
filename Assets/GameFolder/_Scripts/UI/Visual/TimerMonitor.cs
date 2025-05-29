using SKC.AIF.Helpers;
using UnityEngine;
using UnityEngine.UI;

namespace SKC.AIF.UI
{
	public class TimerMonitor : MonoBehaviour
	{
		[SerializeField] Timer _timer;
		[SerializeField] Image _image;

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
			_image.fillAmount = eventArgs.Value / eventArgs.MaxValue;
		}
	}
}
