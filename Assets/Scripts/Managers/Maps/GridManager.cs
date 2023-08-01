using System.Collections.Generic;
using Components.Maps;
using Managers.Cameras;
using Managers.Player;
using Managers.Scene;
using Models.Maps;
using Unity.VisualScripting;
using UnityEngine;

namespace Managers.Maps
{
    public class GridManager : MonoBehaviour
    {
        public static GridManager Instance;
        
        private readonly List<GameObject> _cells = new(560);
    

        // Start is called before the first frame update
        void Start()
        {       
        }

        private void Awake()
        {
            Instance = this;
        }

        public void UpdateCells(Map map)
        {
            foreach (var cell in _cells)
            {
                if (!cell.IsDestroyed())
                {
                    Destroy(cell);
                }
            }
            
            foreach (var cellId in MapTools.EveryCellId)
            {
                CreateCellGrid(map, cellId);
            }
        }
        private void CreateCellGrid(Map map, short cellId)
        {
            var cellData = map.Cells[cellId];

            var point = SceneConverter.GetSceneCoordByCellId(cellId)!;

            var posX = point.X;
            var posY = point.Y;

            var cell = new GameObject();
            cell.transform.SetParent(gameObject.transform);
            cell.transform.position = new Vector3(posX, posY, 0);

            /*var cellComponent = cell.AddComponent<CellComponent>();
            cellComponent.CellId   = cellId;
            cellComponent.CellData = cellData;*/

            var lr = cell.AddComponent<LineRenderer>();
            lr.sortingLayerName  = "UI";
            lr.sortingOrder      = 32700;
            lr.startWidth        = 0.01f;
            lr.endWidth          = 0.01f;
            lr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            lr.receiveShadows    = false;
            lr.name = $"Cell ({cellId}, {point.X}, {point.Y})";

            if (cellData.IsWalkable)
            {
                var transparentMaterial = new Material(Shader.Find("Sprites/Default"))
                {
                    color = new Color(1, 1, 1, 0.12f), // White with 50% opacity
                };

                lr.positionCount = 5;
                lr.material      = transparentMaterial;

                // Set the positions
                lr.SetPosition(0, new Vector3(posX, posY, 0)); // Bottom left corner
                lr.SetPosition(1, new Vector3(posX + GridCameraController.CellWidth / 2f, posY + GridCameraController.CellHeight / 2f, 0)); // Top left corner
                lr.SetPosition(2, new Vector3(posX + GridCameraController.CellWidth, posY, 0)); // Top right corner
                lr.SetPosition(3, new Vector3(posX + GridCameraController.CellWidth / 2f, posY - GridCameraController.CellHeight / 2f, 0)); // Bottom right corner
                lr.SetPosition(4, new Vector3(posX, posY, 0)); // Back to bottom left to close the shape

                lr.startColor = new Color(1, 1, 1, 1f); // White with 50% opacity
                lr.endColor   = new Color(1, 1, 1, 1f); // White with 50% opacity
                
                // spriteRenderer.sprite = _cellSprite;

                var hitbox = cell.AddComponent<PolygonCollider2D>();

                var points = new Vector2[4];
                points[0] = new Vector2(0, 0);                                               // Bottom left corner
                points[1] = new Vector2(GridCameraController.CellWidth / 2, GridCameraController.CellHeight / 2); // Top left corner
                points[2] = new Vector2(GridCameraController.CellWidth, 0);                                       // Top right corner
                points[3] = new Vector2(GridCameraController.CellWidth / 2, -GridCameraController.CellHeight / 2);                     // Bottom right corner
                hitbox.SetPath(0, points);
            }

            var textObject = new GameObject();
            textObject.transform.SetParent(cell.transform);

            var cellComponent = cell.AddComponent<CellComponent>();
            cellComponent.CellId   = cellId;
            cellComponent.Cell = cellData;

            // draw a text containing the cell id
            /* var text = textObject.AddComponent<TextMeshPro>();
            text.alignment            = TextAlignmentOptions.Center;
            //text.text                 = cellId.ToString();
            text.fontSize             = 14.5f;
            text.color                = Color.white;
            text.transform.position   = new Vector3(posX + CellWidth / 2, posY, 0);
            text.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            text.sortingOrder         = 1001;
            */

            _cells.Add(cell);
        }

        // Update is called once per frame

    }
}
