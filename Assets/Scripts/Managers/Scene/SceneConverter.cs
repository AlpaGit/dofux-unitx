using System.Drawing;

namespace Managers.Scene
{
    public static class SceneConverter
    {
        public const float MaxHeight = CellHeight * 20;
        public const float CellHeight = 0.43f;

        public static PointF GetSceneCoordByCellId(int cellId)
        {
            var point = GetPixelCoordByCellId(cellId);

            return new PointF(point.X / 100f, point.Y / 100f);
        }

        public static PointF GetPixelCoordByCellId(int cellId)
        {
            // there is 14 cells per line
            // there is 20*2 lines
            // if the line is even we add the cell width / 2

            // the x = 0 and y = 0 is at the top left corner

            var line = cellId / 14;
            var col  = cellId % 14;

            var x = col * 86;
            var y = (float)-(line * (43 / 2));

            if (line % 2 != 0)
            {
                x += 43;
            }

            y += MaxHeight * 86f;

            return new PointF(x, y);
        }
    }
}