using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace ServerSim
{
    class Floor
    {
        private const float COLIDEDISTANCE = 0.001f;
        private Colider2D colider;
        private List<int> figuresAbove = new List<int>();
        public Floor(Vector2 pos, float width, float height, float angle)
        {
            colider = new Colider2D(pos, width, height, angle);
        }

        public float getYofFloor()
        {
            return colider.getTopRight().Y;
        }
        public bool isInRange(Figure figure)
        {
            Colider2D figureColider = figure.GetColider2D();
            if (figureColider.getBotLeft().X < colider.getTopRight().X && figureColider.getBotLeft().X > colider.getTopLeft().X || figureColider.getBotRight().X < colider.getTopRight().X && figureColider.getBotRight().X > colider.getTopLeft().X)
                return true;
            return false;
        }
        public bool checkFigureColision(Figure figure)
        {
            int id = figure.getID();
            if (isInRange(figure))
            {
                if (getYofFloor() < figure.GetColider2D().getBotLeft().Y)
                    if (!figuresAbove.Contains(id))
                        figuresAbove.Add(id);
                if (figuresAbove.Contains(id) && getYofFloor() > figure.GetColider2D().getBotLeft().Y)
                    return true;
            }
            else
            {
                if (figuresAbove.Contains(id))
                    figuresAbove.Remove(id);
            }
            return false;
        }
        public void removeFigure(int figureID)
        {
            if (figuresAbove.Contains(figureID))
                figuresAbove.Remove(figureID);
        }
    }
}
