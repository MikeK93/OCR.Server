namespace TextRecognition
{
    public class Box
    {
        public int X1 { get; set; }
        public int X2 { get; set; }
        public int Y1 { get; set; }
        public int Y2 { get; set; }

        public int Height => Y2 - Y1;
        public int Width => X2 - X1;

        public bool CheckIfInBox(int x, int y) => x >= X1 && x <= X2 && y >= Y1 && y <= Y2;

        public override string ToString()
        {
            return $"[{X1},{Y1}][{X2},{Y2}] W={Width} H={Height}";
        }
    }

}
