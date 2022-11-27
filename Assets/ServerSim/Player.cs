using MidProject;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace ServerSim
{
    class Player : Figure
    {
        private struct SkillCD
        {
            public int skillID;
            public double lastUse;
            public SkillCD(int skillID, double lastUse)
            {
                this.skillID = skillID;
                this.lastUse = lastUse;
            }
        }
        private List<int> skills = new List<int>();
        private List<SkillCD> skillCDs = new List<SkillCD>();
        private float invulnerableDuration = 1;
        private DateTime invulnerableTime = DateTime.UtcNow;
        private int playerType;
        private bool onLadder = false;
        private bool updateOnLadder = false;
        public Player(int playerType, Colider2D colider, float lifePoints, float moveSpeed, float damage)
        {
            this.colider = colider;
            this.playerType = playerType;
            this.lifePoints = lifePoints;
            this.moveSpeed = moveSpeed;
            this.damage = damage;
        }
        public Player Clone()
        {
            return new Player(playerType, colider.Clone(), lifePoints, moveSpeed, damage);
        }
        public bool getOnLadder()
        {
            return onLadder;
        }
        public void setOnLadder(bool onLadder)
        {
            if (this.onLadder != onLadder)
                updateOnLadder = true;
            this.onLadder = onLadder;
        }
        public int getPlayerType()
        {
            return playerType;
        }
        private double getLastUseByID(int skillID)
        {
            foreach (SkillCD cd in skillCDs)
                if (cd.skillID == skillID)
                    return cd.lastUse;
            SkillCD skill = new SkillCD(skillID, DateTime.Now.Subtract(DateTime.MinValue).TotalSeconds);
            skillCDs.Add(skill);
            return DateTime.Now.Subtract(DateTime.MinValue).TotalSeconds;
        }
        private void removeSkillCD(int skillID)
        {
            SkillCD current= skillCDs[0];
            foreach (SkillCD cd in skillCDs)
                if (cd.skillID == skillID)
                    current=cd;
            if (skillCDs.Contains(current))
                skillCDs.Remove(current);
        }
        public void setSkillLastUse(int skillID)
        {
            getLastUseByID(skillID);
            removeSkillCD(skillID);
            skillCDs.Add(new SkillCD(skillID, DateTime.Now.Subtract(DateTime.MinValue).TotalSeconds));
        }
        public bool isSkillOnCD(int skillID)
        {
            Skill skill = DataHolder.getSkill(skillID);
            if (DateTime.Now.Subtract(DateTime.MinValue).TotalSeconds - getLastUseByID(skillID) < skill.getCooldown())
                return true;
            return false;
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
        public new void sendUpdateToClient()
        {
            base.sendUpdateToClient();
            if (updateOnLadder)
                ComSim.instance.receiveMsgClient(MsgCoder.onLadderOrder(figureID, onLadder));
            updateOnLadder = false;
        }
    }
}
