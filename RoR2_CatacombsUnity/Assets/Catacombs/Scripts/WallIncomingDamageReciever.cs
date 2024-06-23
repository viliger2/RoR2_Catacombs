using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace DS1Catacombs
{
    public class WallIncomingDamageReciever : MonoBehaviour, IOnIncomingDamageServerReceiver
    {
        public void OnIncomingDamageServer(DamageInfo damageInfo)
        {
            Log.Debug(damageInfo.attacker);
        }
    }
}
