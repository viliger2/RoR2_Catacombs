﻿using BepInEx;
using BepInEx.Configuration;
using RoR2;
using RoR2.ContentManagement;
using System.Collections.Generic;
using UnityEngine;

[assembly: HG.Reflection.SearchableAttribute.OptIn]

namespace DS1Catacombs
{
    [BepInPlugin("com.Viliger.DS1Catacombs", "DS1Catacombs", Version)]
    [BepInDependency(R2API.DirectorAPI.PluginGUID)]
    [BepInDependency(R2API.StageRegistration.PluginGUID)]
    public class DS1CatacombsPlugin : BaseUnityPlugin
    {
        public const string Author = "Viliger";
        public const string Name = nameof(DS1CatacombsPlugin);
        public const string Version = "0.9.4";
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

            On.RoR2.MusicController.Start += MusicController_Start;
            ContentManager.collectContentPackProviders += GiveToRoR2OurContentPackProviders;
            Language.collectLanguageRootFolders += CollectLanguageRootFolders;
        }

        private void MusicController_Start(On.RoR2.MusicController.orig_Start orig, MusicController self)
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
