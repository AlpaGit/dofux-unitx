using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using GAF.Scripts.Asset;
using GAF.Scripts.Core;
using Managers.Scene;
using Models.Maps;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Models.Actors
{
    public class ActorSprite
    {
        protected readonly GameObject GameObject;
        protected readonly string AssetPath;
        protected readonly GAFMovieClip MovieClip;

        private UniTask? _moveTask;
        private CancellationTokenSource _moveCancellationTokenSource;
        private ClientMovementPath _currentPath;

        protected string CurrentAnim;

        public short CellId { get; set; }


        public ActorSprite(int id, string name)
        {
            AssetPath             = $"Content/Sprites/{id}/{id}.asset";

            GameObject = new GameObject(name);
            GameObject.SetActive(false);

            GameObject.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
            // change the order in layer to make sure that the sprite is always on top of the map

            MovieClip                          = GameObject.AddComponent<GAFMovieClip>();
            MovieClip.settings.spriteLayerName = "Default";
        }

        public void StopMove()
        {
            if (_moveTask != null)
            {
                _moveCancellationTokenSource.Cancel();
            }
        }

        public virtual async UniTask Load(string animName)
        {
            if (CurrentAnim == animName)
            {
                return;
            }

            CurrentAnim = animName;
            var asset     = await Addressables.LoadAssetAsync<GAFAnimationAsset>(AssetPath).Task;
            var timelines = asset.getTimelines();
            var anim      = timelines.FirstOrDefault(x => x.linkageName == animName);

            if (anim == null)
            {
                return;
            }

            MovieClip.gafTransform.Childs.Clear();
            MovieClip.clear(true);
            MovieClip.initialize(asset, (int)anim.id);
            MovieClip.usePlaceholder = false;
            MovieClip.reload();
            MovieClip.setDefaultSequence(true);

            GameObject.SetActive(true);
            RefreshLayer();
        }

        public void Reload()
        {
            MovieClip.reload();
        }

        public void SetCell(short cell)
        {
            var point = SceneConverter.GetSceneCoordByCellId(cell);
            GameObject.transform.position = new Vector3(point.X + 0.43f, point.Y, 0);

            CellId = cell;
            RefreshLayer();
        }

        public void SetCurrentPath(ClientMovementPath path)
        {
            _currentPath = path;
        }

        public void RefreshLayer()
        {
            MovieClip.settings.spriteLayerValue = CellId - (-2 * 1000);
            MovieClip.reload();
        }

        public void FlipIfNeeded(Direction orientation)
        {
            MovieClip.settings.flipX = MapDirection.RequireFlipSide((int)orientation);
            MovieClip.reload();
        }

        public async UniTask MoveTo(short cell, Direction orientation, float speed)
        {
            FlipIfNeeded(orientation);

            // start tweening to the cell
            // after X seconds, set the cell
            var point        = SceneConverter.GetSceneCoordByCellId(cell);
            var nextPosition = new Vector3(point.X + 0.43f, point.Y, 0);
            // Reload();

            if (_moveTask != null)
            {
                _moveCancellationTokenSource.Cancel();
            }
            
            _moveCancellationTokenSource = new CancellationTokenSource();
            _moveTask = GameObject.transform.DOMove(nextPosition, speed)
                                  .SetEase(Ease.Linear)
                                  .ToUniTask(
                                      TweenCancelBehaviour.KillWithCompleteCallbackAndCancelAwait,
                                      _moveCancellationTokenSource.Token);
            //_moveTask = MoveAsync(nextPosition, speed, _moveCancellationTokenSource.Token).SuppressCancellationThrow();

            if (_moveTask != null)
            {
                await _moveTask.Value;
            }

            SetCell(cell);
        }

        private async UniTask MoveAsync(Vector3 targetPosition, float duration, CancellationToken cancellationToken)
        {
            var startPosition = GameObject.transform.position; // Remember the start position
            var elapsed       = 0f;                             // Time elapsed since the start of the movement

            while (elapsed < duration)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return;
                }
                
                elapsed += Time.deltaTime;  // Update the elapsed time
                var t = elapsed / duration; // Calculate how far along the duration we are (0 to 1)

                // Update the character's position
                GameObject.transform.position = Vector3.Lerp(startPosition, targetPosition, t);

                await UniTask.Yield(); // Wait until the next frame
            }

            // Ensure the character is exactly at the target position
            GameObject.transform.position = targetPosition;
        }
    }
}