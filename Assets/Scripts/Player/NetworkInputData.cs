using Fusion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public struct NetworkInputData : INetworkInput
    {
        public Vector2 Direction { get; set; }
    }
}
