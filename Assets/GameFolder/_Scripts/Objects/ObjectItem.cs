using SKC.AIF.Helpers;
using UnityEngine;

namespace SKC.AIF.Storage
{
    public class ObjectItem : MonoBehaviour
    {
        [SerializeField] ObjectDefinition _definition;

        Vector3 _defaultLocalScale;

        public ObjectDefinition Definition => _definition;

        void Awake()
        {
            _defaultLocalScale = transform.localScale;
        }

        public void ReleaseToPool()
        {
            transform.localScale = _defaultLocalScale;
            TweenHelper.KillAllTweens(transform);
            _definition.Pool.PutBackToPool(this);
        }

        public void JumpAndDisappear(Vector3 targetPoint, float jumpPower, float duration)
        {
            TweenHelper.LocalJumpAndRotate(transform, targetPoint, Vector3.zero, jumpPower, duration, DisappearSlowlyToPool);
        }

        void DisappearSlowlyToPool()
        {
            TweenHelper.DisappearSlowly(transform, 0.2f, ReleaseToPool);
        }
    }
}
