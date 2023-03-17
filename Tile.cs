namespace Test
{
    public class Tile
    {
    
        private int SeaState { get; init; }
        public Tile(int v)
        {
            this.SeaState = v;
        }

        public string Parser()
        {

                switch (SeaState)
                {
                    case 1:
                        return "SeaStart.txt";
                    case 2:
                        return "SeaMiss.txt";
                    case 3:
                        return "SeaHit.txt";
                    case 4:
                        return "CrosshairStart.txt";
                    case 5:
                        return "CrosshairMiss.txt";
                    case 6:
                        return "CrosshairHit.txt";
                    case 7:
                        return "Miss1.txt";
                    case 8:
                        return "Miss2.txt";

                    default:
                        return "ERROR";

                }
            


        }

        
    }
}
