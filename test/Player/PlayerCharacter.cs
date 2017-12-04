
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace test
{
    class PlayerCharacter
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

        public PlayerCharacter(GraphicsDevice graphicsDevice)
        {
            _isAlive = true;
            //Temporary spawn
            _position.X = 400;
            _position.Y = 300;

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
        public void checkIfGoingTroughScreenEdges(int screenWidth, int screenHeight)
        {
            //change hardcoded values
            if (_position.X > screenWidth)
            {
                _position.X = 0;
            }
            else if (_position.X < 0)
            {
                _position.X = screenWidth;
            }
            else if (_position.Y > screenHeight)
            {
                _position.Y = 0;
            }
            else if (_position.Y < 0)
            {
                _position.Y = screenHeight;
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (_isAlive) { 
                //Draw the player, /2 mas tm za rotacijsko tocko, k je nasred sprite-a
                spriteBatch.Draw(spaceShipsSheet, new Rectangle((int)_position.X, (int)_position.Y, (int)spaceShipsSheet.Width, (int)spaceShipsSheet.Height), null, Color.White, _angle, new Vector2(spaceShipsSheet.Width / 2, spaceShipsSheet.Height / 2), SpriteEffects.None, 0f);
                //Draw the box for collsion detection
                _collision.setPosition(_position);
                _collision.setAngle(_angle);
                _collision.drawCollisionBox(spriteBatch);
            }
        }

        public void checkControls(KeyboardState state, int screenWidth, int screenHeight, List<Planetoids> rocks)
        {
            if (_isAlive)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.R))
                {
                    int id = 0;
                    foreach(Planetoids p in rocks)
                    {
                        Console.WriteLine(id+" "+p.getPosition().X+" "+p.getPosition().Y);
                        id++;
                    }
                    
                }
                checkIfGoingTroughScreenEdges(screenWidth, screenHeight);
                if (Keyboard.GetState().IsKeyDown(Keys.Left))
                {
                    _angle -= 0.1f;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Right))
                {
                    _angle += 0.1f;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Up))
                {
                    if (_driving == false)
                    {
                        _driving = true;
                        _accel = 1;
                    }
                    _direction = new Vector2((float)Math.Sin(_angle), -(float)Math.Cos(_angle));
                    _position += _direction * _accel;
                    _accel += 0.2f;
                    if (_accel > 5)
                    {
                        _accel = 5;
                    }
                }
                else
                {
                    _driving = false;
                    _accel -= 0.1f;
                }
                _position += _direction * _accel;

                if (_accel < 0)
                {
                    _accel = 0;
                }
            }
        }
    }
}
