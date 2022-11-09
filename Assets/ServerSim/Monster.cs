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
        private float damage;
        private float lifePoints;
        private int monsterType;
        public Monster(int monsterType ,Colider2D colider, float damage , float lifePoints)
        {
            this.monsterType = monsterType;
            this.colider = colider;
            this.damage = damage;
            this.lifePoints = lifePoints;
        }
        public int getMonsterType()
        {
            return monsterType;
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
