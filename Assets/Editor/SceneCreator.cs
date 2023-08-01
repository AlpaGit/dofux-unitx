using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Components.Maps;
using Cysharp.Threading.Tasks;
using DofusCoube.FileProtocol.Dlm;
using Managers.Files;
using Managers.Maps;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace Editor
{
    public class SceneCreator : MonoBehaviour
    {
        private static List<long> _maps = new List<long>
        {
            224920576,
            67372032,
            69994498,
            83886090,
            83887114,
            103547392,
            103547394,
            103548416,
            103549440,
            122159617,
            122159619,
            122159621,
            122159623,
            122160641,
            122160643,
            122160645,
            122161665,
            122161667,
            122161669,
            188743681,
            188743682,
            188743683,
            188743684,
            188743685,
            188743686,
            188743687,
            188744193,
            188744194,
            188744195,
            188744196,
            188744197,
            188744198,
            188744199,
            188744705,
            188744706,
            188744710,
            188744711,
            188745217,
            188745218,
            188745222,
            188745223,
            188745729,
            188745730,
            188745734,
            188745735,
            188746241,
            188746242,
            188746246,
            188746247,
            188746753,
            188746754,
            188746755,
            188746756,
            188746757,
            188746758,
            188746759,
            191102976,
            191102978,
            191102980,
            191104000,
            191104002,
            191104004,
            191105024,
            191105026,
            191105028,
            191106048,
            191106050,
            191106052,
            191889409,
            191889411,
            191889413,
            191889415,
            191889665,
            191890433,
            191890435,
            191890437,
            191890439,
            191891457,
            192021505,
            192021507,
            192021509,
            192021511,
            192021761,
            192413696,
            192413698,
            192413700,
            192413702,
            192413704,
            192413706,
            192414720,
            192414722,
            192414724,
            192414726,
            192414728,
            192414730,
            192415744,
            192415746,
            192415748,
            192415750,
            192415752,
            192415754,
            192416768,
            192416770,
            192416772,
            192416776,
            192416778,
            192417792,
            192417798,
            192545798,
            192675840,
            192675842,
            192675844,
            192675846,
            192675848,
            192676864,
            192676866,
            192676868,
            192676870,
            192677888,
            192677890,
            192937984,
            192937988,
            192937990,
            192937994,
            192939008,
            192939010,
            192939012,
            192939014,
            192939016,
            192939018,
            192940032,
            192940034,
            192940036,
            192940038,
            192940040,
            196085770,
            196086794
        };
        
        private static Scene? _lastSceneCreated = null;

        [MenuItem("My Tools/Create Scenes of Maps 0")]
        static async void CreateScenes()
        {
            await CreateSceneOfMaps();
        }


        private static async UniTask CreateSceneOfMaps()
        {
            EditorSceneManager.OpenScene("Assets/Scenes/GameUI.unity");

            var directoryPath = @"C:\Users\Alpa\Documents\Maps\";
            Directory.CreateDirectory("Assets/Scenes/Maps/");
            
            var files = Directory.EnumerateFiles(directoryPath, "*.dlm", SearchOption.AllDirectories);
            Instantiate(Resources.Load<GameObject>("Systems"));
            
            await DatacenterRepository.Instance.LoadAssets();
            DatacenterRepository.Instance.hideFlags = HideFlags.DontSave;
            
            
            foreach (var file in files)
            {
                if (_lastSceneCreated != null)
                {
                    EditorSceneManager.CloseScene(_lastSceneCreated.Value, true);
                }
                
                var fileName = Path.GetFileNameWithoutExtension(file);
                var mapId    = long.Parse(fileName);
                if (!_maps.Contains(mapId))
                {
                    continue;
                }
                var dlmReader = new DlmReader(await File.ReadAllBytesAsync(file));
                var map       = dlmReader.ReadMap();
                await CreateMap(map);
                //EditorSceneManager.OpenScene("Assets/Scenes/GameUI.unity");
            }

            EditorUtility.ClearProgressBar();
        }

        private static async Task CreateMap(DlmMap dlmMap)
        {
            var assetName = dlmMap.Id.ToString();
            var folder    = dlmMap.Id % 10;
            var mapScene  = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Additive);
            _lastSceneCreated = mapScene;
            EditorUtility.DisplayProgressBar("Creating Scenes",
                $"Creating scene for map {assetName}", 1f);

            var localPath = $"Assets/Scenes/Maps/{folder}/{assetName}.unity";

            if (File.Exists(localPath))
            {
                //return;
            }

            Directory.CreateDirectory($"Assets/Scenes/Maps/{folder}");

            var grid = new GameObject
            {
                name = $"Map {assetName}",
            };

            var mapComponent = grid.AddComponent<MapComponent>();
            var mapGenerator = grid.AddComponent<MapGenerator>();
            await mapGenerator.Initialize(dlmMap);
            await mapGenerator.Render();
            mapComponent.mapInformation = mapGenerator.mapInformation;
            mapComponent.backgroundColor = new Color(dlmMap.BackgroundColor.R,
                dlmMap.BackgroundColor.G,
                dlmMap.BackgroundColor.B,
                dlmMap.BackgroundColor.A);

            DestroyImmediate(mapGenerator);
            //CreateNew(grid, localPath);

            EditorSceneManager.SaveScene(mapScene, localPath);

            var dt = FindObjectOfType<DatacenterRepository>();
            if (dt != null)
            {
                dt.gameObject.hideFlags = HideFlags.DontSave;
                DestroyImmediate(dt);
            }
        }

        static void CreateNew(GameObject obj, string localPath)
        {
            // Ensure the file name is unique, in case an existing Prefab has the same name.
            //localPath = AssetDatabase.GenerateUniqueAssetPath(localPath);

            // Create a new Prefab at the path given.
            //PrefabUtility.SaveAsPrefabAssetAndConnect(obj, localPath, InteractionMode.AutomatedAction);
        }
    }
}