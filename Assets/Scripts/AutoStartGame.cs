using Fusion;
using Fusion.Photon.Realtime;
using Fusion.Sockets;
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class AutoStartGame : MonoBehaviour
    {
        private async void Start()
        {
            // Небольшая задержка для стабильной инициализации Unity
            await Task.Delay(1000);

            var runner = GetComponent<NetworkRunner>() ?? gameObject.AddComponent<NetworkRunner>();
            runner.ProvideInput = true;

            // Генерируем уникальный ID для каждого процесса на одном ПК
            var authValues = new AuthenticationValues();
            authValues.UserId = Guid.NewGuid().ToString();

            Debug.Log($"[Network] Запуск в режиме AutoHostOrClient. Unique ID: {authValues.UserId}");

            // Запуск сессии Fusion с автоматическим определением роли
            await runner.StartGame(new StartGameArgs
            {
                GameMode = GameMode.AutoHostOrClient, // КТО ПЕРВЫЙ ЗАШЕЛ — ТОТ И ХОСТ
                SessionName = "MyTestRoom",          // Имя комнаты одинаковое для всех
                AuthValues = authValues,
                Scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex),
                SceneManager = gameObject.GetComponent<NetworkSceneManagerDefault>() ?? gameObject.AddComponent<NetworkSceneManagerDefault>()
            });
        }
    }
}
