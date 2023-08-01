using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Models.Maps;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Components.Maps
{
    public class MapComponent : MonoBehaviour
    {
        public MapBasicInformation mapInformation = new();
        [SerializeField] public Color backgroundColor;
        private static readonly int Color1 = Shader.PropertyToID("_Color");

        private readonly HashSet<GameObject> _sprites = new();
        private AsyncOperationHandle<IList<Texture2D>> _handle;
        private AsyncOperationHandle<Shader> _shaderHandle;

        public bool IsLoaded
        {
            get;
            private set;
        }
        
        private async void Start()
        {
            _shaderHandle = Addressables.LoadAssetAsync<Shader>("Shaders/ColorMatrixShader.shader");
            var colorMatrixShader = await _shaderHandle;
            // var colorMatrixShader = Shader.Find("Custom/ColorMatrixShader");

            var tiles = FindObjectsByType<TileSprite>(FindObjectsSortMode.None);
            var keys  = tiles.Select(x => x.key).Distinct();
            _handle = Addressables.LoadAssetsAsync<Texture2D>(keys, (e) => { }, Addressables.MergeMode.Union);

            var assets = await _handle;

            var textures = assets.ToDictionary(tileSprite => tileSprite.name);

            foreach (var tile in tiles)
            {
                var texture = textures[tile.id];

                var pivot  = tile.type == 0 ? new Vector2(0f, 1f) : new Vector2(0.5f, 0.5f);
                var sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), pivot);

                if (tile.IsDestroyed())
                {
                    return;
                }

                _sprites.Add(tile.gameObject);

                var sr = tile.GetComponent<SpriteRenderer>();
                sr.sprite = sprite;

                if (!tile.colorMultiplicatorIsOne)
                {
                    var material = new Material(colorMatrixShader);

                    if (tile.type == 0)
                    {
                        material.SetColor(Color1, new Color()
                        {
                            r = tile.colorMultiplicatorR / 255f,
                            g = tile.colorMultiplicatorG / 255f,
                            b = tile.colorMultiplicatorB / 255f,
                            a = 1f
                        });
                    }
                    else
                    {
                        material.SetColor(Color1, new Color()
                        {
                            r = tile.colorMultiplicatorR,
                            g = tile.colorMultiplicatorG,
                            b = tile.colorMultiplicatorB,
                            a = tile.colorMultiplicatorA
                        });
                    }


                    sr.sharedMaterial = material;
                    
                }
            }

            IsLoaded = true;
            if (Camera.main == null)
            {
                return;
            }

            Camera.main.backgroundColor = backgroundColor;
        }

        private void OnDestroy()
        {
            foreach (var sprite in _sprites)
            {
                if (sprite != null)
                {
                    Destroy(sprite); // This won't release the asset, but will destroy the GameObject
                }
            }
            
            _sprites.Clear();

            Addressables.Release(_handle);   
            Addressables.Release(_shaderHandle);
            Addressables.ReleaseInstance(gameObject);
            

            //GC.Collect();
        }
    }
}