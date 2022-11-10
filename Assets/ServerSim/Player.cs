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
        private List<int> skills = new List<int>();
        private float invulnerableDuration = 0.5f;
        private DateTime invulnerableTime = DateTime.UtcNow;
        public Player(Vector2 pos)
        {
            colider = new Colider2D(pos, 5, 5, 0);
            this.pos = pos;
        }
        public bool isInvulnerable()
        {
            if (invulnerableDuration < (DateTime.UtcNow - invulnerableTime).TotalSeconds)
                return false;
            return true;
        }
        public void gotAttacked(float dmg)
        {
            if (!isInvulnerable())
                gotHit(dmg);
        }
        protected new void gotHit(float dmg)
        {
            base.gotHit(dmg);
            invulnerableTime = DateTime.UtcNow;
        }
    }
}
