namespace LiveDots
{
    public class Tuplet
    {
        public BrailleNote Note { get; set; }
        public int Num { get; set; }

        public Tuplet() { }

        public Tuplet(BrailleNote n, int num)
        {
            Note = n;
            Num = num;
        }
    }
}
