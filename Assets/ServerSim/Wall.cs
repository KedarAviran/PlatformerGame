using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace ServerSim
{
    class Wall
    {
        private Colider2D colider;
        public Wall(Vector2 pos, float width, float height)
        {
            colider = new Colider2D(pos, width, height,0);
        }
        public Colider2D GetColider2D()
        {
            return colider;
        }
        public bool isColiding(Colider2D figure)
        {
            return (colider.isParallelColiding(figure) && colider.getBotRight().Y < figure.getBotRight().Y);
        }
    }
}
