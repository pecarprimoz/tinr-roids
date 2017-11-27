using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace test
{
    
    class Projectile
    {
        Texture2D projectileSheet;
        CollsionDetection _collision;
        Vector2 _position;
        Vector2 _direction;
        bool isFlying;
        bool canShoot;
        float _angle;
        float _width;
        float _height;
        public void RefreshProjectile()
        {
            isFlying = false;
            canShoot = true;
        }
        public Texture2D getProjectileSheet()
        {
            return projectileSheet;
        }
        public CollsionDetection getCollision()
        {
            return _collision;
        }
        public Boolean getisFlying()
        {
            return isFlying;
        }
        public void setisFlying(bool isfly)
        {
            isFlying = isfly;
        }
        public Projectile(GraphicsDevice graphicsDevice)
        {
            canShoot = true;
            isFlying = false;
            if (projectileSheet == null)
            {
                //Poglej tukaj ce je path pravilen, ce ne se atlas ne bo izriseval pravilno. 
                //NOTE TO SELF, MORS PREMAKNT VSE V C:\Users\primoz-pc\source\repos\test\test\bin\Windows\x86\Debug\Content, KER SE OD TAM ZAGANJA DEBUGGER
                using (var stream = TitleContainer.OpenStream("Content/Projectiles/shotsmall.png"))
                {
                    projectileSheet = Texture2D.FromStream(graphicsDevice, stream);
                    _width = projectileSheet.Width;
                    _height = projectileSheet.Height;
                }
            }
            //Init collision detection
            _collision = new CollsionDetection(_position, graphicsDevice, _width, _height, _angle, 1);
        }
        public void checkControls(KeyboardState state)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                canShoot = false;
                isFlying = true;
                _direction = new Vector2((float)Math.Sin(_angle), -(float)Math.Cos(_angle));
            }
        }

        public void UpdateWithShip(PlayerCharacter pc, int gdWidth, int gdHeight)
        {
           
            if (canShoot) {
                _position = pc.getPosition();
                _direction = pc.getDirection();
                _angle = pc.getAngle();
            }
            _position += _direction * 7;
        }
        public bool isOutOfBounds(int width,int height)
        {
            return (_position.X > width || _position.X < 0) || (_position.Y > height || _position.Y < 0);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            _collision.setPosition(_position);
            _collision.setAngle(_angle);
            _collision.drawCollisionBox(spriteBatch);
            //Draw the player, /2 mas tm za rotacijsko tocko, k je nasred sprite-a
            spriteBatch.Draw(projectileSheet, new Rectangle((int)_position.X, (int)_position.Y, (int)projectileSheet.Width, (int)projectileSheet.Height), null, Color.White, _angle, new Vector2(projectileSheet.Width / 2, projectileSheet.Height / 2), SpriteEffects.None, 0f);
            //Draw the box for collsion detection
        }
    }
}
