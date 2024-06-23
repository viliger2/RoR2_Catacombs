using EntityStates;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.Networking;

namespace DS1Catacombs
{
    public class DestructibleWallMainState : EntityStates.Idle
    {
        public override void OnEnter()
        {
            base.OnEnter();
            if (NetworkServer.active)
            {
                characterBody.AddBuff(RoR2.RoR2Content.Buffs.HiddenInvincibility);
            }
        }
    }
}
