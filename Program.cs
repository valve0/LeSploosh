using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml;
using LeSploosh;
using LeSploosh.Audio;
using NAudio.Wave;

internal class Program
{
    private static void Main(string[] args)
    {



        // later in the app...
        
        //AudioPlaybackEngine.Instance.PlaySound("crash.wav");

        //Makes Window holding Console full screen
        DisplaySetup Display = new(); 
         
        PrintTerminal PrintTerminal = new();

        int origWidth = Console.WindowWidth;
        int origHeight = Console.WindowHeight;
        //Console.SetWindowSize(120, 50);


        //%    Set up the game   %//

        //Play background music
        AudioPlaybackEngine.Instance.PlaySound("pirateship.mp3");

        //Start Intro
        PrintTerminal.PrintIntro();

        

        //Setting up the variables for the game
        int gridSize = 8;
        int numSmallSquid = 0;
        int numMediumSquid = 1; //1
        int numLargeSquid = 1; //1
        int numGiantSquid = 1; //1
        int shotCounter = 24;
 
        //%    The Gameplay Loop   %//


        GameInfo Game = new GameInfo(numSmallSquid, numMediumSquid, numLargeSquid, numGiantSquid, gridSize, shotCounter);
        string directory = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName) + @"\LeSploosh\Text Files\";

        int noSquidRemaining = Game.NumberOfSquid;


        //Could do this better
        int numberOfSquidParts = numSmallSquid * 1 + numMediumSquid * 2 + numLargeSquid * 3 + numGiantSquid * 4;

        int gameState = 0;
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


            //Win/Fail state check
            if (Game.NumberOfSquid == 0)
            {
                //Win
                gameState = 3;
            }
            else if(Game.ShotCounter == 0)
            {
                //Lose out of shots
                gameState = 1;
            }
            else if (numberOfSquidParts > Game.ShotCounter)
            {
                //Lose not enough cannon balls
                gameState = 2;
            }

        } while(gameState == 0);

        //%    End conditions   %//

        Console.Clear();

        if (gameState == 1)
        {
            //You ran out of cannon balls!
            PrintTerminal.PrintFail(1);
            
        }
        else if (gameState == 2)
        {
            //There aren't enough cannon balls left to get them all!
            PrintTerminal.PrintFail(2);
            
        }
        else
        {
            PrintTerminal.PrintWin();
        }

        // on shutdown close audio engine
        AudioPlaybackEngine.Instance.Dispose();

    }
}