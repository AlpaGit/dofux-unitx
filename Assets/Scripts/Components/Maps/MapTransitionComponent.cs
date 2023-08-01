using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Components.Maps
{
    public class MapTransitionComponent : MonoBehaviour
    {
        private SpriteRenderer _spriteRenderer;
        public static MapTransitionComponent Instance { get; private set; }

        public void Awake()
        {
            Instance = this;
        }

        // Start is called before the first frame update
        void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        // Update is called once per frame
        void Update()
        {
        }

        public async UniTask StartTransition()
        {
            await _spriteRenderer.DOFade(1f, 0.25f);
           // var tweenScale = transform.DOScale(26f, 0.5f);

        }

        public async UniTask EndTransition()
        {
            await _spriteRenderer.DOFade(0f, 0.5f);
            //var tweenScale = transform.DOScale(0f, 2.5f);

            //await Task.WhenAll(tweenFade.AsyncWaitForCompletion(), tweenScale.AsyncWaitForCompletion());
        }
    }
}