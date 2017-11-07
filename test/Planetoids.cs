using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
namespace test
{
    class Planetoids
    {
        //Atlas vseh ladji
        CollsionDetection _collision;
        Texture2D planetoidsSheet;
        Animation spin;
        Animation currentAnimation;
        float planetWidth;
        float planetHeight;
        //Vector2 _position;
        Vector2 _actualPos;
        //Vector2 _direction;
        int size_reduction;
        float _angle;
        bool _isHit;
        public CollsionDetection getCollision()
        {
            return _collision;
        }
        public void setIsHit(bool b)
        {
            _isHit = true;
        }
        Random r = new Random(DateTime.Now.Millisecond);
        public Planetoids(GraphicsDevice graphicsDevice)
        {
            _isHit = false;
            int _startPosX;
            int _startPosY;

            _startPosX = r.Next(0, (int)graphicsDevice.Viewport.Width);
            _startPosY = r.Next(0, (int)graphicsDevice.Viewport.Height);
            //_position.Y = 0;_position.Y = 130;_position.Y = 260;_position.Y = 390;

            size_reduction = 2;
            planetHeight = 130;
            planetWidth = 130;
            _angle = 0f;
            _actualPos.X = _startPosX;
            _actualPos.Y = _startPosY;
            Console.WriteLine(_actualPos);
            if (planetoidsSheet == null)
            {
                //Poglej tukaj ce je path pravilen, ce ne se atlas ne bo izriseval pravilno. 
                //NOTE TO SELF, MORS PREMAKNT VSE V C:\Users\primoz-pc\source\repos\test\test\bin\Windows\x86\Debug\Content, KER SE OD TAM ZAGANJA DEBUGGER
                using (var stream = TitleContainer.OpenStream("Content/Planetoids/planetoids.png"))
                {
                    planetoidsSheet = Texture2D.FromStream(graphicsDevice, stream);
                }
            }
            _collision = new CollsionDetection(_actualPos, graphicsDevice, planetWidth / size_reduction, planetHeight / size_reduction, _angle, 2);
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

        public void Update(GameTime gameTime)
        {
            currentAnimation = spin;
            currentAnimation.Update(gameTime);
        }
        
    }
}
