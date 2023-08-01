using DofusCoube.FileProtocol.Dlm;
using Models.Maps;
using UnityEngine;

namespace Components.Maps
{
    public class CellComponent : MonoBehaviour
    {
        public short CellId { get; set; }
        public Cell Cell { get; set; }
    }
}