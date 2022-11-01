using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace ServerSim
{
    class Figure
    {
        protected Colider2D colider;
        protected int figureID;
        protected Vector2 pos;
        protected void updateColider()
        {
            colider.updateColider(pos);
        }
        public void updatePosition(Vector2 pos)
        {
            this.pos = pos;
            updateColider();
        }
        public int getID()
        {
            return figureID;
        }
        public void setID(int id)
        {
            this.figureID = id;
        }

    }
}
