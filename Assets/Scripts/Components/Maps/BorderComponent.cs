using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Managers.Player;
using Models.Maps;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Components.Maps
{
    public class BorderComponent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        private SpriteRenderer _spriteRenderer;
        [SerializeField] private Direction direction;
        
        void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        // Update is called once per frame
        void Update()
        {
        
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            PlayedCharacterManager.Instance.SetHandCursor();

            var currentColor = _spriteRenderer.color;
            _spriteRenderer.DOColor(new Color(currentColor.r, currentColor.g, currentColor.b, 0.5f), 0.5f);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            var currentColor = _spriteRenderer.color;
            _spriteRenderer.DOColor(new Color(currentColor.r, currentColor.g, currentColor.b, 0f), 0.5f);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                PlayedCharacterManager.Instance.MoveToBorder(direction).Forget();
            }
        }
    }
}
