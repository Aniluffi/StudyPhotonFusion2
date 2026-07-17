using Fusion;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Player
{
    public class PlayerInput : MonoBehaviour,INetworkInput
    {
        private InputSystem_Actions _inputActions;

        private NetworkEvents _networkEvents;

        private void Awake()
        {
            _inputActions = new InputSystem_Actions();
        }

        private void OnEnable()
        {
            _inputActions.Enable();

            _networkEvents = FindFirstObjectByType<NetworkEvents>();

            if(_networkEvents != null )
            {
                _networkEvents.OnInput.AddListener(OnInputCollected);
            }
        }

        private void OnDisable()
        {
            _inputActions.Disable();

            if(_networkEvents != null )
            {
                _networkEvents.OnInput.RemoveAllListeners();
            }
        }

        private void OnInputCollected(NetworkRunner runner, NetworkInput input)
        {
            var inputData = new NetworkInputData();

            // Читаем значение из Input Actions
            Vector2 moveVector = _inputActions.Player.Move.ReadValue<Vector2>();

            // ДОБАВЬ ЭТОТ ЛОГ:
            if (moveVector != Vector2.zero)
            {
                Debug.Log($"[PlayerInput] Кнопки нажимаются! Вектор: {moveVector}");
            }

            // Записываем вектор движения
            inputData.Direction = moveVector;

            // Передаем собранную структуру во Fusion
            input.Set(inputData);
        }
    }
}
