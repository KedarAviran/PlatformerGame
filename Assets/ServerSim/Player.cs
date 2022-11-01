using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace ServerSim
{
    class Player : Figure
    {
        private int level;
        private int lifePoints;
        private int playerID;
        private float moveSpeed = 0.5f;
        private List<int> skills = new List<int>();
        public void moveRight()
        {
            updatePosition(pos + new Vector2(moveSpeed, 0));
        }
        public void moveLeft()
        {
            updatePosition(pos + new Vector2(-moveSpeed, 0));
        }
    }
}
