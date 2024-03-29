﻿namespace LeSploosh
{
    internal class Tile
    {

        public TileState SeaState { get; set; }

        private int[] tilePosition;

        private bool squidPresent;
        public bool SquidPresent { 
            get {return squidPresent; }
            set
            {
                squidPresent = value;
            }
        }

        public bool Attackable { get; set; }
        public bool CrosshairBool { get; set; }

        private Squid? squid;
        public Squid? Squid
        {
            get { return squid; }


            set { squid = value; }
        }

        public Tile (int[] tilePosition)
        {
            this.SeaState = TileState.GameStart;
            this.SquidPresent = false;
            this.Attackable = true;
            this.CrosshairBool = false;
            this.tilePosition = tilePosition;   
        }


        public void SetSquid(Squid squid) 
        { 
            this.squid = squid;

            SquidPresent = true;

            squid.AddSquidPosition(this.tilePosition);


        }

        public string ReturnTileString()
        {
            TextFileRepository TextFileRepository = new();
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

            string str = TextFileRepository.LoadStringFromFile(fileName);
            return str;
        }





    }
}
