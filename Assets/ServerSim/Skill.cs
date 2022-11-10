using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace ServerSim
{
    class Skill
    {
        int skillID;
        Colider2D colider;
        float damage;
        public Skill(int skillID, Colider2D colider,float damage)
        {
            this.skillID = skillID;
            this.colider = colider;
            this.damage = damage;
        }
        public int getID()
        {
            return skillID;
        }
        public Colider2D GetColider2D()
        {
            return colider;
        }
        public float getDamage()
        {
            return damage;
        }
    }
}
