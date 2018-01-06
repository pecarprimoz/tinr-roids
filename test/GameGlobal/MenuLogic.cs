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
        // 1 - main menu, 2 - game screen, 3 - settings
        int activeComponent;
        public int getActiveComponent()
        {
            return activeComponent;
        }
        public void setActiveComponent(int c)
        {
            activeComponent = c;
        }
        public MenuLogic(GraphicsDeviceManager graphics, ContentManager content, GraphicsDevice graphicsDevice)
        {
            activeComponent = 1;
            graphicsMenu = graphics;
            contentMenu = content;
            graphicsDeviceMenu = graphicsDevice;

            Console.WriteLine(graphicsDeviceMenu);
        }
        public GameMain setupGame()
        {
            return new GameMain(graphicsMenu, contentMenu, graphicsDeviceMenu);
        }
        public MainMenu setupMainMenu()
        {
            return new MainMenu(graphicsMenu, contentMenu, graphicsDeviceMenu);
        }
        public SettingsMenu setupSettingsMenu()
        {
            return new SettingsMenu(graphicsMenu, contentMenu, graphicsDeviceMenu);
        }
    }
    

}
