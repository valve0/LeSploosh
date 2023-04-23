using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeSploosh
{
    //Class sets up the game and stores all the relevant infomration for the states of differnt elements.
    internal class GameInfo
    {
        
        public int ActiveGridNumber { get; set; }
        public int NumberOfSquid { get; set;}
        private int NumSmallSquid { get; init; }
        private int NumMediumSquid { get; init; }
        private int NumLargeSquid { get; init; }
        private int NumGiantSquid { get; init; }
        public int Size { get; init; }
        private int NumberOfTiles { get; init; }
        public int ShotCounter { get; set; }

        //private List<Tile> Tiles { get; set; }
        public Tile[,] Tiles { get; init; }
        private List<int> Squids { get; init; }

        public GameInfo(int noSmall, int noMedium, int noLarge, int noGiant, int mapSize, int shotCounter)
        {


            //this.NumSmallSquid = small;
            //var smallSquidTuple = Tuple.Create("small", 1, small);
            //this.NumMediumSquid = medium;
            //var mediumSquidTuple = Tuple.Create("medium", 2, medium);
            //this.NumLargeSquid = large;
            //var largeSquidTuple = Tuple.Create("large", 3, large);
            //this.NumGiantSquid = giant;
            //var giantSquidTuple = Tuple.Create("giant", 4, giant);

            this.NumberOfSquid = noSmall + noMedium + noLarge + noGiant;
            this.Tiles = new Tile[mapSize, mapSize]; //Create a 2d array of size mapsize
            this.Size = mapSize;
            this.NumberOfTiles = mapSize * mapSize;
            this.ShotCounter = shotCounter;

            var squidTuples = new (string, int, int)[]
            {
                ("small", 1, noSmall),
                ("medium", 2, noMedium),
                ("large", 3, noLarge),
                ("giant", 4, noGiant)
            };



            //squidTuples.Add(smallSquidTuple);
            //squidTuples.Add(mediumSquidTuple);
            //squidTuples.Add(largeSquidTuple);
            //squidTuples.Add(giantSquidTuple);






            //int[] squidNumbers = new int[4];
            //squidNumbers[0] = small;
            //squidNumbers[1] = medium;
            //squidNumbers[2] = large;
            //squidNumbers[3] = giant;

            //this.NumberOfSquid = NumSmallSquid + NumMediumSquid + NumLargeSquid + NumGiantSquid;


            // Create a list of tuples which stores the number and length of each squid 

            //this.Tiles = new List<Tile>();
            //this.Tiles = new Tile[NumberOfTiles];

            //this.Squids = new List<int>();


            //Fill aray of tiles with new tiles, setting them to default start value
            for (int row = 0; row < mapSize; row++) 
            {
                for (int col = 0; col < mapSize; col++)
                {
                    //Fill list with default value of sea state
                    Tiles[row,col] = new Tile();
                }
            }

            //Set the first tile to have the crosshair to begin with.
            Tiles[0,0].CrosshairBool = true;
            //first grid number is defeault start position for crosshair
            this.ActiveGridNumber = 0;



            PlaceSquids(squidTuples, Tiles);



            //Placing the squids

            ////Loop through each group of squid starting with small
            //foreach ( int squid in squidNumbers)
            //{

            //    for (int i = 0; i < squid; i++)
            //    {

            //        bool squidPlaced = false;
            //        do // Keep looping until squid placed
            //        {
            //            int spot = random.Next(NumberOfTiles);

            //            //check to see if no squid already in this spot
            //            if (!Tiles[spot].SquidPresent)
            //            {
            //                Tiles[spot].SquidPresent = true;
            //                squidPlaced = true;
            //            }

            //        } while (squidPlaced != true);
            //    }
            //}
        }


        internal void PlaceSquids(Tuples[] squidTuples, Tile[,] Tiles)
        {
            Random random = new Random();

            foreach (squidTuple in squidTuples)
            {

                bool squidPlaced = false;

                if (squidTuples.Item3)//For a given squid size there are squid
                {
                    do
                    {

                        //Loop through the number of squid for a given size
                        foreach (squid in squidTuples.Item3)
                        {
                            bool allPartsPlaced = false;


                            do
                            {


                                // Loop through the parts of the squid
                                for (int i = 0; i < squidTuples.Item2; i++)
                                {
                                    if (i == 0)
                                    {
                                        //Choose a random spot to place the first part of the squid
                                        firstPartPlaced = false;
                                        do // Keep looping until squid placed
                                        {
                                            int startTile = random.Next(Tiles.Length);

                                            //check to see if no squid already in this spot
                                            if (!Tiles[startTile].SquidPresent)
                                            {
                                                Tiles[startTile].SquidPresent = true;
                                                firstPartPlaced = true;
                                            }

                                        } while (firstPartPlaced == false);
                                    }

                                }   //Continue to place remaining parts of the squid




                                //Try to place remaining parts of squid
                                for (int i = 1; i < squidTuples.Item2; i++)
                                {

                                    //Chose a random direction (0 = up, 1 = right, 2 = down, 3 = left)
                                    //Generate a rnadom number between 0 and 3 inclusive
                                    var directions = new List<int>();
                                    int direction = GetNewDirection(directions);
                                    directions.Add(direction);

                                    // Try to place remaining squid length in direction generated from placed startTile 
                                    if (Tiles[startTile + direction].SquidPresent)



                                }


                                    
                                    
                                
                                

                            } while (allPartsPlaced == false);

                        }








                    } while (squidPlaced == false)




                }


            }

        }



        internal int GetNewDirection(List<int> directions)
        {
            
            //Keep looping until a new direction has been generated.
            do
            {
                int direction = Random.Next(4);

            }while(directions.Contains(direction))

            return direction;



        }

        internal bool AttackCheck(int attackGridNumber)
        {
            if (Tiles[attackGridNumber].Attackable)
            {
                PrintTerminal.PrintLine("Valid Square to attack");
                return true;

            }
            PrintTerminal.PrintLine("Invalid square to attack");
            return false;
        }

        public void ReduceShotCount()
        {
            ShotCounter--;
        }

        public void ReduceSquidCount()
        {
            NumberOfSquid--;
        }

        public void MoveCursorUp()
        {
            //Check to see if not at top
            if (ActiveGridNumber - Size >= 0 )
            {
                Tiles[ActiveGridNumber].CrosshairBool = false;

                ActiveGridNumber = ActiveGridNumber - Size;

                Tiles[ActiveGridNumber].CrosshairBool = true;

                PrintTerminal.PrintGameInfo(this);
            }
        }

        public void MoveCursorDown()
        {
            //Check to see if not at bottom
            if (ActiveGridNumber + Size <= (Size * Size - 1))
            {
                Tiles[ActiveGridNumber].CrosshairBool = false;

                ActiveGridNumber = ActiveGridNumber + Size;

                Tiles[ActiveGridNumber].CrosshairBool = true;

                PrintTerminal.PrintGameInfo(this);
            }
        }

        public void MoveCursorLeft()
        {
            //Check to see if not at far left
            if (ActiveGridNumber % Size != 0)
            {
                Tiles[ActiveGridNumber].CrosshairBool = false;

                ActiveGridNumber = ActiveGridNumber - 1;

                Tiles[ActiveGridNumber].CrosshairBool = true;

                PrintTerminal.PrintGameInfo(this);
            }
        }

        public void MoveCursorRight()
        {
            //Check to see if not at far right
            if (ActiveGridNumber % Size != (Size - 1))
            {
                Tiles[ActiveGridNumber].CrosshairBool = false;

                ActiveGridNumber = ActiveGridNumber + 1;

                Tiles[ActiveGridNumber].CrosshairBool = true;

                PrintTerminal.PrintGameInfo(this);
            }
        }

        //Game Logic

        public bool Attack()
        {
            int attackGridNumber = this.ActiveGridNumber;

            if (!this.Tiles[attackGridNumber].Attackable)
            {
                PrintTerminal.PrintLine("Tile not attackable");
                return false;

            }

            //Assumption that Attack Check is run before this method
            if (this.Tiles[attackGridNumber].SquidPresent)
            {
                //Loop through animation
                foreach (TileState state in Animations.hit)
                {
                    PrintTerminal.AnimateTile(state, attackGridNumber, this);
                }

                this.Tiles[attackGridNumber].Attackable = false;
                this.ReduceShotCount();
                this.ReduceSquidCount();
                PrintTerminal.PrintLine("Squid Hit!");
                return true;
            }
            else if (!this.Tiles[attackGridNumber].SquidPresent)
            {
                //Temporarly turn off Crosshair
                this.Tiles[attackGridNumber].CrosshairBool = false;
                //Loop through animation
                foreach (TileState state in Animations.miss)
                {
                    PrintTerminal.AnimateTile(state, attackGridNumber, this);
                }
                //Turn crosshair back on
                this.Tiles[attackGridNumber].CrosshairBool = true;

                //Set tile to not be attackable
                this.Tiles[attackGridNumber].Attackable = false;
                this.ReduceShotCount();
                PrintTerminal.PrintLine("Miss");
                return true;
            }

            PrintTerminal.PrintLine("ERROR");
            return false;

        }
    }
}
