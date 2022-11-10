using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace ServerSim
{
    class Wall
    {
        private Colider2D colider;
        List<int> figures = new List<int>();
        public Wall(Vector2 pos, float width, float height)
        {
            colider = new Colider2D(pos, width, height,0);
        }
        public Colider2D GetColider2D()
        {
            return colider;
        }
        public bool isColiding(Figure figure)
        {
            if (colider.isParallelColiding(figure.GetColider2D()) && colider.getBotRight().Y > figure.GetColider2D().getBotRight().Y)
                figures.Add(figure.getID());
            else
            if (figures.Contains(figure.getID()))
                figures.Remove(figure.getID());
            if (colider.isParallelColiding(figure.GetColider2D()) && colider.getBotRight().Y < figure.GetColider2D().getBotRight().Y && !figures.Contains(figure.getID()))
                return true;
            return false;
        }
    }
}
