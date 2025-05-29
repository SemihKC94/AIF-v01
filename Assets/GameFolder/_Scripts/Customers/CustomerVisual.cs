using SKC.AIF.Storage;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace SKC.AIF.Customers
{
    public class CustomerVisual : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _shopVisualize;
        [SerializeField] private TextMeshProUGUI _orderCountText;
        [SerializeField] private Image _orderImage;

        private int _orderCount;

        public void SetOrder(ObjectDefinition orderDefinition, int count)
        {
            _shopVisualize.alpha = 1.0f;
            _orderImage.sprite = orderDefinition.Sprite;
            _orderCount = count;
            _orderCountText.text = _orderCount.ToString();
        }

        public void UpdateVisual()
        {
            _orderCount--;
            _orderCountText.text = _orderCount.ToString();
            
            if(_orderCount <= 0) _shopVisualize.alpha = 0.0f;
        }
    }
}