using UnityEngine;
using TMPro;
using SKC.AIF.Save;

namespace SKC.AIF.UI
{
	public class IntVariableMonitor : MonoBehaviour
	{
		[SerializeField] IntValue _monitorVariable;
		[SerializeField] TextMeshProUGUI _monitorText;

		void OnEnable()
		{
			_monitorVariable.ValueSet += SetText;
		}

		void OnDisable()
		{
			_monitorVariable.ValueSet -= SetText;
		}

		void Start()
		{
			SetText(_monitorVariable.RuntimeValue);
		}

		void SetText(int obj)
		{
			_monitorText.text = obj.ToString();
		}
	}
}
