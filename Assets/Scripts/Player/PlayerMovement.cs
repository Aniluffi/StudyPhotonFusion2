using Fusion;
using System;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerMovement : NetworkBehaviour
    {
        private NetworkCharacterController _characterController;

        [SerializeField]
        private float _speed = 5f;

        

        [Networked]
        private float _currentSpeed { get; set; }

        private float _minSpeed = 0.1f;

        public Action<Vector3> OnMoveRotation;

        private void Awake()
        {
            _characterController = GetComponent<NetworkCharacterController>();
        }

        public override void FixedUpdateNetwork()
        {
            if (GetInput<NetworkInputData>(out var inputData))
            {
                var moveDirection = new Vector3(inputData.Direction.x, 0, inputData.Direction.y).normalized;

                _characterController.Move(moveDirection * _speed);

                if(moveDirection.magnitude > _minSpeed)
                {
                    OnMoveRotation?.Invoke(moveDirection);

                    _currentSpeed = _speed;
                }
                else
                {
                    _currentSpeed = 0;
                }

            }
            else
            {
                // Если в консоли бесконечно сыпется это — у персонажа нет Input Authority!
                Debug.LogWarning("[Movement] Ввод для этого персонажа НЕ получен!");
            }
        }

        public float GetSpeed() => _currentSpeed;
    }
}
