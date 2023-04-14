using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    internal class PrintTerminal
    {
        static string padding = "   ";
        static string padding2 = "       ";

        public static void PrintGameInfo(GameInfo GameInfo)
        {

            //Load the static border files
            ASCIRepository ASCIRepository = new();

            string[] borderVert = ASCIRepository.LoadASCIFromFile("BorderVert.txt");
            string[] borderHor = ASCIRepository.LoadASCIFromFile("BorderHor.txt");
            string[] frameEndL = ASCIRepository.LoadASCIFromFile("FrameEndL.txt");
            string[] frameEndR = ASCIRepository.LoadASCIFromFile("FrameEndR.txt");
            string[] frameMid = ASCIRepository.LoadASCIFromFile("FrameMid.txt");

            //Store constant textfile line length
            int txtFileLength = 3;

            int numberOfTiles = GameInfo.Tiles.Length;
            int mapSize = GameInfo.GameInfoSize;



            //Create an list that will store each line and threrofe ech row of the grid as strings
            //A row in the grid is made up of txtFileLength * GameInfoSize seperate strings
            List<string> strings = new List<string>();

            for (int i = 0; i < txtFileLength * mapSize; i++)
            {
                strings.Add("");
            }


            //Loop through the files and get the Gamestates- assigning the correct file for each
            for (int i = 0; i < numberOfTiles; i++)
            {
                //string txtPath = $"{directory}{this.Tiles[i].seaState}.txt";

                string fileName = $"{GameInfo.Tiles[i].seaState}.txt";

                //Int division
                int mapRow = (i / mapSize);
                int mapRowAdder = mapRow * txtFileLength;

                //Read each line of text file and store in the appropriate string
                for (int j = 0; j < txtFileLength; j++)
                {

                    //Get the current line of the file and add to string in the list
                    strings[j + mapRowAdder] = strings[j + mapRowAdder] + ASCIRepository.LoadASCIFromFile(fileName).Skip(j).Take(1).First();



                    //This needs to use the iterator for the file not the file length- so it works on the first file for each row
                    //Vertical Borders
                    if (i % mapSize == 0) // Start border
                    {
                        strings[j + mapRowAdder] = borderVert[0] + strings[j + mapRowAdder] + borderVert[0];
                    }
                    else
                    {
                        strings[j + mapRowAdder] = strings[j + mapRowAdder] + borderVert[0];
                    }
                }
            }

            string colNames = "";

            for (int i = 0; i < GameInfo.GameInfoSize; i++)
            {
                colNames = $"{colNames}{padding2}{i}";
            }


            int frameMultiplier = 8 * GameInfo.GameInfoSize;
            int borderMultiplier = 8 * GameInfo.GameInfoSize + 1;
            //int borderMultiplier = 8 * mapSize + 1;

            //string frameMid2 = new string(frameMid, multiplier);

            var frameMid2 = new StringBuilder().Insert(0, frameMid[0], frameMultiplier).ToString();

            var borderTop = new StringBuilder().Insert(0, borderHor[0], borderMultiplier).ToString();

            string topFrame = frameEndL[0] + frameMid2 + frameEndR[0];


            //  PRINTING //


            // TOP FRAME
            PrintTerminal.PrintLine(topFrame);
            PrintTerminal.PrintLine();


            // COLUMN NAMES
            PrintTerminal.PrintLine(colNames);

            //PADDING AND BORDER TOP
            Console.Write(padding);
            PrintTerminal.PrintLine(borderTop);


            // TILES SIDE NUMBERS AND MID BORDERS 
            for (int i = 0; i < strings.Count; i++)
            {
                // For every second string in tile square print the number next to it
                if (i % txtFileLength == 1)
                {
                    Console.Write($" {i / txtFileLength} ");
                    PrintTerminal.PrintLine(strings[i]);
                }
                else
                {
                    Console.Write(padding);
                    PrintTerminal.PrintLine(strings[i]);
                }
                //After each row is printed, print the border
                if ((i + 1) % 3 == 0)
                {
                    Console.Write(padding);
                    PrintTerminal.PrintLine(borderTop);
                }


            }

            //Printing bottom frame
            PrintTerminal.PrintLine();
            PrintTerminal.PrintLine(topFrame);
            PrintTerminal.PrintLine($"Number of squid remaining: {GameInfo.NumberOfSquid}");
            PrintTerminal.PrintLine($"Number of shots remaining: {GameInfo.ShotCounter}");


        }

        public static void PrintLine(string line)
        { Console.WriteLine(line); }

        //Overloaded for no input i.e new line
        public static void PrintLine()
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
                        PrintTerminal.PrintLine(line);
                    }
                } while (line != null);
            }

        }

        public static void AnimateTile(GameState state, int attackGridNumber, GameInfo Game)
        {
            Game.Tiles[attackGridNumber].seaState = state;
            Console.Clear();
            PrintGameInfo(Game);
            Thread.Sleep(Animations.waitTime);
        }


    }
}
