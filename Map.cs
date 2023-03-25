using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    internal class Map
    {
        public int NumberOfSquid { get; set;}
        private int NumSmallSquid { get; init; }
        private int NumMediumSquid { get; init; }
        private int NumLargeSquid { get; init; }
        private int NumGiantSquid { get; init; }
        public int MapSize { get; init; }
        private int NumberOfTiles { get; init; }

        public int ShotCounter { get; set; }

        //private List<Tile> Tiles { get; set; }
        public Tile[] Tiles { get; init; }
        private List<int> Squids { get; init; }

        public Map(int small, int medium, int large, int giant, int mapSize, int shotCounter)
        {
            Random random = new Random();
            
            this.NumSmallSquid = small;
            this.NumMediumSquid = medium;
            this.NumLargeSquid = large;
            this.NumGiantSquid = giant;
            this.MapSize = mapSize;
            this.NumberOfTiles = mapSize * mapSize;
            this.ShotCounter = shotCounter;


            int[] squidNumbers = new int[4];
            squidNumbers[0] = small;
            squidNumbers[1] = medium;
            squidNumbers[2] = large;
            squidNumbers[3] = giant;

            this.NumberOfSquid = NumSmallSquid + NumMediumSquid + NumLargeSquid + NumGiantSquid;

            //this.Tiles = new List<Tile>();
            this.Tiles = new Tile[NumberOfTiles];
            this.Squids = new List<int>();


            //Fill aray of tiles with new tiles, setting them to default start value
            for (int i = 0; i < NumberOfTiles; i++) 
            {
                //Fill list with default value of sea state
                //Tiles.Add(new Tile());
                Tiles[i] = new Tile();
            
            }

            
            //Loop through each group of squid starting with small
            foreach( int squid in squidNumbers)
            {

                for (int i = 0; i< squid; i++)
                {

                    bool squidPlaced = false;
                    do // Keep looping until squid placed
                    {
                        int spot = random.Next(NumberOfTiles);

                        //check to see if no squid already in this spot
                        if (!Tiles[spot].SquidPresent)
                        {
                            Tiles[spot].SquidPresent = true;
                            squidPlaced = true;
                        }

                    } while (squidPlaced != true);

                }



            }



            // Generate the positions of the squid based on the number and the numberOfTiles
            // Place squid in decending order of size to make sure there is space


            //Select a random spot in the sea
            //int spot = random.Next(numberOfTiles);


            //Generate a random orienttaion of the squid, where is it pointing? Up down, left or right?
            //Up = 0, Down = 1, Left = 2, Right = 3.
            //int direction = random.Next(4);


            //Create a list of the spots that the squid would fill
            //List<int> spots = new List<int>();
            //spots.Add(spot);



            //Check whether there is enough free space to place the squid at these spots




        }

        internal bool AttackCheck(int attackGridNumber)
        {
            if (Tiles[attackGridNumber].Attackable)
            {
                Console.WriteLine("Valid Square to attack");
                return true;

            }
            Console.WriteLine("Invalid square to attack");
            return false;
        }

        internal bool Attack(Map Sea, int attackGridNumber)
        {

            if (!Tiles[attackGridNumber].Attackable) 
            {
                Console.WriteLine("Tile not attackable");
                return false; 
            
            }
            
            //Assumption that Attack Check is run before this method
            if (Tiles[attackGridNumber].SquidPresent)
            {
                //Hit a valid square with a squid
                Console.Clear();
                Tiles[attackGridNumber].SeaState = 3;
                PrintMap(Sea);              
                Thread.Sleep(1000);
                Console.Clear();
                Tiles[attackGridNumber].SeaState = 1;
                PrintMap(Sea);
                Thread.Sleep(1000);
                Console.Clear();
                Tiles[attackGridNumber].SeaState = 3;
                PrintMap(Sea);

                Tiles[attackGridNumber].Attackable = false;
                ShotCounter--;
                NumberOfSquid--;
                Console.WriteLine("Squid Hit!");
                return true;

            }
            else if (!Tiles[attackGridNumber].SquidPresent)
            {
                //Miss
                Console.Clear();
                Tiles[attackGridNumber].SeaState = 7;
                PrintMap(Sea);
                Thread.Sleep(1000);
                Console.Clear();
                Tiles[attackGridNumber].SeaState = 8;
                PrintMap(Sea);
                Thread.Sleep(1000);
                Console.Clear();
                Tiles[attackGridNumber].SeaState = 7;
                PrintMap(Sea);
                Thread.Sleep(1000);
                Console.Clear();
                Tiles[attackGridNumber].SeaState = 2;
                PrintMap(Sea);



                Tiles[attackGridNumber].Attackable = false;
                ShotCounter--;
                Console.WriteLine("Miss");
                return true;
            }
            Console.WriteLine("ERROR");
            return false;
        }

        public void PrintMap(Map Sea)

        {

            string padding = "   ";
            string padding2 = "       ";

            //Get directory of solution
            string directory = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName) + @"\Test\";

            //Load the static border files
            string borderVert = File.ReadAllText($"{directory}BorderVert.txt");
            string borderHor = File.ReadAllText($"{directory}BorderHor.txt");
            string frameEndL = File.ReadAllText($"{directory}FrameEndL.txt");
            string frameEndR = File.ReadAllText($"{directory}FrameEndR.txt");
            string frameMid = File.ReadAllText($"{directory}FrameMid.txt");

            //Store constanct textfile line length
            int txtFileLength = 3;

            //Create an list that will store each line and threrofe ech row of the grid as strings
            //A row in the grid is made up of txtFileLength * MapSize seperate strings
            List<string> strings = new List<string>();

            for (int i = 0; i < txtFileLength * Sea.MapSize; i++)
            {
                strings.Add("");
            }


            StringBuilder sb = new StringBuilder();


            //Loop over every file in filenames
            //foreach (string filename in filenames) 
            for (int i = 0; i < Sea.Tiles.Length; i++)
            {
                string txtPath = $"{directory}{Sea.Tiles[i].Parser(Sea.Tiles[i].SeaState)}";

                if (File.Exists(txtPath))
                {
                    //Int division
                    int mapRow = (i / Sea.MapSize);
                    int mapRowAdder = mapRow * txtFileLength;

                    //Read each line of text file and store in the appropriate string
                    for (int j = 0; j < txtFileLength; j++)
                    {

                        //Get the current line of the file and add to  string in the list
                        strings[j + mapRowAdder] = strings[j + mapRowAdder] + File.ReadLines(txtPath).Skip(j).Take(1).First();



                        //This needs to use the iterator for the file not the file length- so it works on the first file for each row
                        //Vertical Borders
                        if (i % Sea.MapSize == 0) // Start border
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

            for (int i = 0; i < Sea.MapSize; i++)
            {

                colNames = $"{colNames}{padding2}{i}";

            }




            int frameMultiplier = 8 * Sea.MapSize;
            int borderMultiplier = 8 * Sea.MapSize + 1;
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
                if ((i + 1) % 3 == 0)
                {
                    Console.Write(padding);
                    Console.WriteLine(borderTop);
                }


            }

            //Printing bottom frame
            Console.WriteLine();
            Console.WriteLine(topFrame);
            Console.WriteLine($"Number of squid remaining: {Sea.NumberOfSquid}");
            Console.WriteLine($"Number of shots remaining: {Sea.ShotCounter}");


        }



    }
}
