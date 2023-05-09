using LeSploosh.Audio;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace LeSploosh
{
    //Class sets up the game and stores all the relevant infomration for the states of differnt elements.
    internal partial class GameInfo
    {
        private int[] activeGridNumber;
        
        private readonly int size;
        private readonly int totalShots;
        private int shotsMade;
        private (string name, int squidsize, int noSquid)[] squidTuples;

        private Tile[,] tiles;
        private List<Squid> listOfSquids;
        
        private readonly int highScore;

        private bool[] bombs;
        private int squidKilledCounter;
        private int numberOfSquidAlive;
        private readonly bool[] SquidImages;

        private string feedBack;

        public int GameState { get; private set; }



        public GameInfo((string name, int squidsize, int noSquid)[] squidTuples, int gridsize, int totalShots)
        {


            this.totalShots = totalShots;

             listOfSquids = new List<Squid>();

            this.squidTuples = squidTuples;
                  
            int numberOfDifferentSquid = 0;


            foreach (var squidTuple in squidTuples)
            {
                if (squidTuple.noSquid > 0)
                    numberOfDifferentSquid++;

                numberOfSquidAlive += squidTuple.noSquid;

            }

                 
            this.GameState = 0;

            this.feedBack = string.Empty;


            this.tiles = new Tile[gridsize, gridsize]; //Create a 2d array of size mapsize
            this.size = gridsize;
            this.shotsMade = 0;

            this.squidKilledCounter = 0;

            this.highScore = int.Parse(TextFileRepository.LoadStringFromFile("highScore.txt").Replace("\r\n", string.Empty));

            this.bombs = new bool[totalShots];
            this.SquidImages = new bool[numberOfSquidAlive];

            //Set each value initally to true = cannonball available
            for (int i = 0; i < this.bombs.Length; i++)
            {
                bombs[i] = true;
            }

            //Set each value initally to true = squid alive
            for (int i = 0; i < this.SquidImages.Length; i++)
            {
                SquidImages[i] = true;
            }



            //Fill array of tiles with new tiles, setting them to default start value
            for (int row = 0; row < gridsize; row++)
            {
                for (int col = 0; col < gridsize; col++)
                {
                    //Fill list with default value of sea state
                    int[] tilePosition = { row, col };

                    tiles[row, col] = new Tile(tilePosition);
                }
            }

            //Set the first tile to have the crosshair to begin with.
            tiles[0, 0].CrosshairBool = true;
            //first grid number is defeault start position for crosshair
            this.activeGridNumber = new int[] { 0, 0 };
    
            PlaceSquids();

            var test = listOfSquids;
            var test2 = 1;
        }


        private void PlaceFirstPartOfSquid(ref List<int[]> squidPartPositions)
        {

            Random Random = new Random();
            //Choose a random spot to place the first part of the squid
            bool firstPartPlaced = false;

            do // Keep looping until squid placed
            {
                //Randomly place first part
                squidPartPositions[0][0] = Random.Next(size);
                squidPartPositions[0][1] = Random.Next(size);

                //check to see if no squid already in this spot
                if (tiles[squidPartPositions[0][0], squidPartPositions[0][1]].SquidPresent == false) 
                    firstPartPlaced = true;

            } while (firstPartPlaced == false);


        }

        private bool PlaceSubsequentSquidPart((string name, int squidsize, int noSquid) squidTuple, int squidNo, int squidPart, int direction, ref List<int[]> squidPartPositions)
        {
            //Generate a rnadom number between 0 and 3 inclusive
            //Chose a random direction (0 = up, 1 = right, 2 = down, 3 = left)

            //Based on the first part of the squid generate the following tiles int he supplied direction
            int[] nextTile = GenerateNextTile(squidPartPositions[0][0], squidPartPositions[0][1], squidPart, direction);
            int nextTileRow = nextTile[0];
            int nextTileCol = nextTile[1];

            //Make sure new grid reference is valid (on board and no squid present)
            if (nextTileRow >= 0 && nextTileRow < size && nextTileCol >= 0 && nextTileCol < size && tiles[nextTileRow, nextTileCol].SquidPresent == false)
            {

                // Add squid position to list of squid positions
                squidPartPositions[squidPart][0] = nextTileRow;
                squidPartPositions[squidPart][1] = nextTileCol;

                //check to see if at end of loop and therefore last part of squid placed successfully
                if (squidPart + 1 == squidTuple.squidsize)
                {

                    //Instantiate squid object add to list of squids
                    listOfSquids.Add(new Squid { Id = squidNo, size = squidTuple.squidsize, HitCounter = 0, SquidStatus = true });


                    //Loop through all the current squid positions and create a squid object in their locations
                    foreach (var positon in squidPartPositions)
                    {
                        // Place a squid at this position
                        tiles[positon[0], positon[1]].SetSquid(listOfSquids.Last());

                        
                    }

                }

                return true; // Subsequent Part placed

            }

            //Cannot place squid here
            return false;

        }

        private int[] GenerateNextTile(int startTileRow, int startTileCol, int squidPart, int direction)
        {

            int rowAdder = 0;
            int colAdder = 0;

            int[] nexttiles = new int[2];
            //Chose a random direction (0 = up, 1 = right, 2 = down, 3 = left)
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

            nexttiles[0] = startTileRow + (rowAdder * squidPart);
            nexttiles[1] = startTileCol + (colAdder * squidPart);

            return nexttiles;

        }

        private void PlaceSquids()
        {
            //Loop through array of tuples
            foreach (var squidTuple in squidTuples)
            {
                //Loop through the number of squid for a given size
                for (int squidNo = 1; squidNo < squidTuple.noSquid + 1; squidNo++)
                {
                    //Initialise
                    bool squidPartPlaced = false;
                    bool squidPlaced = false;

                    do //Loop through the parts of the squid- only leave when it is placed successfully
                    {
                        //Create a temporary variable to hold the row,col positions for each parts of the squid
                        List<int[]> squidPartPositions = new List<int[]>();

                        //create empty list of arrays to match the size of the squid
                        for (int i = 0; i < squidTuple.squidsize; i++)
                        {
                            int[] array = new int[2];
                            squidPartPositions.Add(array);
                        }
                        //genreate new direction
                        Random Random = new Random();
                        int direction = Random.Next(4);

                        // Loop through the parts for the given squid
                        for (int squidPart = 0; squidPart < squidTuple.squidsize; squidPart++)
                        {

                            //Place first part of squid
                            if (squidPart == 0)
                            {
                                //Get and place first part of squid
                                PlaceFirstPartOfSquid(ref squidPartPositions);
                            }
                            else
                            {
                                // Try and place the subsequent part of squid
                                squidPartPlaced = PlaceSubsequentSquidPart(squidTuple, squidNo, squidPart, direction, ref squidPartPositions);

                                if (squidPartPlaced == false)
                                    break; //Redo squid place loop
                                else if (squidPartPlaced == true && squidPart + 1 == squidTuple.squidsize)
                                    squidPlaced = true;
                            }


                        }

                    } while (squidPlaced == false);

                }

            }

        }

        private bool ValidMove(ConsoleKey key)
        {
            if (activeGridNumber[0] == 0 && key == ConsoleKey.UpArrow) //At top
                return false;

            else if (activeGridNumber[0] == size - 1 && key == ConsoleKey.DownArrow) //At bottom
                return false;

            else if (activeGridNumber[1] == 0 && key == ConsoleKey.LeftArrow) //At left edge
                return false;

            else if (activeGridNumber[1] == size - 1 && key == ConsoleKey.RightArrow) //At right edge
                return false;
            else
                return true;
        }

        private void MoveActiveGrid(ConsoleKey key)
        {

            switch (key)
            {
                case ConsoleKey.UpArrow:
                    activeGridNumber[0]--;
                    break;
                case ConsoleKey.DownArrow:
                    activeGridNumber[0]++;
                    break;
                case ConsoleKey.LeftArrow:
                    activeGridNumber[1]--;
                    break;
                case ConsoleKey.RightArrow:
                    activeGridNumber[1]++;
                    break;
            }
        }

        public void MoveCursor(Display display, ConsoleKey key)
        {
            //Check to see if not at edges of grid
            if (ValidMove(key))
            {

                //Set current tile cross hair status to false
                tiles[activeGridNumber[0], activeGridNumber[1]].CrosshairBool = false;

                MoveActiveGrid(key);

                //Set current tile crosshair status to true
                tiles[activeGridNumber[0], activeGridNumber[1]].CrosshairBool = true;
                feedBack = "";
                PrintGameInfo(display);

                //Play sound moving cursor
                AudioPlaybackEngine.Instance.PlaySound(Sounds.cursorMove);


            }
        }

        public void Attack(Display display)
        {

            if (!tiles[activeGridNumber[0], activeGridNumber[1]].Attackable) // Tile not attackable
            {

                feedBack = "          Cannot attack this square          ";

            }
            //Assumption that Attack Check is run before this method
            else if (tiles[activeGridNumber[0], activeGridNumber[1]].SquidPresent) // Squid present and attackable
            {
                tiles[activeGridNumber[0], activeGridNumber[1]].Squid.IncreaseHitCounter();

                if (!UpdateSquidCount(tiles[activeGridNumber[0], activeGridNumber[1]]))
                {
                    AudioPlaybackEngine.Instance.PlaySound(Sounds.squidHit);
                    feedBack = "          Squid Hit!          ";
                }
                else
                {
                    AudioPlaybackEngine.Instance.PlaySound(Sounds.squidDead);
                    feedBack = "          Squid Killed!          ";
                    SquidImages[squidKilledCounter] = false;
                    squidKilledCounter++;

                }

                Thread.Sleep(100);

                //Run animation
                AnimateTile(display, Animations.hit, activeGridNumber);

                MakeShot();

            }
            else if (!tiles[activeGridNumber[0], activeGridNumber[1]].SquidPresent) // No squid present and attackable
            {

                AudioPlaybackEngine.Instance.PlaySound(Sounds.miss);

                Thread.Sleep(100);

                //Run animation
                AnimateTile(display, Animations.miss, activeGridNumber);

                //Set tile to not be attackable
                MakeShot();

                feedBack = "            Miss            ";

            }


            PrintGameInfo(display);

            UpdateGameState();
            return;

        }

        private void UpdateGameState()
        {
            //Win/Fail state check
            if (numberOfSquidAlive == 0)
            {
                //Win
                GameState = 2;
            }
            else if (totalShots - shotsMade == 0)
            {
                //Lose out of shots
                GameState = 4;
            }

        }

        private void MakeShot()
        {
            //Make next cannon ball false in array (fired)
            bombs[shotsMade] = false;
            shotsMade++;

            tiles[activeGridNumber[0], activeGridNumber[1]].Attackable = false;
        }

        private bool UpdateSquidCount(Tile tile) //True means a change has been made
        {
            if (tile.Squid.SquidStatus == false) // I.e if the squid is now dead
            {
                numberOfSquidAlive--;
                return true;
            }

            return false; //No change to squid count
        }




        private void AnimateTile(Display display, TileState[] tileStates, int[] activeGridNumber)
        {

            //Temporarily turn off Crosshair
            tiles[activeGridNumber[0], activeGridNumber[1]].CrosshairBool = false;

            foreach (TileState state in tileStates)
            {
                tiles[activeGridNumber[0], activeGridNumber[1]].SeaState = state;
                PrintGameInfo(display);
                Thread.Sleep(Animations.waitTime);
            }

            //Turn crosshair back on
            tiles[activeGridNumber[0], activeGridNumber[1]].CrosshairBool = true;
        }




    }
}
