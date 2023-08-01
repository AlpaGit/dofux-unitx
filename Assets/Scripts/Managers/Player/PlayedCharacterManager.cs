using System.Linq;
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
        
        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
