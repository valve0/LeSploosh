using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeSploosh
{
    public static class Animations
    {
        
        public static TileState[] miss = { TileState.Miss1, TileState.Miss2, TileState.Miss1, TileState.GameMiss };
        public static TileState[] hit = { TileState.GameHit, TileState.GameStart, TileState.GameHit };
        public static int waitTime = 800; //Time in millseconds between each state

    }
}
