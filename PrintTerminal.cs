using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeSploosh
{
    internal class PrintTerminal
    {

        //Store constants e.g. height padding, textfile line length & width
        private static string heightPadding = string.Concat(Enumerable.Repeat($"\n", 10));
        private static int txtFileLength = 3;
        private static int txtFileWidth = 8;
        //public int HighScore { get; set; }

        private static string directory = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName) + @"\LeSploosh\Text Files\";

        private static int HighScore = int.Parse(ASCIRepository.LoadASCIFromFile("HighScore.txt").Replace("\r\n", string.Empty));
        private static string weclomeString = "WelcomeTo.txt";
        private static string titleCard = "TitleCard.txt";


        //public PrintTerminal()
        //{
        //    HighScore = int.Parse(ASCIRepository.LoadASCIFromFile("HighScore.txt").Replace("\r\n", string.Empty));


        //}


        public static void PrintGameInfo(GameInfo GameInfo, string feedbackString)
        {
            Console.SetCursorPosition(0, 0);

            //Load the static border files
            string borderVert = ASCIRepository.LoadASCIFromFile("BorderVert.txt").Replace("\r\n", string.Empty);
            string borderHor = ASCIRepository.LoadASCIFromFile("BorderHor.txt").Replace("\r\n", string.Empty);
            string frameEndL = ASCIRepository.LoadASCIFromFile("FrameEndL.txt").Replace("\r\n", string.Empty);
            string frameEndR = ASCIRepository.LoadASCIFromFile("FrameEndR.txt").Replace("\r\n", string.Empty);
            string frameMid = ASCIRepository.LoadASCIFromFile("FrameMid.txt").Replace("\r\n", string.Empty);
            
            //Create a string based on the chacrter supplied and to the length of txtFileWdith * Size + 1
            frameMid = String.Concat(Enumerable.Repeat(borderHor, txtFileWidth * GameInfo.Size + 1));
            borderHor = String.Concat(Enumerable.Repeat(borderHor, txtFileWidth * GameInfo.Size + 1));

            string topFrame = frameEndL + frameMid + frameEndR;


            //Title Card
            PrintFile(titleCard);
            PrintString();
            PrintString();

            // TOP FRAME
            //PrintString(heightPadding);
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

            //As we are not clearingt he screen after each print- we wnat to make sure this line gets cleared othersiwse it can remnain as a ghost
            if (feedbackString == "")
            {
                feedbackString = "                    ";
            }

            PrintString(feedbackString);

            
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


        public static void PrintString(string line, string color, string position) //Overloaded constructor with no color argument
        {
            float positionAdjuster = ReturnPositionAdjuster(position);
            Console.BackgroundColor = ConsoleColor.Blue;

            // Center the output of the string
            Console.SetCursorPosition((int)((Console.WindowWidth * positionAdjuster) - line.Length / 2), Console.CursorTop); //Console.CursorTop


            var cursorPos = Console.GetCursorPosition();
            Console.WriteLine(line);
            //Console.WriteLine("AAAA");
            //var test = 1;

        }


        //Overloaded for no input i.e new line
        public static void PrintString()
        { Console.WriteLine(); }

        public static string ReadLine()
        { return Console.ReadLine(); }




        public static void PrintFile(string file)
        {

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
                        //Console.SetCursorPosition((Console.WindowWidth - line.Length) / 2, Console.CursorTop);
                        PrintTerminal.PrintString(line);
                    }
                } while (line != null);
            }

        }



        public static void PrintFile(string file, string position)
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
                        //Console.SetCursorPosition((int)((Console.WindowWidth - line.Length) * positionAdjuster), Console.CursorTop);
                        PrintTerminal.PrintString(line, "w", position);
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

                case "r":
                    {
                        return 0.66f;                     
                    }
                default:
                    {
                        return 2;                       
                    }
            }
        }

        public static void AnimateTile(TileState state, int[] ActiveGridNumber, GameInfo Game)
        {
            Game.Tiles[ActiveGridNumber[0], ActiveGridNumber[1]].SeaState = state;
            Console.SetCursorPosition(0, 0);
            PrintGameInfo(Game, "");
            Thread.Sleep(Animations.waitTime);
        }


        public void PrintFail(int state)
        {
            
            string loseFile = "YouLose.txt";

            Console.Clear();

            PrintString(heightPadding);

            PrintFile(loseFile);

            switch (state)
            {

                case 1:
                    PrintTerminal.PrintString("You ran out of cannon balls!");
                    break;

                case 2: 
                    PrintTerminal.PrintString("You didnt have enough cannon balls to finish them!");
                    break;

            }


        }

        public void PrintWin()
        {
            string winFile =  "YouWin.txt";

            PrintString(heightPadding);

            Console.Clear();

            PrintFile(winFile);

           PrintTerminal.PrintString("You ran out of cannon balls!");
        }

        public void PrintTitleCard()
        {

            //In miliseconds
            int waitTime = 3000;

            string welcomeStartHeight = string.Concat(Enumerable.Repeat($"\n", 50));

            PrintString(welcomeStartHeight);

            //Console.Clear();

            PrintFile("WelcomeTo.txt");
            Thread.Sleep(waitTime);
            //Console.Clear();
            //PrintString(heightPadding);
            var cursorPos = Console.GetCursorPosition();

            //Console.SetCursorPosition(cursorPos.Item1, cursorPos.Item2 - 1);
            //Move Welcome to down
            for (int i = 0; i < 50; i++)
            {
                //Add another row to the cursor position
                cursorPos = Console.GetCursorPosition();

                //Clear previous printed string line that is still visible
                Console.SetCursorPosition(cursorPos.Item1, cursorPos.Item2-1);

                PrintString("                                                                                                                                                              ");


                //Move up werlcome to string one line at a time
                cursorPos.Item2 = cursorPos.Item2 - 17;
                Console.SetCursorPosition(cursorPos.Item1, cursorPos.Item2);
                PrintFile(weclomeString);
               
                Thread.Sleep(100);
            }



            PrintString($"\n\n");
            PrintFile(titleCard);

            Console.ReadLine();

            Console.Clear();

        }

        public void PrintIntro()
        {
            PrintTerminal.PrintFile("Salvatore.txt", "r");
            // Center the output of the string
            Console.SetCursorPosition(0, 5);
            //PrintString($"\n\n\n\n", "l");




            string file = directory + "script.txt";
            string printString = File.ReadAllText(file);

            //Position of script to be printed
            string position = "l";
            using (StringReader reader = new StringReader(printString))
            {
                string line = string.Empty;
                do
                {
                    line = reader.ReadLine();
                    if (line != null && line != "")
                    {
                        //Console.SetCursorPosition((int)((Console.WindowWidth - line.Length) * positionAdjuster), Console.CursorTop);

                        if (line.Substring(0, 2).Equals("%%"))//Sentinal charcaters found %%- slection needs to be made
                        {
                            //Get selection number to be made
                            if (line[2] == '1')
                            {
                                string selection = MakeSelection("Selection1L.txt", "Selection1R.txt", "w", position);

                                if (selection == "left") //Exit program
                                {

                                }
                                //Else continue
                                PrintString();

                            }
                            else if (line[2] == '2')
                            {
                                string selection = MakeSelection("Selection2L.txt", "Selection2R.txt", "w", position);


                                if (selection == "right") //Repeat
                                {

                                }
                                //Else continue
                                PrintString();

                            }
                        }
                        else 
                        { 

                        PrintString(line, "w", "l");
                        //Wait for user to press any key to continue
                        //WaitForAnyInput();
                        PrintString();
                        Console.ReadKey(false);
                        }
                    } 

                } while (line != null);

                string finalLine = $"Excellant so far our best sailor has managed to destroy all of zee giant squid using only {HighScore} cannonballs! May you fight as bravely!";
                PrintString(finalLine,"w","l");

                Console.ReadKey(false);
            }


            //PrintTerminal.PrintFile("Script.txt", "l");
        }


        private string MakeSelection(string leftSelected, string rightSelected, string color, string position)
        {
            //Check currently selected box
            int txtFileHeight = 4;
            string selection = string.Empty;
            PrintTerminal.PrintString("[Use Arrow Keys to move cursor and space to make selection]", color, position);

            PrintFile(leftSelected, "l");
            do
            {
                var test2 = Console.GetCursorPosition();
                
                var ch = Console.ReadKey(false).Key;
                
                

                switch (ch)
                {
                    case ConsoleKey.Spacebar:
                        return selection;

                    case ConsoleKey.LeftArrow:
                        selection = "left";
                        //Move curosr back up thew height of the slection boxes so it can reprint over them
                        var test = Console.GetCursorPosition();

                        Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop - txtFileHeight);
                        PrintFile(leftSelected, "l");
                        break;

                    case ConsoleKey.RightArrow:
                        selection = "right";
                        Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop - txtFileHeight);
                        PrintFile(rightSelected, "l");
                        break;
                    default:
                        //Invalid selection: do nothing
                        break;
                }
            } while (true); //Loop forever until selection made

            
        }

        public void WaitForAnyInput()
        {
            Console.ReadKey(false);
        }

    }
}
