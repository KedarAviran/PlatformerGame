using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace ServerSim
{
    class Player : Figure
    {
        private int level;
        private int lifePoints;
        private List<int> skills = new List<int>();
        public Player(Vector2 pos)
        {
            colider = new Colider2D(pos, 5, 5, 0);
            this.pos = pos;
        }
    }
}
