using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SKC.AIF.Interfaces.Interactables;
using SKC.AIF.Data;

namespace SKC.AIF.Character
{
    public class CharacterMover : MonoBehaviour , IInteractable
    {
        [SerializeField] Rigidbody _rbd;
        [SerializeField] CharacterMoveData actorMovementData;
        [SerializeField] HumanoidAnimator _humanoidAnimationManager;
        [SerializeField] InputControls inputChannel;

        Vector3 _currentInputVector;
        private Vector3 lookTarget;

        bool IInteractable.Interactable => _currentInputVector.sqrMagnitude < 0.2f;

        void OnEnable()
        {
            inputChannel.JoystickUpdate += OnJoystickUpdate;
        }

        void OnDisable()
        {
            inputChannel.JoystickUpdate -= OnJoystickUpdate;
        }

        void FixedUpdate()
        {
            _rbd.velocity = new Vector3(_currentInputVector.x, _rbd.velocity.y, _currentInputVector.z);
        }

        void OnJoystickUpdate(Vector2 newMoveDirection)
        {
            if (newMoveDirection.magnitude >= 1f)
            {
                newMoveDirection.Normalize();
            }

            _currentInputVector = new Vector3(newMoveDirection.x * actorMovementData.SideMoveSpeed, 0f, newMoveDirection.y * actorMovementData.ForwardMoveSpeed);
            _humanoidAnimationManager.PlayMove(newMoveDirection);
            lookTarget = new Vector3(_currentInputVector.x, 0f, _currentInputVector.z);
            if (lookTarget != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(lookTarget, Vector3.up);
            }
        }

        public void LookTarget(Vector3 target)
        {
            lookTarget = new Vector3(target.x, 0f, target.z);
            transform.LookAt(lookTarget);
        }
    }
}
