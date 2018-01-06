using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using test.GameGlobal;

namespace test
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {

        GraphicsDeviceManager graphics;
        MenuLogic menuLogic;
        GameMain game;
        MainMenu menu;
        SettingsMenu settings;
        int screenWidth = 800;
        int screenHeight = 600;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            menuLogic = new MenuLogic(graphics,this.Content, this.GraphicsDevice);
            game = menuLogic.setupGame();
            menu = menuLogic.setupMainMenu();
            settings = menuLogic.setupSettingsMenu();
        }
        protected override void Initialize()
        {
            this.IsMouseVisible = true;
            base.Initialize();
        }
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            //game.LoadContent();
        }

        protected override void UnloadContent()
        {
        }

        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            if (menu.getIsButtonPlayPressed())
            {
                menuLogic.setActiveComponent(2);
                menu.setIsButtonPlayPressed(false);
            }
            else if (game.getIsButtonPressed())
            {
                menuLogic.setActiveComponent(1);
                game.setIsButtonPressed(false);
            }
            else if (menu.getIsButtonSettingsPressed())
            {
                menuLogic.setActiveComponent(3);
                menu.setIsButtonSettingsPressed(false);
            }
            else if (settings.getIsButtonBackPressed())
            {
                menuLogic.setActiveComponent(1);
                settings.setIsButtonBackPressed(false);
            }

            if (menuLogic.getActiveComponent() == 1)
            {
                menu.Update(gameTime);
            }
            else if (menuLogic.getActiveComponent() == 2) { 
                game.Update(gameTime);
            }
            else if(menuLogic.getActiveComponent() == 3)
            {
                settings.Update(gameTime);
            }

            

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            if(menuLogic.getActiveComponent() == 1)
            {
                menu.Draw(gameTime);
            }
            else if(menuLogic.getActiveComponent() == 2)
            {
                game.Draw(gameTime);
            }
            else if (menuLogic.getActiveComponent() == 3)
            {
                settings.Draw(gameTime);
            }
            base.Draw(gameTime);
        }

        //Pomožne funkcije za lepoto update funkcije
       
    }
}