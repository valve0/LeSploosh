using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml;
using LeSploosh;

internal class Program
{
    private static void Main(string[] args)
    {

        //Makes Window holding Console full screen
        DisplaySetup Display = new(); 
         
        PrintTerminal PrintTerminal = new();

        int origWidth = Console.WindowWidth;
        int origHeight = Console.WindowHeight;
        //Console.SetWindowSize(120, 50);


        //%    Set up the game   %//

        //Setting up the variables for the game
        int mapSize = 8;
        int numSmallSquid = 0;
        int numMediumSquid = 1;
        int numLargeSquid = 1;
        int numGiantSquid = 1;
        int shotCounter = 24;
 
        //%    The Gameplay Loop   %//


        GameInfo Game = new GameInfo(numSmallSquid, numMediumSquid, numLargeSquid, numGiantSquid, mapSize, shotCounter);
        string directory = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName) + @"\LeSploosh\Text Files\";
        string winFile = directory + "YouWin.txt";
        string loseFile = directory + "YouLose.txt";
        int noSquidRemaining = Game.NumberOfSquid;

        // The Game Loop
        Console.Clear();
        //Load the static border files
        
        PrintTerminal.PrintGameInfo(Game, "");
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
            PrintTerminal.PrintString("You ran out of cannon balls!");
        }
        else
        {
            PrintTerminal.PrintString("There aren't enough cannon balls left to get them all!");
        }

        string endState = (Game.NumberOfSquid == 0) ? winFile : loseFile;
        PrintTerminal.PrintFile(endState);


      
    }
}