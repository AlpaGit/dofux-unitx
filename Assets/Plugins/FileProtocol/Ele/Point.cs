namespace DofusCoube.FileProtocol.Ele
{
    public struct Point
    {
        public Point(short x, short y)
        {
            X = x;
            Y = y;
        }

        public int Y { get; set; }

        public int X { get; set; }
    }
}