using UnityEngine.Events;
using UnityEngine;

namespace SKC.AIF.Helpers
{
    public class AnimationEventTrigger : MonoBehaviour
    {
        public UnityEvent AnimationEvent;

        public void InvokeEvent()
        {
            AnimationEvent.Invoke();
        }
    }
}
