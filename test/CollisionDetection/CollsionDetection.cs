using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test
{
    class CollsionDetection
    {
        Texture2D collisionBox;
        Texture2D collisionCircle;
        Vector2 _position;
        float _width;
        float _height;
        float _angle;
        GraphicsDevice graphicsDevice;
        int _type;

        public void setPosition(Vector2 newpos)
        {
            _position = newpos;
        }
        public void setAngle(float newang)
        {
            _angle = newang;
        }

        public CollsionDetection(Vector2 pos, GraphicsDevice gd, float spriteWidth, float spriteHeight, float ag, int typeOfCollision)
        {
            _position = pos;
            graphicsDevice = gd;
            _width = spriteWidth;
            _height = spriteHeight;
            _angle = ag;
            _type = typeOfCollision;

            //Set up the bounding box for the ship
            if (typeOfCollision == 1)
            {
                Color[] colorData = new Color[(int)_width * (int)_height];
                collisionBox = new Texture2D(graphicsDevice, (int)_width, (int)_height);
                for (int i = 0; i < _width * _height; i++)
                    colorData[i] = Color.Red;
                collisionBox.SetData<Color>(colorData);
            }

        }
        public CollsionDetection(Vector2 pos, GraphicsDevice gd, float spriteWidth, float spriteHeight, float ag, int typeOfCollision,int rockR)
        {
            _position = pos;
            graphicsDevice = gd;
            _width = spriteWidth;
            _height = spriteHeight;
            _angle = ag;
            _type = typeOfCollision;

            
            if (typeOfCollision == 2)
            {
                
                collisionCircle = CreateCircle(rockR, gd);
            }

        }


        public void drawCollisionBox(SpriteBatch spriteBatch)
        {
            if (_type == 1)
            {
                spriteBatch.Draw(collisionBox, new Rectangle((int)_position.X, (int)_position.Y, (int)collisionBox.Width, (int)collisionBox.Height), null, Color.Red * 0.2f, _angle, new Vector2(_width / 2, _height / 2), SpriteEffects.None, 0f);
            }
            else if(_type == 2)
            {
                spriteBatch.Draw(collisionCircle, new Rectangle((int)_position.X, (int)_position.Y, (int)collisionCircle.Width, (int)collisionCircle.Height), null, Color.Red, _angle, new Vector2(_width / 2, _height / 2), SpriteEffects.None, 0f);
            }
        }
        public Texture2D CreateCircle(int radius, GraphicsDevice gd)
        {
            int outerRadius = radius * 2 + 2; // So circle doesn't go out of bounds
            Texture2D texture = new Texture2D(gd, outerRadius, outerRadius);

            Color[] data = new Color[outerRadius * outerRadius];

            // Colour the entire texture transparent first.
            for (int i = 0; i < data.Length; i++)
                data[i] = Color.Transparent;

            // Work out the minimum step necessary using trigonometry + sine approximation.
            double angleStep = 1f / radius;

            for (double angle = 0; angle < Math.PI * 2; angle += angleStep)
            {
                int x = (int)Math.Round(radius + radius * Math.Cos(angle));
                int y = (int)Math.Round(radius + radius * Math.Sin(angle));

                data[y * outerRadius + x + 1] = Color.White;
            }

            texture.SetData(data);
            return texture;
        }
        //#TODO, CHANGE THE 30 TO CIRCLE.R, DONE
        public bool DoRectangleCircleOverlap(CollsionDetection cir, CollsionDetection rect, int _circSize)
        {
            Vector2 circleDistance;
            circleDistance.X = Math.Abs((cir._position.X - rect._position.X));
            circleDistance.Y = Math.Abs(cir._position.Y - rect._position.Y);

            if (circleDistance.X > (rect._width / 2 + _circSize)) { return false; }
            if (circleDistance.Y > (rect._height / 2 + _circSize)) { return false; }

            if (circleDistance.X <= (rect._width / 2)) { return true; }
            if (circleDistance.Y <= (rect._height / 2)) { return true; }

            double cornerDistance_sq = Math.Pow((circleDistance.X - rect._width / 2),2) + Math.Pow((circleDistance.Y - rect._height / 2),2);

            return (cornerDistance_sq <= (_circSize ^ 2));

        }
        public bool DoCircleCircleOverlap(CollsionDetection cirA, CollsionDetection cirB,int _cirSizeA,int _cirSizeB)
        {
            
            float dx = cirA._position.X - cirB._position.X;
            float dy = cirA._position.Y - cirB._position.Y;
            int radii = _cirSizeA + _cirSizeB;
            int sqrradi = radii * radii;
            float distsqrt = (dx * dx) + (dy * dy);
            return (distsqrt <= sqrradi);
        }

    }
}
