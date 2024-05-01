using System;
using System.Collections; 
using System.Linq;
using RoR2;
using RoR2.ContentManagement;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using UnityEngine.ResourceManagement.AsyncOperations;
using RoR2.ExpansionManagement;
using System.Collections.Generic;
using RoR2.Networking;
using R2API;
using UnityEngine.ResourceManagement.ResourceProviders;

namespace DS1Catacombs.Content
{
    public static class DS1CatacombsContent
    {
        internal const string ScenesAssetBundleFileName = "catacombsstage";
        internal const string AssetsAssetBundleFileName = "catacombsassets";
        internal const string SoundBankFileName = "DS1CatacombsMusic.bnk";
        internal const string InitSoundBankFileName = "DS1CatacombsInit.bnk";

        private static AssetBundle _scenesAssetBundle;
        private static AssetBundle _assetsAssetBundle;

        internal static MusicTrackDef MusicTrack;

        internal static UnlockableDef[] UnlockableDefs;
        internal static SceneDef[] SceneDefs;

        public static SceneDef DS1SceneDef;
        internal static Sprite DS1ScenePreviewSprite;
        internal static Material DS1BazaarSeer;

        public static List<Material> SwappedMaterials = new List<Material>(); //debug

        public static Dictionary<string, string> ShaderLookup = new Dictionary<string, string>()
        {
            {"stubbedror2/base/shaders/hgstandard", "RoR2/Base/Shaders/HGStandard.shader"},
            {"stubbedror2/base/shaders/hgsnowtopped", "RoR2/Base/Shaders/HGSnowTopped.shader"},
            {"stubbedror2/base/shaders/hgtriplanarterrainblend", "RoR2/Base/Shaders/HGTriplanarTerrainBlend.shader"},
            {"stubbedror2/base/shaders/hgintersectioncloudremap", "RoR2/Base/Shaders/HGIntersectionCloudRemap.shader" },
            {"stubbedror2/base/shaders/hgcloudremap", "RoR2/Base/Shaders/HGCloudRemap.shader" },
            {"stubbedror2/base/shaders/hgdistortion", "RoR2/Base/Shaders/HGDistortion.shader" },
            {"stubbedcalm water/calmwater - dx11 - doublesided", "Calm Water/CalmWater - DX11 - DoubleSided.shader" },
            {"stubbedcalm water/calmwater - dx11", "Calm Water/CalmWater - DX11.shader" },
            {"stubbednature/speedtree", "RoR2/Base/Shaders/SpeedTreeCustom.shader"}
        };

        internal static void LoadSoundBank(string soundbanksFolderPath)
        {
            var customMusicData = new SoundAPI.Music.CustomMusicData();
            customMusicData.BepInPlugin = DS1CatacombsStage.PluginInfo.Metadata;
            customMusicData.PlayMusicSystemEventName = "DS1_Play_Music_System";
            customMusicData.BanksFolderPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(typeof(ContentProvider).Assembly.Location), "Soundbanks");
            customMusicData.InitBankName = "DS1CatacombsInit.bnk";
            customMusicData.SoundBankName = "DS1CatacombsMusic.bnk";

            Log.Info(string.Format("BanksFolderPath {0}, InitBankName {1}, SoundBankName {2}", customMusicData.BanksFolderPath, customMusicData.InitBankName, customMusicData.SoundBankName));

            customMusicData.SceneDefToTracks = new Dictionary<SceneDef, IEnumerable<SoundAPI.Music.MainAndBossTracks>>();

            var mainCustomTrack = ScriptableObject.CreateInstance<SoundAPI.Music.CustomMusicTrackDef>();
            mainCustomTrack.cachedName = "DS1CustomMusic";
            mainCustomTrack.SoundBankName = customMusicData.SoundBankName;
            mainCustomTrack.CustomStates = new List<SoundAPI.Music.CustomMusicTrackDef.CustomState>();

            var cstate1 = new SoundAPI.Music.CustomMusicTrackDef.CustomState();
            cstate1.GroupId = 487602916U; // gathered from the MOD's Init bank txt file
            cstate1.StateId = 145640315U; // gathered from the MOD's Init bank txt file
            mainCustomTrack.CustomStates.Add(cstate1);
            var cstate2 = new SoundAPI.Music.CustomMusicTrackDef.CustomState();
            cstate2.GroupId = 792781730U; // gathered from the GAME's Init bank txt file
            cstate2.StateId = 89505537U; // gathered from the GAME's Init bank txt file
            mainCustomTrack.CustomStates.Add(cstate2);

            List<SoundAPI.Music.MainAndBossTracks> value = new List<SoundAPI.Music.MainAndBossTracks>
            {
                new SoundAPI.Music.MainAndBossTracks(mainCustomTrack, null)
            };

            var scene = Addressables.LoadAssetAsync<SceneDef>("RoR2/Base/blackbeach/blackbeach.asset").WaitForCompletion();

            customMusicData.SceneDefToTracks.Add(scene, value);
            //customMusicData.SceneDefToTracks.Add(DS1CatacombsContent.DS1SceneDef, value);

            SoundAPI.Music.Add(customMusicData);


