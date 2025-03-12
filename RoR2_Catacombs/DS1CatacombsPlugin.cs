using BepInEx;
using BepInEx.Configuration;
using R2API;
using RoR2;
using RoR2.ContentManagement;
using System.Collections.Generic;
using UnityEngine;

[assembly: HG.Reflection.SearchableAttribute.OptIn]

namespace DS1Catacombs
{
    [BepInPlugin("com.Viliger.DS1Catacombs", "DS1Catacombs", Version)]
    [BepInDependency(R2API.DirectorAPI.PluginGUID)]
    [BepInDependency(R2API.SoundAPI.PluginGUID)]
    [BepInDependency("JaceDaDorito.LocationsOfPrecipitation")]
    [BepInDependency("Viliger.RegisterCommandChest")]
    [BepInDependency("com.rob.Direseeker", BepInDependency.DependencyFlags.SoftDependency)]
    public class DS1CatacombsPlugin : BaseUnityPlugin
    {
        public const string Author = "Viliger";
        public const string Name = nameof(DS1CatacombsPlugin);
        public const string Version = "1.1.1";
        public const string GUID = Author + "." + Name;

        public static ConfigEntry<bool> EnableShitpostMusic;
        public static ConfigEntry<bool> AnyoneCanDestroyWalls;

        public static DS1CatacombsPlugin instance;
        public static PluginInfo PluginInfo;

        private void Awake()
        {

#if DEBUG == true
            On.RoR2.Networking.NetworkManagerSystemSteam.OnClientConnect += (s, u, t) => { };
#endif

            EnableShitpostMusic = Config.Bind("Catacombs", "Enable shitpost music", false, "Enables shitpost music.");
            AnyoneCanDestroyWalls = Config.Bind("Catacombs", "Anyone can destroy walls", false, "Wall destruction is no longer minor exclusive.");

            instance = this;
            PluginInfo = Info;

            Log.Init(Logger);

            On.RoR2.MusicController.StartIntroMusic += MusicController_StartIntroMusic;
            ContentManager.collectContentPackProviders += GiveToRoR2OurContentPackProviders;
            Language.collectLanguageRootFolders += CollectLanguageRootFolders;

            if(DireseekerCompat.enabled)
            {
                var directorCardHolder = new DirectorAPI.DirectorCardHolder
                {
                    Card = new DirectorCard
                    {
                        spawnCard = DireseekerCompat.GetDireseekerSpawnCard(),
                        selectionWeight = 3,
                        spawnDistance = DirectorCore.MonsterSpawnDistance.Standard,
                        preventOverhead = false,
                        minimumStageCompletions = 5
                    },
                    MonsterCategory = DirectorAPI.MonsterCategory.Champions
                };

                DirectorAPI.Helpers.AddNewMonsterToStage(
                    directorCardHolder,
                    false,
                    DirectorAPI.Stage.Custom,
                    "catacombs_DS1_Catacombs"
                );
            }
        }

        private void MusicController_StartIntroMusic(On.RoR2.MusicController.orig_StartIntroMusic orig, MusicController self)
        {
            orig(self);
            AkSoundEngine.PostEvent("DS1_Play_Music_System", self.gameObject);
        }

        private void Destroy()
        {
            Language.collectLanguageRootFolders -= CollectLanguageRootFolders;
        }
        private void GiveToRoR2OurContentPackProviders(ContentManager.AddContentPackProviderDelegate addContentPackProvider)
        {
            addContentPackProvider(new Content.ContentProvider());
        }
        public void CollectLanguageRootFolders(List<string> folders)
        {
            folders.Add(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(base.Info.Location), "Language"));
        }
    }
}
