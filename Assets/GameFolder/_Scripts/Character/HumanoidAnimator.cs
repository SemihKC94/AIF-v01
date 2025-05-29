using UnityEngine;

namespace SKC.AIF.Character
{
    public class HumanoidAnimator : MonoBehaviour
    {
        [SerializeField] Animator _animator;
        [SerializeField] string _interactionName;
        [SerializeField] string _interactionSpeedName;
        [SerializeField] string _moveName;

        int _moveId;
        int _interactionId;
        int _interactionSpeedId;

        void Awake()
        {
            _moveId = Animator.StringToHash(_moveName);
            _interactionId = Animator.StringToHash(_interactionName);
            _interactionSpeedId = Animator.StringToHash(_interactionSpeedName);
        }

        public void PlayMove(Vector2 moveVector)
        {
            _animator.SetFloat(_moveId, moveVector.magnitude);
        }

        public void PlayMove(float moveMagnitude)
        {
            _animator.SetFloat(_moveId, moveMagnitude);
        }

        public void PlayInteraction(int type, float speed)
        {
            _animator.SetFloat(_interactionSpeedId, speed);
            _animator.SetInteger(_interactionId, type);
        }

        public void StopInteraction()
        {
            _animator.SetInteger(_interactionId, -1);
        }
    }
}