            //var akResult = AkSoundEngine.AddBasePath(soundbanksFolderPath);
            //if (akResult == AKRESULT.AK_Success)
            //{
            //    Log.Info($"Added bank base path : {soundbanksFolderPath}");
            //}
            //else
            //{
            //    Log.Error(
            //        $"Error adding base path : {soundbanksFolderPath} " +
            //        $"Error code : {akResult}");
            //}

            //akResult = AkSoundEngine.LoadBank(InitSoundBankFileName, out var _);
            //if (akResult == AKRESULT.AK_Success)
            //{
            //    Log.Info($"Added bank : {InitSoundBankFileName}");
            //}
            //else
            //{
            //    Log.Error(
            //        $"Error loading bank : {InitSoundBankFileName} " +
            //        $"Error code : {akResult}");
            //}      

            //akResult = AkSoundEngine.LoadBank(SoundBankFileName, out var _);
            //if (akResult == AKRESULT.AK_Success)
            //{
            //    Log.Info($"Added bank : {SoundBankFileName}");
            //}
            //else
            //{
            //    Log.Error(
            //        $"Error loading bank : {SoundBankFileName} " +
            //        $"Error code : {akResult}");
            //}       
        }

        internal static IEnumerator LoadAssetBundlesAsync(AssetBundle scenesAssetBundle, AssetBundle assetsAssetBundle, IProgress<float> progress, ContentPack contentPack, string musicFolderFullPath)
        {
            _scenesAssetBundle = scenesAssetBundle;
            _assetsAssetBundle = assetsAssetBundle;
            Log.Debug($"Asset bundles found. Loading asset bundles...");

            yield return LoadAllAssetsAsync(_assetsAssetBundle, progress, (Action<Material[]>)((assets) =>
            {
                var materials = assets;

                if (materials != null)
                {
                    foreach (Material material in materials)
                    {
                        if (!material.shader.name.StartsWith("Stubbed")) { continue; }

                        var replacementShader = Addressables.LoadAssetAsync<Shader>(ShaderLookup[material.shader.name.ToLower()]).WaitForCompletion();
                        if (replacementShader)
                        {
                            material.shader = replacementShader;
                            SwappedMaterials.Add(material);
                        }
                    }
                }
                
            }));

            yield return LoadAllAssetsAsync(_assetsAssetBundle, progress, (Action<UnlockableDef[]>)((assets) =>
            {
                UnlockableDefs = assets;
                contentPack.unlockableDefs.Add(assets);
            }));

             yield return LoadAllAssetsAsync(_assetsAssetBundle, progress, (Action<Sprite[]>)((assets) =>
             {
                 DS1ScenePreviewSprite = assets.First(a => a.name == "texCatacombsPreview");
             }));

            yield return LoadAllAssetsAsync(_assetsAssetBundle, progress, (Action<SceneDef[]>)((assets) =>
            {
                SceneDefs = assets;
                DS1SceneDef = SceneDefs.First(sd => sd.cachedName == "catacombs_DS1_Catacombs");
                Log.Debug(DS1SceneDef.nameToken);
                contentPack.sceneDefs.Add(assets);
            }));

            yield return LoadAllAssetsAsync(_assetsAssetBundle, progress, (Action <MusicTrackDef[]>)((assets) => {
                MusicTrack = assets.First(mt => mt.cachedName == "MainTrackDef");
            }));

            DS1BazaarSeer = StageRegistration.MakeBazaarSeerMaterial(DS1ScenePreviewSprite.texture);
            DS1SceneDef.previewTexture = DS1ScenePreviewSprite.texture;
            DS1SceneDef.portalMaterial = DS1BazaarSeer;

            //var dioramaPrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/golemplains/GolemplainsDioramaDisplay.prefab");
            //while (!dioramaPrefab.IsDone)
            //{
            //    yield return null;
            //}
            //DS1SceneDef.dioramaPrefab = dioramaPrefab.Result;

            var mainTrackDefRequest = Addressables.LoadAssetAsync<MusicTrackDef>("RoR2/Base/Common/muSong13.asset");
            while (!mainTrackDefRequest.IsDone)
            {
                yield return null;
            }
            var bossTrackDefRequest = Addressables.LoadAssetAsync<MusicTrackDef>("RoR2/Base/Common/muSong05.asset");
            while (!bossTrackDefRequest.IsDone)
            {
                yield return null;
            }

            DS1SceneDef.mainTrack = mainTrackDefRequest.Result;
            DS1SceneDef.bossTrack = bossTrackDefRequest.Result;

            StageRegistration.RegisterSceneDefToLoop(DS1SceneDef);

            //DS1CatacombsContent.LoadSoundBank(musicFolderFullPath);

            Log.Debug(DS1SceneDef.destinationsGroup);
        }

        private static IEnumerator LoadAllAssetsAsync<T>(AssetBundle assetBundle, IProgress<float> progress, Action<T[]> onAssetsLoaded) where T : UnityEngine.Object
        {
            var sceneDefsRequest = assetBundle.LoadAllAssetsAsync<T>();
            while (!sceneDefsRequest.isDone)
            {
                progress.Report(sceneDefsRequest.progress);
                yield return null;
            }

            onAssetsLoaded(sceneDefsRequest.allAssets.Cast<T>().ToArray());

            yield break;
        } 
    }
}
