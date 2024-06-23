using RoR2;
using RoR2.Audio;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;

namespace DS1Catacombs
{
    public class EasterEggOnEnterManager : MonoBehaviour
    {
        public Transform chestSpawnLocation;

        public GameObject Vamos;

        private bool triggered = false;

        private void OnTriggerEnter(Collider other)
        {
            if(triggered)
            {
                return;
            }

            if(other.gameObject.TryGetComponent<HurtBox>(out var hurtBox))
            {
                triggered = true;

                VamosBegoneWithYou();
            }
        }

        private void VamosBegoneWithYou()
        {
            AkSoundEngine.PostEvent("DS1_Vamos_Begone", Vamos);
            Invoke("MakeVamosMove", 2f);
        }

        private void VamosFocus()
        {
            AkSoundEngine.PostEvent("DS1_Vamos_Focus", Vamos);
        }

        private void MakeVamosMove()
        {
            if (Vamos && Vamos.TryGetComponent<PathFollower>(out var pathFollower))
            {
                pathFollower.enabled = true;
            }
            Invoke("VamosFocus", 2f);
            Invoke("SpawnChest", 3f);
        }

        private void SpawnChest()
        {
            if(NetworkServer.active)
            {
                var goldChest = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/GoldChest/GoldChest.prefab").WaitForCompletion();
                goldChest.transform.localPosition = new Vector3(0.6709f, -1.0345f, 0f);
                goldChest.GetComponent<PurchaseInteraction>().automaticallyScaleCostWithDifficulty = true;
                NetworkServer.Spawn(UnityEngine.Object.Instantiate(goldChest, chestSpawnLocation));
            }
        }

    }
}
