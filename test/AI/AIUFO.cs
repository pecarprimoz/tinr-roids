
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace test
{
    class AIUFO
    {
        //Atlas vseh ladji
        Texture2D spaceShipsSheet;
        CollsionDetection _collision;
        bool _driving;
        bool _isAlive;
        Vector2 _position;
        Vector2 _direction;
        float _accel;
        float _angle;
        float _width;
        float _height;
        Vector2 currentDest;
        int screenWidth;
        int screenHeight;

        public CollsionDetection getCollision()
        {
            return _collision;
        }
        public float getWidth()
        {
            return _width;
        }
        public float getHeight()
        {
            return _height;
        }
        public void setDirection(Vector2 dir)
        {
            _direction = dir;
        }
        public void setPosition(Vector2 pos)
        {
            _position = pos;
        }
        public Vector2 getPosition()
        {
            return _position;
        }
        public bool getIsAlive()
        {
            return _isAlive;
        }
        public Vector2 getDirection()
        {
            return _direction;
        }
        public float getAngle()
        {
            return _angle;
        }
        public void setIsAlive(bool b)
        {
            _isAlive = b;
        }
        public Vector2 generateRandomPointOnMap()
        {
            Random r = new Random();
            return new Vector2(r.Next(20, screenWidth - 20), r.Next(20, screenHeight - 20));
        }
        public AIUFO(GraphicsDevice graphicsDevice)
        {
            _isAlive = true;
            //Temporary spawn
            
            screenWidth = graphicsDevice.Viewport.Width;
            screenHeight = graphicsDevice.Viewport.Height;
            currentDest = generateRandomPointOnMap();
            _position = new Vector2(300, 300);

            if (spaceShipsSheet == null)
            {
                //Poglej tukaj ce je path pravilen, ce ne se atlas ne bo izriseval pravilno. 
                //NOTE TO SELF, MORS PREMAKNT VSE V C:\Users\primoz-pc\source\repos\test\test\bin\Windows\x86\Debug\Content, KER SE OD TAM ZAGANJA DEBUGGER
                using (var stream = TitleContainer.OpenStream("Content/Ships/main-ship-v1.png"))
                {
                    spaceShipsSheet = Texture2D.FromStream(graphicsDevice, stream);
                    _width = spaceShipsSheet.Width;
                    _height = spaceShipsSheet.Height;
                }
            }
            //Init collision detection
            _collision = new CollsionDetection(_position, graphicsDevice, _width, _height, _angle, 1);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (_isAlive)
            {
                //Draw the player, /2 mas tm za rotacijsko tocko, k je nasred sprite-a
                spriteBatch.Draw(spaceShipsSheet, new Rectangle((int)_position.X, (int)_position.Y, (int)spaceShipsSheet.Width, (int)spaceShipsSheet.Height), null, Color.White, _angle, new Vector2(spaceShipsSheet.Width / 2, spaceShipsSheet.Height / 2), SpriteEffects.None, 0f);
                //Draw the box for collsion detection
                _collision.setPosition(_position);
                _collision.setAngle(_angle);
                _collision.drawCollisionBox(spriteBatch);
            }
        }
        public void UpdateAI(PlayerCharacter pc, GameTime gametime)
        {
            _direction = Vector2.Normalize(pc.getPosition() - _position);
            _position += _direction * _accel;
            _angle = (float)Math.Atan2(_direction.Y, -_direction.X);
            Console.WriteLine(_angle);
        }
        public void RotateUFO()
        {
            
        }

        public void Type1AI()
        {
            if (Vector2.Distance(_position, currentDest) > 5)
            {
                _direction = Vector2.Normalize(currentDest - _position);
                _position += _direction * _accel;
                _accel += 0.2f;
                if (_accel > 5)
                {
                    _accel = 5;
                }
            }
            else
            {
                currentDest = generateRandomPointOnMap();
            }

        }
    }
}
