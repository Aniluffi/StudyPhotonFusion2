using Assets.Scripts.Other;
using Fusion;
using Fusion.Sockets;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkRunnerHandler : MonoBehaviour, INetworkRunnerCallbacks
{
    [SerializeField] private NetworkPrefabRef playerPrefab; // Перетащи сюда префаб игрока в инспекторе!
    private NetworkRunner _runner;

    private void Start()
    {
        Debug.Log("[FUSION] Скрипт запустился. Начинаем старт игры...");
        StartGame(GameMode.Shared);
    }

    async void StartGame(GameMode mode)
    {
        try
        {
            // 1. Безопасно получаем или добавляем NetworkRunner
            _runner = GetComponent<NetworkRunner>();

            if (_runner == null)
            {
                _runner = gameObject.AddComponent<NetworkRunner>();
            }

            _runner.ProvideInput = true; // Разрешаем ввод на этом клиенте

            // 2. Безопасно получаем или добавляем SceneManager
            var sceneManager = GetComponent<NetworkSceneManagerDefault>();
            if (sceneManager == null)
            {
                sceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>();
            }

            // 3. Проверяем, добавлена ли сцена в Build Settings
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

            if (currentSceneIndex < 0)
            {
                Debug.LogError("[FUSION] КРИТИЧЕСКАЯ ОШИБКА: Текущая сцена не добавлена в Build Settings! Пожалуйста, добавь её.");
                return;
            }

            Debug.Log($"[FUSION] Попытка запуска сессии в режиме {mode} на сцене с индексом {currentSceneIndex}...");

            // 4. Запускаем сессию
            var result = await _runner.StartGame(new StartGameArgs()
            {
                GameMode = mode,
                SessionName = "TestRoom",
                Scene = SceneRef.FromIndex(currentSceneIndex),
                SceneManager = sceneManager
            });

            if (result.Ok)
            {
                Debug.Log("[FUSION] Сессия успешно запущена! Ждем подключения игрока...");
            }
            else
            {
                Debug.LogError($"[FUSION] Ошибка запуска сессии: {result.ShutdownReason}");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"[FUSION] Исключение при старте: {e.Message}\n{e.StackTrace}");
        }
    }

    // Вызывается автоматически, когда локальный клиент (или любой другой) заходит в комнату
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log($"[FUSION] Игрок {player} подключился. Локальный игрок: {runner.LocalPlayer}");

        if (player == runner.LocalPlayer)
        {
            Debug.Log("[FUSION] Это наш локальный игрок! Спавним персонажа...");

            if (playerPrefab == null)
            {
                Debug.LogError("[FUSION] Ошибка: Ссылка на playerPrefab пуста в инспекторе NetworkRunnerHandler!");
                return;
            }

            Vector3 spawnPosition = new Vector3(UnityEngine.Random.Range(-3f, 3f), 1f, UnityEngine.Random.Range(-3f, 3f));
            NetworkObject spawnedObj = runner.Spawn(playerPrefab, spawnPosition, Quaternion.identity, player);

            Debug.Log($"[FUSION] Персонаж успешно заспавнен: {spawnedObj.name}");
        }
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        var data = new NetworkInputData();

        // Считываем ввод через новый Input System
        Vector2 moveInput = Vector2.zero;
        if (UnityEngine.InputSystem.Keyboard.current != null)
        {
            var keyboard = UnityEngine.InputSystem.Keyboard.current;
            if (keyboard.wKey.isPressed || keyboard.upArrowKey.isPressed) moveInput.y += 1f;
            if (keyboard.sKey.isPressed || keyboard.downArrowKey.isPressed) moveInput.y -= 1f;
            if (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed) moveInput.x -= 1f;
            if (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed) moveInput.x += 1f;
        }

        data.Direction = moveInput;
        input.Set(data);
    }

    public void OnConnectedToServer(NetworkRunner runner)
    {
        
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
        
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
        
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
        
    }

    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {
        
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
        
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
        
    }

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        
    }

    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        
    }

    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {
        
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    {
        
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
        
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
        
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
        
    }
}
