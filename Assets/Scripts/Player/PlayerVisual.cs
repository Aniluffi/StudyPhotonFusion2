using Assets.Scripts.Player;
using Fusion;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerVisual : NetworkBehaviour
{
    private Animator _animator;

    [SerializeField]
    private PlayerMovement _playerMovement;

    private const string _speedFloat = "Speed";

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _playerMovement.OnMoveRotation += PlayerMovement_OnMoveRotation;
    }

    public override void FixedUpdateNetwork()
    {
        _animator.SetFloat(_speedFloat, _playerMovement.GetSpeed());
    }

    private void PlayerMovement_OnMoveRotation(Vector3 networkInputData)
    {
            // Рассчитываем нужное вращение
            Quaternion targetRotation = Quaternion.LookRotation(networkInputData);

            // ИСПРАВЛЕНИЕ: Меняем localRotation, чтобы вращался ТОЛЬКО этот дочерний объект,
            // а родитель с NetworkCharacterController оставался неподвижным.
            transform.localRotation = targetRotation;
    }

    private void OnDestroy()
    {
        if (_playerMovement != null)
        {
            _playerMovement.OnMoveRotation -= PlayerMovement_OnMoveRotation;
        }
    }
}
