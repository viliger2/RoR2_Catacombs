using EntityStates;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace DS1Catacombs
{
    public class DestructibleWallDeath : BaseState
    {
        //public static GameObject worldCollider;

        public override void OnEnter()
        {
            base.OnEnter();
            var worldCollider = gameObject.transform.Find("WorldCollider");
            if (worldCollider)
            {
                worldCollider.GetComponent<Collider>().enabled = false;
            }
        }
    }
}
