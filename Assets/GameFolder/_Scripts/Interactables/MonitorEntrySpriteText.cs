using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SKC.AIF.UI
{
	public class MonitorEntrySpriteText : MonoBehaviour
	{
		[SerializeField] Image _image;
		[SerializeField] TextMeshProUGUI _text;

		public void Initialize(Sprite sprite, string startingValue)
		{
			_image.sprite = sprite;
			_text.text = startingValue;
		}

		public void SetText(string text)
		{
			_text.text = text;
		}
	}
}
