using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeSploosh
{
    internal static class Sounds
    {

        //Store all refernces to sound files in one place

        private static string directory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\Sounds\";

        public static string backgroundMusic = $"{directory}pirateship.mp3";

        public static string cursorMove = $"{directory}CursorMove.mp3";

        public static string squidHit = $"{directory}kaboom.mp3";

        public static string squidDead = $"{directory}SquidDead.mp3";

        public static string miss = $"{directory}sploosh.mp3";

    }
}
