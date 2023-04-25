using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeSploosh
{
    internal class ASCIRepository
    {

        static string directory = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName) + @"\LeSploosh\Text Files\";

        private void CheckForExisitingASCIFile(string fileName)
        {
            string path = $"{directory}{fileName}";

            bool exisitingfileFound = File.Exists(path);
            if (!exisitingfileFound)
            {
                //Create the Directory
                if(!Directory.Exists(path))
                    Directory.CreateDirectory(directory);

                //Create an empty file
                using FileStream fs = File.Create(path);
                
            }
        }

        public string LoadASCIFromFile(string fileName)
        {
            string path = $"{directory}{fileName}";
            //string ASCI = string.Empty;

            StringBuilder ASCI = new StringBuilder();

            try
            {
                CheckForExisitingASCIFile(fileName);

                //ASCI = File.ReadAllLines(path);
      
                foreach(string line in File.ReadAllLines(path))
                {
                    if(line != null)
                        ASCI.AppendLine(line);
                }



            }
            catch (FileNotFoundException fnfex)
            {
                    Console.ForegroundColor = ConsoleColor.Red;
                    PrintTerminal.PrintLine("The file couldn't be found!");
                    PrintTerminal.PrintLine(fnfex.Message);
                    PrintTerminal.PrintLine(fnfex.StackTrace);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                PrintTerminal.PrintLine("Something went wrong while loading the file!");
                PrintTerminal.PrintLine(ex.Message);
            }
            finally
            {
                Console.ResetColor();
            }

            return ASCI.ToString();

        }


    }
}
