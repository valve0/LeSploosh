using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    internal class Map
    {
        public int NumberOfSquid => NumMediumSquid + NumLargeSquid + NumGiantSquid;
        private int NumSmallSquid { get; init; }
        private int NumMediumSquid { get; init; }
        private int NumLargeSquid { get; init; }
        private int NumGiantSquid { get; init; }
        public int MapSize { get; init; }
        //private List<Tile> Tiles { get; set; }
        public Tile[] Tiles { get; init; }
        private List<int> Squids { get; init; }

        public Map(int small, int medium, int large, int giant, int numberOfTiles)
        {
            Random random = new Random();
            
            this.NumSmallSquid = small;
            this.NumMediumSquid = medium;
            this.NumLargeSquid = large;
            this.NumGiantSquid = giant;

            int[] squidNumbers = new int[4];
            squidNumbers[0] = small;
            squidNumbers[1] = medium;
            squidNumbers[2] = large;
            squidNumbers[3] = giant;


            //this.Tiles = new List<Tile>();
            this.Tiles = new Tile[numberOfTiles];
            this.Squids = new List<int>();


            //Fill aray of tiles with new tiles, setting them to default start value
            for (int i = 0; i < numberOfTiles; i++) 
            {
                //Fill list with default value of sea state
                //Tiles.Add(new Tile());
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
                        int spot = random.Next(numberOfTiles);

                        //check to see if no squid already in this spot
                        if (!Tiles[spot].SquidPresent)
                        {
                            Tiles[spot].SquidPresent = true;
                            squidPlaced = true;
                        }

                    } while (squidPlaced != true);

                }



            }



            // Generate the positions of the squid based on the number and the numberOfTiles
            // Place squid in decending order of size to make sure there is space


            //Select a random spot in the sea
            //int spot = random.Next(numberOfTiles);


            //Generate a random orienttaion of the squid, where is it pointing? Up down, left or right?
            //Up = 0, Down = 1, Left = 2, Right = 3.
            //int direction = random.Next(4);


            //Create a list of the spots that the squid would fill
            //List<int> spots = new List<int>();
            //spots.Add(spot);



            //Check whether there is enough free space to place the squid at these spots




        }

        internal bool attack(int attackGridNumber)
        {
            if (Tiles[attackGridNumber].SquidPresent && Tiles[attackGridNumber].Attackable)
            {
                //Hit a valid square with a squid
                Tiles[attackGridNumber].SeaState = 3;
                return true;

            }
            else if(!Tiles[attackGridNumber].SquidPresent && Tiles[attackGridNumber].Attackable)
            {
                //Miss
                Tiles[attackGridNumber].SeaState = 2;
                return true;
            }

            return false;
        }
    }
}
