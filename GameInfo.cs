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
    internal class GameInfo
    {
        private List<int[]> squidPositions = new List<int[]>();
        public int[] ActiveGridNumber { get; set; }
        public int NumberOfSquid { get; set; }
        private int NumSmallSquid { get; init; }
        private int NumMediumSquid { get; init; }
        private int NumLargeSquid { get; init; }
        private int NumGiantSquid { get; init; }
        public int Size { get; init; }
        private int NumberOfTiles { get; init; }
        public int ShotsLeft { get; set; }

        //Needed?
        public static string PrintPosition = "c";
        public int ShotsMade { get; set; }

        public Tile[,] Tiles { get; init; }

        private List<Squid> listOfSquids;

        public int HighScore { get; set; }

        public bool[] CannonBalls { get; set; }

        public bool[] SquidImages { get; set; }

        public string FeedBack { get; set; }

        public int GameState { get; set; }

        private int SquidKilledCounter { get; set; }

        private (string name, int squidSize, int noSquid)[] squidTuples;




        //Store constants e.g. height padding, textfile line length & width
        private static string heightPadding = string.Concat(Enumerable.Repeat($"\n", 10));
        private static int txtFileLength = 3;
        private static int txtFileWidth = 8;


        private static string directory = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName) + @"\LeSploosh\Text Files\";


        private static string weclomeString = "WelcomeTo.txt";
        private static string titleCard = "TitleCard.txt";


        
        public GameInfo((string name, int squidSize, int noSquid)[] squidTuples, int gridSize, int totalShots)
        {

             listOfSquids = new List<Squid>();

            this.squidTuples = squidTuples;
                  
            int numberOfDifferentSquid = 0;


            foreach (var squidTuple in squidTuples)
            {
                if (squidTuple.noSquid > 0)
                    numberOfDifferentSquid++;

                NumberOfSquid += squidTuple.noSquid;

            }

                 
            this.GameState = 0;

            this.FeedBack = string.Empty;


            this.Tiles = new Tile[gridSize, gridSize]; //Create a 2d array of size mapsize
            this.Size = gridSize;
            this.NumberOfTiles = gridSize * gridSize;
            this.ShotsLeft = totalShots;
            this.ShotsMade = 0;

            this.SquidKilledCounter = 0;

            this.HighScore = int.Parse(TextFileRepository.LoadStringFromFile("HighScore.txt").Replace("\r\n", string.Empty));

            this.CannonBalls = new bool[totalShots];
            this.SquidImages = new bool[NumberOfSquid];

            //Set each value initally to true = cannonball available
            for (int i = 0; i < this.CannonBalls.Length; i++)
            {
                CannonBalls[i] = true;
            }

            //Set each value initally to true = squid alive
            for (int i = 0; i < this.SquidImages.Length; i++)
            {
                SquidImages[i] = true;
            }



            //Fill array of tiles with new tiles, setting them to default start value
            for (int row = 0; row < gridSize; row++)
            {
                for (int col = 0; col < gridSize; col++)
                {
                    //Fill list with default value of sea state
                    int[] tilePosition = { row, col };

                    Tiles[row, col] = new Tile(tilePosition);
                }
            }

            //Set the first tile to have the crosshair to begin with.
            Tiles[0, 0].CrosshairBool = true;
            //first grid number is defeault start position for crosshair
            this.ActiveGridNumber = new int[] { 0, 0 };
    
            PlaceSquids();

            var test = listOfSquids;
            var test2 = 1;
        }


        public void UpdateGameState()
        {
            //Win/Fail state check
            if (NumberOfSquid == 0)
            {
                //Win
                GameState = 2;
            }
            else if (ShotsLeft == 0)
            {
                //Lose out of shots
                GameState = 4;
            }

        }


        private void PlaceFirstPartOfSquid(ref List<int[]> squidPartPositions)
        {

            Random Random = new Random();
            //Choose a random spot to place the first part of the squid
            bool firstPartPlaced = false;

            do // Keep looping until squid placed
            {
                //Randomly place first part
                squidPartPositions[0][0] = Random.Next(Size);
                squidPartPositions[0][1] = Random.Next(Size);

                //check to see if no squid already in this spot
                if (Tiles[squidPartPositions[0][0], squidPartPositions[0][1]].SquidPresent == false) 
                    firstPartPlaced = true;

            } while (firstPartPlaced == false);


        }


        private bool PlaceSubsequentSquidPart((string name, int squidSize, int noSquid) squidTuple, int squidNo, int squidPart, int direction, ref List<int[]> squidPartPositions)
        {
            //Generate a rnadom number between 0 and 3 inclusive
            //Chose a random direction (0 = up, 1 = right, 2 = down, 3 = left)

            //Based on the first part of the squid generate the following tiles int he supplied direction
            int[] nextTile = GenerateNextTile(squidPartPositions[0][0], squidPartPositions[0][1], squidPart, direction);
            int nextTileRow = nextTile[0];
            int nextTileCol = nextTile[1];

            //Make sure new grid reference is valid (on board and no squid present)
            if (nextTileRow >= 0 && nextTileRow < Size && nextTileCol >= 0 && nextTileCol < Size && Tiles[nextTileRow, nextTileCol].SquidPresent == false)
            {

                // Add squid position to list of squid positions
                squidPartPositions[squidPart][0] = nextTileRow;
                squidPartPositions[squidPart][1] = nextTileCol;

                //check to see if at end of loop and therefore last part of squid placed successfully
                if (squidPart + 1 == squidTuple.squidSize)
                {

                    //Instantiate squid object add to list of squids
                    listOfSquids.Add(new Squid { Id = squidNo, Size = squidTuple.squidSize, HitCounter = 0, SquidStatus = true });


                    //Loop through all the current squid positions and create a squid object in their locations
                    foreach (var positon in squidPartPositions)
                    {
                        // Place a squid at this position
                        Tiles[positon[0], positon[1]].SetSquid(listOfSquids.Last());

                        
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

            int[] nextTiles = new int[2];
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

            nextTiles[0] = startTileRow + (rowAdder * squidPart);
            nextTiles[1] = startTileCol + (colAdder * squidPart);

            return nextTiles;

        }


        internal void PlaceSquids()
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
                        for (int i = 0; i < squidTuple.squidSize; i++)
                        {
                            int[] array = new int[2];
                            squidPartPositions.Add(array);
                        }
                        //genreate new direction
                        Random Random = new Random();
                        int direction = Random.Next(4);

                        // Loop through the parts for the given squid
                        for (int squidPart = 0; squidPart < squidTuple.squidSize; squidPart++)
                        {

                            //Place first part of squid
                            if (squidPart == 0)
                            {
                                //Get start position
                                PlaceFirstPartOfSquid(ref squidPartPositions);
                            }
                            else
                            {
                                squidPartPlaced = PlaceSubsequentSquidPart(squidTuple, squidNo, squidPart, direction, ref squidPartPositions);

                                if (squidPartPlaced == false)
                                    break; //Redo squid place loop
                                else if (squidPartPlaced == true && squidPart + 1 == squidTuple.squidSize)
                                    squidPlaced = true;
                            }


                        }

                    } while (squidPlaced == false);

                }

            }

        }



        public void MoveCursor(ConsoleKey key)
        {
            //Check to see if not at edges of grid
            if (ValidMove(key))
            {

                //Set current tile cross hair status to false
                Tiles[ActiveGridNumber[0], ActiveGridNumber[1]].CrosshairBool = false;

                MoveActiveGrid(key);

                //Set current tile crosshair status to true
                Tiles[ActiveGridNumber[0], ActiveGridNumber[1]].CrosshairBool = true;
                FeedBack = "";
                PrintGameInfo();

                //Play sound moving cursor
                AudioPlaybackEngine.Instance.PlaySound("CursorMove.mp3");


            }
        }


        private bool ValidMove(ConsoleKey key)
        {
            if (ActiveGridNumber[0] == 0 && key == ConsoleKey.UpArrow) //At top
                return false;

            else if (ActiveGridNumber[0] == Size - 1 && key == ConsoleKey.DownArrow) //At bottom
                return false;

            else if (ActiveGridNumber[1] == 0 && key == ConsoleKey.LeftArrow) //At left edge
                return false;

            else if (ActiveGridNumber[1] == Size - 1 && key == ConsoleKey.RightArrow) //At right edge
                return false;
            else
                return true;
        }


        private void MoveActiveGrid(ConsoleKey key)
        {

            switch (key)
            {
                case ConsoleKey.UpArrow:
                    ActiveGridNumber[0]--;
                    break;
                case ConsoleKey.DownArrow:
                    ActiveGridNumber[0]++;
                    break;
                case ConsoleKey.LeftArrow:
                    ActiveGridNumber[1]--;
                    break;
                case ConsoleKey.RightArrow:
                    ActiveGridNumber[1]++;
                    break;
            }
        }


        public void Attack()
        {

            if (!Tiles[ActiveGridNumber[0], ActiveGridNumber[1]].Attackable) // Tile not attackable
            {

                FeedBack = "          Cannot attack this square          ";

            }
            //Assumption that Attack Check is run before this method
            else if (Tiles[ActiveGridNumber[0], ActiveGridNumber[1]].SquidPresent) // Squid present and attackable
            {
                Tiles[ActiveGridNumber[0], ActiveGridNumber[1]].Squid.IncreaseHitCounter();

                if (!UpdateSquidCount(Tiles[ActiveGridNumber[0], ActiveGridNumber[1]]))
                {
                    AudioPlaybackEngine.Instance.PlaySound("kaboom.mp3");
                    FeedBack = "          Squid Hit!          ";
                }
                else
                {
                    AudioPlaybackEngine.Instance.PlaySound("SquidDead.mp3");
                    FeedBack = "          Squid Killed!          ";
                    SquidImages[SquidKilledCounter] = false;
                    SquidKilledCounter++;

                }

                Thread.Sleep(100);

                //Run animation
                AnimateTile(Animations.hit, ActiveGridNumber);

                MakeShot();

            }
            else if (!Tiles[ActiveGridNumber[0], ActiveGridNumber[1]].SquidPresent) // No squid present and attackable
            {

                AudioPlaybackEngine.Instance.PlaySound("sploosh.mp3");

                Thread.Sleep(100);

                //Run animation
                AnimateTile(Animations.miss, ActiveGridNumber);

                //Set tile to not be attackable
                MakeShot();

                FeedBack = "            Miss            ";

            }


            PrintGameInfo();
            return;

        }

        public void MakeShot()
        {
            //Make next cannon ball false in array (fired)
            CannonBalls[ShotsMade] = false;
            ShotsLeft--;
            ShotsMade++;

            Tiles[ActiveGridNumber[0], ActiveGridNumber[1]].Attackable = false;
        }

        public bool UpdateSquidCount(Tile tile) //True means a change has been made
        {
            if (tile.Squid.SquidStatus == false) // I.e if the squid is now dead
            {
                NumberOfSquid--;
                return true;
            }

            return false; //No change to squid count
        }

        public void PrintTitleCard()
        {
            int linesInTitle = 17;

            //In miliseconds
            int waitTime = 2000;

            string welcomeStartHeight = string.Concat(Enumerable.Repeat($"\n", 50));

            PrintTerminal.PrintString(welcomeStartHeight);

            PrintTerminal.PrintFile("WelcomeTo.txt");
            Thread.Sleep(waitTime);

            //var cursorPos = Console.GetCursorPosition();

            //Move Welcome to down
            for (int i = 0; i < 50; i++)
            {
                //Print null string
                PrintTerminal.PrintString("                                                                                                                                                              ", cursorTop: Console.CursorTop - 1);

                //Move up werlcome to string one line at a time
                PrintTerminal.PrintFile(weclomeString, cursorTop: Console.CursorTop - linesInTitle);

                Thread.Sleep(100);
            }


            Thread.Sleep(1000);
            PrintTerminal.PrintString($"\n\n");
            PrintTerminal.PrintFile(titleCard);


            Thread.Sleep(1500);
            PrintTerminal.PrintString();
            PrintTerminal.PrintString("Press any key to continue");
            Thread.Sleep(200);
            Console.ReadKey(false);

            //REMOVE?
            Console.BackgroundColor = ConsoleColor.Blue;

            Console.Clear();
        }


        public void Intro()
        {

            PrintTitleCard();

            PrintTerminal.PrintFile("Salvatore.txt", 0.66f);

            PrintTerminal.PrintFile("IntroScript1.txt", 0.33f, cursorTop: 5);
            PrintTerminal.PrintString();

            bool selection = PrintTerminal.PrintSelection("Selection1L.txt", "Selection1R.txt", 0.33f);
            PrintTerminal.PrintString();

            if (selection == false)
            {
                GameState = 1;
                return; // leave intro
            }
                

            PrintTerminal.PrintFile("IntroScript2.txt", 0.33f);
            PrintTerminal.PrintString();

            PrintTerminal.PrintSelection("Selection2L.txt", "Selection2R.txt", 0.33f);
            PrintTerminal.PrintString();


            string finalLine = $"Excellant. So far our best sailor has managed to destroy all of zee giant squid using only {HighScore} cannonballs! May you fight as bravely!";
            PrintTerminal.PrintString(finalLine, verticalAlignment: 0.33f);

            Console.ReadKey(false);

            Console.BackgroundColor = ConsoleColor.Blue;
            Console.Clear();

        }


        public bool PrintEnd()
        {


            string HishScoreFileName = "HighScore.txt";

            //Hide the crosshair
            Tiles[ActiveGridNumber[0], ActiveGridNumber[1]].CrosshairBool = false;

            string script = string.Empty;
            bool highScoreMade = false;

            switch (GameState)
            {

                case 1:
                    script = "StraightQuit.txt";
                    return false;
                    

                case 2:
                    script = "WinScript.txt";
                    break;

                case 3:
                    script = "WinScript.txt";
                    highScoreMade = true;
                    break;

                case 4:
                    script = "LoseScript.txt";
                    ShowRemainingSquidParts();
                    break;

            }


            if (GameState != 1)
            {

                FeedBack = "           Press any key to continue          ";
                PrintGameInfo();
                //Wait for user input before continuing
                Console.ReadKey(false);
            }

            //Remove previous screen
            Console.Clear();
            //Print salvatore on the right
            PrintTerminal.PrintFile("Salvatore.txt", 0.66f);


            PrintTerminal.PrintFile(script, 0.33f, cursorTop: 5);

            if (highScoreMade == true)
                PrintTerminal.PrintString($"Whoa. Wait a sec. {ShotsMade}?! That's a new record...", 0.33f);


            if (ShotsMade < HighScore && GameState == 2)
            {
                TextFileRepository.WriteStringToFile(HishScoreFileName, ShotsMade.ToString());
            }

            return PlayAgain();

        }

        private void ShowRemainingSquidParts()
        {

            foreach (var squid in listOfSquids)
            {
                //For each squid loop through their positioins. 
                
                for(int i = 0; i < squid.squidPositions.Count; i++) 
                {
                    // If the tile at the position does not show a hit- change its state to a "Squid Here"
                    if (Tiles[squid.squidPositions[i][0],squid.squidPositions[i][1]].SeaState != (TileState) 2)
                    {
                        //Change non hit squid tile to "Squid Here state"
                        Tiles[squid.squidPositions[i][0], squid.squidPositions[i][1]].SeaState = (TileState) 8;
                    }
                }
            }
        }


        public static bool PlayAgain()
        {

            PrintTerminal.PrintString("Would you like to play again?", 0.33f);

            return PrintTerminal.PrintSelection("PlayAgainL.txt", "PlayAgainR.txt", 0.33f, color: "w");
        }

        private void PrintSquidRemaining()
        {

            string SquidCol = string.Empty;

            for (int i = 0; i < SquidImages.Length; i++)
            {
                SquidCol = SquidCol + this.ReturnSquidString(i) +"\n";
            }

            PrintTerminal.PrintString(SquidCol, verticalAlignment: 0.63f, cursorTop: 24);
  
        }

        private string ReturnSquidString(int squidNumber)
        {
            TextFileRepository TextFileRepository = new();
            string fileName = string.Empty;

            if (SquidImages[squidNumber] == true)
                fileName = "Squid1.txt";
            else
                fileName = "Squid2.txt";

            string str = TextFileRepository.LoadStringFromFile(fileName);
            return str;
        }


        private void AnimateTile(TileState[] tileStates, int[] ActiveGridNumber)
        {

            //Temporarily turn off Crosshair
            Tiles[ActiveGridNumber[0], ActiveGridNumber[1]].CrosshairBool = false;

            foreach (TileState state in tileStates)
            {
                Tiles[ActiveGridNumber[0], ActiveGridNumber[1]].SeaState = state;
                PrintGameInfo();
                Thread.Sleep(Animations.waitTime);
            }

            //Turn crosshair back on
            Tiles[ActiveGridNumber[0], ActiveGridNumber[1]].CrosshairBool = true;
        }

        public void PrintGameInfo()
        {

            Console.SetCursorPosition(0, 0);

            //Load the static border files
            string borderVert = TextFileRepository.LoadStringFromFile("BorderVert.txt");
            string borderHor = TextFileRepository.LoadStringFromFile("BorderHor.txt").Replace("\r\n", string.Empty);
            string frameEndL = TextFileRepository.LoadStringFromFile("FrameEndL.txt").Replace("\r\n", string.Empty);
            string frameEndR = TextFileRepository.LoadStringFromFile("FrameEndR.txt").Replace("\r\n", string.Empty);
            string frameMid = TextFileRepository.LoadStringFromFile("FrameMid.txt").Replace("\r\n", string.Empty);

            string position = GameInfo.PrintPosition;

            //Create a string based on the chacrter supplied and to the length of txtFileWdith * Size + 1
            frameMid = String.Concat(Enumerable.Repeat(borderHor, txtFileWidth * Size + 1));
            borderHor = String.Concat(Enumerable.Repeat(borderHor, txtFileWidth * Size + 1));

            string topFrame = frameEndL + frameMid + frameEndR;

            //Title Card
            PrintTerminal.PrintFile(titleCard);
            PrintTerminal.PrintString();
            PrintTerminal.PrintString();
       

            // TOP FRAME
            //PrintString(heightPadding);
            PrintTerminal.PrintString($"High Score {HighScore}");
            PrintTerminal.PrintString();
            PrintTerminal.PrintString(topFrame);

            //BORDER TOP
            PrintTerminal.PrintString();
            PrintTerminal.PrintString();
            PrintTerminal.PrintString(borderHor);


            List<string> listOfStrings = new List<string>();

            //Loop through all the tiles in the 2d array
            for (int row = 0; row < Size; row++)
            {
                //Clean list of previous row strings
                listOfStrings.Clear();

                //Create array of strings you wish to add side by side
                for (int col = 0; col < Size; col++)
                {
                    listOfStrings.Add(borderVert);
                    listOfStrings.Add(Tiles[row, col].ReturnTileString());

                    if (col == Size - 1)
                        listOfStrings.Add(borderVert); //Add final Vertical border at end of string
                }


                PrintTerminal.PrintString(PrintTerminal.ReturnStringsSideBySide(listOfStrings));
                //After each row of tiles is printed- print a long single horizontal border before the next.
                PrintTerminal.PrintString(borderHor);

            }


            //Printing bottom frame


            PrintTerminal.PrintString();

            PrintTerminal.PrintString("'Arrows Keys' to move the crosshair and 'Space' to attack");
            PrintTerminal.PrintString();

            //As we are not clearing the screen after each print- we wnat to make sure this line gets cleared othersiwse it can remnain as a ghost
            if(FeedBack.Length > 0)
                PrintTerminal.PrintString(FeedBack);
            else
                PrintTerminal.PrintString();

            PrintTerminal.PrintString();
            PrintTerminal.PrintString(topFrame);



            PrintCannonBalls();

            PrintSquidRemaining();



        }

        private void PrintCannonBalls()
        {

            //genertate a string with cannonballs in array 5 rows 3 columns with correct states printed
            //Collect into a single string and print that to console

            int rows = 8;
            int cols = 3;

            string position = string.Empty;


            StringBuilder sb = new StringBuilder();

            //Loop through rows of 8x3 collection of bombs and create a string for each row and then adding to complete string

            for (int i = 0; i < rows; i++)
            {
                List <string> bombRowList = new List<string>();

                //For 3 columns this will give us a string in the 3rd column, then 2nd, then 1st into an array respectively
                for(int j = cols - 1; j >= 0; j--)
                {
                    //Add to end of list
                    bombRowList.Add(ReturnBombString(j * rows + i));
                }

                sb.AppendLine(PrintTerminal.ReturnStringsSideBySide(bombRowList));
            }

            PrintTerminal.PrintString(sb.ToString(), verticalAlignment:0.37f, cursorTop: 25);

            PrintTerminal.PrintString($"Bombs remaining: {ShotsLeft}  ", verticalAlignment: 0.37f, cursorTop: 23);
            

        }
        

        private string ReturnBombString(int CannonBallNumber)
        {
            TextFileRepository TextFileRepository = new();
            string fileName = string.Empty;

            if (CannonBalls[CannonBallNumber] == true)
                fileName = "Bomb1.txt";
            else
                fileName = "Bomb2.txt";


            string str = TextFileRepository.LoadStringFromFile(fileName);
            return str;
        }




    }
}
