using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.ComponentModel.Design;

namespace LeSploosh
{
    internal class Display
    {
        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();
        private static IntPtr ThisConsole = GetConsoleWindow();
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        private const int HIDE = 0;
        private const int MAXIMIZE = 3;
        private const int MINIMIZE = 6;
        private const int RESTORE = 9;

        public float scriptAlignment;
        public float salvatoreAlignment;
        public float squidAlignment;
        public float bombAlignment;

        public int bombsTopCursor;
        public int squidTopCursor;


        public Display()
        {

            string text = TextFileRepository.LoadStringFromFile("Resolution.txt");
            string[] arr = text.Split('x');
            int[] resolutions = Array.ConvertAll(arr, s => int.Parse(s));
            int resolutionWidth = resolutions[0];
            int resolutionHeight = resolutions[1];

            switch (resolutionWidth)
            {
                case 3440:
                    scriptAlignment = 0.33f;
                    salvatoreAlignment = 0.66f;
                    squidAlignment = 0.63f;
                    bombAlignment = 0.37f;
                    bombsTopCursor = 25;
                    squidTopCursor = 24;
                    break;

                case 1920:

                    scriptAlignment = 0.3f;
                    salvatoreAlignment = 0.76f;
                    squidAlignment = 0.75f;
                    bombAlignment = 0.25f;
                    bombsTopCursor = 22;
                    squidTopCursor = 20;
                    break;
            }



            Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
            ShowWindow(ThisConsole, MAXIMIZE);
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.Clear();




        }
    }
}

//Source: TechDogLover OR kiaNasirzadeh on https://stackoverflow.com/questions/4423085/c-sharp-full-screen-console#:~:text=You%20can%20right%20click%20on,this%20changes%20to%20be%20persist.
