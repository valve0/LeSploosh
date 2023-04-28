namespace LeSploosh
{
    internal class Tile
    {

        public TileState SeaState { get; set; }
        public bool SquidPresent { get; set; }
        public bool Attackable { get; set; }
        public bool CrosshairBool { get; set; }

        public Tile ()
        {
            this.SeaState = TileState.GameStart;
            this.SquidPresent = false;
            this.Attackable = true;
            this.CrosshairBool = false;
        }

        public string ReturnTileString()
        {
            ASCIRepository ASCIRepository = new();
            string fileName = string.Empty;

            if (this.CrosshairBool == true)
            {
                //Figure out what kind of crosshair to print for the tile
                if (this.SeaState == TileState.GameStart)
                {
                    fileName = $"CrosshairStart.txt";
                }
                else if (this.SeaState == TileState.GameMiss)
                {
                    fileName = $"CrosshairMiss.txt";
                }
                else if (this.SeaState == TileState.GameHit)
                {
                    fileName = $"CrosshairHit.txt";
                }
            }
            else
            {
                fileName = $"{this.SeaState}.txt";
            }

            string str = ASCIRepository.LoadASCIFromFile(fileName);
            return str;
        }



    }
}
