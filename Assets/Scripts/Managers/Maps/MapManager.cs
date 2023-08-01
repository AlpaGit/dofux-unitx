using System.Threading.Tasks;
using Components.Maps;
using Cysharp.Threading.Tasks;
using Managers.Files;
using Models.Maps;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Managers.Maps
{
    public class MapManager : MonoBehaviour
    {
        public static MapManager Instance { get; private set; }
        public SceneInstance? CurrentMapScene { get; private set; }
        public Map CurrentMap { get; private set; }
        
        private AsyncOperationHandle<SceneInstance> _currentMapSceneHandle;

        public void Awake()
        {
            Instance = this;
        }


        // Start is called before the first frame update
        async void Start()
        {
            await DatacenterRepository.Instance.LoadAssets();
            await LoadMap(191105026);
        }

        private async UniTask LoadMap(long id)
        {        
            if (!_currentMapSceneHandle.IsDone)
            {
                return;
            }
            
            var mapPosition = DatacenterRepository.Instance.GetMapPosition(id);

            if (mapPosition == null)
            {
                return;
            }

            var folder = id % 10;


            var mapComponent = FindObjectOfType<MapComponent>();

            if (mapComponent != null)
            {
                Addressables.ReleaseInstance(mapComponent.gameObject);
                // Destroy(mapComponent.gameObject);
            }

            if (CurrentMapScene != null)
            {
                await Addressables.UnloadSceneAsync(CurrentMapScene.Value, true);
                Resources.UnloadUnusedAssets();      
                CurrentMapScene = null;
            }

            _currentMapSceneHandle =
                Addressables.LoadSceneAsync($"Assets/Scenes/Maps/{folder}/{id}.unity", LoadSceneMode.Additive);
            CurrentMapScene = await _currentMapSceneHandle;

            mapComponent = FindObjectOfType<MapComponent>();
            CurrentMap   = new Map(mapComponent.mapInformation, mapPosition);
            GridManager.Instance.UpdateCells(CurrentMap);
            // Instantiate(map);
        }
        // Update is called once per frame

        private async void Update()
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                await LoadNextMap(0);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                await LoadNextMap(1);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                await LoadNextMap(2);
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                await LoadNextMap(3);
            }
        }

        private async Task LoadNextMap(int direction)
        {
            var scrollAction = DatacenterRepository.Instance.GetMapScrollAction(CurrentMap.Id);
            var nextMapId    = CurrentMap.Id;

            switch (direction)
            {
                case 0:
                    nextMapId = CurrentMap.BasicInformation.topNeighbourId;

                    if (scrollAction is { TopExists: true })
                    {
                        nextMapId = (long)scrollAction.TopMapId;
                    }

                    break;
                case 1:
                    nextMapId = CurrentMap.BasicInformation.rightNeighbourId;

                    if (scrollAction is { RightExists: true })
                    {
                        nextMapId = (long)scrollAction.RightMapId;
                    }

                    break;
                case 2:
                    nextMapId = CurrentMap.BasicInformation.bottomNeighbourId;

                    if (scrollAction is { BottomExists: true })
                    {
                        nextMapId = (long)scrollAction.BottomMapId;
                    }

                    break;
                case 3:
                    nextMapId = CurrentMap.BasicInformation.leftNeighbourId;

                    if (scrollAction is { LeftExists: true })
                    {
                        nextMapId = (long)scrollAction.LeftMapId;
                    }

                    break;
            }

            if (nextMapId > 0)
            {
                await LoadMap(nextMapId);
            }
        }
    }
}