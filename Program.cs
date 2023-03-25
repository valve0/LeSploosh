
//Setting file variables
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml;
using Test;

internal class Program
{
    private static void Main(string[] args)
    {

        //Get directory of solution
        string directory = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName) + @"\Test\";
        String solutionName = Path.GetFileName(Process.GetCurrentProcess().MainModule.FileName);
        solutionName = Path.GetFileNameWithoutExtension(solutionName);
        int origWidth = Console.WindowWidth;
        int origHeight = Console.WindowHeight;

        //Console.SetWindowSize(120, 60);


        //intialise variables
        //One square sized squid
        int numSmallSquid = 0;
        //Two square sized squid
        int numMediumSquid = 0;
        // Three square sized squid
        int numLargeSquid = 0;
        //Four square sized squid
        int numGiantSquid = 0;

        //string difficulty = "";
        int mapSize = 0;
        int shotCounter = 0;
        bool loop = true;

        do {
            Console.WriteLine("Please select a diffulty (Easy/Medium/Hard)");
            string difficulty = Console.ReadLine();

            switch (difficulty)
            {
                case "Easy":
                    {
                        mapSize = 3;
                        numSmallSquid = 3;
                        shotCounter = 5;
                        loop = false;
                        break;
                    }

                case "Medium":
                    {
                        mapSize = 4;
                        numSmallSquid = 3;
                        numMediumSquid = 1;
                        shotCounter = 5;
                        loop = false;
                        break;
                    }

                case "Hard":
                    { 
                        mapSize = 8;
                        numMediumSquid = 1;
                        numLargeSquid = 1;
                        numGiantSquid = 1;
                        shotCounter = 5;
                        loop = false;
                        break;
                    }

                default:
                    Console.WriteLine("Please select a correct difficulty.");
                    break;

            }
        } while (loop);


        Map Sea = new Map(numSmallSquid, numMediumSquid, numLargeSquid, numGiantSquid, mapSize, shotCounter);

        


        int noSquidRemaining = Sea.NumberOfSquid;

        // The Game Loop
        Console.Clear();
        Sea.PrintMap(Sea);
        do
        {
            
            Console.WriteLine("Please select a grid number to attack: ");
            int attackGridNumber = int.Parse(Console.ReadLine());
            Sea.Attack(Sea, attackGridNumber);
            //Console.Clear();
            //PrintMap(Sea);



        } while(Sea.NumberOfSquid > 0 && Sea.ShotCounter > 0);

        string winFile = directory + "YouWin.txt";
        string loseFile = directory + "YouLose.txt";

        if (Sea.NumberOfSquid == 0)
        {
            PrintFile(winFile);          
        }
        else
        {
            PrintFile(loseFile);
        }

        void PrintFile(string file)
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
                        Console.WriteLine(line);
                    }
                } while (line != null);
            }

        }



    }
}