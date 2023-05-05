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

        //private static int textFileLength = 3;


        private static string directory = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName) + @"\LeSploosh\Text Files\";

        public static void PrintFile(string file, float verticalAlignment = 0.5f, string color = "w", int cursorTop = -1)
        {

            file = directory + file;
            string printString = File.ReadAllText(file);

            PrintTerminal.PrintString(printString, verticalAlignment, color);

        }

        public static void PrintString(string stringToPrint, float verticalAlignment = 0.5f, string color = "w", int cursorTop = -1)
        {

            //Cannot set the default values be Console.Cursor Top at compile time
            if (cursorTop == -1)
                cursorTop = Console.CursorTop;

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

            //string printString = File.ReadAllText(stringToPrint);
            using (StringReader reader = new StringReader(stringToPrint))
            {
                string line = string.Empty;
                do
                {
                    line = reader.ReadLine();
                    if (line != null)
                    {

                        // Center the output of the string
                        Console.SetCursorPosition((int)((Console.WindowWidth * verticalAlignment) - line.Length / 2), cursorTop);

                        // Center the output of the string using longest line method
                        //Console.SetCursorPosition((int)((Console.WindowWidth * verticalAlignmentAdjuster) - longestLineLength / 2), Console.CursorTop);

                        //Print the line
                        Console.WriteLine(line);

                        //Update new cursor position with cursor now
                        cursorTop = Console.CursorTop;
                    }

                } while (line != null);
            }

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


        public static bool PrintSelection(string leftSelected, string rightSelected, float verticalAlignment = 0.5f, string color = "w", int cursorTop = -1)
        {
            //Check currently selected box
            int txtFileHeight = 4;
            bool selection = true; // default to true as arrow starts on the left
            PrintTerminal.PrintString("[Use Arrow Keys to move cursor and space to make selection]", verticalAlignment: verticalAlignment, color: color, cursorTop: cursorTop);

            PrintFile(leftSelected, verticalAlignment: verticalAlignment);
            do
            {

                var ch = Console.ReadKey(false).Key;

                switch (ch)
                {
                    case ConsoleKey.Spacebar:
                        return selection;

                    case ConsoleKey.LeftArrow:
                        selection = true;
                        //Move curosr back up thew height of the slection boxes and reprint over them                      
                        PrintFile(file: leftSelected, color: color, verticalAlignment: verticalAlignment, cursorTop: Console.CursorTop - txtFileHeight);
                        break;

                    case ConsoleKey.RightArrow:
                        selection = false;
                        PrintFile(file: rightSelected, color: color, verticalAlignment: verticalAlignment, cursorTop: Console.CursorTop - txtFileHeight);
                        break;
                    default:
                        //Invalid key inputted: do nothing
                        break;
                }
            } while (true); //Loop forever until selection made
        }


        public static string ReturnStringsSideBySide(List<string> listOfStrings)
        {
            int txtFileLength = listOfStrings[0].Split('\n').Length;

            string[] stringRows = new string[txtFileLength];


            //Go through each string get the approriate striung line and add to the array.
            for (int i = 0; i < stringRows.Length; i++)
            {
                foreach (string s in listOfStrings)
                {

                    using (StringReader reader = new StringReader(s))
                    {
                        string line = string.Empty;
                        int lineNumber = 0;

                        do
                        {
                            line = reader.ReadLine();

                            if (lineNumber == i)
                            {
                                // do something with the line
                                stringRows[i] = (stringRows[i] + line).Replace("\r\n", string.Empty);

                            }
                            lineNumber++;

                        } while (line != null);

                    }
                }


            }

            StringBuilder sb = new StringBuilder();

            foreach (string s in stringRows)
            {
                sb.AppendLine(s);
            }

            return sb.ToString();


        }

    }
}
