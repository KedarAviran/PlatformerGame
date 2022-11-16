using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using MidProject;

namespace ServerSim
{
    class Figure
    {
        public bool update = false;
        private bool onAir = true;
        protected Colider2D colider;
        protected int figureID;
        protected Vector2 pos = Vector2.Zero;
        protected float damage = 5;
        protected float lifePoints = 100;
        protected float jumpVelocity = 0.5f;
        protected float baseGravityFactor = -2f; // unit per sec
        protected float verticalVelocity = 0;
        protected float moveSpeed = 0.1f;
        protected float minVerticalVelocity = -5f;
        protected void updateColider()
        {
            colider.updateColider(pos);
        }
        public void updatePosition(Vector2 pos)
        {
            this.pos = pos;
            updateColider();
            update = true;
        }
        public Vector2 getPos()
        {
            return new Vector2(pos.X, pos.Y);
        }
        public float getVerticalVelocity()
        {
            return verticalVelocity;
        }
        public Colider2D GetColider2D()
        {
            return colider;
        }
        public int getID()
        {
            return figureID;
        }
        public void setID(int id)
        {
            this.figureID = id;
        }
        public bool getOnAir()
        {
            return onAir;
        }
        public void move(int dir)
        {
            switch (dir)
            {
                case (int)MsgCoder.Direction.Right:
                    updatePosition(pos + new Vector2(moveSpeed, 0));
                    break;
                case (int)MsgCoder.Direction.Left:
                    updatePosition(pos + new Vector2(-moveSpeed, 0));
                    break;
                case (int)MsgCoder.Direction.Jump:
                    jump();
                    break;
                case (int)MsgCoder.Direction.Up:
                    updatePosition(pos + new Vector2(0, moveSpeed));
                    break;
                case (int)MsgCoder.Direction.Down:
                    updatePosition(pos + new Vector2(0, -moveSpeed));
                    break;
            }
            update = true;
        }
        public void jump()
        {
            if (onAir)
                return;
            verticalVelocity = jumpVelocity;
            setOnAir(true);
        }
        public bool isAlive()
        {
            if (lifePoints > 0)
                return true;
            return false;
        }
        protected void gotHit(float dmg)
        {
            lifePoints -= dmg;
            ComSim.instance.receiveMsgClient(MsgCoder.newLifeOfFigureOrder(figureID, lifePoints));
        }
        public float getDamage()
        {
            return damage;
        }
        public void setOnAir(bool onAir)
        {
            if (onAir)
                this.onAir = true;
            else
            {
                this.onAir = false;
                verticalVelocity = 0;
            }
        }
        
        public void applyGravity(TimeSpan span)
        {
            verticalVelocity += baseGravityFactor * (float)span.TotalSeconds;
            if (verticalVelocity < minVerticalVelocity) // apply min velocity
                verticalVelocity = minVerticalVelocity;
            updatePosition(pos + new Vector2(0, verticalVelocity));
        }
        public void sendUpdateToClient()
        {
            if(update)
                ComSim.instance.receiveMsgClient(MsgCoder.newLocationOrder(figureID, pos));
            update = false;
        }

    }
}
