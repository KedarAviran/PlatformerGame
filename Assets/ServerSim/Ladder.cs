using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace ServerSim
{
    class Ladder
    {
        Colider2D colider;
        public Ladder(Vector2 pos, float width, float height)
        {
            colider = new Colider2D(pos, width, height, 0);
        }
        public bool isOnLadder(Colider2D figure)
        {
            return colider.isColiding(figure);
        }
    }
}
