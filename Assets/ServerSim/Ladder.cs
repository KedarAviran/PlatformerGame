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
            if (figure.getBotRight().X < colider.getBotLeft().X || figure.getBotLeft().X > colider.getBotRight().X || figure.getBotRight().Y > colider.getTopLeft().Y || figure.getTopLeft().Y < colider.getBotRight().Y)
                return false;
            return true;
        }
    }
}
