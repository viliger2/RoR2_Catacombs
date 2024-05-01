using R2API;
using RoR2;
using RoR2.ContentManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;

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
            var akResult = AkSoundEngine.AddBasePath(soundbanksFolderPath);
            if (akResult == AKRESULT.AK_Success)
            {
                Log.Info($"Added bank base path : {soundbanksFolderPath}");
            }
            else
            {
                Log.Error(
                    $"Error adding base path : {soundbanksFolderPath} " +
                    $"Error code : {akResult}");
            }

            akResult = AkSoundEngine.LoadBank(InitSoundBankFileName, out var _);
            if (akResult == AKRESULT.AK_Success)
            {
                Log.Info($"Added bank : {InitSoundBankFileName}");
            }
            else
            {
                Log.Error(
                    $"Error loading bank : {InitSoundBankFileName} " +
                    $"Error code : {akResult}");
            }

            akResult = AkSoundEngine.LoadBank(SoundBankFileName, out var _);
            if (akResult == AKRESULT.AK_Success)
            {
                Log.Info($"Added bank : {SoundBankFileName}");
            }
            else
            {
                Log.Error(
                    $"Error loading bank : {SoundBankFileName} " +
                    $"Error code : {akResult}");
            }
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

            SetupMusic();

            StageRegistration.RegisterSceneDefToLoop(DS1SceneDef);

            Log.Debug(DS1SceneDef.destinationsGroup);
        }

        private static void SetupMusic()
        {
            DS1BazaarSeer = StageRegistration.MakeBazaarSeerMaterial(DS1ScenePreviewSprite.texture);
            DS1SceneDef.previewTexture = DS1ScenePreviewSprite.texture;
            DS1SceneDef.portalMaterial = DS1BazaarSeer;

            var mainCustomTrack = ScriptableObject.CreateInstance<SoundAPI.Music.CustomMusicTrackDef>();
            mainCustomTrack.cachedName = "DS1CustomMainMusic";
            mainCustomTrack.CustomStates = new List<SoundAPI.Music.CustomMusicTrackDef.CustomState>();

            var cstate1 = new SoundAPI.Music.CustomMusicTrackDef.CustomState();
            cstate1.GroupId = 487602916U; // gathered from the MOD's Init bank txt file
            if (DS1CatacombsPlugin.EnableShitpostMusic.Value)
            {
                cstate1.StateId = 145640315U; // Maxwell's theme
            }
            else
            {
                cstate1.StateId = 2254536284U; // AuroraBorealis
            }
            mainCustomTrack.CustomStates.Add(cstate1);
            var cstate2 = new SoundAPI.Music.CustomMusicTrackDef.CustomState();
            cstate2.GroupId = 792781730U; // gathered from the GAME's Init bank txt file
            cstate2.StateId = 89505537U; // gathered from the GAME's Init bank txt file
            mainCustomTrack.CustomStates.Add(cstate2);

            DS1SceneDef.mainTrack = mainCustomTrack;

            var bossCustomTrack = ScriptableObject.CreateInstance<SoundAPI.Music.CustomMusicTrackDef>();
            bossCustomTrack.cachedName = "DS1CustomBossMusic";
            bossCustomTrack.CustomStates = new List<SoundAPI.Music.CustomMusicTrackDef.CustomState>();

            var cstate11 = new SoundAPI.Music.CustomMusicTrackDef.CustomState();
            cstate11.GroupId = 487602916U; // gathered from the MOD's Init bank txt file
            if (DS1CatacombsPlugin.EnableShitpostMusic.Value)
            {
                cstate11.StateId = 3403129731U; // ARE YOU READY
            } else 
            { 
                cstate11.StateId = 3699353111U; // DiesIrae
            }

            bossCustomTrack.CustomStates.Add(cstate11);
            var cstate12 = new SoundAPI.Music.CustomMusicTrackDef.CustomState();
            cstate12.GroupId = 792781730U; // gathered from the GAME's Init bank txt file
            cstate12.StateId = 580146960U; // gathered from the GAME's Init bank txt file
            bossCustomTrack.CustomStates.Add(cstate12);

            DS1SceneDef.bossTrack = bossCustomTrack;
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
