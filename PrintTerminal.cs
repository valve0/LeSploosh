using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeSploosh
{
    internal class PrintTerminal
    {
        private static string widthPadding = string.Concat(Enumerable.Repeat(" ",Console.LargestWindowWidth/2));
        private static string heightPadding = string.Concat(Enumerable.Repeat($"\n", 10));

        //Store constants textfile line length & width
        private static int txtFileLength = 3;
        private static int txtFileWidth = 8;

        public static void PrintGameInfo(GameInfo GameInfo, string attackResult)
        {
            Console.SetCursorPosition(0, 0);
            //Load the static border files


            
            string padding = widthPadding;

            string borderVert = ASCIRepository.LoadASCIFromFile("BorderVert.txt").Replace("\r\n", string.Empty);
            string borderHor = ASCIRepository.LoadASCIFromFile("BorderHor.txt").Replace("\r\n", string.Empty);
            string frameEndL = ASCIRepository.LoadASCIFromFile("FrameEndL.txt").Replace("\r\n", string.Empty);
            string frameEndR = ASCIRepository.LoadASCIFromFile("FrameEndR.txt").Replace("\r\n", string.Empty);
            string frameMid = ASCIRepository.LoadASCIFromFile("FrameMid.txt").Replace("\r\n", string.Empty);
            
            //Create a string based on the chacrter supplied and to the length of txtFileWdith * Size + 1
            frameMid = String.Concat(Enumerable.Repeat(borderHor, txtFileWidth * GameInfo.Size + 1));
            borderHor = String.Concat(Enumerable.Repeat(borderHor, txtFileWidth * GameInfo.Size + 1));

            string topFrame = frameEndL + frameMid + frameEndR;

            // TOP FRAME
            PrintString(heightPadding);
            PrintString(topFrame);

            //BORDER TOP
            PrintString();
            PrintString();
            PrintString(borderHor);

            string[] rowStrings = new string[txtFileLength];

            //Loop through all the tiles in the 2d array
            for (int row = 0; row < GameInfo.Size; row++)
            {

                for (int col =  0; col < GameInfo.Size; col++)
                {


                    string tileString = GameInfo.Tiles[row, col].ReturnTileString();

                    int counter = 0;
                    

                    using (StringReader reader = new StringReader(tileString))
                    {
                        string line = string.Empty;
                        do
                        {
                            line = reader.ReadLine();
                            if (line != null)
                            {
                                // do something with the line
                                rowStrings[counter] = (rowStrings[counter] + line + borderVert).Replace("\r\n", string.Empty);

                                counter++;
                            }

                        } while (line != null);
                    }

                } 

                //Print each row of grid and clear ready for next row
                foreach (string rowString in rowStrings)
                {
                    //Add a vertical border to the start of each line
                    string tileLine = borderVert + rowString;

                    PrintString(tileLine);
                }

                //After each row of tiles is printed- print a long single horizontal border before the next.
                PrintString(borderHor);

                //Empty the array of strings ready for next row
                for (int i = 0; i < rowStrings.Length; i++)
                    rowStrings[i] = string.Empty;
                        
            }

          
            //Printing bottom frame
            

            string squidRemaining = $"Number of squid remaining: {GameInfo.NumberOfSquid}";
            string shotsRemaining = $"Number of shots remaining: {GameInfo.ShotCounter}";
            string userInstructions = "Please use the Arrows Keys to move the crosshair and press Space to attack";

            PrintString();
            PrintString(squidRemaining);
            PrintString(shotsRemaining);
            PrintString(userInstructions);
            PrintString();
            PrintString(attackResult);

            
            PrintString();
            PrintString(topFrame);

        }


        public static void PrintString(string line, string color)
        {
            
            // Center the output of the string
            Console.SetCursorPosition((Console.WindowWidth - line.Length) / 2, Console.CursorTop);

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

            }

            Console.WriteLine(line);

            //Return deafult color of console
            Console.ForegroundColor = ConsoleColor.Blue;

        }

        public static void PrintString(string line) //Overloaded constructor with no color argument
        {

            Console.BackgroundColor = ConsoleColor.Blue;

            // Center the output of the string
            Console.SetCursorPosition((Console.WindowWidth - line.Length) / 2, Console.CursorTop);

            Console.WriteLine(line);

        }

        //Overloaded for no input i.e new line
        public static void PrintString()
        { Console.WriteLine(); }

        public static string ReadLine()
        { return Console.ReadLine(); }


        public static void PrintFile(string file)
        {
            string printString = File.ReadAllText(file);
            using (StringReader reader = new StringReader(printString))
            {
                string line = string.Empty;
                do
                {
                    line = reader.ReadLine();
                    if (line != null)
                    {
                        Console.SetCursorPosition((Console.WindowWidth - line.Length) / 2, Console.CursorTop);
                        PrintTerminal.PrintString(line);
                    }
                } while (line != null);
            }

        }

        public static void AnimateTile(TileState state, int[] ActiveGridNumber, GameInfo Game)
        {
            Game.Tiles[ActiveGridNumber[0], ActiveGridNumber[1]].SeaState = state;
            Console.SetCursorPosition(0, 0);
            PrintGameInfo(Game, "");
            Thread.Sleep(Animations.waitTime);
        }


    }
}
