using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Permissions;
using BepInEx;
using RoR2.ContentManagement;
using UnityEngine;
using RoR2;
using System.Linq;
using System.Security;
using BepInEx.Configuration;
using BepInEx.Bootstrap;
using System.Runtime.CompilerServices;
using R2API;
using UnityEngine.AddressableAssets;
using DS1Catacombs.Content;

[assembly: HG.Reflection.SearchableAttribute.OptIn]

namespace DS1Catacombs
{
    [BepInPlugin(GUID, Name, Version)]
    [BepInDependency(R2API.DirectorAPI.PluginGUID)]
    [BepInDependency(R2API.StageRegistration.PluginGUID)]
    [BepInDependency(R2API.SoundAPI.PluginGUID)]
    public class DS1CatacombsStage : BaseUnityPlugin
    {
        public const string Author = "Viliger";
        public const string Name = nameof(DS1CatacombsStage);
        public const string Version = "0.9.2";
        public const string GUID = Author + "." + Name;

        public static DS1CatacombsStage instance;
        public static PluginInfo PluginInfo;

        private void Awake()
        {
            instance = this;
            PluginInfo = Info;

            Log.Init(Logger);

            ContentManager.collectContentPackProviders += GiveToRoR2OurContentPackProviders;
            Language.collectLanguageRootFolders += CollectLanguageRootFolders;

            var musicFolderFullPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(typeof(ContentProvider).Assembly.Location), "Soundbanks");

            //DS1CatacombsContent.LoadSoundBank(musicFolderFullPath);

            //var customMusicData = new SoundAPI.Music.CustomMusicData();
            //customMusicData.BepInPlugin = Info.Metadata;
            //customMusicData.PlayMusicSystemEventName = "DS1_Play_Music_System";
            //customMusicData.BanksFolderPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(typeof(ContentProvider).Assembly.Location), "Soundbanks");
            //customMusicData.InitBankName = "DS1CatacombsInit.bnk";
            //customMusicData.SoundBankName = "DS1CatacombsMusic.bnk";

            //Log.Info(string.Format("BanksFolderPath {0}, InitBankName {1}, SoundBankName {2}", customMusicData.BanksFolderPath, customMusicData.InitBankName, customMusicData.SoundBankName));

            //customMusicData.SceneDefToTracks = new();

            //var mainCustomTrack = ScriptableObject.CreateInstance<SoundAPI.Music.CustomMusicTrackDef>();
            //mainCustomTrack.cachedName = "DS1CustomMusic";
            //mainCustomTrack.SoundBankName = customMusicData.SoundBankName;
            //mainCustomTrack.CustomStates = new List<SoundAPI.Music.CustomMusicTrackDef.CustomState>();

            //var cstate1 = new SoundAPI.Music.CustomMusicTrackDef.CustomState();
            //cstate1.GroupId = 487602916U; // gathered from the MOD's Init bank txt file
            //cstate1.StateId = 145640315U; // gathered from the MOD's Init bank txt file
            //mainCustomTrack.CustomStates.Add(cstate1);
            //var cstate2 = new SoundAPI.Music.CustomMusicTrackDef.CustomState();
            //cstate2.GroupId = 792781730U; // gathered from the GAME's Init bank txt file
            //cstate2.StateId = 89505537U; // gathered from the GAME's Init bank txt file
            //mainCustomTrack.CustomStates.Add(cstate2);

            //List<SoundAPI.Music.MainAndBossTracks> value = new List<SoundAPI.Music.MainAndBossTracks>
            //{
            //    new SoundAPI.Music.MainAndBossTracks(mainCustomTrack, null)
            //};

            //var scene = Addressables.LoadAssetAsync<SceneDef>("RoR2/Base/blackbeach/blackbeach.asset").WaitForCompletion();

            //customMusicData.SceneDefToTracks.Add(DS1CatacombsContent.DS1SceneDef, value);
            //customMusicData.SceneDefToTracks.Add(scene, value);

            //SoundAPI.Music.Add(customMusicData);
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
