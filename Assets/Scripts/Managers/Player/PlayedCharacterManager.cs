using System.Linq;
using Components.Maps;
using Cysharp.Threading.Tasks;
using Managers.Maps;
using Models.Actors;
using Models.Maps;
using UnityEngine;

namespace Managers.Player
{
    public class PlayedCharacterManager : MonoBehaviour
    {      
        public static PlayedCharacterManager Instance { get; set; }

        public ActorSprite ActorSprite { get; set; }
        public Direction Orientation { get; set; }
        
        private static Texture2D _cursorTexture;
        private static Vector2 _hotSpot = Vector2.zero; // change this if you want to adjust the 'active' spot of the cursor
        private static CursorMode _cursorMode = CursorMode.Auto;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }
        
        // Start is called before the first frame update
        async void Start()
        {         
            _cursorTexture = Resources.Load<Texture2D>("Cursors/pointinghand");
            _hotSpot                = new Vector2(_cursorTexture.width / 2f, _cursorTexture.height / 2f);
            _cursorMode             = CursorMode.ForceSoftware;

            ActorSprite = new ActorSprite(431, "Poutching Ball");
            await ActorSprite.Load("AnimStatique_1");
            ActorSprite.SetCell(327);
        }
        
        private int GetRealOrientation(uint orientation)
        {
            if (orientation == 3)
            {
                orientation = 1;
            }

            if (orientation == 7)
            {
                orientation = 5;
            }

            return (int)orientation;
        }

        public async UniTask Move(short cell)
        {
            var path = PathFindingManager.FindClientPath(MapManager.Instance.CurrentMap, 
                ActorSprite.CellId, cell, false);

            if (path == null)
            {
                return;
            }
            
            if(path.Start.CellId == path.End.CellId)
            {
                return;
            }
            
            ActorSprite.StopMove();
            var isRunning = true;
            var animName  = "AnimCourse";

            if (path.Path.Count <= 4)
            {
                isRunning = false;
                animName  = "AnimMarche";
            }

            ActorSprite.SetCurrentPath(path);
            
            var speed    = isRunning ? 0.17f : 0.30f;
            var lastNode = path.Path.First();

            foreach (var pathNode in path.Path.Skip(1))
            {
                await ActorSprite.Load(animName + "_" + GetRealOrientation(lastNode.Orientation));
                await ActorSprite.MoveTo(pathNode.Step.CellId, (Direction)lastNode.Orientation, speed);
                Orientation = (Direction)lastNode.Orientation;
                lastNode = pathNode;
            }

            await ActorSprite.Load(animName + "_" + GetRealOrientation(lastNode.Orientation));
            await ActorSprite.MoveTo(cell, (Direction)lastNode.Orientation, speed);
            Orientation = (Direction)lastNode.Orientation;

            await ActorSprite.Load("AnimStatique_" + GetRealOrientation(lastNode.Orientation));
            ActorSprite.FlipIfNeeded(Orientation);
            ActorSprite.SetCurrentPath(null);

        }
        
        public void SetHandCursor()
        {
            Cursor.SetCursor(_cursorTexture, _hotSpot, _cursorMode);
        }
        
        public void SetDefaultCursor()
        {
            Cursor.SetCursor(null, Vector2.zero, _cursorMode);
        }

        public async UniTask MoveToBorder(Direction direction)
        {
            // we need to get the closest cell to the cursor and that is on the border
            
            // and then we change map
            MapManager.Instance.LoadNextMap(direction).Forget();
        }
    }
}
