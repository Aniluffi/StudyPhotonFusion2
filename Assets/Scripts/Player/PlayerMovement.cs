using Assets.Scripts.Other;
using Fusion;
using Unity.Cinemachine;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    private CharacterController _characterController;

    [SerializeField]
    private float _speed = 5f;

    [SerializeField] 
    private CinemachineCamera _cameraFollowTarget; // Сюда в инспекторе перетащи объект персонажа

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    public override void FixedUpdateNetwork()
    {
        if (GetInput<NetworkInputData>(out var data))
        {
            Vector3 moveDirection = new Vector3(data.Direction.x, 0, data.Direction.y).normalized;
            _characterController.Move(moveDirection * _speed * Runner.DeltaTime);
        }
    }

    
}
