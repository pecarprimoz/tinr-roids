
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace test
{
    class PlayerCharacter
    {
        //Atlas vseh ladji
        static Texture2D spaceShipsSheet;
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
        public PlayerCharacter(GraphicsDevice graphicsDevice)
        {
            if (spaceShipsSheet == null)
            {
                //Poglej tukaj ce je path pravilen, ce ne se atlas ne bo izriseval pravilno. 
                //NOTE TO SELF, MORS PREMAKNT VSE V C:\Users\primoz-pc\source\repos\test\test\bin\Windows\x86\Debug\Content, KER SE OD TAM ZAGANJA DEBUGGER
                using (var stream = TitleContainer.OpenStream("Content/barrier-frontier-spaceships.png"))
                {
                    spaceShipsSheet = Texture2D.FromStream(graphicsDevice, stream);
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            //Fix this, za časno postavil koordinate da je ladjica neprekrita, popravi atlas !!!
            Vector2 topLeftPos = new Vector2(this.X+150, this.Y+150);
            Color tintColor = Color.White;
            Rectangle ship_select = new Rectangle(60, 0, 175-60, 166);
            spriteBatch.Draw(spaceShipsSheet, topLeftPos,ship_select, tintColor);
        }
    }
}
