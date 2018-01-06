
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
        Animation currentAnimation;
        float chaseTimer;
        Texture2D aiSheet;
        CollsionDetection _collision;
        bool _isAlive;
        int size_amp;
        Animation spin;
        Vector2 _position;
        Vector2 _direction;
        float _accel;
        float _angle;
        float _width;
        float _height;
        int type;
        int screenWidth;
        int screenHeight;
        Vector2 currentDest;
        int aiR;
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
            chaseTimer = 0.0f;
            currentDest = generateRandomPointOnMap();
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

        public AI(GraphicsDevice graphicsDevice, int t)
        {
            screenHeight = graphicsDevice.Viewport.Height;
            screenWidth = graphicsDevice.Viewport.Width;
            type = t;
            _isAlive = true;
            Vector2 tmpSpawn = setNegativeSpawnPoint(graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height);
            //Temporary spawn
            _position.X = tmpSpawn.X;
            _position.Y = tmpSpawn.Y;
            _width = 28;
            _height = 28;
            size_amp = 2;
            aiR = 28;
            if (aiSheet == null)
            {
                //Poglej tukaj ce je path pravilen, ce ne se atlas ne bo izriseval pravilno. 
                //NOTE TO SELF, MORS PREMAKNT VSE V C:\Users\primoz-pc\source\repos\test\test\bin\Windows\x86\Debug\Content, KER SE OD TAM ZAGANJA DEBUGGER
                using (var stream = TitleContainer.OpenStream("Content/AI/Mine/mines_together.png"))
                {
                    aiSheet = Texture2D.FromStream(graphicsDevice, stream);
                }
            }
            //Init collision detection
            _collision = new CollsionDetection(_position, graphicsDevice, _width * size_amp, _height * size_amp, _angle, 2, aiR);
            spin = new Animation();
            for (int i = 0; i < 3; i++)
            {
                spin.AddFrame(new Rectangle((i * (int)_width), 0, (int)_width, (int)_height), TimeSpan.FromSeconds(.15));
            }
            currentAnimation = spin;

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (_isAlive)
            {
                var sourceRectangle = currentAnimation.CurrentRectangle;
                //Draw the player, /2 mas tm za rotacijsko tocko, k je nasred sprite-a
                spriteBatch.Draw(aiSheet, new Rectangle((int)_position.X, (int)_position.Y, (int)_width *size_amp, (int)_height * size_amp), sourceRectangle, Color.White, _angle, new Vector2(_width / 2, _height /2 ), SpriteEffects.None, 0f);
                //Draw the box for collsion detection
                _collision.setPosition(_position);
                _collision.setAngle(_angle);
                _collision.drawCollisionBox(spriteBatch);

            }
        }
        public Vector2 generateRandomPointOnMap()
        {
            Random r = new Random();
            return new Vector2(r.Next(20,screenWidth-20),r.Next(20,screenHeight-20));
        }
        public void Type1AI()
        {   
            if (Vector2.Distance(_position,currentDest) > 5)
            {
                _direction = Vector2.Normalize(currentDest - _position);
                _position += _direction * _accel;
                _angle = (float)Math.Atan2(_direction.Y, -_direction.X);
                //Console.WriteLine(_angle);
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
        public void Type2AI(PlayerCharacter pc)
        {
            if (pc.getIsAlive())
            {
                _direction = Vector2.Normalize(pc.getPosition() - _position);
                _position += _direction * _accel;
                _angle = (float)Math.Atan2(_direction.Y, -_direction.X);
                _accel += 0.2f;
                if (_accel > 5)
                {
                    _accel = 5;
                }
            }
            else
            {
                Vector2 nul = new Vector2(0,0);
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
        public void Type3AI(PlayerCharacter pc)
        {
            if (chaseTimer > 3.0f)
            {
                chaseTimer = 0;
            }
            if (pc.getIsAlive())
            {
                if(Vector2.Distance(pc.getPosition(),_position) > 60 && chaseTimer==0.0f)
                {
                    Type1AI();
                }
                else
                {
                    chaseTimer += (float)0.01f;
                    _direction = Vector2.Normalize(pc.getPosition() - _position);
                    _position += _direction * _accel;
                    _angle = (float)Math.Atan2(_direction.Y, -_direction.X);
                    //Console.WriteLine(_angle);
                    _accel += 0.2f;
                    if (_accel > 5)
                    {
                        _accel = 5;
                    }
                }
            }
            else
            {
                chaseTimer = 0.0f;
                Type1AI();
            }
        }
        public void UpdateAI(PlayerCharacter pc, GameTime gametime)
        {
            //Ok recimo da lah uporabm za AI k se hoče na vsak način zadet vate
            //faza obračanja in faza 
            
            currentAnimation.Update(gametime);
            if (type == 1)
            {
                Type1AI();
            }
            else if (type == 2)
            {
                Type2AI(pc);
            }
            else if(type == 3)
            {
                Type3AI(pc);
            }
        }
    }
}
