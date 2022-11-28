using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace ServerSim
{
    class Ladder
    {
        Colider2D colider;
        private const float DISTANCEFROMCENTER = 0.4f;
        public Ladder(Vector2 pos, float width, float height)
        {
            colider = new Colider2D(pos, width, height, 0);
        }
        public bool isOnLadder(Colider2D figure)
        {
            return (colider.isParallelColiding(figure) && Math.Abs((figure.getBotLeft().X + figure.getBotRight().X) / 2 - (colider.getBotLeft().X + colider.getBotRight().X) / 2) < DISTANCEFROMCENTER);
        }
    }
}
