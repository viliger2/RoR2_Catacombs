using EntityStates;
using RoR2.Audio;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace DS1Catacombs
{
    public class DestructibleWallDeath : BaseState
    {
        public override void OnEnter()
        {
            base.OnEnter();
            var worldCollider = gameObject.transform.Find("Model/WorldCollider");
            if (worldCollider)
            {
                GameObject fracturedWall = UnityEngine.Object.Instantiate(DS1Catacombs.Content.DS1CatacombsContent.FracturedWall, transform.position, transform.rotation);
                foreach(Rigidbody rb in fracturedWall.GetComponentsInChildren<Rigidbody>())
                {
                    Vector3 force = (rb.transform.position - transform.position).normalized * 10;
                    rb.AddForce(force);
                }
                AkSoundEngine.PostEvent("DS1_Wall_Destroy", fracturedWall);

                EntityState.Destroy(gameObject);
            } else
            {
                Log.Debug("couldn't find WorldCollider");
            }
        }
    }
}
