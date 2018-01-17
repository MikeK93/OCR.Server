namespace TextRecognition
{
    public class Box
    {
        public int X1 { get; set; }
        public int X2 { get; set; }
        public int Y1 { get; set; }
        public int Y2 { get; set; }

        public bool CheckIfInBox(int x, int y) => x >= X1 && x <= X2 && y >= Y1 && y <= Y2;

    }

}
