using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
namespace test
{
    class Planetoids
    {
        //Atlas vseh ladji
        static Texture2D planetoidsSheet;
        Animation spin;
        Animation currentAnimation;
        float planetWidth;
        float planetHeight;
        public float X
        {
            get;
            set;
        }
        public float Y
        {
            get;
            set;
        }
        public Planetoids(GraphicsDevice graphicsDevice, int row)
        {
            switch (row)
            {
                case 0:
                    X = 0;
                    Y = 0;
                    break;
                case 1:
                    X = 0;
                    Y = 130;
                    break;
                case 2:
                    X = 0;
                    Y = 260;
                    break;
                case 3:
                    X = 0;
                    Y = 390;
                    break;
                case 4:
                    X = 360;
                    Y = 0;
                    break;
                default:
                    X = 0;
                    Y = 0;
                    break;
            }
            planetWidth = 130;
            planetHeight = 130;
            if (planetoidsSheet == null)
            {
                //Poglej tukaj ce je path pravilen, ce ne se atlas ne bo izriseval pravilno. 
                //NOTE TO SELF, MORS PREMAKNT VSE V C:\Users\primoz-pc\source\repos\test\test\bin\Windows\x86\Debug\Content, KER SE OD TAM ZAGANJA DEBUGGER
                using (var stream = TitleContainer.OpenStream("Content/Planetoids/planetoids.png"))
                {
                    planetoidsSheet = Texture2D.FromStream(graphicsDevice, stream);
                }
            }
            spin = new Animation();
            for(int i=0; i<12; i++)
            {
                spin.AddFrame(new Rectangle((i*(int)planetWidth), (int)Y, (int)planetWidth, (int)planetHeight), TimeSpan.FromSeconds(.15));
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 topLeftPos = new Vector2(this.X, this.Y);
            Color tintColor = Color.White;
            var sourceRectangle = currentAnimation.CurrentRectangle;
            
            spriteBatch.Draw(planetoidsSheet, topLeftPos, sourceRectangle,Color.White);
        }
       
        public void Update(GameTime gameTime)
        {
            currentAnimation = spin;
            currentAnimation.Update(gameTime);
        }
    }
}
