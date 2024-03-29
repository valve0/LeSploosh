﻿using System.Diagnostics;
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


        Display display = new();

        PrintTerminal.PrintFile("Setup.txt");
        Console.ReadLine();
        Console.Clear();

        //%    Set up the game   %//


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
                Game.PrintIntro();
                introPlayed = true;
            }


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

            }

            //%    End conditions   %//
            //gameComplete = true;

            playAgain = Game.PrintEnd();

            Console.BackgroundColor = ConsoleColor.Blue;
            //Console.Clear();

        } while (playAgain == true);

        //Console.Clear();

    }

}