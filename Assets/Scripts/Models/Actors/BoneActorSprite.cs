using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using GAF.Scripts.Asset;
using UnityEngine.AddressableAssets;

namespace Models.Actors
{
    public class BoneActorSprite : ActorSprite
    {
        public BoneActorSprite(int id, string name)
            : base(id, name)
        {
        }

        public override async UniTask Load(string animName)
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
    }
}