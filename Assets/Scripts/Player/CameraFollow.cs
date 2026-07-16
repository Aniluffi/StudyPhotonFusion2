using Fusion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Cinemachine;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class CameraFollow : NetworkBehaviour
    {
        public override void Spawned()
        {
            if (HasInputAuthority)
            {
                var vCam = FindFirstObjectByType<CinemachineCamera>();

                if (vCam != null)
                {
                    vCam.Target.TrackingTarget = transform;
                }
            }
        }
    }
}
