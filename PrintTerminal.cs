using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeSploosh
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
            int mapSize = GameInfo.Size;



            //Create an list that will store each line and therefore each row of the grid as strings
            //A row in the grid is made up of txtFileLength * Size seperate strings
            List<string> strings = new List<string>();

            for (int i = 0; i < txtFileLength * mapSize; i++)
            {
                strings.Add("");
            }


            //Loop through the files and get the Gamestates- assigning the correct file for each

            //Loop through all the tiles in the 2d array


            for(int row = 0; row < GameInfo.Size; row++)
            {
                //Create new string to store row of tile strings
                string tileRowString = string.Empty;
               
                //Add left border to Row string



                for (int col =  0; col < GameInfo.Size; col++)
                {

                    //Return correct state of tile for row,col back to tileRowString


                }  




                //Add each row of Tiles to new string in list of strings
                strings.Add(tileRowString)

            }








            for (int i = 0; i < numberOfTiles; i++)
            {

                string fileName = string.Empty;

                //Logic to determine whether the file should be a type of seastate or a crosshair

                if (GameInfo.Tiles[i].CrosshairBool == true)
                {
                    //Figure out what kind of crosshair to print for the tile
                    if(GameInfo.Tiles[i].SeaState == TileState.GameStart)
                    {
                        fileName = $"CrosshairStart.txt";
                    }
                    else if(GameInfo.Tiles[i].SeaState == TileState.GameMiss)
                    {
                        fileName = $"CrosshairMiss.txt";
                    }
                    else if(GameInfo.Tiles[i].SeaState == TileState.GameHit)
                    {
                        fileName = $"CrosshairHit.txt";
                    }
                }
                else
                {
                    fileName = $"{GameInfo.Tiles[i].SeaState}.txt";
                }

                

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

            for (int i = 0; i < GameInfo.Size; i++)
            {
                colNames = $"{colNames}{padding2}{i}";
            }


            int frameMultiplier = 8 * GameInfo.Size;
            int borderMultiplier = 8 * GameInfo.Size + 1;
            //int borderMultiplier = 8 * mapSize + 1;

            //string frameMid2 = new string(frameMid, multiplier);

            var frameMid2 = new StringBuilder().Insert(0, frameMid[0], frameMultiplier).ToString();

            var borderTop = new StringBuilder().Insert(0, borderHor[0], borderMultiplier).ToString();

            string topFrame = frameEndL[0] + frameMid2 + frameEndR[0];

            //Cleare console just beofre we print to reduce stutter;
            Console.SetCursorPosition(0, 0);

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
            PrintTerminal.PrintLine("Please use the Arrows Keys to move the crosshair and press Space to attack");


        }



        internal string ReturnTileString(Tile tile)
        {
            if (GameInfo.Tiles[i].CrosshairBool == true)
            {
                //Figure out what kind of crosshair to print for the tile
                if (GameInfo.Tiles[i].SeaState == TileState.GameStart)
                {
                    fileName = $"CrosshairStart.txt";
                }
                else if (GameInfo.Tiles[i].SeaState == TileState.GameMiss)
                {
                    fileName = $"CrosshairMiss.txt";
                }
                else if (GameInfo.Tiles[i].SeaState == TileState.GameHit)
                {
                    fileName = $"CrosshairHit.txt";
                }
            }
            else
            {
                fileName = $"{GameInfo.Tiles[i].SeaState}.txt";
            }


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

        public static void AnimateTile(TileState state, int attackGridNumber, GameInfo Game)
        {
            Game.Tiles[attackGridNumber].SeaState = state;
            Console.SetCursorPosition(0, 0);
            PrintGameInfo(Game);
            Thread.Sleep(Animations.waitTime);
        }


    }
}
