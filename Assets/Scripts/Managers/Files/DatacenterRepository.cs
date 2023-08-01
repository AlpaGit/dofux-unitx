using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DofusCoube.FileProtocol.D2i;
using DofusCoube.FileProtocol.D2o;
using DofusCoube.FileProtocol.Datacenter.World;
using DofusCoube.FileProtocol.Ele;
using JetBrains.Annotations;
using Models;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Managers.Files
{
    public class DatacenterRepository : MonoBehaviour
    {
        private static DatacenterRepository _instance;

        public static DatacenterRepository Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<DatacenterRepository>();
                }

                return _instance;
            }
            set => _instance = value;
        }

        private static D2IReader _d2IReader;
        private static Dictionary<long, MapScrollAction> _mapScrollActions;
        private static Dictionary<long, MapPosition> _mapPositions;

        private static EleInstance _eleInstance;
    
        private static readonly string[] AssetsRequired = new[]
        {
            "Content/Data/MapScrollActions.asset",
            "Content/Data/MapPositions.asset",
            "Content/Data/EleInstance.asset",
        };
    
        public DatacenterRepository()
        {
            Debug.Log("DatacenterRepository constructor");
        }
    
        public void Awake()
        {
            hideFlags = HideFlags.DontSaveInBuild | HideFlags.DontSaveInEditor;
            _instance = this;
        }
    
        public async UniTask LoadAssets()
        {
            //Debug.Log("Loading i18n_fr");

            _mapPositions     ??= new Dictionary<long, MapPosition>();
            _mapScrollActions ??= new Dictionary<long, MapScrollAction>();

            await InternalLoadAssets();
        }

        private async UniTask InternalLoadAssets()
        {     
            await UniTask.SwitchToMainThread();

            var binaryObject = await Addressables.LoadAssetAsync<BinaryDataObject>("Content/Data/i18n_fr.asset");
            _d2IReader = new D2IReader(binaryObject.data);

            var tasks = new List<UniTask>();

            foreach (var asset in AssetsRequired)
            {
                tasks.Add(UniTask.RunOnThreadPool(async () =>
                {
                    await LoadAsset(asset);
                }));
            }
        
            await UniTask.WhenAll(tasks);
        }   
    
        private async UniTask LoadAsset( string asset)
        {
            await UniTask.SwitchToMainThread();
            var dataObject = await Addressables.LoadAssetAsync<BinaryDataObject>(asset);
            await UniTask.SwitchToThreadPool();
        
            switch (asset)
            {
                case "Content/Data/MapScrollActions.asset":
                    LoadMapScrollActions(dataObject);
                    break;
                case "Content/Data/MapPositions.asset":
                    LoadMapPositions(dataObject);
                    break;
                case "Content/Data/EleInstance.asset":
                    _eleInstance = new EleReader(dataObject.data).ReadElements();
                    break;
            }
        }
    
        private void LoadMapScrollActions(BinaryDataObject binaryDataObject)
        {
            var fileAccessor = GetGameDataFileAccessor();
            fileAccessor.Init("MapScrollActions", binaryDataObject.data);

            var mapScrollActions = fileAccessor.GetObjects<MapScrollAction>("MapScrollActions");

            if (mapScrollActions != null)
            {
                _mapScrollActions = mapScrollActions.ToDictionary(x => (long)x.Id);
            }
        }

        private void LoadMapPositions(BinaryDataObject binaryDataObject)
        {
            var fileAccessor = GetGameDataFileAccessor();
            fileAccessor.Init("MapPositions", binaryDataObject.data);

            var mapPositions = fileAccessor.GetObjects<MapPosition>("MapPositions");

            if (mapPositions != null)
            {
                _mapPositions = mapPositions.ToDictionary(x => (long)x.Id);
            }
        }

    
        private GameDataFileAccessor GetGameDataFileAccessor()
        {
            return new GameDataFileAccessor(_d2IReader);
        }

        [CanBeNull]
        public MapScrollAction GetMapScrollAction(long id)
        {
            return _mapScrollActions.TryGetValue(id, out var mapScrollAction) ? mapScrollAction : null;
        }

        [CanBeNull]
        public MapPosition GetMapPosition(long id)
        {
            return _mapPositions.TryGetValue(id, out var mapPosition) ? mapPosition : null;
        }

        [CanBeNull]
        public EleGraphicalData GetGraphicalData(int id)
        {
            return _eleInstance.GetGraphicalData(id);
        }

    }
}