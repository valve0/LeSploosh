namespace Test
{
    internal class Tile
    {

        //public int GameState { get; set; }

        public GameState seaState;
        public bool SquidPresent { get; set; }
        public bool Attackable { get; set; }

        //public Tile(int v)
        //{
        //    this.GameState = v;
        //}
        public Tile () //Overloading method (default)
        {
            this.seaState = GameState.GameStart;
            this.SquidPresent = false;
            this.Attackable = true;    
        }



        //public string Parser(int GameState)
        //{

        //        switch (GameState)
        //        {
        //            case 1:
        //                return "GameStart.txt";
        //            case 2:
        //                return "GameMiss.txt";
        //            case 3:
        //                return "GameHit.txt";
        //            case 4:
        //                return "CrosshairStart.txt";
        //            case 5:
        //                return "CrosshairMiss.txt";
        //            case 6:
        //                return "CrosshairHit.txt";
        //            case 7:
        //                return "Miss1.txt";
        //            case 8:
        //                return "Miss2.txt";

        //            default:
        //                return "ERROR";

        //        }
            


        //}

        
    }
}
