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

        public Display()
        {

            Console.SetWindowSize(240, 63);
            ShowWindow(ThisConsole, MAXIMIZE);
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.Clear();

        }
    }
}

//Source: TechDogLover OR kiaNasirzadeh on https://stackoverflow.com/questions/4423085/c-sharp-full-screen-console#:~:text=You%20can%20right%20click%20on,this%20changes%20to%20be%20persist.
