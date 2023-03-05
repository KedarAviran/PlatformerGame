using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace ServerSim
{
    class Skill
    {
        private int skillID;
        private Colider2D colider;
        private Vector2 reletivePos;
        private float damage;
        private float cooldown;
        public Skill(int skillID, Colider2D colider,Vector2 reletivePos,float damage,float cooldown)
        {
            this.skillID = skillID;
            this.colider = colider;
            this.reletivePos = reletivePos;
            this.damage = damage;
            this.cooldown = cooldown;
        }
        public float getCooldown()
        {
            return cooldown;
        }
        public int getID()
        {
            return skillID;
        }
        public Vector2 getReletivePos()
        {
            return reletivePos;
        }
        public Colider2D GetColider2D()
        {
            return colider.Clone();
        }
        public float getDamage()
        {
            return damage;
        }
    }
}
