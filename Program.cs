using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml;
using LeSploosh;
using LeSploosh.Audio;
using NAudio.Wave;
using System.Runtime.InteropServices;

internal class Program
{

    // Structure used by GetWindowRect
    struct Rect
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }



    private static void Main(string[] args)
    {

        // Import the necessary functions from user32.dll
        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        [DllImport("user32.dll")]
        static extern bool GetWindowRect(IntPtr hWnd, out Rect lpRect);
        [DllImport("user32.dll")]
        static extern bool MoveWindow(IntPtr hWnd, int x, int y, int nWidth, int nHeight, bool bRepaint);
        // Constants for the ShowWindow function
        const int SW_MAXIMIZE = 3;
        // Get the handle of the console window
        IntPtr consoleWindowHandle = GetForegroundWindow();
        // Maximize the console window
        ShowWindow(consoleWindowHandle, SW_MAXIMIZE);
        // Get the screen size
        Rect screenRect;
        GetWindowRect(consoleWindowHandle, out screenRect);
        // Resize and reposition the console window to fill the screen
        int width = screenRect.Right - screenRect.Left;
        int height = screenRect.Bottom - screenRect.Top;
        MoveWindow(consoleWindowHandle, screenRect.Left, screenRect.Top, width, height, true);



























        //Console.WriteLine("Before");
        Console.WriteLine("| ~ | ~ |");
        //Console.WriteLine($"Window height: {Console.WindowHeight}");
        //Console.WriteLine($"Window width: {Console.WindowWidth}");
        //Console.WriteLine($"Largest Window height {Console.LargestWindowHeight}");
        //Console.WriteLine($"Largest window width {Console.LargestWindowWidth}");
        //Console.WriteLine($"Buffer height {Console.BufferHeight}");
        //Console.WriteLine($"Buffer width {Console.BufferWidth}");
        Console.WriteLine($"{Console.CursorSize}");
        Console.ReadKey();
        Console.WriteLine($"{Console.CursorSize}");
        Console.ReadKey();


        //Makes Window holding Console full screen
        Display display = new();

        //Console.WriteLine("After");
        //Console.WriteLine("| ~ | ~ |");
        //Console.WriteLine($"Window height: {Console.WindowHeight}");
        //Console.WriteLine($"Window width: {Console.WindowWidth}");
        //Console.WriteLine($"Largest Window height {Console.LargestWindowHeight}");
        //Console.WriteLine($"Largest window width {Console.LargestWindowWidth}");
        //Console.WriteLine($"Buffer height {Console.BufferHeight}");
        //Console.WriteLine($"Buffer width {Console.BufferWidth}");

        //Console.ReadKey();
        //Console.SetCursorPosition(0, 0);

        //%    Set up the game   %//
        //Console.SetWindowSize(200, 60);

        bool introPlayed = false;
        bool playAgain = false;

        //Play background music
        AudioPlaybackEngine.Instance.PlaySound(Sounds.backgroundMusic);

        do
        {

            //Setting up the variables for the game. These can all be tweeked.
            int gridsize = 8;
            int totalShots = 24;
            (string name, int squidsize, int noSquid)[] squidTuples = new (string name, int squidsize, int noSquid)[]
            {
                ("small", 1, 0),
                ("medium", 2, 1),
                ("large", 3, 1),
                ("giant", 4, 1)
            };

            GameInfo Game = new GameInfo(squidTuples, gridsize, totalShots);


            if (introPlayed == false)
            {
                Game.PrintIntro(display);
                introPlayed = true;
            }


            //%    The Gameplay Loop   %//


            while (Game.GameState == 0)
            {
                Game.PrintGameInfo(display);
                var key = Console.ReadKey(false).Key;

                switch (key)
                {
                    case ConsoleKey.Spacebar:
                        Game.Attack(display);
                        break;

                    case ConsoleKey.UpArrow:
                    case ConsoleKey.DownArrow:
                    case ConsoleKey.LeftArrow:
                    case ConsoleKey.RightArrow:
                        Game.MoveCursor(display, key);
                        break;

                    default:
                        //Invalid selection: do nothing
                        break;
                }

            }

            //%    End conditions   %//
            //gameComplete = true;

            playAgain = Game.PrintEnd(display);

            Console.BackgroundColor = ConsoleColor.Blue;
            Console.Clear();

        } while (playAgain == true);

        Console.Clear();
        //Console.BackgroundColor = ConsoleColor.Blue;

        //Print salvatore on the right
        PrintTerminal.PrintFile("Salvatore.txt", display.salvatoreAlignment);

        PrintTerminal.PrintFile("QuitScript.txt", display.scriptAlignment, cursorTop: 5);

    }

}