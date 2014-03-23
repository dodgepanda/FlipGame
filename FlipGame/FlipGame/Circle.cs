using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlipGame
{
    class Circle
    {
        int xPos, yPos, radius;

        public Circle(int x, int y, int r)
        {
            xPos = x;
            yPos = y;
            radius = r;
        }

        public Microsoft.Xna.Framework.Vector2 getCenter()
        {
            return new Microsoft.Xna.Framework.Vector2(xPos+radius, yPos+radius);
        }

        public bool intersects(Circle c)
        {
            double distance = Math.Sqrt(Math.Pow(getCenter().X - c.getCenter().X, 2) + Math.Pow(getCenter().Y - c.getCenter().Y, 2));
            return radius + c.radius > distance;
        }
    }
}
