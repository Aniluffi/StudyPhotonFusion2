using Fusion;
using UnityEngine;

namespace Assets.Scripts.Player
{
    [RequireComponent(typeof(NetworkEvents))]
    [RequireComponent(typeof(NetworkRunner))]
    public class PlayerSpawner : MonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] private NetworkPrefabRef _playerPrefab; // Наш сетевой префаб Player

        [Header("Spawn Points")]
        [SerializeField] private Transform[] _spawnPoints; // Точки спавна на карте

        private NetworkEvents _networkEvents;

        private void Awake()
        {
            _networkEvents = GetComponent<NetworkEvents>();
        }

        private void OnEnable()
        {
            _networkEvents = FindFirstObjectByType<NetworkEvents>();
            if (_networkEvents != null)
            {
                // ИСПРАВЛЕНО: В Fusion 2 событие называется PlayerJoined (без "On")
                _networkEvents.PlayerJoined.AddListener(OnPlayerJoined);
            }
        }

        private void OnDisable()
        {
            if (_networkEvents != null)
            {
                // ИСПРАВЛЕНО: Тут тоже убираем "On"
                _networkEvents.PlayerJoined.RemoveListener(OnPlayerJoined);
            }
        }

        private void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
        {
            if (!runner.IsServer) return;

            Vector3 spawnPosition = Vector3.zero;
            Quaternion spawnRotation = Quaternion.identity;

            if (_spawnPoints != null && _spawnPoints.Length > 0)
            {
                // ИСПРАВЛЕНО: Вместо RawEncodedID используем свойство PlayerId
                int playerIndex = player.PlayerId;
                Transform selectedPoint = _spawnPoints[playerIndex % _spawnPoints.Length];

                spawnPosition = selectedPoint.position;
                spawnRotation = selectedPoint.rotation;
            }

            Debug.Log($"[Spawner] Спавним игрока {player.PlayerId} на позиции {spawnPosition}");

            runner.Spawn(_playerPrefab, spawnPosition, spawnRotation, player);
        }
    }
}
