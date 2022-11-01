using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace ServerSim
{
    class Monster : Figure
    {
        bool aggro = false;
        bool patrolActive = false;
        private bool patrolSide = true;  //true = right false = left
        private float moveSpeed = 0.5f;
        public Monster(Colider2D colider)
        {
            this.colider = colider;
        }
        public void patrol()
        {
            
        }
        public Monster Clone()
        {
            return this;
        }
    }
}
