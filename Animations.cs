using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public static class Animations
    {
        
        public static SeaState[] miss = { SeaState.Miss1, SeaState.Miss2, SeaState.Miss1, SeaState.SeaMiss };
        public static SeaState[] hit = { SeaState.SeaHit, SeaState.SeaStart, SeaState.SeaHit };
        public static int waitTime = 800; //Time in millseconds between each state

    }
}
