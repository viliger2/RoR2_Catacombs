using RoR2;
using System;
using System.Collections.Generic;
using System.Text;

namespace DS1Catacombs
{
    public static class DireseekerCompat
    {
        private static bool? _enabled;

        public static bool enabled
        {
            get
            {
                if (_enabled == null)
                {
                    _enabled = BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.rob.Direseeker");
                }
                return (bool)_enabled;
            }
        }

        public static CharacterSpawnCard GetDireseekerSpawnCard()
        {
            return DireseekerMod.Modules.SpawnCards.bossSpawnCard;
        }
    }
}
