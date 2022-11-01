using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace ServerSim
{
    class Colider2D
    {
        private Vector2 center;
        private Vector2 topRight;
        private Vector2 topLeft;
        private Vector2 botRight;
        private Vector2 botLeft;
        private float angle;
        private float width;
        private float height;
        
        public Colider2D(Vector2 center , float width, float height , float angle)
        {
            this.center = center;
            this.width = width;
            this.height = height;
            this.angle = angle;
            defaultPoints();
            calculatePoints();
        }
        public Vector2 getTopRight()
        {
            return new Vector2(topRight.X, topRight.Y);
        }
        public Vector2 getTopLeft()
        {
            return new Vector2(topLeft.X, topLeft.Y);
        }
        public Vector2 getBotLeft()
        {
            return new Vector2(botLeft.X, botLeft.Y);
        }
        public Vector2 getBotRight()
        {
            return new Vector2(botRight.X, botRight.Y);
        }
        public void updateColider(Vector2 center)
        {
            this.center = center;
            defaultPoints();
            calculatePoints();
        }
        private void defaultPoints()
        {
            topRight = center + new Vector2(width / 2, height / 2);
            topLeft = center + new Vector2(-width / 2, height / 2);
            botRight = center + new Vector2(width / 2, -height / 2);
            botLeft = center + new Vector2(-width / 2, -height / 2);
        }
        private void calculatePoints()
        {
            calculatePoint(topLeft);
            calculatePoint(topRight);
            calculatePoint(botLeft);
            calculatePoint(botRight);
        }
        private void calculatePoint(Vector2 point)
        {
            float rotatedX = (point.X - center.X) * (float)Math.Cos(angle) - (point.Y - center.Y) * (float)Math.Sin(angle);
            float rotatedY = (point.X - center.X) * (float)Math.Sin(angle) + (point.Y - center.Y) * (float)Math.Cos(angle);
            point.X = rotatedX + center.X;
            point.Y = rotatedY + center.Y;
        }
        private float scalarValue(Vector2 p1, Vector2 p2)
        {
            return p1.X * p2.X + p1.Y * p2.Y;
        }
        public bool isColiding(Vector2 point)
        {
            Vector2 AM = topLeft - point;
            Vector2 AB = topLeft - topRight;
            Vector2 AD = topLeft - botLeft;
            if (0 < scalarValue(AM, AB) && scalarValue(AM, AB) < scalarValue(AB, AB) && 0 < scalarValue(AM, AD) && scalarValue(AM, AD) < scalarValue(AD, AD))
                return true;
            return false;
        }
        public bool isColiding(Colider2D colider)
        {
            if (isColiding(colider.topLeft) || isColiding(colider.topRight) || isColiding(colider.botRight) || isColiding(colider.botLeft))
                return true;
            return false;
        }
    }
}
