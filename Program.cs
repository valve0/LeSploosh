using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml;
using LeSploosh;

internal class Program
{
    private static void Main(string[] args)
    {
        PrintTerminal PrintTerminal = new();

        //int origWidth = Console.WindowWidth;
        //int origHeight = Console.WindowHeight;

        //Console.SetWindowSize(120, 60);


        //%    Set up the game   %//

        //intialise variables
        //One square sized squid
        int numSmallSquid = 0;
        //Two square sized squid
        int numMediumSquid = 0;
        // Three square sized squid
        int numLargeSquid = 0;
        //Four square sized squid
        int numGiantSquid = 0;

        //Map size is a square of the int value i.e 3 = 3x3
        int mapSize = 0;
        int shotCounter = 0;
        bool loop = true;

        do {
            PrintTerminal.PrintLine("Please select a diffulty (Easy/Medium/Hard)");
            string difficulty = PrintTerminal.ReadLine();

            switch (difficulty)
            {
                case "Easy":
                    {
                        mapSize = 3;
                        numSmallSquid = 3;
                        numMediumSquid = 0;
                        numLargeSquid = 0;
                        numGiantSquid = 0;
                        shotCounter = 5;
                        loop = false;
                        break;
                    }

                case "Medium":
                    {
                        mapSize = 4;
                        numSmallSquid = 3;
                        numMediumSquid = 1;
                        numLargeSquid = 0;
                        numGiantSquid = 0;
                        shotCounter = 5;
                        loop = false;
                        break;
                    }

                case "Hard":
                    { 
                        mapSize = 8;
                        numSmallSquid = 0;
                        numMediumSquid = 1;
                        numLargeSquid = 1;
                        numGiantSquid = 1;
                        shotCounter = 5;
                        loop = false;
                        break;
                    }

                default:
                    PrintTerminal.PrintLine("Please select a correct difficulty.");
                    break;

            }
        } while (loop);


        //%    The Gameplay Loop   %//
        

        GameInfo Game = new GameInfo(numSmallSquid, numMediumSquid, numLargeSquid, numGiantSquid, mapSize, shotCounter);
        string directory = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName) + @"\LeSploosh\Text Files\";
        string winFile = directory + "YouWin.txt";
        string loseFile = directory + "YouLose.txt";
        int noSquidRemaining = Game.NumberOfSquid;

        // The Game Loop
        Console.Clear();
        //Load the static border files
        
        PrintTerminal.PrintGameInfo(Game);
        do
        {
           
            var ch = Console.ReadKey(false).Key;

            switch (ch)
            {
                case ConsoleKey.Spacebar:
                    Game.Attack();
                    break;
                case ConsoleKey.UpArrow:
                    Game.MoveCursorUp();
                    break;
                case ConsoleKey.DownArrow:
                    Game.MoveCursorDown();
                    break;
                case ConsoleKey.LeftArrow:
                    Game.MoveCursorLeft();
                    break;
                case ConsoleKey.RightArrow:
                    Game.MoveCursorRight();
                    break;
                default:
                    //Invalid selection: do nothing
                    break;
            }

        } while(Game.NumberOfSquid > 0 && Game.ShotCounter > 0);

        //%    End conditions   %//

        Console.Clear();

        if (Game.ShotCounter == 0)
        {
            PrintTerminal.PrintLine("You ran out of cannon balls!");
        }
        else
        {
            PrintTerminal.PrintLine("There aren't enough cannon balls left to get them all!");
        }

        string endState = (Game.NumberOfSquid == 0) ? winFile : loseFile;
        PrintTerminal.PrintFile(endState);


      
    }
}