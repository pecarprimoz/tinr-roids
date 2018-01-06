
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;

namespace test
{
    class ScreenUI
    {
        SpriteFont font;
        Texture2D shipHP;
        private int posX;
        private int posY;
        String word;
        public ScreenUI(ContentManager content,int posx, int posy, String s)
        {
            if (font == null)
            {
                //Poglej tukaj ce je path pravilen, ce ne se atlas ne bo izriseval pravilno. 
                //NOTE TO SELF, MORS PREMAKNT VSE V C:\Users\primoz-pc\source\repos\test\test\bin\Windows\x86\Debug\Content, KER SE OD TAM ZAGANJA DEBUGGER
                font = content.Load<SpriteFont>("Fonts/ScreenUI");   
            }
            posX = posx;
            posY = posy;
            word = s;
        }
        public ScreenUI(GraphicsDevice graphicsDevice, int posx, int posy,int hp)
        {

            if (shipHP == null)
            {
                //Poglej tukaj ce je path pravilen, ce ne se atlas ne bo izriseval pravilno. 
                //NOTE TO SELF, MORS PREMAKNT VSE V C:\Users\primoz-pc\source\repos\test\test\bin\Windows\x86\Debug\Content, KER SE OD TAM ZAGANJA DEBUGGER
                if(hp>=3)
                using (var stream = TitleContainer.OpenStream("Content/Ships/main-ship-v1.png"))
                {
                    shipHP = Texture2D.FromStream(graphicsDevice, stream);
                }
                else if (hp == 2)
                {
                    using (var stream = TitleContainer.OpenStream("Content/Ships/main-ship-v1-damage1.png"))
                    {
                        shipHP = Texture2D.FromStream(graphicsDevice, stream);
                    }
                }
                else if (hp <= 1)
                {
                    using (var stream = TitleContainer.OpenStream("Content/Ships/main-ship-v1-damage2.png"))
                    {
                        shipHP = Texture2D.FromStream(graphicsDevice, stream);
                    }
                }
            }
            posX = posx;
            posY = posy;
        }

        public void DrawFont(SpriteBatch spriteBatch)
        {
            Vector2 textMiddlePoint = font.MeasureString(word) / 2;
            // Places text in center of the screen
            Vector2 position = new Vector2(posX, posY);
            spriteBatch.DrawString(font, word, position, Color.White, 0, textMiddlePoint, 1.0f, SpriteEffects.None, 1.0f);
        }
        public void DrawImage(SpriteBatch spriteBatch)
        {
            Vector2 imageMiddlePoint = new Vector2(shipHP.Width, shipHP.Height) / 2;
            //RESIZE THIS
            spriteBatch.Draw(shipHP,new Vector2(posX,posY),null, Color.White, 0.0f, imageMiddlePoint, 0.5f,SpriteEffects.None, 0f);
        }
        public void UpdateValues(String w)
        {
            word = w;
        }
    }
}
