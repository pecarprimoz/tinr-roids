﻿using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
namespace test
{
    class Planetoids
    {
        CollsionDetection _collision;
        Texture2D planetoidsSheet;
        Animation spin;
        Animation currentAnimation;
        float planetWidth;
        float planetHeight;
        Vector2 _actualPos;
        Vector2 _direction;
        int size_reduction;
        float _angle;
        Vector2 _accel;
        bool _isHit;
        int rockR;
        int _mass;
        public int getMass()
        {
            return _mass;
        }
        public Vector2 getPosition()
        {
            return _actualPos;
        }
        public CollsionDetection getCollision()
        {
            return _collision;
        }
        public int getRockR()
        {
            return rockR;
        }
        public void setIsHit(bool b)
        {
            _isHit = true;
        }
        public Vector2 getDirection()
        {
            return _direction;
        }
        public void setDirection(Vector2 direction)
        {
            _direction = direction;
        }
        public Vector2 generateNewDirection()
        {
            Random r = new Random(DateTime.Now.Millisecond);
            int _mode = r.Next(0, 8);
            switch (_mode)
            {
                case (0):
                    return new Vector2(0, -1);
                case (1):
                    return new Vector2(0, 1);
                case (2):
                    return new Vector2(1, 0);
                case (3):
                    return new Vector2(-1, 0);
                case (4):
                    return new Vector2(-1, 1);
                case (5):
                    return new Vector2(-1, -1);
                case (6):
                    return new Vector2(1, 1);
                case (7):
                    return new Vector2(1, -1);
                default: return new Vector2(0, -1);
            }
        }
        
        public Planetoids(GraphicsDevice graphicsDevice)
        {
            Random r = new Random(DateTime.Now.Millisecond);
            _isHit = false;
            _accel.X = 1;
            _accel.Y = 1;
            int _startPosX;
            int _startPosY;
            _direction = generateNewDirection();
            //Console.WriteLine(_direction);
            _startPosX = r.Next(0, (int)graphicsDevice.Viewport.Width);
            _startPosY = r.Next(0, (int)graphicsDevice.Viewport.Height);

            //fix collision detection so that R is set by size reduction, DONE
            int reduction_for_current_rock = r.Next(1, 5);
            size_reduction = reduction_for_current_rock;
            planetHeight = 130;
            planetWidth = 130;
            _angle = 0f;
            _actualPos.X = _startPosX;
            _actualPos.Y = _startPosY;
            rockR = 65 / size_reduction;
            _mass = 5 / size_reduction;
            //Console.WriteLine(_actualPos);
            if (planetoidsSheet == null)
            {
                //Poglej tukaj ce je path pravilen, ce ne se atlas ne bo izriseval pravilno. 
                //NOTE TO SELF, MORS PREMAKNT VSE V C:\Users\primoz-pc\source\repos\test\test\bin\Windows\x86\Debug\Content, KER SE OD TAM ZAGANJA DEBUGGER
                using (var stream = TitleContainer.OpenStream("Content/Planetoids/planetoids.png"))
                {
                    planetoidsSheet = Texture2D.FromStream(graphicsDevice, stream);
                }
            }
            _collision = new CollsionDetection(_actualPos, graphicsDevice, planetWidth / size_reduction, planetHeight / size_reduction, _angle, 2,rockR);
            spin = new Animation();
            Random pick_a_rock = new Random(DateTime.Now.Millisecond);
            int nice_rock = pick_a_rock.Next(0, 4);
            for (int i = 0; i < 12; i++)
            {
                spin.AddFrame(new Rectangle((i * (int)planetWidth), (int)nice_rock * 130, (int)planetWidth, (int)planetHeight), TimeSpan.FromSeconds(.15));
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!_isHit) { 
                var sourceRectangle = currentAnimation.CurrentRectangle;
                spriteBatch.Draw(planetoidsSheet, new Rectangle((int)_actualPos.X, (int)_actualPos.Y, (int)planetWidth / size_reduction, (int)planetHeight / size_reduction), sourceRectangle, Color.White, _angle, new Vector2(planetWidth / 2, planetHeight / 2), SpriteEffects.None, 0f);
                _collision.setPosition(_actualPos);
                _collision.setAngle(_angle);
                _collision.drawCollisionBox(spriteBatch);
            }
        }
        public void checkIfGoingTroughScreenEdges(int screenWidth, int screenHeight)
        {
            if (!_isHit)
            {
                //change hardcoded values
                if (_actualPos.X > screenWidth)
                {
                    _actualPos.X = 0;
                }
                if (_actualPos.X < 0)
                {
                    _actualPos.X = screenWidth;
                }
                if (_actualPos.Y > screenHeight)
                {
                    _actualPos.Y = 0;
                }
                if (_actualPos.Y < 0)
                {
                    _actualPos.Y = screenHeight;
                }
            }

        }

        public void Update(GameTime gametime)
        {
            if (!_isHit)
            {
                _actualPos += _direction * _accel;
                currentAnimation = spin;
                currentAnimation.Update(gametime);
            }
        }
        public void UpdateOnRockCollision(Planetoids rockB)
        {
            // get the mtd

            Vector2 delta = _actualPos - rockB._actualPos;
            float d = delta.Length();
            if (d < 0)
            {
                d = 0.2f;
            }

            Vector2 mtd = delta * (((getRockR() + rockB.getRockR()) - d) / d);

            float im1 = 1 / getMass() ;
            float im2 = 1 / rockB.getMass();
            
            _actualPos = _actualPos+(mtd*(im1 / (im1 + im2)));
            rockB._actualPos = rockB._actualPos-(mtd*(im2 / (im1 + im2)));
            

            Vector2 v = (_accel*_direction)-(rockB._accel*rockB._direction);
            float vn = Vector2.Dot(v, Vector2.Normalize(mtd));

            if (vn > 0.0f) return;

            float i = (-(1.0f + 0.2f) * vn) / (im1 + im2);
            Vector2 impulse =mtd*i;

            _accel = _accel * _direction + (impulse*im1);
            rockB._accel = rockB._accel * rockB._direction - (impulse*im2);

        }

    }
}
