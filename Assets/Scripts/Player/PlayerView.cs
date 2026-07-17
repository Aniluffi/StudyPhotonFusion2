using Unity.Cinemachine;
using UnityEngine;
using Fusion;

public class PlayerView : NetworkBehaviour
{
    public override void Spawned()
    {
        // Проверяем, принадлежит ли этот объект локальному игроку (есть ли у нас Input Authority)
        if (HasInputAuthority)
        {
            // Ищем виртуальную камеру на сцене по тегу или типу
            // Примечание: для старых версий Cinemachine тип будет CinemachineVirtualCamera
            var vCam = FindFirstObjectByType<CinemachineCamera>();

            if (vCam != null)
            {
                // Назначаем трансформ нашего игрока в качестве цели для следования и поворота
                vCam.Follow = transform;
                vCam.LookAt = transform;

                Debug.Log("[Camera] Виртуальная камера успешно привязана к локальному игроку.");
            }
            else
            {
                Debug.LogError("[Camera] Не найдена Cinemachine Camera на сцене! Проверь её наличие.");
            }
        }
    }
}
