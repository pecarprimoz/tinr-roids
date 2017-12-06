
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace test
{
    class AI
    {
        //Atlas vseh ladji
        Texture2D spaceShipsSheet;
        CollsionDetection _collision;
        bool _isAlive;
        Vector2 _position;
        Vector2 _direction;
        float _accel;
        float _angle;
        float _width;
        float _height;
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
        public Vector2 setNegativeSpawnPoint(int w, int h)
        {
            Vector2 startPos = new Vector2();
            Random r = new Random();
            int startX, startY, randomVal;
            startX = r.Next(0, 2);
            startY = r.Next(0, 2);
            randomVal = r.Next(90, 151);
            if(startX == 0)
            {
                startPos.X = -randomVal;
            }
            else
            {
                startPos.X = w + randomVal;
            }
            if(startY == 0)
            {
                startPos.Y = -randomVal;
            }
            else
            {
                startPos.Y = h + randomVal;
            }
            return startPos;
        }

        public AI(GraphicsDevice graphicsDevice)
        {
            _isAlive = true;
            Vector2 tmpSpawn = setNegativeSpawnPoint(graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height);
            //Temporary spawn
            _position.X = tmpSpawn.X;
            _position.Y = tmpSpawn.Y;

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
        public void UpdateAI(int screenWidth, int screenHeight, PlayerCharacter pc)
        {
            //Ok recimo da lah uporabm za AI k se hoče na vsak način zadet vate
            //faza obračanja in faza premikanja
            if (pc.getIsAlive()) { 
                _direction = Vector2.Normalize(pc.getPosition()-_position);
            
                _position += _direction * _accel;
                _angle = (float)Math.Atan2(_direction.Y, -_direction.X);
                Console.WriteLine(_angle);
                _accel += 0.2f;
                if (_accel > 5)
                {
                    _accel = 5;
                }
            }
            else
            {
                Vector2 nul = new Vector2();
                nul.X = 0;
                nul.Y = 0;
                _direction = Vector2.Normalize(nul - _position);

                _position += _direction * _accel;
                _angle = (float)Math.Atan2(_direction.Y, -_direction.X);
                Console.WriteLine(_angle);
                _accel += 0.2f;
                if (_accel > 10)
                {
                    _accel = 10;
                }
            }
        }
    }
}
