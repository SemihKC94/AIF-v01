using DG.Tweening;
using SKC.AIF.Character;
using SKC.AIF.Interfaces.Interactables;
using UnityEngine;
using UnityEngine.AI;

namespace SKC.AIF.Worker
{
    public class WorkerMovement : MonoBehaviour, IInteractable
    {
        [SerializeField] NavMeshAgent _navMeshAgent;
        [SerializeField] HumanoidAnimator _humanoidAnimationManager;

        Vector3 _loadPoint;
        Vector3 _unloadPoint;
        float _loadTime;
        float _unloadTime;
        bool _moveOn = false;

        bool _isCurrentDestinationLoad;

        public bool Interactable => !enabled;

        void Update()
        {
            if (_navMeshAgent.remainingDistance < 0.1f)
            {
                _moveOn = false;
                if (_isCurrentDestinationLoad)
                {
                    DOVirtual.DelayedCall(_loadTime, Resume, false);
                }
                else
                {
                    DOVirtual.DelayedCall(_unloadTime, Resume, false);
                }
                _isCurrentDestinationLoad = !_isCurrentDestinationLoad;
                _humanoidAnimationManager.PlayMove(0f);
                enabled = false;

                return;
            }

            if (!_moveOn) _moveOn = true;
        }

        public void Initialize(Vector3 loadPoint, Vector3 unloadPoint, float loadTime, float unloadTime)
        {
            _loadPoint = loadPoint;
            _unloadPoint = unloadPoint;
            _loadTime = loadTime;
            _unloadTime = unloadTime;
            _isCurrentDestinationLoad = true;

            Resume();
        }

        public void Pause()
        {
            _navMeshAgent.ResetPath();
            _humanoidAnimationManager.PlayMove(0f);
            enabled = false;
        }

        public void Resume()
        {
            enabled = true;
            if (_isCurrentDestinationLoad)
            {
                _navMeshAgent.SetDestination(_loadPoint);
            }
            else
            {
                _navMeshAgent.SetDestination(_unloadPoint);
            }
            _humanoidAnimationManager.PlayMove(Vector2.one);
        }

        public void LookTarget(Vector3 target)
        {
            if (_moveOn) return;

            Vector3 lookTarget = new Vector3(target.x, 0f, target.z);
            transform.LookAt(lookTarget);
        }
    }
}
