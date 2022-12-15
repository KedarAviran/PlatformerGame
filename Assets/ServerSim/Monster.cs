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
        private const float STAGGERDURATION = 0.8f;
        private Player aggroPlayer;
        private bool isAggro = false;
        private bool patrolActive = false;
        private bool hasPatrolTarget = false;
        private bool isIdle = false;
        private bool updateResume = false;
        private DateTime idleTime = DateTime.UtcNow;
        private int minIdleTime = 0;
        private int maxIdleTime = 5;
        private int monsterType;
        private float patrolTarget;
        private float patrolMinX;
        private float patrolMaxX;
        private float jumpPatrolChance = 0.005f;
        private float idleDuration = 0;

        public Monster(int monsterType ,Colider2D colider,float MAXHP, float moveSpeed, float damage,float jumpChance)
        {
            this.monsterType = monsterType;
            this.colider = colider;
            this.MAXHP = MAXHP;
            this.lifePoints = MAXHP;
            this.moveSpeed = moveSpeed;
            this.jumpPatrolChance = jumpChance;
            this.damage = damage;
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
            {
                if (isIdle)
                    updateResume = true;
                isIdle = false;
                return true;
            }
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
        public void patrol(TimeSpan delta)
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
                move((int)MsgCoder.Direction.Jump , delta);
            if (patrolTarget > pos.X)
                move((int)MsgCoder.Direction.Right, delta);
            else
                move((int)MsgCoder.Direction.Left, delta);
        }
        public void gotAttacked(float dmg,Player player)
        {
            base.gotHit(dmg);
            goIdle(STAGGERDURATION);
            isIdle = true;
            setAggroPlayer(player);
        }
        public void aggroMove(TimeSpan delta)
        {
            Random rnd = new Random();
            if (!isAggro)
                return;
            if (!checkIdleTimer())
                return;
            if (aggroPlayer.getPos().X > getPos().X)
                move((int)MsgCoder.Direction.Right, delta);
            if (aggroPlayer.getPos().X < getPos().X)
                move((int)MsgCoder.Direction.Left, delta);
            if ((float)rnd.NextDouble() > (1 - jumpPatrolChance))
                move((int)MsgCoder.Direction.Jump, delta);

        }
        public new void sendUpdateToClient()
        {
            base.sendUpdateToClient();
            if (updateResume)
                ComSim.instance.receiveMsgClient(MsgCoder.setTriggerOrder(figureID, "Resume"));
            updateResume = false;
        }
        public Monster Clone()
        {
            return new Monster(monsterType, colider.Clone(), lifePoints, moveSpeed, damage, jumpPatrolChance);
        }
    }
}
