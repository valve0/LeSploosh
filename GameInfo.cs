using LeSploosh.Audio;
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
        private List<int[]> squidPositions = new List<int[]>();
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
        //private List<int> Squids { get; init; }

        public GameInfo(int noSmall, int noMedium, int noLarge, int noGiant, int gridSize, int shotCounter)
        {

            this.NumberOfSquid = noSmall + noMedium + noLarge + noGiant;




            
            //Create an array of squids equal to the number of squids on the grid
            ///Squid[] squids = new Squid[NumberOfSquid];

            this.Tiles = new Tile[gridSize, gridSize]; //Create a 2d array of size mapsize
            this.Size = gridSize;
            this.NumberOfTiles = gridSize * gridSize;
            this.ShotCounter = shotCounter;

            (string name, int squidSize,  int noSquid)[] squidTuples =
            {
                ("small", 1, noSmall),
                ("medium", 2, noMedium),
                ("large", 3, noLarge),
                ("giant", 4, noGiant)
            };

            //Fill aray of tiles with new tiles, setting them to default start value
            for (int row = 0; row < gridSize; row++) 
            {
                for (int col = 0; col < gridSize; col++)
                {
                    //Fill list with default value of sea state
                    int[] tilePosition = { row, col };

                    Tiles[row,col] = new Tile(tilePosition);
                }
            }

            //Set the first tile to have the crosshair to begin with.
            Tiles[0,0].CrosshairBool = true;
            //first grid number is defeault start position for crosshair
            this.ActiveGridNumber = new int[] { 0, 0 };


            //for (int i = 0; i < NumberOfSquid; i++)
            //{
            //    Squid[] squid


            //}

            //Squid[] squids = new Squid[NumberOfSquid];

            //int squidCounter = 0;
            //foreach (var squidTuple in squidTuples)
            //{
            //    for (int  i = 0; i < squidTuple.noSquid; i++)
            //    {
            //        squids[squidCounter] = new Squid(squidTuple.squidSize);
            //        squidCounter++;
            //    }

                
            
            //}


                PlaceSquids(squidTuples, Tiles);

        }


        internal void PlaceSquids((string name, int squidSize, int noSquid)[] squidTuples, Tile[,] Tiles)
        {
            Random random = new Random();

            //Used to loop through array of squids
            int squidCounter = 0;

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
                    



                    // REDO THIS- LOOP THROUGH THE NUMBER OF SQUID PARTS, USE THE PART AS A REFERNCE TO BUILD THE COL ADSDER ASN ROW ADDER OFF OF- TOO COMPLICATED AT THEW MOMENT

                    do
                    {

                        //Instanitate a squid of that size
                        //string squidName = $"Squid{squidCounter}";

                        //Loop through the number of squid for a given size
                        for (int squidNo = 1; squidNo < squidTuple.noSquid + 1; squidNo++)
                        {
                            //Initialise
                            bool allPartsPlaced = false;





                            do
                            {

                                //Create a temporary vcariable to hold the positiuons for the parts of the squid
                                List<int[]> squidPartPositions = new List<int[]>();

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




                                // Loop through the parts for the given squid
                                for (int squidPart = 0; squidPart < squidTuple.squidSize; squidPart++)
                                {

                                    //Place first part of squid
                                    if (squidPart == 0)
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
                                                //Assign the squid object into the tile  
                                                //Tiles[startTileRow, startTileCol].SetSquid(squid);

                                                //Tiles[startTileRow, startTileCol].SquidPresent = true;
                                                firstPartPlaced = true;

                                                squidPartPositions.Add(new int[] { startTileRow, startTileCol });


                                                //Need to set this here as 
                                                //Tiles[startTileRow, startTileCol].SquidPresent = true;


                                            }

                                        } while (firstPartPlaced == false);

                                        //Check for squids of length 1
                                        if (squidTuple.squidSize == 1)
                                            allPartsPlaced = true;

                                    }
                                    else
                                    {

                                        //Continue to place remaining parts of the squid

                                        //If you cant palce ther remaining parts of the squid currently redos the whole process from the top- needs optimising.

                                        //Try to place remaining parts of squid 

                                        // As the squid parts get longer you move further away from starting Tile position
                                        int nextTileRow = startTileRow + (rowAdder * squidPart);
                                        int nextTileCol = startTileCol + (colAdder * squidPart);

                                        //Make sure new grid reference is valid (on board and no squid present)
                                        if (nextTileRow >= 0 && nextTileRow < Size && nextTileCol >= 0 && nextTileCol < Size && Tiles[nextTileRow, nextTileCol].SquidPresent == false)
                                        {

                                            //Check if space is free in the direction selected

                                            //Set the selected row to have a squid part
                                            //Add the same squid object to next tile
                                            //Tiles[nextTileRow, nextTileCol].SetSquid(squid);

                                            squidPartPositions.Add(new int[] { nextTileRow, nextTileCol });

                                            //check to see if at end of loop and therefore last part of squid placed successfully
                                            if (squidPart == squidTuple.squidSize - 1)
                                            {
                                                allPartsPlaced = true;

                                                //Instantiate squid object and assign to the locations defined
                                                Squid squid = new Squid(squidTuple.squidSize, squidPlacedCounter + 1);

                                                //Add current squid to list of all squid positions
                                                foreach (var positon in squidPartPositions)
                                                {
                                                    //this.squidPositions.Add(positon);
                                                    Tiles[positon[0], positon[1]].SetSquid(squid);
                                                }



                                            }

                                        }

                                    }
                                }

                                if (allPartsPlaced == true)
                                    squidPlacedCounter++;

                            } while (allPartsPlaced == false);

                            squidCounter++;
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

                PrintTerminal.PrintGameInfo(this, "");

                AudioPlaybackEngine.Instance.PlaySound("CursorMove.mp3");
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

                PrintTerminal.PrintGameInfo(this, "");

                AudioPlaybackEngine.Instance.PlaySound("CursorMove.mp3");
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

                PrintTerminal.PrintGameInfo(this, "");

                AudioPlaybackEngine.Instance.PlaySound("CursorMove.mp3");
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

                PrintTerminal.PrintGameInfo(this, "");

                AudioPlaybackEngine.Instance.PlaySound("CursorMove.mp3");
            }
        }

        //Game Logic

        public bool Attack()
        {

            if (!Tiles[ActiveGridNumber[0], ActiveGridNumber[1]].Attackable)
            {
                PrintTerminal.PrintGameInfo(this, "Tile not attackable");
                return false;

            }

            //Assumption that Attack Check is run before this method
            if (Tiles[ActiveGridNumber[0], ActiveGridNumber[1]].SquidPresent)
            {
                int currentSquidCount = NumberOfSquid;
                Tiles[ActiveGridNumber[0], ActiveGridNumber[1]].Squid.IncreaseHitCounter();
                this.UpdateSquidCount(Tiles[ActiveGridNumber[0], ActiveGridNumber[1]]);
                int newSquidCount = NumberOfSquid;

                string message = string.Empty;

                if (currentSquidCount == newSquidCount)
                {
                    AudioPlaybackEngine.Instance.PlaySound("kaboom.mp3");
                    message = "Squid Hit!";
                }
                else
                {
                    AudioPlaybackEngine.Instance.PlaySound("SquidDead.mp3");
                    message = "Squid Killed!";

                }
                    


                Thread.Sleep(100);
                //Loop through animation
                foreach (TileState state in Animations.hit)
                {
                    PrintTerminal.AnimateTile(state, ActiveGridNumber, this);
                }

                this.Tiles[ActiveGridNumber[0], ActiveGridNumber[1]].Attackable = false;

                
                this.ReduceShotCount();
                
                PrintTerminal.PrintGameInfo(this, message);

                return true;
            }
            else if (!Tiles[ActiveGridNumber[0], ActiveGridNumber[1]].SquidPresent)
            {
                
                AudioPlaybackEngine.Instance.PlaySound("sploosh.mp3");
                Thread.Sleep(100);

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
                PrintTerminal.PrintGameInfo(this, "Miss");
                

                return true;
            }

            PrintTerminal.PrintGameInfo(this, "");
            PrintTerminal.PrintString("ERROR");
            return false;

        }

        internal bool AttackCheck(int attackGridNumber)
        {
            if (Tiles[ActiveGridNumber[0], ActiveGridNumber[1]].Attackable)
            {
                PrintTerminal.PrintString("Valid Square to attack");
                return true;

            }
            PrintTerminal.PrintString("Invalid square to attack");
            return false;
        }

        public void ReduceShotCount()
        {
            ShotCounter--;
        }

        public void UpdateSquidCount(Tile tile)
        {
            if (tile.Squid.SquidStatus == false) // I.e if the squid is now dead
                NumberOfSquid--;
        }
    }
}
