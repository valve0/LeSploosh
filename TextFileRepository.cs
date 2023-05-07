using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeSploosh
{
    internal class TextFileRepository
    {

        //public static string directory = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName) + @"\LeSploosh\Text Files\";
          
        public static string directory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\Text Files\";


        public static string LoadStringFromFile(string fileName)
        {
            string path = $"{directory}{fileName}";

            StringBuilder stringBuilder = new StringBuilder();


            try
            {
                if(File.Exists(path))
                {
                    //ASCI = File.ReadAllLines(path);

                    foreach (string line in File.ReadAllLines(path))
                    {
                        if (line != null)
                            stringBuilder.AppendLine(line);

                    }
                }
                
            }
            catch (FileNotFoundException fnfex)
            {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("The file couldn't be found!");
                    Console.WriteLine(fnfex.Message);
                    Console.WriteLine(fnfex.StackTrace);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Something went wrong while loading the file!");
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.ResetColor();
            }

            return stringBuilder.ToString().TrimEnd('\r', '\n');

        }

        public static void WriteStringToFile(string fileName, string stringToWrite) 
        {
            string path = $"{directory}{fileName}";

            //Overwrites all text in file
            File.WriteAllText(path, stringToWrite);
        }


        public static int GetNumberOfLinesFile(string fileName)
        {
            string stringToCount = LoadStringFromFile(fileName);

            return stringToCount.Split('\n').Count();
        }

    }
}
