using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    internal class GameInfo
    {
        public int NumberOfSquid { get; set;}
        private int NumSmallSquid { get; init; }
        private int NumMediumSquid { get; init; }
        private int NumLargeSquid { get; init; }
        private int NumGiantSquid { get; init; }
        public int GameInfoSize { get; init; }
        private int NumberOfTiles { get; init; }

        public int ShotCounter { get; set; }

        //private List<Tile> Tiles { get; set; }
        public Tile[] Tiles { get; init; }
        private List<int> Squids { get; init; }

        public GameInfo(int small, int medium, int large, int giant, int mapSize, int shotCounter)
        {
            Random random = new Random();
            
            this.NumSmallSquid = small;
            this.NumMediumSquid = medium;
            this.NumLargeSquid = large;
            this.NumGiantSquid = giant;
            this.GameInfoSize = mapSize;
            this.NumberOfTiles = mapSize * mapSize;
            this.ShotCounter = shotCounter;


            int[] squidNumbers = new int[4];
            squidNumbers[0] = small;
            squidNumbers[1] = medium;
            squidNumbers[2] = large;
            squidNumbers[3] = giant;

            this.NumberOfSquid = NumSmallSquid + NumMediumSquid + NumLargeSquid + NumGiantSquid;

            //this.Tiles = new List<Tile>();
            this.Tiles = new Tile[NumberOfTiles];
            this.Squids = new List<int>();


            //Fill aray of tiles with new tiles, setting them to default start value
            for (int i = 0; i < NumberOfTiles; i++) 
            {
                //Fill list with default value of sea state
                Tiles[i] = new Tile();
            
            }

            
            //Loop through each group of squid starting with small
            foreach( int squid in squidNumbers)
            {

                for (int i = 0; i< squid; i++)
                {

                    bool squidPlaced = false;
                    do // Keep looping until squid placed
                    {
                        int spot = random.Next(NumberOfTiles);

                        //check to see if no squid already in this spot
                        if (!Tiles[spot].SquidPresent)
                        {
                            Tiles[spot].SquidPresent = true;
                            squidPlaced = true;
                        }

                    } while (squidPlaced != true);
                }
            }
        }


        internal bool AttackCheck(int attackGridNumber)
        {
            if (Tiles[attackGridNumber].Attackable)
            {
                Console.WriteLine("Valid Square to attack");
                return true;

            }
            Console.WriteLine("Invalid square to attack");
            return false;
        }

        internal bool Attack(GameInfo Game, int attackGridNumber)
        {

            if (!Tiles[attackGridNumber].Attackable) 
            {
                Console.WriteLine("Tile not attackable");
                return false; 
            
            }
            
            //Assumption that Attack Check is run before this method
            if (Tiles[attackGridNumber].SquidPresent)
            {
                //Loop through animation
                foreach (GameState state in Animations.hit)
                {
                    AnimateTile(state, attackGridNumber, Game);
                }

                Tiles[attackGridNumber].Attackable = false;
                ShotCounter--;
                NumberOfSquid--;
                Console.WriteLine("Squid Hit!");
                return true;



            }
            else if (!Tiles[attackGridNumber].SquidPresent)
            {

                //Loop through animation
                foreach (GameState state in Animations.miss)
                {
                    AnimateTile(state, attackGridNumber, Game);
                }

                Tiles[attackGridNumber].Attackable = false;
                ShotCounter--;
                Console.WriteLine("Miss");
                return true;
            }
            Console.WriteLine("ERROR");
            return false;
        }

        private void AnimateTile(GameState state, int attackGridNumber, GameInfo Game)
        {
            Tiles[attackGridNumber].seaState = state;
            Console.Clear();
            PrintTerminal PrintTerminal = new();
            PrintTerminal.PrintGameInfo(Game);
            Thread.Sleep(Animations.waitTime);
        }
        
        
    }
}
