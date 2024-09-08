using BepInEx;
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
    [BepInDependency("com.Viliger.SM64BBF")]
    //[BepInDependency(R2API.SoundAPI.PluginGUID)]
    public class DS1CatacombsPlugin : BaseUnityPlugin
    {
        public const string Author = "Viliger";
        public const string Name = nameof(DS1CatacombsPlugin);
        public const string Version = "0.9.3";
        public const string GUID = Author + "." + Name;

        public static ConfigEntry<bool> EnableShitpostMusic;

        public static DS1CatacombsPlugin instance;
        public static PluginInfo PluginInfo;

        private void Awake()
        {
            EnableShitpostMusic = Config.Bind("Catacombs", "Enable shitpost music", false, "Enables shitpost music.");

            instance = this;
            PluginInfo = Info;

            Log.Init(Logger);

            On.RoR2.MusicController.StartIntroMusic += MusicController_StartIntroMusic;
            ContentManager.collectContentPackProviders += GiveToRoR2OurContentPackProviders;
            Language.collectLanguageRootFolders += CollectLanguageRootFolders;
        }

        private void MusicController_StartIntroMusic(On.RoR2.MusicController.orig_StartIntroMusic orig, MusicController self)
        {
            orig(self);
            // AkSoundEngine.PostEvent("DS1_Play_Music_System", self.gameObject);
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
