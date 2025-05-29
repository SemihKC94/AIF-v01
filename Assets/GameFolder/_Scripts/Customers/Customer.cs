using DG.Tweening;
using SKC.AIF.Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace SKC.AIF.Customers
{
    public class Customer : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _spawnParticles;
        [SerializeField] NavMeshAgent _navMeshAgent;
        [SerializeField] HumanoidAnimator _humanoidAnimationManager;

        private CustomerPath[] _myPath;
        private CustomerBase _customerBase;
        private CustomerVisual _customerVisual;

        Vector3 _loadPoint;
        Vector3 _unloadPoint;
        int _currentPathIndex;
        int _maxPathIndex;
        bool _moveOn = false;
        bool _isInitialized = false;

        bool _isCurrentDestinationLoad;

        public bool Interactable => !enabled;

        void Update()
        {
            if(!_isInitialized) return;
            
            if (_navMeshAgent.remainingDistance < 0.1f)
            {
                _moveOn = false;
                _currentPathIndex++;
                if (_currentPathIndex >= _maxPathIndex) _currentPathIndex = _maxPathIndex;

                if (_currentPathIndex != _maxPathIndex)
                {
                    DOVirtual.DelayedCall(_myPath[_currentPathIndex].loadTime, Resume, false);
                }
                else
                {
                    DOVirtual.DelayedCall(_myPath[_currentPathIndex].unloadTime, ExitPoint, false);
                }
                _isCurrentDestinationLoad = !_isCurrentDestinationLoad;
                _humanoidAnimationManager.PlayMove(0f);
                enabled = false;

                return;
            }

            if (!_moveOn) _moveOn = true;
        }

        public void Initialize(CustomerPath[] customerPaths, CustomerBase customerBase)
        {
            _myPath = customerPaths;
            _maxPathIndex = _myPath.Length;
            _currentPathIndex = 0;
            _isCurrentDestinationLoad = true;
            _isInitialized = true;
            _spawnParticles.Play();
            _customerBase =  customerBase;

            enabled = true;
            _navMeshAgent.SetDestination(_myPath[_currentPathIndex]._loadPoint);
        }

        private void SetCustomerOrderVisual()
        {
            _customerVisual.SetOrder(_customerBase.orderInformation._orderDefinition,_customerBase.orderInformation.Amount);
        }

        public void UpdateVisual()
        {
            _customerVisual.UpdateVisual();
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
            _navMeshAgent.SetDestination(_myPath[_currentPathIndex]._loadPoint);
            _humanoidAnimationManager.PlayMove(Vector2.one);
        }

        public void ExitPoint()
        {
            GameObject.FindObjectOfType<CustomerSpawner>().DeInitializeCustomer(this);
            _navMeshAgent.SetDestination(_myPath[_currentPathIndex]._loadPoint);
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
