using MidProject;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace ServerSim
{
    class Monster : Figure
    {
        private const float TARGETDISTANCE = 0.2f;
        private const float STAGGERDURATION = 1f;
        private Player aggroPlayer;
        bool isAggro = false;
        private DateTime idleTime = DateTime.UtcNow;
        private float idleDuration =0;
        private int minIdleTime = 0;
        private int maxIdleTime = 2;
        bool patrolActive = false;
        private bool hasPatrolTarget = false;
        private float patrolTarget;
        private float patrolMinX;
        private float patrolMaxX;
        private float jumpPatrolChance = 0.005f;
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
        public void setPatrolLimit(float minX, float maxX)
        {
            this.patrolMaxX = maxX;
            this.patrolMinX = minX;
            setPatrol(true);
        }
        public void setPatrol(bool patrol)
        {
            this.patrolActive = patrol;
        }
        public void setAggroPlayer(Player player)
        {
            this.aggroPlayer = player;
            isAggro = true;
        }
        private void setNewPatrolTarget(Random rnd)
        {
            patrolTarget = (float)rnd.NextDouble() + (rnd.Next((int)patrolMinX, (int)patrolMaxX));
            hasPatrolTarget = true;
        }
        private bool checkIdleTimer()
        {
            TimeSpan delta = DateTime.UtcNow - idleTime;
            if (delta.TotalSeconds > idleDuration)
                return true;
            return false;
        }
        private void goIdle(float duration)
        {
            idleTime = DateTime.UtcNow;
            idleDuration = duration;
        }
        private void reachedTarget(Random rnd)
        {
            hasPatrolTarget = false;
            goIdle(rnd.Next(minIdleTime, maxIdleTime));
        }
        public void patrol()
        {
            Random rnd = new Random();
            if (isAggro)
                return;
            if (!patrolActive)
                return;
            if (!checkIdleTimer())
                return;
            if (Math.Abs(patrolTarget - pos.X) < TARGETDISTANCE)
                reachedTarget(rnd);
            if (!hasPatrolTarget)
                setNewPatrolTarget(rnd);
            if ((float)rnd.NextDouble() > (1-jumpPatrolChance))
                move((int)MsgCoder.Direction.Jump);
            if (patrolTarget > pos.X)
                move((int)MsgCoder.Direction.Right);
            else
                move((int)MsgCoder.Direction.Left);
        }
        public void gotAttacked(float dmg,Player player)
        {
            base.gotHit(dmg);
            goIdle(STAGGERDURATION);
            setAggroPlayer(player);
        }
        public void aggroMove()
        {
            Random rnd = new Random();
            if (!isAggro)
                return;
            if (aggroPlayer.getPos().X > getPos().X)
                move((int)MsgCoder.Direction.Right);
            if (aggroPlayer.getPos().X < getPos().X)
                move((int)MsgCoder.Direction.Left);
            if ((float)rnd.NextDouble() > (1 - jumpPatrolChance))
                move((int)MsgCoder.Direction.Jump);

        }
        public Monster Clone()
        {
            return new Monster(monsterType, colider.Clone(), damage, lifePoints);
        }
    }
}
