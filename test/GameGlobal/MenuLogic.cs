using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test.GameGlobal
{
    class MenuLogic
    {
        GraphicsDeviceManager graphicsMenu;
        ContentManager contentMenu;
        GraphicsDevice graphicsDeviceMenu;
        public MenuLogic(GraphicsDeviceManager graphics, ContentManager content, GraphicsDevice graphicsDevice)
        {
            graphicsMenu = graphics;
            contentMenu = content;
            graphicsDeviceMenu = graphicsDevice;

            Console.WriteLine(graphicsDeviceMenu);
        }
        public GameMain setupGame()
        {
            return new GameMain(graphicsMenu, contentMenu, graphicsDeviceMenu);
        }
    }
    

}
