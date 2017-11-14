using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test
{
    class BackgroundLoader
    {
        List<Background> backgrounds;
        public List<Background> getBackgroundList()
        {
            return backgrounds;
        }
        public BackgroundLoader(GraphicsDevice graphicsDevice)
        {
            
            backgrounds = new List<Background>();
            //Poglej tukaj ce je path pravilen, ce ne se atlas ne bo izriseval pravilno. 
            //NOTE TO SELF, MORS PREMAKNT VSE V C:\Users\primoz-pc\source\repos\test\test\bin\Windows\x86\Debug\Content, KER SE OD TAM ZAGANJA DEBUGGER
            using (var stream = TitleContainer.OpenStream("Content/Background/bkgd_0.png"))
            {
                backgrounds.Add(new Background(Texture2D.FromStream(graphicsDevice, stream), new Vector2(125,125), 0.6f, false));
            }
            using (var stream = TitleContainer.OpenStream("Content/Background/bkgd_1.png"))
            {
                backgrounds.Add(new Background(Texture2D.FromStream(graphicsDevice, stream), new Vector2(125, 125), 0.8f, false));
            }
            using (var stream = TitleContainer.OpenStream("Content/Background/bkgd_2.png"))
            {
                backgrounds.Add(new Background(Texture2D.FromStream(graphicsDevice, stream), new Vector2(125, 125), 1.1f, false));
            }
            using (var stream = TitleContainer.OpenStream("Content/Background/bkgd_3.png"))
            {
                backgrounds.Add(new Background(Texture2D.FromStream(graphicsDevice, stream), new Vector2(125, 125), 0.6f, false));
            }
            using (var stream = TitleContainer.OpenStream("Content/Background/bkgd_4.png"))
            {
                backgrounds.Add(new Background(Texture2D.FromStream(graphicsDevice, stream), new Vector2(125, 125), 0.6f, false));
            }
            using (var stream = TitleContainer.OpenStream("Content/Background/bkgd_5.png"))
            {
                backgrounds.Add(new Background(Texture2D.FromStream(graphicsDevice, stream), new Vector2(125, 125), 0.8f, false));
            }
            using (var stream = TitleContainer.OpenStream("Content/Background/bkgd_6.png"))
            {
                backgrounds.Add(new Background(Texture2D.FromStream(graphicsDevice, stream), new Vector2(125, 125), 0.8f,true));
            }
            using (var stream = TitleContainer.OpenStream("Content/Background/bkgd_7.png"))
            {
                backgrounds.Add(new Background(Texture2D.FromStream(graphicsDevice, stream), new Vector2(125, 125), 0.8f, true));
            }
        }
        public Vector2 checkDirection(KeyboardState state, PlayerCharacter character)
        {   // VSE LEPO POPRAVLJENO, ASK BROTHER ABOUT THIS IF CLAUSE CUZ IKD ANYMORE
            Vector2 direction = Vector2.Zero;
            Vector2 playerDirection = character.getDirection();
            //U
            if (state.IsKeyDown(Keys.Up) && (playerDirection.X > -1/4 && playerDirection.X < 1/4) && (playerDirection.Y < 0))
            {
                direction += new Vector2(0, -1);
            }
            //D
            
            else if (state.IsKeyDown(Keys.Up) && (playerDirection.X > -1 / 4 && playerDirection.X < 1 / 4) && (playerDirection.Y > 0))
            {
                direction += new Vector2(0, -1);
            }
            //L
            else if (state.IsKeyDown(Keys.Up) && (playerDirection.X < 0) && (playerDirection.Y < 1 / 4 && playerDirection.Y > -1 / 4))
            {
                direction += new Vector2(-1, 0);
            }
            //R
            else if (state.IsKeyDown(Keys.Up) && (playerDirection.X > 0) && (playerDirection.Y < 1 / 4 && playerDirection.Y > -1 / 4))
            {
                direction += new Vector2(1, 1);
            }
            //UL
            else { 
                if (state.IsKeyDown(Keys.Up) && (playerDirection.X > -1 && playerDirection.X < 0) && (playerDirection.Y < 0))
                {
                    direction += new Vector2(-1, -1);
                }
                //UR
                else if (state.IsKeyDown(Keys.Up) && (playerDirection.X > 0 && playerDirection.X<1) && (playerDirection.Y < 0))
                {
                    direction += new Vector2(1, -1);
                }
                //DL
                else if (state.IsKeyDown(Keys.Up) && (playerDirection.X > -1 && playerDirection.X < 0) && (playerDirection.Y > 0))
                {
                    direction += new Vector2(-1, 1);
                }
                //DR
                else if (state.IsKeyDown(Keys.Up) && (playerDirection.X > 0 && playerDirection.X < 1) && (playerDirection.Y > 0))
                {
                    direction += new Vector2(1, 1);
                }
            }

            return direction;
        }
    }
}
