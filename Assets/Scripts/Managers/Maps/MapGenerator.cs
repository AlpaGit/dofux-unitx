using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Components.Maps;
using Cysharp.Threading.Tasks;
using DofusCoube.FileProtocol.Dlm;
using DofusCoube.FileProtocol.Ele.Datas;
using Managers.Files;
using Managers.Scene;
using Models.Maps;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Managers.Maps
{
    public class MapGenerator : MonoBehaviour
    {
        public static MapGenerator Current { get; private set; }

        private readonly HashSet<TileSpriteModel> _assetsToLoad = new();
        private readonly HashSet<FixtureSpriteModel> _fixtureToLoad = new();

        private readonly Dictionary<string, Texture2D> _tilesToLoad = new();

        private readonly Dictionary<string, Sprite> _sprites = new();
        private readonly List<GameObject> _tiles = new();
        private static Shader _colorMatrixShader;

        [SerializeField] public MapBasicInformation mapInformation = new();
        private static readonly int Color1 = Shader.PropertyToID("_Color");

        public void Start()
        {
            Current = this;
        }

        public async UniTask Initialize(DlmMap map)
        {
            mapInformation.id                = map.Id;
            mapInformation.leftNeighbourId   = map.LeftNeighbourId;
            mapInformation.rightNeighbourId  = map.RightNeighbourId;
            mapInformation.topNeighbourId    = map.TopNeighbourId;
            mapInformation.bottomNeighbourId = map.BottomNeighbourId;

            _colorMatrixShader = Shader.Find("Custom/ColorMatrixShader");

            foreach (var layer in map.Layers)
            {
                foreach (var cell in layer.Cells)
                {
                    PreloadTile(cell, cell.Id, layer.LayerId);
                }
            }

            foreach (var background in map.BackgroudFixtures)
            {
                PreloadFixture(background, -1);
            }          
            
            foreach (var background in map.ForegroundFixtures)
            {
                PreloadFixture(background, short.MaxValue);
            }


            foreach (var cell in map.Cells)
            {
                mapInformation.cells.Add((ushort)cell.Id, (ushort)cell.Data);
            }

            if (_assetsToLoad.Count == 0 && _fixtureToLoad.Count == 0)
            {
                return;
            }

            var keys = _assetsToLoad.Select(x => x.Key);
            keys = keys.Concat(_fixtureToLoad.Select(x => x.Key));
            
            IEnumerable distinctKeys = keys.ToArray();
            
            var tiles = await Addressables.LoadAssetsAsync<Texture2D>(distinctKeys, (e) =>
                {
                    Debug.Log($"Loaded {e.name}");
                },
                Addressables.MergeMode.Union);
            
            Debug.Log($"Loaded {tiles.Count()} tiles");

            foreach (var tile in tiles)
            {
                if (tile.IsDestroyed())
                {
                    Debug.LogError("Tile is destroyed");
                    return;
                }

                _tilesToLoad.Add(tile.name, tile);
            }
        }

        private void PreloadTile(DlmCell cell, short cellId, int layerId)
        {
            var order      = cellId - (-layerId * 1000);
            var pixelPoint = SceneConverter.GetPixelCoordByCellId(cell.Id)!;

            foreach (var element in cell.Elements)
            {
                order++;

                if (element is not DlmGraphicalElement graphElem)
                {
                    continue;
                }

                if (graphElem.Identifier != 0)
                {
                    mapInformation.identifiedElements.Add(graphElem.Identifier, graphElem.ElementId);
                }

                var graphicalElement = DatacenterRepository.Instance.GetGraphicalData((int)graphElem.ElementId);

                if (graphicalElement is not NormalGraphicalElementData eleGraphicalData)
                {
                    continue;
                }
                
                var id = eleGraphicalData.Gfx;

                if (id == 44652)
                {
                    
                }
                
                graphElem.CalculateFinalTeint();

                var realPositionX = pixelPoint.X + graphElem.PixelOffset.X - eleGraphicalData.Origin.X;
                var realPositionY = pixelPoint.Y - graphElem.PixelOffset.Y + eleGraphicalData.Origin.Y + (graphElem.Altitude * 10);

                realPositionX += 43f;

                realPositionX /= 100f;
                realPositionY /= 100f;

                


                var hexValue = id.ToString("00")[..2];

                _assetsToLoad.Add(new TileSpriteModel()
                {
                    Id                 = eleGraphicalData.Gfx.ToString(),
                    Key                = $"Assets/Tiles/{hexValue}/{id}.png",
                    X                  = realPositionX,
                    Y                  = realPositionY,
                    ShouldFlipX        = eleGraphicalData.HorizontalSymmetry,
                    Order              = order,
                    ColorMultiplicator = graphElem.ColorMultiplicator
                });
            }
        }

        private void PreloadFixture(DlmFixture fixture, int layer)
        {      
            var order      = layer;
            var id         = fixture.FixtureId;
            var pixelPoint = SceneConverter.GetPixelCoordByCellId(0)!;

            if (id == 59731)
            {
                
            }
            
            //const float halfWidth  = MapConstants.Width * 86f * 0.5f;
            //const float halfHeight = MapConstants.Height * (43f / 2f) * 0.5f;

            var realPositionX = pixelPoint.X + fixture.Offset.X - (86f * 0);
            var realPositionY = pixelPoint.Y - fixture.Offset.Y;
 
            realPositionX += 43f;

            // realPositionX /= 100f;
            // realPositionY /= 100f;
            
            var hexValue = id.ToString("00")[..2];

            var r = fixture.RedMultiplier / 127f;
            var e = r + 1f;
            
            var fixtureSprite = new FixtureSpriteModel
            {
                Id       = id.ToString(),
                Key      = $"Assets/Tiles/{hexValue}/{id}.png",
                X        = realPositionX,
                Y        = realPositionY,
                ScaleX   = fixture.ScaleX / 1000f,
                ScaleY   = fixture.ScaleY / 1000f,
                Rotation = fixture.Rotation,
                Order    = order,
                Red      = (fixture.RedMultiplier / 127f) + 1,
                Green    = fixture.GreenMultiplier / 127f +1,
                Blue     = fixture.BlueMultiplier / 127f +1,
                Alpha    = fixture.Alpha / 255f
            };
            
            if(!File.Exists(fixtureSprite.Key))
                Debug.LogError($"File {fixtureSprite.Key} does not exist");
            
            _fixtureToLoad.Add(fixtureSprite);
        }

        public async UniTask Render()
        {
            RenderTiles();
            await RenderFixtures();

           /* foreach (var tile in _tilesToLoad.Values)
            {
                Addressables.Release(tile);
            }*/
            
            _tilesToLoad.Clear();
        }

        private async UniTask RenderFixtures()
        {
            var z = 0;
            foreach (var fixture in _fixtureToLoad)
            {
                if (!_tilesToLoad.TryGetValue(fixture.Id, out var tileSprite))
                {
                    tileSprite = await Addressables.LoadAssetAsync<Texture2D>(fixture.Key);
                }
                
                var go        = new GameObject($"Fixture {fixture.Key}");
                var fixtureSprite = go.AddComponent<TileSprite>();
                fixtureSprite.id                      = fixture.Id;
                fixtureSprite.key                     = fixture.Key;
                fixtureSprite.type                    = 1;
                fixtureSprite.colorMultiplicatorIsOne = false;
                fixtureSprite.colorMultiplicatorR     = fixture.Red;
                fixtureSprite.colorMultiplicatorG     = fixture.Green;
                fixtureSprite.colorMultiplicatorB     = fixture.Blue;
                fixtureSprite.colorMultiplicatorA     = fixture.Alpha;
                
                go.transform.SetParent(gameObject.transform);
                //var middle = new Vector3(tileSprite.width * 0.5f, tileSprite.height * 0.5f);
                
                go.transform.position   = new Vector3((fixture.X + tileSprite.width * 0.5f) / 100f, (fixture.Y - tileSprite.height * 0.5f) / 100f, z--);
                go.transform.localScale = new Vector3(fixture.ScaleX, fixture.ScaleY, 1);

                if (fixture.Rotation != 0)
                {
                    go.transform.localRotation = Quaternion.Euler(0, 0, (-fixture.Rotation * 0.01f));

                    //go.transform.Rotate(0, 0, fixture.Rotation * Mathf.Deg2Rad);
                    //go.transform.Rotate(Vector3.forward, fixture.Rotation * Mathf.Deg2Rad);

                }

                go.transform.SetSiblingIndex(fixture.Order);
                 
                var spriteRenderer = go.AddComponent<SpriteRenderer>();
                spriteRenderer.sprite = Sprite.Create(tileSprite, new Rect(0, 0, tileSprite.width, tileSprite.height), new Vector2(0.5f, 0.5f));
                spriteRenderer.sortingOrder     = fixture.Order;

                spriteRenderer.sharedMaterial = new Material(_colorMatrixShader);
                
                spriteRenderer.sharedMaterial.SetColor(Color1, new Color(fixture.Red, fixture.Green, fixture.Blue, fixture.Alpha));

                //spriteRenderer.material.SetColor(Color1, new Color(fixture.Red, fixture.Green, fixture.Blue, fixture.Alpha));
            }
        }

        private void RenderTiles()
        {
            foreach (var tile in _assetsToLoad)
            {
                if (!_tilesToLoad.TryGetValue(tile.Id, out var tileSprite))
                {
                    continue;
                }

                if (tileSprite == null)
                {
                    Debug.LogError($"Tile {tile.Id} not found");
                    continue;
                }

                if (tile.Id == "44652")
                {
                    
                }

                var go            = new GameObject($"Tile {tile.Key}");
                var textureLoader = go.AddComponent<TileSprite>();
                textureLoader.key                     = tile.Key;
                textureLoader.id                      = tile.Id;
                textureLoader.type                    = 0;
                textureLoader.colorMultiplicatorR     = tile.ColorMultiplicator.Red;
                textureLoader.colorMultiplicatorG     = tile.ColorMultiplicator.Green;
                textureLoader.colorMultiplicatorB     = tile.ColorMultiplicator.Blue;
                textureLoader.colorMultiplicatorIsOne = tile.ColorMultiplicator.IsOne;
            
                go.transform.SetParent(gameObject.transform);
                go.transform.position = new Vector3(tile.X, tile.Y, 0);

                _tiles.Add(go);

                if (!_sprites.TryGetValue(tile.Key, out var sprite))
                {
                    sprite = Sprite.Create(tileSprite, new Rect(0, 0, tileSprite.width, tileSprite.height), new Vector2(0f, 1f));

                    _sprites.Add(tile.Key, sprite);
                }

            
                var spriteRenderer = go.AddComponent<SpriteRenderer>();
                spriteRenderer.sprite           = sprite;
                spriteRenderer.sortingLayerName = "Default";
                spriteRenderer.sortingOrder     = tile.Order;

                if (!tile.ColorMultiplicator.IsOne)
                {
                    spriteRenderer.sharedMaterial = new Material(_colorMatrixShader);
                    spriteRenderer.sharedMaterial.SetColor(Color1, new Color(tile.ColorMultiplicator.Red / 255f, 
                        tile.ColorMultiplicator.Green / 255f, 
                        tile.ColorMultiplicator.Blue / 255f, 
                        1));

                    /*spriteRenderer.material.SetColor(Color1, new Color(
                        tile.ColorMultiplicator.Red / 255f,
                        tile.ColorMultiplicator.Green / 255f, 
                        tile.ColorMultiplicator.Blue / 255f));*/
                }

                if (tile.ShouldFlipX)
                {
                    spriteRenderer.transform.position = new Vector3(tile.X + tileSprite.width / 100f, tile.Y, 0);
                    spriteRenderer.flipX              = true;
                }
            }
            

            _assetsToLoad.Clear();
            //_sprites.Clear();
            //_tilesToLoad.Clear();
        }
    }
}