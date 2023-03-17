
//Setting file variables
using System.Text;
using Test;

internal class Program
{
    private static void Main(string[] args)
    {

        int origWidth = Console.WindowWidth;
        int origHeight = Console.WindowHeight;

        Console.SetWindowSize(120, 60);

        Console.Write("Please input the size of grid you wish to print (E.g. 3 for 3x3, etc): ");
        int mapSize = int.Parse(Console.ReadLine());
        //int num = 1;


        void PrintFile(List<string> filenames, int mapSize)

        {
            //int mapSize = 3;

            string padding = "   ";
            string padding2 = "       ";


            string directory = @"C:\Users\tommy\Documents\C Sharp Projects\c Sharp Exercises and Solutions\Test\";
            
            //Load the border file
            string borderVert = File.ReadAllText($"{directory}BorderVert.txt");
            string borderHor = File.ReadAllText($"{directory}BorderHor.txt");
            string frameEndL = File.ReadAllText($"{directory}FrameEndL.txt");
            string frameEndR = File.ReadAllText($"{directory}FrameEndR.txt");
            string frameMid = File.ReadAllText($"{directory}FrameMid.txt");

            //int txtFileLength = File.ReadLines(file.FullName).Count();
            int txtFileLength = 3;

            //string rowLabel = $"{currentRow}"
            
            
            //Create an list based on number of lines in each text file

            List<string> strings = new List<string>();
            
            for(int i = 0; i < txtFileLength * mapSize; i++)
            {
                strings.Add("");
            }
            
            
            //strings.Add("");
            //strings.Add("");


            
            StringBuilder sb = new StringBuilder();


            //Loop over every file in filenames
            //foreach (string filename in filenames) 
            for (int i = 0; i < filenames.Count; i++)       
            {
                string txtPath = $"{directory}{filenames[i]}";

                if (File.Exists(txtPath))
                {
                    //Int division
                    int mapRow = (i / mapSize);
                    int mapRowAdder = mapRow * txtFileLength;

                        //Read each line of text file and store in the appropriate string
                        for (int j = 0; j < txtFileLength; j++)
                        {

                            //Get the current line of the file and add to  string in the list
                            strings[j + mapRowAdder] = strings[j + mapRowAdder] + File.ReadLines(txtPath).Skip(j).Take(1).First();



                            //This needs to use the iterator for the file not the file length- so it works on the first file for each row
                            //Vertical Borders
                            if (i % mapSize == 0) // Start border
                            {
                                strings[j + mapRowAdder] = borderVert + strings[j + mapRowAdder] + borderVert;
                            }
                            else 
                            {
                                strings[j + mapRowAdder] = strings[j + mapRowAdder] + borderVert;
                            }

                        }
                        
                    

                }
                //string text = sb.ToString();
      

            }
            string colNames = "";

            for(int i = 0; i < mapSize; i++)
            {

                colNames = $"{colNames}{padding2}{i}";
                
            }




            int frameMultiplier = 8 * mapSize;
            int borderMultiplier = 8 * mapSize + 1;
            //int borderMultiplier = 8 * mapSize + 1;

            //string frameMid2 = new string(frameMid, multiplier);

            var frameMid2 = new StringBuilder().Insert(0, frameMid, frameMultiplier).ToString();

            var borderTop = new StringBuilder().Insert(0, borderHor, borderMultiplier).ToString();

            string topFrame = frameEndL + frameMid2 + frameEndR;

            
            //  PRINTING //
            

            // TOP FRAME
            Console.WriteLine(topFrame);
            Console.WriteLine();


            // COLUMN NAMES
            Console.WriteLine(colNames);

            //PADDING AND BORDER TOP
            Console.Write(padding);
            Console.WriteLine(borderTop);

            
            // TILES SIDE NUMBERS AND MID BORDERS 
            for (int i = 0; i < strings.Count; i++)
            {
                // For every second string in tile square print the number next to it
                if (i % txtFileLength == 1)
                {
                    Console.Write($" {i / txtFileLength} ");
                    Console.WriteLine(strings[i]);
                }
                else
                {
                    Console.Write(padding);
                    Console.WriteLine(strings[i]);
                }
                //After each row is printed, print the border
                if ((i + 1) % 3 ==0 )
                {
                    Console.Write(padding);
                    Console.WriteLine(borderTop);
                }
                

            }
            //Console.Write(padding);
            //Console.WriteLine(borderTop);
            
            Console.WriteLine();
            Console.WriteLine(topFrame);
            












            ////string text2 = text + text;

            //using (StringReader reader = new StringReader(text))
            //{
            //    string line = string.Empty;
            //    do
            //    {
            //        line = reader.ReadLine();
            //        if (line != null)
            //        {
            //            //Console.SetCursorPosition((Console.WindowWidth - line.Length) / 2, Console.CursorTop);


            //                Console.Write(line);
            //                Console.Write(' ');
            //                Console.Write(line);
            //                Console.Write('\n');


            //        }
            //    } while (line != null);
            //}


        }

        //string[] filenames = { "SeaStart.txt", "SeaMiss.txt", "SeaHit.txt", "Crosshair.txt", "CrosshairHit.txt", "CrossMiss.txt", "Miss1.txt", "Miss2.txt" };


        //foreach(string filename in filenames) 
        //{
        //    PrintFile(filename);

        //    Console.WriteLine();
        //}

        List<string> strings = new List<string>();

        for (int i = 0; i < mapSize * mapSize; i++)
        {
            strings.Add("SeaStart.txt");
        }
        
        PrintFile(strings, mapSize);

        //List<SeaTile> tiles = new List<SeaTile>();

        //for (int i = 0; i < mapSize * mapSize; i++)
        //{

        //    tiles.Add(new SeaTile(1));

        //}

        ////Print a map of mapSize x mapSize
        //for (int i = 0; i < mapSize; i++)
        //{

        //    for (int j = 0; j < mapSize; j++) 
            
        //    {
        //        PrintFile(tiles[j + i * mapSize].Parser());

        //        if (j == mapSize - 1)
        //            Console.WriteLine();

        //    }
            
        //}


    }
}