﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeSploosh
{
    internal partial class GameInfo
    {

        public void PrintIntro()
        {

            PrintTitleCard();

            PrintTerminal.PrintFile("Salvatore.txt", verticalAlignmentOffset: salvatoreAlignmentOffset);

            PrintTerminal.PrintFile("IntroScript1.txt", cursorTop: 5, verticalAlignmentOffset: scriptAlignmentOffset);
            PrintTerminal.PrintString();

            if (PrintTerminal.PrintSelection("Selection1L.txt", "Selection1R.txt", verticalAlignmentOffset: scriptAlignmentOffset) == false)
            {
                GameState = 1;
                PrintQuitScript();
                return;
            }

            PrintTerminal.PrintString();

            PrintTerminal.PrintFile("IntroScript2.txt", verticalAlignmentOffset: scriptAlignmentOffset);
            PrintTerminal.PrintString();

            PrintTerminal.PrintSelection("Selection2L.txt", "Selection2R.txt", verticalAlignmentOffset: scriptAlignmentOffset);
            PrintTerminal.PrintString();


            string finalLine = $"Excellant. So far our best sailor has managed to destroy all of zee giant squid using only {highScore} bombs!\n May you fight as bravely!";
            PrintTerminal.PrintString(finalLine, verticalAlignmentOffset: scriptAlignmentOffset);

            Console.ReadKey(false);

            Console.BackgroundColor = ConsoleColor.Blue;
            Console.Clear();

        }


        public void PrintTitleCard()
        {
            int linesInTitle = 17;

            PrintTerminal.PrintFile("WelcomeTo.txt",0.5f,"w",50);
            //In miliseconds
            Thread.Sleep(2000);

            //var cursorPos = Console.GetCursorPosition();

            //Move Welcome to down
            for (int i = 0; i < 45; i++)
            {
                //Print null string
                PrintTerminal.PrintString("                                                                                                                                                              ", cursorTop: Console.CursorTop - 1);

                //Move up werlcome to string one line at a time
                PrintTerminal.PrintFile("WelcomeTo.txt", cursorTop: Console.CursorTop - linesInTitle);

                Thread.Sleep(100);
            }


            Thread.Sleep(1000);
            PrintTerminal.PrintString($"\n\n");
            PrintTerminal.PrintFile("TitleCard.txt");


            Thread.Sleep(1500);
            PrintTerminal.PrintString();
            PrintTerminal.PrintString("Press any key to continue");
            Thread.Sleep(200);
            Console.ReadKey(false);

            //REMOVE?
            Console.BackgroundColor = ConsoleColor.Blue;

            Console.Clear();
        }

        public void PrintGameInfo()
        {

            Console.SetCursorPosition(0, 0);

            //Load the static border files
            string borderVert = TextFileRepository.LoadStringFromFile("BorderVert.txt");
            string borderHor = TextFileRepository.LoadStringFromFile("BorderHor.txt").Replace("\r\n", string.Empty);
            string frameEndL = TextFileRepository.LoadStringFromFile("FrameEndL.txt").Replace("\r\n", string.Empty);
            string frameEndR = TextFileRepository.LoadStringFromFile("FrameEndR.txt").Replace("\r\n", string.Empty);
            string frameMid = TextFileRepository.LoadStringFromFile("FrameMid.txt").Replace("\r\n", string.Empty);

            //This is the standard textfile width for each tile
            int tiletextFileWidth = 8;

            //Create a string based on the chacrter supplied and to the length of txtFileWdith * size + 1
            frameMid = String.Concat(Enumerable.Repeat(borderHor, tiletextFileWidth * size + 1));
            borderHor = String.Concat(Enumerable.Repeat(borderHor, tiletextFileWidth * size + 1));

            string topFrame = frameEndL + frameMid + frameEndR;

            //Title Card
            PrintTerminal.PrintFile("TitleCard.txt");
            PrintTerminal.PrintString();
            //PrintTerminal.PrintString();


            // TOP FRAME
            //PrintString(heightPadding);
            PrintTerminal.PrintString($"High Score {highScore}");
            //PrintTerminal.PrintString();
            PrintTerminal.PrintString(topFrame);

            //BORDER TOP
            PrintTerminal.PrintString();
            //PrintTerminal.PrintString();
            PrintTerminal.PrintString(borderHor);


            List<string> listOfStrings = new List<string>();

            //Loop through all the tiles in the 2d array
            for (int row = 0; row < size; row++)
            {
                //Clean list of previous row strings
                listOfStrings.Clear();

                //Create array of strings you wish to add side by side
                for (int col = 0; col < size; col++)
                {
                    listOfStrings.Add(borderVert);
                    listOfStrings.Add(tiles[row, col].ReturnTileString());

                    if (col == size - 1)
                        listOfStrings.Add(borderVert); //Add final Vertical border at end of string
                }


                PrintTerminal.PrintString(PrintTerminal.ReturnStringsSideBySide(listOfStrings));
                //After each row of tiles is printed- print a long single horizontal border before the next.
                PrintTerminal.PrintString(borderHor);

            }


            //Printing bottom frame


            PrintTerminal.PrintString();

            PrintTerminal.PrintString("'Arrows Keys' to move the crosshair and 'Space' to attack");
            PrintTerminal.PrintString();

            //As we are not clearing the screen after each print- we wnat to make sure this line gets cleared othersiwse it can remnain as a ghost
            if (feedBack.Length > 0)
                PrintTerminal.PrintString(feedBack);
            else
                PrintTerminal.PrintString();

            //PrintTerminal.PrintString();
            PrintTerminal.PrintString(topFrame);



            PrintBombs();

            PrintSquidRemaining();



        }

        private void PrintSquidRemaining()
        {

            string SquidCol = string.Empty;

            for (int i = 0; i < SquidImages.Length; i++)
            {
                SquidCol = SquidCol + this.ReturnSquidString(i) + "\n";
            }

            PrintTerminal.PrintString(SquidCol, cursorTop: squidsTopCusor, verticalAlignmentOffset: squidAlignmentOffset);

        }

        private string ReturnSquidString(int squidNumber)
        {
            TextFileRepository TextFileRepository = new();
            string fileName = string.Empty;

            if (SquidImages[squidNumber] == true)
                fileName = "Squid1.txt";
            else
                fileName = "Squid2.txt";

            string str = TextFileRepository.LoadStringFromFile(fileName);
            return str;
        }

        private void PrintBombs()
        {

            //genertate a string with cannonballs in array 5 rows 3 columns with correct states printed
            //Collect into a single string and print that to console

            int rows = 8;
            int cols = 3;

            string position = string.Empty;


            StringBuilder sb = new StringBuilder();

            //Loop through rows of 8x3 collection of bombs and create a string for each row and then adding to complete string

            for (int i = 0; i < rows; i++)
            {
                List<string> bombRowList = new List<string>();

                //For 3 columns this will give us a string in the 3rd column, then 2nd, then 1st into an array respectively
                for (int j = cols - 1; j >= 0; j--)
                {
                    //Add to end of list
                    bombRowList.Add(ReturnBombString(j * rows + i));
                }

                sb.AppendLine(PrintTerminal.ReturnStringsSideBySide(bombRowList));
            }

            PrintTerminal.PrintString(sb.ToString(), cursorTop: bombsTopCursor, verticalAlignmentOffset: bombAlignmentOffset);

            PrintTerminal.PrintString($"Shots made: {shotsMade}  ", cursorTop: bombsTopCursor -2, verticalAlignmentOffset: bombAlignmentOffset);


        }


        private string ReturnBombString(int CannonBallNumber)
        {
            TextFileRepository TextFileRepository = new();
            string fileName = string.Empty;

            if (bombs[CannonBallNumber] == true)
                fileName = "Bomb1.txt";
            else
                fileName = "Bomb2.txt";

            string str = TextFileRepository.LoadStringFromFile(fileName);
            return str;
        }


        public bool PrintEnd()
        {


            //Hide the crosshair
            tiles[activeGridNumber[0], activeGridNumber[1]].CrosshairBool = false;

            string script = string.Empty;
            bool highScoreMade = false;

            switch (GameState)
            {

                case 1:
                    script = "StraightQuit.txt";
                    return false;


                case 2:
                    script = "WinScript.txt";
                    break;

                case 3:
                    script = "LoseScript.txt";
                    ShowRemainingSquidParts();
                    break;

            }
            //Wait as little bit before allow the user to proceed
            Thread.Sleep(2000);

            if (GameState != 1)
            {

                feedBack = "           Press any key to continue          ";
                PrintGameInfo();

                


                //Wait for user input before continuing
                Console.ReadKey(false);
            }

            //Remove previous screen
            Console.Clear();
            //Print salvatore on the right
            PrintTerminal.PrintFile("Salvatore.txt", verticalAlignmentOffset: salvatoreAlignmentOffset);


            PrintTerminal.PrintFile(script, cursorTop: 5, verticalAlignmentOffset: scriptAlignmentOffset);


            if (shotsMade < highScore)
            {
                PrintTerminal.PrintString($"Whoa. Wait a sec. {shotsMade}?! That's a new record...", verticalAlignmentOffset: scriptAlignmentOffset);
                TextFileRepository.WriteStringToFile("HighScore.txt", shotsMade.ToString());
            }
            

            PrintTerminal.PrintString();

            //Print and return the play again method
            if (PrintTerminal.PrintSelection("PlayAgainL.txt", "PlayAgainR.txt",
                    verticalAlignmentOffset: scriptAlignmentOffset) == false)
            {
                PrintQuitScript();
                return false;
            }
            
            else
                return true;
        

        }

        public void PrintQuitScript()
        {
            Console.Clear();
            
            PrintTerminal.PrintFile("Salvatore.txt", verticalAlignmentOffset: salvatoreAlignmentOffset);

            PrintTerminal.PrintFile("QuitScript.txt", cursorTop: 5, verticalAlignmentOffset: scriptAlignmentOffset);
        }

        private void ShowRemainingSquidParts()
        {

            foreach (var squid in listOfSquids)
            {
                //For each squid loop through their positioins. 

                for (int i = 0; i < squid.squidPositions.Count; i++)
                {
                    // If the tile at the position does not show a hit- change its state to a "Squid Here"
                    if (tiles[squid.squidPositions[i][0], squid.squidPositions[i][1]].SeaState != (TileState)2)
                    {
                        //Change non hit squid tile to "Squid Here state"
                        tiles[squid.squidPositions[i][0], squid.squidPositions[i][1]].SeaState = (TileState)8;
                    }
                }
            }
        }



    }
}
