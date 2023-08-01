using DofusCoube.FileProtocol.Dlm;
using UnityEngine;

namespace Models.Maps
{
    public class TileSprite : MonoBehaviour
    {
        public string id;
        public string key;
        public byte type;
        [SerializeField] public bool colorMultiplicatorIsOne;
        [SerializeField] public float colorMultiplicatorR;
        [SerializeField] public float colorMultiplicatorG;
        [SerializeField] public float colorMultiplicatorB;    
        [SerializeField] public float colorMultiplicatorA;
    }

    public class TileSpriteModel
    {
        public string Id { get; set; }
        public string Key { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        
        public int Order { get; set; }
        
        public bool ShouldFlipX { get; set; }
        public ColorMultiplicator ColorMultiplicator { get; set; }
    }

    public class FixtureSpriteModel
    {
        public string Id { get; set; }
        public string Key { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float ScaleX { get; set; }
        public float ScaleY { get; set; }
        public float Rotation { get; set; }
        public int Order { get; set; }
        public float Red { get; set; }
        public float Green { get; set; }
        public float Blue { get; set; }
        public float Alpha { get; set; }
    
    }
}