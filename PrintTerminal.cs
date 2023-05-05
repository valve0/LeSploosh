using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace LeSploosh
{
    internal class PrintTerminal
    {

        private static string directory = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName) + @"\LeSploosh\Text Files\";

        public static void PrintString(string line, string color, string position) //Overloaded constructor with no color argument
        {

            //Set the color of the font
            switch (color.ToLower())
            {
                case "b":
                    Console.ForegroundColor = ConsoleColor.Blue;
                    break;

                case "r":
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;

                case "g":
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;

                case "w":
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = ConsoleColor.Blue;
                    break;

            }

            //Get position adjuster value based on position required
            float positionAdjuster = ReturnPositionAdjuster(position);
            
            // Center the output of the string
            Console.SetCursorPosition((int)((Console.WindowWidth * positionAdjuster) - line.Length / 2), Console.CursorTop);
            var test = Console.GetCursorPosition();
 
            //Console.SetCursorPosition((int)((Console.WindowWidth * positionAdjuster) - line.Length / 2), Console.CursorTop);
            //Print the line
            Console.WriteLine(line);

            //Return deafult color of console
            //Console.ForegroundColor = ConsoleColor.Blue;


        }


        public static void PrintString(string line, string color, string position, int longestLineLength) //Overloaded constructor with no color argument
        {

            //Set the color of the font
            switch (color.ToLower())
            {
                case "b":
                    Console.ForegroundColor = ConsoleColor.Blue;
                    break;

                case "r":
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;

                case "g":
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;

                case "w":
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = ConsoleColor.Blue;
                    break;

            }

            //Get position adjuster value based on position required
            float positionAdjuster = ReturnPositionAdjuster(position);

            // Center the output of the string
            Console.SetCursorPosition((int)((Console.WindowWidth * positionAdjuster) - longestLineLength / 2), Console.CursorTop);
            var test = Console.GetCursorPosition();

            //Console.SetCursorPosition((int)((Console.WindowWidth * positionAdjuster) - line.Length / 2), Console.CursorTop);
            //Print the line
            Console.WriteLine(line);

            //Return deafult color of console
            Console.ForegroundColor = ConsoleColor.Blue;


        }


        //Necssary as some files are over multiple lines so string.length would not retrieve the longest line
        public static int ReturnLongestLineLength(string printString)
        {
            int longestLineLength = 0;
            
            using (StringReader reader = new StringReader(printString))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Length > longestLineLength)
                        longestLineLength = line.Length;
                }
            }

            return longestLineLength;
        }


        //Overloaded for no input i.e new line
        public static void PrintString()
        { 
            Console.WriteLine();
            Console.CursorVisible = false;
        }


        public static void PrintFile(string file, string color, string position)
        {
            float positionAdjuster = ReturnPositionAdjuster(position);

            file = directory + file;
            string printString = File.ReadAllText(file);
            using (StringReader reader = new StringReader(printString))
            {
                string line = string.Empty;
                do
                {
                    line = reader.ReadLine();
                    if (line != null)
                    {
                     
                        PrintTerminal.PrintString(line, color, position);
                    }
                } while (line != null);
            }

        }


        private static float ReturnPositionAdjuster(string position)
        {
            switch (position.ToLower())
            {
                case "c": //Center
                    {
                        return 0.5f;
                    }

                case "l":
                    {
                        return 0.33f;                      
                    }

                case "lc":
                    {
                        return 0.40f;
                    }

                case "llc":
                    {
                        return 0.37f;
                    }
                case "lllc":
                    {
                        return 0.34f;
                    }

                case "r":
                    {
                        return 0.66f;                     
                    }
                case "rc":
                    {
                        return 0.66f;
                    }
                default:
                    {
                        return 2;                       
                    }
            }
        }


        public static bool MakeSelection(string leftSelected, string rightSelected, string color, string position)
        {
            //Check currently selected box
            int txtFileHeight = 4;
            bool selection = true; // default to true as arrow starts on the left
            PrintTerminal.PrintString("[Use Arrow Keys to move cursor and space to make selection]", color, position);

            PrintFile(leftSelected, "w", position);
            do
            {
                var test2 = Console.GetCursorPosition();
                
                var ch = Console.ReadKey(false).Key;
  
                switch (ch)
                {
                    case ConsoleKey.Spacebar:
                        return selection;

                    case ConsoleKey.LeftArrow:
                        selection = true;
                        //Move curosr back up thew height of the slection boxes so it can reprint over them                      
                        Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop - txtFileHeight);
                        PrintFile(leftSelected, "w", position);
                        break;

                    case ConsoleKey.RightArrow:
                        selection = false;
                        Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop - txtFileHeight);
                        PrintFile(rightSelected, "w", position);
                        break;
                    default:
                        //Invalid key inputted: do nothing
                        break;
                }
            } while (true); //Loop forever until selection made

            
        }

        public static void PrintGoodBye()
        {
            Console.Clear();
            //Console.BackgroundColor = ConsoleColor.Blue;

            //Print salvatore on the right
            PrintTerminal.PrintFile("Salvatore.txt", "w", "r");

            // Move cursor down a little from top (padding)
            Console.SetCursorPosition(0, 5);

            PrintTerminal.PrintFile("QuitScript.txt", "w", "l");

        }

        public static bool PlayAgain()
        {

            PrintTerminal.PrintString("Would you like to play again?", "w", "l");

            return PrintTerminal.MakeSelection("PlayAgainL.txt", "PlayAgainR.txt", "w", "l");
        }

    }
}
