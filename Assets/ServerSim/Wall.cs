using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace ServerSim
{
    class Wall
    {
        Colider2D colider;
        public Wall(Vector2 pos, float width, float height)
        {
            colider = new Colider2D(pos, width, height,0);
        }
        public bool isColiding(Colider2D figure)
        {
            return colider.isParallelColiding(figure);
        }
    }
}
