using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace ServerSim
{
    class Floor
    {
        // y=mx+n
        private float m;
        private float n;

        private Colider2D colider;
        public Floor(Vector2 pos, float width, float height, float angle)
        {
            colider = new Colider2D(pos, width, height, angle);
            m = (colider.getTopLeft().Y - colider.getTopRight().Y) / (colider.getTopLeft().X - colider.getTopRight().X);
            n = colider.getTopLeft().Y - m * colider.getTopLeft().X;
        }
        public float getYByX(float x)
        {
            return m * x + n;
        }
        public bool isDirectlyAbove(Colider2D figure)
        {
            if (figure.getBotLeft().Y > colider.getTopLeft().Y || figure.getBotLeft().Y > colider.getTopRight().Y || figure.getBotRight().Y > colider.getTopLeft().Y || figure.getBotRight().Y > colider.getTopRight().Y)
                if (figure.getBotLeft().X < colider.getTopRight().X && figure.getBotLeft().X > colider.getTopLeft().X || figure.getBotRight().X < colider.getTopRight().X && figure.getBotRight().X > colider.getTopLeft().X)
                    return true;
            return false;
        }
        public float distanceFromFeetToTop(Colider2D figure)
        {
            float distanceRight = figure.getBotRight().Y - getYByX(figure.getBotRight().X);
            float distanceLeft = figure.getBotLeft().Y - getYByX(figure.getBotLeft().X);
            if (distanceRight * distanceLeft > 0)
                return Math.Min(distanceRight, distanceLeft);
            return Math.Max(distanceRight, distanceLeft);
        }
    }
}
