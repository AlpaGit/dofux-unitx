using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DofusCoube.FileProtocol.Dlm;
using Managers.Player;
using Models.Maps;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Components.Maps
{
    public class CellComponent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        public short CellId { get; set; }
        public Cell Cell { get; set; }

        public void OnPointerEnter(PointerEventData eventData)
        {
            PlayedCharacterManager.Instance.SetHandCursor();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            PlayedCharacterManager.Instance.SetDefaultCursor();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                PlayedCharacterManager.Instance.Move(CellId).Forget();
            }
        }
    }
}