using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public static class Animations
    {
        
        public static GameState[] miss = { GameState.Miss1, GameState.Miss2, GameState.Miss1, GameState.GameMiss };
        public static GameState[] hit = { GameState.GameHit, GameState.GameStart, GameState.GameHit };
        public static int waitTime = 800; //Time in millseconds between each state

    }
}
