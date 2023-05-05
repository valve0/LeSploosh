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

        Console.BackgroundColor = ConsoleColor.Blue;
        Console.Clear();

        // later in the app...

        //AudioPlaybackEngine.Instance.PlaySound("crash.wav");

        //Makes Window holding Console full screen
        DisplaySetup Display = new();

        //Create PrintTerminal object-necesssray if all static?
        PrintTerminal PrintTerminal = new();


        //Console.BackgroundColor = ConsoleColor.Blue;
        //Console.Clear();

        //%    Set up the game   %//

        //Play Intro

        bool introPlayed = true;
        bool playAgain = false;

        //Play background music
        AudioPlaybackEngine.Instance.PlaySound("pirateship.mp3");

        do
        {

            //Setting up the variables for the game
            int gridSize = 8;
            int totalShots = 24;


            (string name, int squidSize, int noSquid)[] squidTuples = new (string name, int squidSize, int noSquid)[]
            {
                ("small", 1, 0),
                ("medium", 2, 1),
                ("large", 3, 1),
                ("giant", 4, 1)
            };

            GameInfo Game = new GameInfo(squidTuples, gridSize, totalShots);


            //string directory = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName) + @"\LeSploosh\Text Files\";


            //Console.BackgroundColor = ConsoleColor.Blue;
            //Console.Clear();

            //if (introPlayed == false)
            //    Game.PrintIntro();
            //Console.BackgroundColor = ConsoleColor.Blue;
            ////Console.Clear();

            

            //%    The Gameplay Loop   %//


            while (Game.GameState == 0)
            {
                Game.PrintGameInfo();
                var key = Console.ReadKey(false).Key;

                switch (key)
                {
                    case ConsoleKey.Spacebar:
                        Game.Attack();
                        break;

                    case ConsoleKey.UpArrow:
                    case ConsoleKey.DownArrow:
                    case ConsoleKey.LeftArrow:
                    case ConsoleKey.RightArrow:
                        Game.MoveCursor(key);
                        break;

                    default:
                        //Invalid selection: do nothing
                        break;
                }

                Game.UpdateGameState();

            }

            //%    End conditions   %//
            //gameComplete = true;

            playAgain = Game.Ending();

            Console.BackgroundColor = ConsoleColor.Blue;
            Console.Clear();

        } while (playAgain == true);

        Console.Clear();
        //Console.BackgroundColor = ConsoleColor.Blue;

        //Print salvatore on the right
        PrintTerminal.PrintFile("Salvatore.txt", 0.66f);

        // Move cursor down a little from top (padding)
        Console.SetCursorPosition(0, 5);

        PrintTerminal.PrintFile("QuitScript.txt", 0.33f);

    }

}