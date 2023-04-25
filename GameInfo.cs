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
        
        public int[] ActiveGridNumber { get; set; }
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

        public GameInfo(int noSmall, int noMedium, int noLarge, int noGiant, int mapSize, int shotCounter, string difficulty)
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

            (string name, int squidSize,  int noSquid)[] squidTuples =
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
            this.ActiveGridNumber = new int[] { 0, 0 };

            PlaceSquids(squidTuples, Tiles, difficulty);



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


        internal void PlaceSquids((string name, int squidSize, int noSquid)[] squidTuples, Tile[,] Tiles, string difficulty)
        {
            Random random = new Random();

            //Loop through array of tuples
            foreach (var squidTuple in squidTuples)
            {
                //Initialise
                bool allSquidPlaced = false;
                int startTileRow = 0;
                int startTileCol = 0;
                int squidPlacedCounter = 0;

                if (squidTuple.noSquid > 0)//If there are squid present for a given tuple
                {
                    do
                    {

                        //Loop through the number of squid for a given size
                        for (int squidNo = 1; squidNo < squidTuple.noSquid + 1; squidNo++)
                        {
                            //Initialise
                            bool allPartsPlaced = false;

                            do
                            {
                                // Loop through the parts for the given squid
                                for (int i = 0; i < squidTuple.squidSize; i++)
                                {
                                    if (i == 0)
                                    {
                                        //Choose a random spot to place the first part of the squid
                                        bool firstPartPlaced = false;

                                        do // Keep looping until squid placed
                                        {
                                            startTileRow = random.Next(Size);
                                            startTileCol = random.Next(Size);

                                            //check to see if no squid already in this spot
                                            if (Tiles[startTileRow, startTileCol].SquidPresent == false)
                                            {
                                                Tiles[startTileRow, startTileCol].SquidPresent = true;
                                                firstPartPlaced = true;
                                            }

                                        } while (firstPartPlaced == false);
                                    }

                                }   //Continue to place remaining parts of the squid

                                //If you cant palce ther remaining parts of the squid currently redos the whole process from the top- needs optimising.


                                //Try to place remaining parts of squid 
                                for (int squidPart = 2; squidPart < squidTuple.squidSize + 1; squidPart++)
                                {
                                    
                                    
                                    //Generate a rnadom number between 0 and 3 inclusive
                                    //Chose a random direction (0 = up, 1 = right, 2 = down, 3 = left)
                                    Random Random = new Random();
                                    int direction = Random.Next(4);
                                    int rowAdder = 0;
                                    int colAdder = 0;

                                    switch (direction)
                                    {
                                        case 0:
                                            {
                                                rowAdder = -1;
                                                colAdder = 0;
                                                break;
                                            }
                                            case 1:
                                            {
                                                rowAdder = 0;
                                                colAdder = 1;
                                                break;
                                            }
                                            case 2:
                                            {
                                                rowAdder = 1;
                                                colAdder = 0;
                                                break;
                                            }
                                            case 3:
                                            {
                                                rowAdder = 0;
                                                colAdder = -1;
                                                break;
                                            }
                                    }

                                    if (startTileRow + rowAdder >= 0 && startTileCol + colAdder >= 0 && startTileRow + rowAdder < Size && startTileCol + colAdder < Size) //Make sure new grid reference is valid
                                    {

                                        //Check if space is free in the direction selected
                                        if (Tiles[startTileRow + rowAdder, startTileCol + colAdder].SquidPresent == false)
                                        {
                                            //Set the selected row to have a squid part
                                            Tiles[startTileRow + rowAdder, startTileCol + colAdder].SquidPresent = true;

                                            //check to see if at end of loop and therefore last part of squid placed successfully
                                            if (squidPart == squidTuple.squidSize)
                                            {
                                                //set flag to true
                                                allPartsPlaced = true;
                                            }

                                        }
                                    }
                                }

                                if (squidTuple.squidSize == 1)
                                    allPartsPlaced = true;

                                if (allPartsPlaced == true)
                                    squidPlacedCounter++;

                            } while (allPartsPlaced == false);

                        }

                        if (squidPlacedCounter == squidTuple.noSquid)
                            allSquidPlaced = true;

                    } while (allSquidPlaced == false);

                }

            }

        }



        internal int GetNewDirection(List<int> directions)
        {
            Random Random = new Random();

            int direction;

            //Keep looping until a new direction has been generated.
            do
            {
                direction = Random.Next(4);

            } while (directions.Contains(direction));

            return direction;



        }

        internal bool AttackCheck(int attackGridNumber)
        {
            if (Tiles[ActiveGridNumber[0],ActiveGridNumber[1]].Attackable) 
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
            if (ActiveGridNumber[0] != 0)
            {

                //Set current tile cross hair status to false
                Tiles[ActiveGridNumber[0], ActiveGridNumber[1]].CrosshairBool = false;

                //Move active grid number up a row
                ActiveGridNumber[0]--;

                //Update new tile cross hair status
                //Set current tile cross hair status to false
                Tiles[ActiveGridNumber[0], ActiveGridNumber[1]].CrosshairBool = true;

                PrintTerminal.PrintGameInfo(this);
            }
        }

        public void MoveCursorDown()
        {
            //Check to see if not at bottom
            if (ActiveGridNumber[0] != Size - 1)
            {

                //Set current tile cross hair status to false
                Tiles[ActiveGridNumber[0], ActiveGridNumber[1]].CrosshairBool = false;

                //Move active grid number down a row
                ActiveGridNumber[0]++;

                //Update new tile cross hair status
                //Set current tile cross hair status to false
                Tiles[ActiveGridNumber[0], ActiveGridNumber[1]].CrosshairBool = true;

                PrintTerminal.PrintGameInfo(this);
            }
        }

        public void MoveCursorLeft()
        {
            //Check to see if not at far left
            if (ActiveGridNumber[1] != 0)
            {

                //Set current tile cross hair status to false
                Tiles[ActiveGridNumber[0], ActiveGridNumber[1]].CrosshairBool = false;

                //Move active grid number down a column
                ActiveGridNumber[1]--;

                //Update new tile cross hair status
                //Set current tile cross hair status to false
                Tiles[ActiveGridNumber[0], ActiveGridNumber[1]].CrosshairBool = true;

                PrintTerminal.PrintGameInfo(this);
            }
        }

        public void MoveCursorRight()
        {
            //Check to see if not at far right
            if (ActiveGridNumber[1] != Size - 1)
            {

                //Set current tile cross hair status to false
                Tiles[ActiveGridNumber[0], ActiveGridNumber[1]].CrosshairBool = false;

                //Move active grid number up a column
                ActiveGridNumber[1]++;

                //Update new tile cross hair status
                //Set current tile cross hair status to false
                Tiles[ActiveGridNumber[0], ActiveGridNumber[1]].CrosshairBool = true;

                PrintTerminal.PrintGameInfo(this);
            }
        }

        //Game Logic

        public bool Attack()
        {

            if (!Tiles[ActiveGridNumber[0], ActiveGridNumber[1]].Attackable)
            {
                PrintTerminal.PrintLine("Tile not attackable");
                return false;

            }

            //Assumption that Attack Check is run before this method
            if (Tiles[ActiveGridNumber[0], ActiveGridNumber[1]].SquidPresent)
            {
                //Loop through animation
                foreach (TileState state in Animations.hit)
                {
                    PrintTerminal.AnimateTile(state, ActiveGridNumber, this);
                }

                this.Tiles[ActiveGridNumber[0], ActiveGridNumber[1]].Attackable = false;
                this.ReduceShotCount();
                this.ReduceSquidCount();
                PrintTerminal.PrintLine("Squid Hit!");
                return true;
            }
            else if (!Tiles[ActiveGridNumber[0], ActiveGridNumber[1]].SquidPresent)
            {
                //Temporarly turn off Crosshair
                Tiles[ActiveGridNumber[0], ActiveGridNumber[1]].CrosshairBool = false;

                //Loop through animation
                foreach (TileState state in Animations.miss)
                {
                    PrintTerminal.AnimateTile(state, ActiveGridNumber, this);
                }
                //Turn crosshair back on
                Tiles[ActiveGridNumber[0], ActiveGridNumber[1]].CrosshairBool = true;

                //Set tile to not be attackable
                Tiles[ActiveGridNumber[0], ActiveGridNumber[1]].Attackable = false;
                this.ReduceShotCount();
                PrintTerminal.PrintLine("Miss");
                return true;
            }

            PrintTerminal.PrintLine("ERROR");
            return false;

        }
    }
}
